using System;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Describes an event where progress was made during chunk population
	/// </summary>
	public class PopulationProgressEventArgs : GenerationProgressEventArgs
	{
		private readonly string _populator;
		private readonly int _populatorsDone, _totalPopulators;

		/// <summary>
		/// Creates a new population progress event
		/// </summary>
		/// <param name="realm">Realm that chunks are being populated in</param>
		/// <param name="startX">Starting x-position of the area that was just finished</param>
		/// <param name="startZ">Starting z-position of the area that was just finished</param>
		/// <param name="countX">Number of chunks along the x-axis that were populated</param>
		/// <param name="countZ">Number of chunks along the z-axis that were populated</param>
		/// <param name="completed">Number of chunks completed at this point</param>
		/// <param name="total">Total number of chunks that will be populated</param>
		/// <param name="populatorName">Name of the current populator</param>
		/// <param name="populatorsDone">Number of chunk populators that have finished</param>
		/// <param name="totalPopulators">Total number of chunk populators that will run</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="realm"/> or <paramref name="populatorName"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="countX"/> or <paramref name="countZ"/> are less than one.
		/// Thrown if <paramref name="total"/> is less than <paramref name="completed"/> or either is 0.
		/// Thrown if <paramref name="populatorsDone"/> is more than <paramref name="totalPopulators"/> or either is less than 1.</exception>
		public PopulationProgressEventArgs (RealmRef realm, int startX, int startZ, int countX, int countZ, ulong completed, ulong total, string populatorName, int populatorsDone, int totalPopulators)
			: base(realm, startX, startZ, countX, countZ, completed, total)
		{
			if(null == populatorName)
				throw new ArgumentNullException("populatorName", "The name of the chunk populator can't be null.");
			if(0 >= totalPopulators)
				throw new ArgumentOutOfRangeException("totalPopulators", "The total number of chunk populators can't be less than 1.");
			if(populatorsDone > totalPopulators)
				throw new ArgumentOutOfRangeException("populatorsDone", "The number of completed chunk populators can't be greater than the total.");

			_populator       = populatorName;
			_populatorsDone  = populatorsDone;
			_totalPopulators = totalPopulators;
		}

		/// <summary>
		/// Name of the active chunk populator
		/// </summary>
		public string PopulatorName
		{
			get { return _populator; }
		}

		/// <summary>
		/// Number of completely finished chunk populators
		/// </summary>
		public int PopulatorsDone
		{
			get { return _populatorsDone; }
		}

		/// <summary>
		/// Total number of chunk populators that will run on the region
		/// </summary>
		public int TotalPopulators
		{
			get { return _totalPopulators; }
		}

		/// <summary>
		/// Number of chunk populators left to run
		/// </summary>
		public int PopulatorsLeft
		{
			get { return _totalPopulators - _populatorsDone; }
		}
	}
}
