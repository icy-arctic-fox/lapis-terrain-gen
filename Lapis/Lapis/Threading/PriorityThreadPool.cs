using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace Lapis.Threading
{
	/// <summary>
	/// Collection of threads that process tasks with varying priorities
	/// </summary>
	public static class PriorityThreadPool
	{
		private static readonly Queue<WorkItem>
			_lowQueue    = new Queue<WorkItem>(),
			_mediumQueue = new Queue<WorkItem>(),
			_highQueue   = new Queue<WorkItem>();
		private static readonly object _locker = _highQueue;

		#region Properties
		/// <summary>
		/// Current number of low priority tasks
		/// </summary>
		public static int LowPriorityCount
		{
			get
			{
				lock(_locker)
					return _lowQueue.Count;
			}
		}

		/// <summary>
		/// Current number of medium priority tasks
		/// </summary>
		public static int MediumPriorityCount
		{
			get
			{
				lock(_locker)
					return _mediumQueue.Count;
			}
		}

		/// <summary>
		/// Current number of high priority tasks
		/// </summary>
		public static int HighPriorityCount
		{
			get
			{
				lock(_locker)
					return _highQueue.Count;
			}
		}

		/// <summary>
		/// Current number of tasks left to process
		/// </summary>
		public static int TaskCount
		{
			get
			{
				lock(_locker)
					return _lowQueue.Count + _mediumQueue.Count + _highQueue.Count;
			}
		}
		#endregion

		/// <summary>
		/// Adds a new task to process in the thread pool
		/// </summary>
		/// <param name="callback">Method to process</param>
		/// <param name="priority">Priority of the task</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is null</exception>
		public static void QueueUserWorkItem (WaitCallback callback, Priority priority = Priority.Medium)
		{
			QueueUserWorkItem(callback, null, priority);
		}

		/// <summary>
		/// Adds a new task to process in the thread pool
		/// </summary>
		/// <param name="callback">Method to process</param>
		/// <param name="state">State object to pass to the method</param>
		/// <param name="priority">Priority of the task</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is null</exception>
		public static void QueueUserWorkItem (WaitCallback callback, object state, Priority priority = Priority.Medium)
		{
			var work = new WorkItem(callback, state);

			Queue<WorkItem> queue;
			switch(priority)
			{
			case Priority.High:
				queue = _highQueue;
				break;
			case Priority.Low:
				queue = _lowQueue;
				break;
			default:
				queue = _mediumQueue;
				break;
			}

			new PermissionSet(PermissionState.Unrestricted).Demand();
			lock(_locker)
				queue.Enqueue(work);
			ThreadPool.UnsafeQueueUserWorkItem(doWork, null);
		}

		#region Thread pool worker
		private static WorkItem getWork ()
		{
			lock(_locker)
			{
				if(_highQueue.Count > 0)
					return _highQueue.Dequeue();
				if(_mediumQueue.Count > 0)
					return _mediumQueue.Dequeue();
				if(_lowQueue.Count > 0)
					return _lowQueue.Dequeue();
			}
			return null;
		}

		private static void doWork (object state)
		{
				WorkItem work;
				lock(_locker)
					work = getWork();
				if(null != work)
					work.Callback.Invoke(work.State);
		}
		#endregion

		private class WorkItem
		{
			private readonly WaitCallback _callback;
			private readonly object _state;

			public WorkItem (WaitCallback callback, object state = null)
			{
				if(null == callback)
					throw new ArgumentNullException("callback", "The work callback can't be null.");

				_callback = callback;
				_state    = state;
			}

			public WaitCallback Callback
			{
				get { return _callback; }
			}

			public object State
			{
				get { return _state; }
			}
		}
	}

	/// <summary>
	/// Priority of task to process
	/// </summary>
	public enum Priority
	{
		/// <summary>
		/// Lowest priority, will be processed after everything else is done
		/// </summary>
		Low,

		/// <summary>
		/// Normal priority
		/// </summary>
		Medium,

		/// <summary>
		/// Highest priority, will be processed first before everything else
		/// </summary>
		High
	}
}
