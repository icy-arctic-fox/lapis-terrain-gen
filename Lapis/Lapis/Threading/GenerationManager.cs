using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lapis.Level;
using Lapis.Level.Generation;

namespace Lapis.Threading
{
	/// <summary>
	/// Manages generating chunks in bulk
	/// </summary>
	public class GenerationManager
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

		private readonly World _world;
		private volatile GenerationSpeed _speed;

		/// <summary>
		/// Creates a new manager that handles generating chunks in bulk
		/// </summary>
		/// <param name="name">Name of the new world</param>
		/// <param name="speed">Speed to generate chunks at</param>
		public GenerationManager (string name, GenerationSpeed speed = GenerationSpeed.Full)
		{
			_world = World.Create(name);
			_speed = speed;
		}

		/// <summary>
		/// Speed at which to generate chunks
		/// </summary>
		public GenerationSpeed Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		public int AddRealm (ITerrainGenerator generator, Dimension dimmension = Dimension.Normal)
		{
			return AddRealm(generator, (int)dimmension, dimmension);
		}

		public int AddRealm (ITerrainGenerator generator, int realmId, Dimension dimension = Dimension.Normal)
		{
			_world.CreateRealm(generator, realmId, dimension);
			return realmId;
		}

		/// <summary>
		/// Generates a rectangular region of chunks
		/// </summary>
		/// <param name="realmId">ID of the realm to generate chunks for</param>
		/// <param name="startX">X-position of the chunk to start generating at</param>
		/// <param name="startZ">Z-position of the chunk to start generating at</param>
		/// <param name="countX">Number of chunks to generate along the x-axis</param>
		/// <param name="countZ">Number of chunks to generate along the z-axis</param>
		/// <param name="overwrite">Whether or not to overwrite existing chunks</param>
		/// <remarks>This method will block until all of the chunks have been generated.</remarks>
		public void GenerateRectange (int realmId, int startX, int startZ, int countX, int countZ, bool overwrite = false)
		{
			var realm    = _world[realmId];
			var list     = new WaitList();
			var unitSize = _generationCount[(int)_speed];

			var lastX = startX + countX;
			var lastZ = startZ + countZ;

			for(var cx = startX; cx < lastX; cx += unitSize)
				for(var cz = startZ; cz < lastZ; cz += unitSize)
				{
					var xSize  = Math.Min(unitSize, lastX - cx);
					var zSize  = Math.Min(unitSize, lastZ - cz);
					var handle = list.NextHandle();
					var work   = new GenerationUnit(realm, cx, cz, xSize, zSize, handle);
					ThreadPool.QueueUserWorkItem(doWork, work);
				}

			// TODO: Implement overwrite functionality

			list.WaitAll();
			_world.Save();
		}

		private void doWork (object state)
		{
			var work = state as GenerationUnit;
			if(null != work)
			{
				var realm = work.Realm;
				var lastX = work.StartX + work.CountX;
				var lastZ = work.StartZ + work.CountZ;
				for(var x = work.StartX; x <= lastX; ++x)
					for(var z = work.StartZ; z <= lastZ; ++z)
						realm.GenerateChunk(x, z);
				Thread.Sleep(_generationDelay[(int)_speed]);
				work.Done();
			}
		}

		/// <summary>
		/// A unit of work
		/// </summary>
		private class GenerationUnit
		{
			public readonly Realm Realm;
			public readonly int StartX, StartZ, CountX, CountZ;
			private readonly ManualResetEvent _handle;

			public GenerationUnit (Realm realm, int startX, int startZ, int countX, int countZ, ManualResetEvent handle)
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
	}

	/// <summary>
	/// Speed at which to generate chunks
	/// </summary>
	public enum GenerationSpeed
	{
		Full,

		Fast,

		Medium,

		Slow,

		VerySlow
	}
}
