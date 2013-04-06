using System;
using System.Threading;
using Lapis.Level;
using Lapis.Level.Generation.Population;

namespace Lapis.Threading
{
	/// <summary>
	/// Manages generating chunks in bulk
	/// </summary>
	public static class BulkGenerator
	{
		#region Generation Speed Values
		/// <summary>
		/// Delay (in milliseconds) between each piece of work
		/// </summary>
		private static readonly int[] _generationDelay = new[] {
			0,    // Full
			2000, // Fast
			5000, // Medium
			7000, // Slow
			10000 // Very slow
		};

		/// <summary>
		/// Diameter of the area of chunks to generate per unit of work
		/// </summary>
		private static readonly int[] _generationCount = new[] {
			32, // Full
			16, // Fast
			8,  // Medium
			4,  // Slow
			1   // Very slow
		};
		#endregion

		private static volatile GenerationSpeed _speed;

		/// <summary>
		/// Speed at which to generate chunks
		/// </summary>
		public static GenerationSpeed Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		/// <summary>
		/// Generates a rectangular region of chunks
		/// </summary>
		/// <param name="realm">Realm to generate chunks in</param>
		/// <param name="startX">X-position of the chunk to start generating at</param>
		/// <param name="startZ">Z-position of the chunk to start generating at</param>
		/// <param name="countX">Number of chunks to generate along the x-axis</param>
		/// <param name="countZ">Number of chunks to generate along the z-axis</param>
		/// <param name="populate">Whether or not to populate the chunks</param>
		/// <param name="overwrite">Whether or not to overwrite existing chunks</param>
		/// <returns>The number of chunks generated</returns>
		/// <remarks>This method will block until all of the chunks have been generated.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="realm"/> is null</exception>
		public static ulong GenerateRectange (this Realm realm, int startX, int startZ, int countX, int countZ, bool populate = true, bool overwrite = false)
		{
			if(null == realm)
				throw new ArgumentNullException("realm", "The realm to generate chunks for can't be null.");

			if(countX <= 0 || countZ <= 0)
				return 0;

			var genList  = new WaitList();
			var unitSize = _generationCount[(int)_speed];

			var lastX = startX + countX;
			var lastZ = startZ + countZ;
			var totalChunks = (ulong)countX * (ulong)countZ;

			for(var cx = startX; cx < lastX; cx += unitSize)
				for(var cz = startZ; cz < lastZ; cz += unitSize)
				{
					var xSize  = Math.Min(unitSize, lastX - cx);
					var zSize  = Math.Min(unitSize, lastZ - cz);
					var handle = genList.NextHandle();
					var work   = new GenerationUnit(realm, cx, cz, xSize, zSize, overwrite, handle);
					ThreadPool.QueueUserWorkItem(doGenerationWork, work);
				}

			genList.WaitAll();

			if(populate)
			{
				var populators = realm.TerrainGenerator.Populators;
				if(null != populators)
				{
					foreach(var populator in populators)
					{
						var popList = new WaitList();

						for(var cx = startX; cx < lastX; cx += unitSize)
							for(var cz = startZ; cz < lastZ; cz += unitSize)
							{
								var xSize  = Math.Min(unitSize, lastX - cx);
								var zSize  = Math.Min(unitSize, lastZ - cz);
								var handle = popList.NextHandle();
								var work   = new PopulationUnit(realm, cx, cz, xSize, zSize, populator, handle);
								ThreadPool.QueueUserWorkItem(doPopulationWork, work);
							}

						popList.WaitAll();
					}

					// TODO: Mark chunks as populated
				}
			}

			realm.Initialized = true; // TODO: Move this to a better place
			realm.Save();
			return totalChunks;
		}

		private static void doGenerationWork (object state)
		{
			var work = state as GenerationUnit;
			if(null != work)
			{
#if TRACE
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "] Generation: " + work.StartX + ", " + work.StartZ + " (" + work.CountX + ", " + work.CountZ + ")");
#endif
				var realm = work.Realm;
				var lastX = work.StartX + work.CountX;
				var lastZ = work.StartZ + work.CountZ;
				for(var x = work.StartX; x <= lastX; ++x)
					for(var z = work.StartZ; z <= lastZ; ++z)
						realm.GenerateChunk(x, z, work.Overwrite);
				Thread.Sleep(_generationDelay[(int)_speed]);
				work.Done();
			}
		}

		private static void doPopulationWork (object state)
		{
			var work = state as PopulationUnit;
			if(null != work)
			{
#if TRACE
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "] Population: " + work.StartX + ", " + work.StartZ + " (" + work.CountX + ", " + work.CountZ + ")");
#endif
				var realm     = work.Realm;
				var populator = work.Populator;
				var lastX     = work.StartX + work.CountX;
				var lastZ     = work.StartZ + work.CountZ;
				for(var x = work.StartX; x <= lastX; ++x)
					for(var z = work.StartZ; z <= lastZ; ++z)
					{
						var chunk = realm[x, z];
						populator.PopulateChunk(chunk);
					}
				Thread.Sleep(_generationDelay[(int)_speed]);
				work.Done();
			}
		}

		/// <summary>
		/// A unit of work
		/// </summary>
		private abstract class WorkUnit
		{
			public readonly Realm Realm;
			public readonly int StartX, StartZ, CountX, CountZ;
			private readonly ManualResetEvent _handle;

			protected WorkUnit (Realm realm, int startX, int startZ, int countX, int countZ, ManualResetEvent handle)
			{
				Realm   = realm;
				StartX  = startX;
				StartZ  = startZ;
				CountX  = countX;
				CountZ  = countZ;
				_handle = handle;
			}

			/// <summary>
			/// Marks the work as completed
			/// </summary>
			public void Done ()
			{
				_handle.Set();
			}
		}

		/// <summary>
		/// A unit of work for terrain generation
		/// </summary>
		private class GenerationUnit : WorkUnit
		{
			public readonly bool Overwrite;

			public GenerationUnit (Realm realm, int startX, int startZ, int countX, int countZ, bool overwrite, ManualResetEvent handle)
				: base(realm, startX, startZ, countX, countZ, handle)
			{
				Overwrite = overwrite;
			}
		}

		/// <summary>
		/// A unit of work for terrain generation
		/// </summary>
		private class PopulationUnit : WorkUnit
		{
			public readonly IChunkPopulator Populator;

			public PopulationUnit (Realm realm, int startX, int startZ, int countX, int countZ, IChunkPopulator populator, ManualResetEvent handle)
				: base(realm, startX, startZ, countX, countZ, handle)
			{
				Populator = populator;
			}
		}
	}
}