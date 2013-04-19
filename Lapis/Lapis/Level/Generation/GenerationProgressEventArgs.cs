using System;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Describes an event where progress was made during chunk generation
	/// </summary>
	public class GenerationProgressEventArgs : EventArgs
	{
		private readonly RealmRef _realm;
		private readonly int _startX, _startZ, _countX, _countZ;
		private readonly ulong _completed, _total;

		/// <summary>
		/// Creates a new generation progress event
		/// </summary>
		/// <param name="realm">Realm that chunks are being generated for</param>
		/// <param name="startX">Starting x-position of the area that was just finished</param>
		/// <param name="startZ">Starting z-position of the area that was just finished</param>
		/// <param name="countX">Number of chunks along the x-axis that were generated</param>
		/// <param name="countZ">Number of chunks along the z-axis that were generated</param>
		/// <param name="completed">Number of chunks completed at this point</param>
		/// <param name="total">Total number of chunks that will be generated</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="realm"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="countX"/> or <paramref name="countZ"/> are less than one.
		/// Also thrown if <paramref name="total"/> is less than <paramref name="completed"/> or either is 0.</exception>
		public GenerationProgressEventArgs (RealmRef realm, int startX, int startZ, int countX, int countZ, ulong completed, ulong total)
		{
			if(ReferenceEquals(null, realm))
				throw new ArgumentNullException("realm", "The realm can't be null.");
			if(0 < countX)
				throw new ArgumentOutOfRangeException("countX", "The number of chunks processed can't be less than 1.");
			if(0 < countZ)
				throw new ArgumentOutOfRangeException("countZ", "The number of chunks processed can't be less than 1.");
			if(0 >= total)
				throw new ArgumentOutOfRangeException("total", "The total number of chunks can't be less than 1.");
			if(completed > total)
				throw new ArgumentOutOfRangeException("completed", "The number of completed chunks can't be greater than the total.");

			_realm  = realm;
			_startX = startX;
			_startZ = startZ;
			_countX = countX;
			_countZ = countZ;
			_completed = completed;
			_total     = total;
		}

		/// <summary>
		/// Realm that chunks are being processed for
		/// </summary>
		public RealmRef Realm
		{
			get { return _realm; }
		}

		/// <summary>
		/// Starting position along the x-axis of the processed region
		/// </summary>
		public int StartX
		{
			get { return _startX; }
		}

		/// <summary>
		/// Starting position along the z-axis of the processed region
		/// </summary>
		public int StartZ
		{
			get { return _startZ; }
		}

		/// <summary>
		/// Number of chunks along the x-axis that were processed
		/// </summary>
		public int CountX
		{
			get { return _countX; }
		}

		/// <summary>
		/// Number of chunks along the z-axis that were processed
		/// </summary>
		public int CountZ
		{
			get { return _countZ; }
		}

		/// <summary>
		/// Number of processed chunks
		/// </summary>
		public int Count
		{
			get { return _countX * _countZ; }
		}

		/// <summary>
		/// Number of completed chunks in the entire batch
		/// </summary>
		public ulong Completed
		{
			get { return _completed; }
		}

		/// <summary>
		/// Total number of chunks that will be processed
		/// </summary>
		public ulong Total
		{
			get { return _total; }
		}

		/// <summary>
		/// Number of chunks that still need to be processed
		/// </summary>
		public ulong Remaining
		{
			get { return _total - _completed; }
		}

		/// <summary>
		/// Whether or not the generation has finished
		/// </summary>
		public bool Done
		{
			get { return _completed >= _total; }
		}

		/// <summary>
		/// Progress of the chunk processing from 0 to 1
		/// </summary>
		public double Progress
		{
			get { return (double)_completed / _total; }
		}
	}
}
