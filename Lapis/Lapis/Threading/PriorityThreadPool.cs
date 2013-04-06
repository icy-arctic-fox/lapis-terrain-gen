using System;
using System.Collections.Generic;
using System.Threading;

namespace Lapis.Threading
{
	/// <summary>
	/// Collection of threads that process tasks with varying priorities
	/// </summary>
	public class PriorityThreadPool
	{
		private volatile int _minThreads, _maxThreads;
		private readonly Queue<WorkItem>
			_lowQueue    = new Queue<WorkItem>(),
			_mediumQueue = new Queue<WorkItem>(),
			_highQueue   = new Queue<WorkItem>();
		private readonly List<Thread> _pool = new List<Thread>();
		private readonly object _locker = new object();
		
		/// <summary>
		/// Creates a new priority thread pool
		/// </summary>
		/// <remarks>The minimum and maximum thread count is set to an ideal amount for the current processor.</remarks>
		public PriorityThreadPool ()
		{
			_minThreads = _maxThreads = Environment.ProcessorCount;
			while(_pool.Count < _minThreads)
				startWorker();
		}

		/// <summary>
		/// Creates a new priority thread pool
		/// </summary>
		/// <param name="minThreads">Minimum number of threads to have active</param>
		/// <param name="maxThreads">Maximum number of threads to have active</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the minimum and maximum number of threads are invalid</exception>
		public PriorityThreadPool (int minThreads, int maxThreads)
		{
			if(minThreads > maxThreads)
				throw new ArgumentOutOfRangeException("minThreads", "The minimum number of threads must be less than or equal to the maximum number of threads.");
			if(1 > maxThreads)
				throw new ArgumentOutOfRangeException("maxThreads", "The maximum number of threads must be at least 1.");

			_minThreads = minThreads;
			_maxThreads = maxThreads;

			while(_pool.Count < _minThreads)
				startWorker();
		}

		#region Properties
		// TODO: Add support for changing the thread pool size (remember to lock _pool)

		/// <summary>
		/// Minimum number of threads in the pool
		/// </summary>
		public int MinimumThreadCount
		{
			get { return _minThreads; }
		}

		/// <summary>
		/// Maximum number of active threads in the pool
		/// </summary>
		public int MaximumThreadCount
		{
			get { return _maxThreads; }
		}

		/// <summary>
		/// Current number of low priority tasks
		/// </summary>
		public int LowPriorityCount
		{
			get
			{
				lock(_lowQueue)
					return _lowQueue.Count;
			}
		}

		/// <summary>
		/// Current number of medium priority tasks
		/// </summary>
		public int MediumPriorityCount
		{
			get
			{
				lock(_mediumQueue)
					return _mediumQueue.Count;
			}
		}

		/// <summary>
		/// Current number of high priority tasks
		/// </summary>
		public int HighPriorityCount
		{
			get
			{
				lock(_highQueue)
					return _highQueue.Count;
			}
		}

		/// <summary>
		/// Current number of tasks left to process
		/// </summary>
		public int TaskCount
		{
			get
			{
				lock(_lowQueue)
					lock(_mediumQueue)
						lock(_highQueue)
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
		public void QueueWork (WaitCallback callback, Priority priority = Priority.Medium)
		{
			QueueWork(callback, null, priority);
		}

		/// <summary>
		/// Adds a new task to process in the thread pool
		/// </summary>
		/// <param name="callback">Method to process</param>
		/// <param name="state">State object to pass to the method</param>
		/// <param name="priority">Priority of the task</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="callback"/> is null</exception>
		public void QueueWork (WaitCallback callback, object state, Priority priority = Priority.Medium)
		{
			var work = new WorkItem(callback);

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

			lock(_locker)
			{
				lock(queue)
					queue.Enqueue(work);
				Monitor.Pulse(_locker);
			}
		}

		#region Thread pool worker
		private WorkItem getWork ()
		{
			lock(_highQueue)
				if(_highQueue.Count > 0)
					return _highQueue.Dequeue();
			lock(_mediumQueue)
				if(_mediumQueue.Count > 0)
					return _mediumQueue.Dequeue();
			lock(_lowQueue)
				if(_lowQueue.Count > 0)
					return _lowQueue.Dequeue();
			return null;
		}

		private void doWork (object state)
		{
			var workerIndex = (int)state;
			try
			{
				while(true)
				{
					WorkItem work;
					lock(_locker)
					{
						work = getWork();
						while(null == work)
							Monitor.Wait(_locker);
					}
					work.Callback.Invoke(work.State);
				}
			}
			finally
			{
				lock(_pool)
					_pool[workerIndex] = null;
			}
		}

		private void startWorker ()
		{
			var threadNum    = _pool.Count;
			var workerThread = new Thread(doWork) {
				Name = "Priority Thread Pool Worker #" + (threadNum + 1),
				IsBackground = true
			};
			workerThread.Start(threadNum);
			_pool.Add(workerThread);
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
