using System;
using System.Linq;
using System.Threading;
using Lapis.Level.Generation.Population;
using Lapis.Threading;
using Lapis.Utility;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Manages generating chunks in bulk
	/// </summary>
	public class BulkGenerator
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

		private readonly Realm _realm;
		private volatile GenerationSpeed _speed;
		private ulong _completed, _total;
		private int _populatorsDone, _populatorCount;
		private readonly object _locker = new object();

		/// <summary>
		/// Creates a new bulk generator
		/// </summary>
		/// <param name="realm">Realm to generate chunks for</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="realm"/> is null</exception>
		public BulkGenerator (Realm realm)
		{
			if(null == realm)
				throw new ArgumentNullException("realm", "The realm to generate chunks for can't be null.");

			_realm = realm;
			_speed = GenerationSpeed.Full;
		}

		/// <summary>
		/// Creates a new bulk generator
		/// </summary>
		/// <param name="realm">Realm to generate chunks for</param>
		/// <param name="speed">Speed to use when generating chunks</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="realm"/> is null</exception>
		public BulkGenerator (Realm realm, GenerationSpeed speed)
		{
			if(null == realm)
				throw new ArgumentNullException("realm", "The realm to generate chunks for can't be null.");

			_realm = realm;
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

		#region Progress events
		/// <summary>
		/// Triggered when progress is made generating chunks
		/// </summary>
		public event EventHandler<GenerationProgressEventArgs> GenerationProgress;

		/// <summary>
		/// Triggered when progress is made populating chunks
		/// </summary>
		public event EventHandler<PopulationProgressEventArgs> PopulationProgress;
		#endregion

		#region Terrain generation
		/// <summary>
		/// Generates a rectangular region of chunks
		/// </summary>
		/// <param name="startX">X-position of the chunk to start generating at</param>
		/// <param name="startZ">Z-position of the chunk to start generating at</param>
		/// <param name="countX">Number of chunks to generate along the x-axis</param>
		/// <param name="countZ">Number of chunks to generate along the z-axis</param>
		/// <param name="overwrite">Whether or not to overwrite existing chunks</param>
		/// <returns>The number of chunks generated</returns>
		/// <remarks>This method will block until all of the chunks have been generated.</remarks>
		public ulong GenerateRectange (int startX, int startZ, int countX, int countZ, bool overwrite = false)
		{
			if(countX <= 0 || countZ <= 0)
				return 0;

			var waitList = new WaitList();
			var unitSize = _generationCount[(int)_speed];

			var lastX = startX + countX;
			var lastZ = startZ + countZ;
			var totalChunks = (ulong)countX * (ulong)countZ;

			lock(_realm)
			{// Lock to prevent other threads from interfering
				lock(_locker)
				{
					_completed = 0;
					_total = totalChunks;
				}

				for(var cx = startX; cx < lastX; cx += unitSize)
					for(var cz = startZ; cz < lastZ; cz += unitSize)
					{
						var xSize  = Math.Min(unitSize, lastX - cx);
						var zSize  = Math.Min(unitSize, lastZ - cz);
						var handle = waitList.NextHandle();
						var work   = new GenerationUnit(cx, cz, xSize, zSize, overwrite, handle);
						PriorityThreadPool.QueueUserWorkItem(doGenerationWork, work);
					}

				waitList.WaitAll();
				_realm.FlushChunks();

				_realm.Initialized = true; // TODO: Move this to a better place
				_realm.Save();
			}
			return totalChunks;
		}

		private void doGenerationWork (object state)
		{
			var work = state as GenerationUnit;
			if(null != work)
			{
#if TRACE
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "] Generation: " + work.StartX + ", " + work.StartZ + " (" + work.CountX + ", " + work.CountZ + ")");
#endif
				var lastX = work.StartX + work.CountX;
				var lastZ = work.StartZ + work.CountZ;
				for(var x = work.StartX; x < lastX; ++x)
					for(var z = work.StartZ; z < lastZ; ++z)
						_realm.GenerateChunk(x, z, work.Overwrite);
				Thread.Sleep(_generationDelay[(int)_speed]);

				ulong completed, total;
				lock(_locker)
				{
					completed = _completed = _completed + ((ulong)work.CountX * (ulong)work.CountZ);
					total     = _total;
				}
				var args = new GenerationProgressEventArgs(_realm, work.StartX, work.StartZ, work.CountX, work.CountZ, completed, total);
				OnGenerationProgress(args);
				work.Done();
			}
		}

		/// <summary>
		/// Triggers the GenerationProgress event
		/// </summary>
		/// <param name="args">Event arguments</param>
		protected virtual void OnGenerationProgress (GenerationProgressEventArgs args)
		{
			GenerationProgress.TriggerEvent(this, args);
		}
		#endregion

		#region Terrain population
		/// <summary>
		/// Populates a rectangular region within the realm
		/// </summary>
		/// <param name="startX">X-position of the chunk to start generating at</param>
		/// <param name="startZ">Z-position of the chunk to start generating at</param>
		/// <param name="countX">Number of chunks to generate along the x-axis</param>
		/// <param name="countZ">Number of chunks to generate along the z-axis</param>
		/// <param name="lightChunks">Whether or not to light the chunks</param>
		/// <remarks>This method will block until all of the chunks have been populated.</remarks>
		public void PopulateRectangle (int startX, int startZ, int countX, int countZ, bool lightChunks = true)
		{
			if(countX <= 0 || countZ <= 0)
				return;

			var unitSize = _generationCount[(int)_speed];

			var lastX = startX + countX;
			var lastZ = startZ + countZ;
			var totalChunks = (ulong)countX * (ulong)countZ;

			lock(_realm)
			{// Lock to prevent other threads from interfering
				lock(_locker)
				{
					_completed = 0;
					_total     = totalChunks;
				}

				var populatorList = _realm.TerrainGenerator.Populators;
				if(null != populatorList)
				{
					var populators = new IChunkPopulator[populatorList.Count];
					if(!lightChunks)
					{// Skip sky light and block light populators
						for(var i = 0; i < populators.Length; ++i)
						{
							var populator = populators[i];
							if(populator is SkyLightPopulator ||
							   populator is BlockLightPopulator)
								populators[i] = null;
						}
					}

					_populatorCount = populators.Count(t => t != null);
					_populatorsDone = 0;
					foreach(var populator in populators)
					{
						if(populator != null)
						{
							var waitList = new WaitList();

							for(var cx = startX; cx < lastX; cx += unitSize)
								for(var cz = startZ; cz < lastZ; cz += unitSize)
								{
									var xSize = Math.Min(unitSize, lastX - cx);
									var zSize = Math.Min(unitSize, lastZ - cz);
									var handle = waitList.NextHandle();
									var work = new PopulationUnit(cx, cz, xSize, zSize, populator, handle);
									PriorityThreadPool.QueueUserWorkItem(doPopulationWork, work);
								}

							waitList.WaitAll();
							++_populatorsDone;
						}
					}
				}

				_realm.Save();
			}
		}

		private void doPopulationWork (object state)
		{
			var work = state as PopulationUnit;
			if(null != work)
			{
				var populator = work.Populator;
				var lastX     = work.StartX + work.CountX;
				var lastZ     = work.StartZ + work.CountZ;
				var populated = _populatorsDone + 1 >= _populatorCount; // True if this is the last populator and we need to mark the chunks as populated
#if TRACE
				Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "] " + populator.PluginName + ": " + work.StartX + ", " + work.StartZ + " (" + work.CountX + ", " + work.CountZ + ")");
#endif
				for(var x = work.StartX; x < lastX; ++x)
					for(var z = work.StartZ; z < lastZ; ++z)
					{
						var chunk = _realm[x, z];
						populator.PopulateChunk(chunk);
						if(populated)
							chunk.MarkAsPopulated(); // TODO: Fix this so that even if no populators run, the chunks still get marked as populated
					}

				Thread.Sleep(_generationDelay[(int)_speed]);

				ulong completed, total;
				lock(_locker)
				{
					completed = _completed = _completed + ((ulong)work.CountX * (ulong)work.CountZ);
					total     = _total;
				}

				var args = new PopulationProgressEventArgs(_realm, work.StartX, work.StartZ, work.CountX, work.CountZ, completed, total, populator.PluginName, _populatorsDone, _populatorCount);
				OnPopulationProgress(args);
				work.Done();
			}
		}

		/// <summary>
		/// Triggers the PopulationProgress event
		/// </summary>
		/// <param name="args">Event arguments</param>
		protected virtual void OnPopulationProgress (PopulationProgressEventArgs args)
		{
			PopulationProgress.TriggerEvent(this, args);
		}
		#endregion

		#region Units of work
		/// <summary>
		/// A unit of work
		/// </summary>
		private abstract class WorkUnit
		{
			public readonly int StartX, StartZ, CountX, CountZ;
			private readonly ManualResetEvent _handle;

			protected WorkUnit (int startX, int startZ, int countX, int countZ, ManualResetEvent handle)
			{
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

			public GenerationUnit (int startX, int startZ, int countX, int countZ, bool overwrite, ManualResetEvent handle)
				: base(startX, startZ, countX, countZ, handle)
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

			public PopulationUnit (int startX, int startZ, int countX, int countZ, IChunkPopulator populator, ManualResetEvent handle)
				: base(startX, startZ, countX, countZ, handle)
			{
				Populator = populator;
			}
		}
		#endregion
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