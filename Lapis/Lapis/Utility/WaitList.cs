using System.Collections.Generic;
using System.Threading;

namespace Lapis.Utility
{
	/// <summary>
	/// A large collection of reset events
	/// </summary>
	/// <remarks>WaitHandle can only wait for a maximum of 64 items at a time.
	/// This class can handle a larger amount of reset events.</remarks>
	public sealed class WaitList
	{
		/// <summary>
		/// Maximum number of reset events that WaitHandle accepts
		/// </summary>
		private const int BatchSize = 64;

		private readonly List<ManualResetEvent[]> _eventList = new List<ManualResetEvent[]>();
		private readonly List<ManualResetEvent> _workingList = new List<ManualResetEvent>(BatchSize);

		/// <summary>
		/// Retrieves the next handle to use
		/// </summary>
		/// <param name="initialState">Initial state of the handle - true to be in the signaled state (complete) or false to be not signaled (incomplete)</param>
		/// <returns>A new wait handle</returns>
		public WaitHandle NextHandle (bool initialState)
		{
			lock(_eventList)
			{
				if(BatchSize == _workingList.Count)
				{// Working set has reached its limit
					var currentList = _workingList.ToArray();
					_eventList.Add(currentList);
					_workingList.Clear();
				}

				var handle = new ManualResetEvent(initialState);
				_workingList.Add(handle);
				return handle;
			}
		}
		
		/// <summary>
		/// Waits for all the event handles to signal
		/// </summary>
		public void WaitAll ()
		{
			lock(_eventList)
			{
				if(0 < _workingList.Count)
				{// Add last set of events
					var currentList = _workingList.ToArray();
					_eventList.Add(currentList);
					_workingList.Clear();
				}

				foreach(var list in _eventList)
					WaitHandle.WaitAll(list);
			}
		}
	}
}
