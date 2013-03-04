using System;
using System.Collections.Generic;

namespace Lapis.Utility
{
	/// <summary>
	/// Saves items in memory so that they can be quickly accessed later instead of reloading
	/// </summary>
	/// <typeparam name="TKey">Type used to reference cached objects</typeparam>
	/// <typeparam name="TValue">Type of object to cache (must implement IDisposable)</typeparam>
	/// <remarks>This class uses the adaptive replacement cache technique.
	/// A description of this algorithm can be found here: http://en.wikipedia.org/wiki/Adaptive_replacement_cache
	/// Items that are no longer cached will be disposed (have Dispose() called on them) if the disposed flag is set.</remarks>
	public class Cache<TKey, TValue> where TValue : IDisposable
	{
		/// <summary>
		/// Default number of items to contain in the entire cache
		/// </summary>
		private const int DefaultCacheSize = 64;

		private readonly LinkedList<TKey> _t1, _b1, _t2, _b2; // These lists track meta-information about object usage
		private readonly Dictionary<TKey, TValue> _cache;     // Fast lookup of cached objects
		private int _capacity, _ghostSize;                    // Size of t1 + t2 and b1, b2 respectively
		private readonly bool _dispose;                       // Dispose objects after removal from cache

		/// <summary>
		/// Creates a new cache with the default cache size
		/// </summary>
		/// <param name="dispose">Whether or not the cached objects should be disposed (call Dispose() on them) when they're removed from the cache</param>
		public Cache (bool dispose)
		{
			_dispose = dispose;

			_t1 = new LinkedList<TKey>();
			_t2 = new LinkedList<TKey>();
			_b1 = new LinkedList<TKey>();
			_b2 = new LinkedList<TKey>();

			_cache = new Dictionary<TKey, TValue>(DefaultCacheSize);
			setSizes(DefaultCacheSize);
		}

		/// <summary>
		/// Creates a new cache with a specified cache size
		/// </summary>
		/// <param name="capacity">Maximum number of items to cache</param>
		/// <param name="dispose">Whether or not the cached objects should be disposed (call Dispose() on them) when they're removed from the cache</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when attempting to set the capacity to 0 or less</exception>
		public Cache (int capacity, bool dispose)
		{
			if(1 > capacity)
				throw new ArgumentOutOfRangeException("capacity", "The cache must have a capacity of at least 1 item.");

			_dispose = dispose;

			_t1 = new LinkedList<TKey>();
			_t2 = new LinkedList<TKey>();
			_b1 = new LinkedList<TKey>();
			_b2 = new LinkedList<TKey>();

			_cache = new Dictionary<TKey, TValue>(capacity);
			setSizes(capacity);
		}

		/// <summary>
		/// Describes a function that retrieves the object if a cache miss occurs
		/// </summary>
		/// <param name="key">Key that references the object</param>
		/// <returns>An object that corresponds to the key</returns>
		public delegate TValue CacheMiss (TKey key);

		/// <summary>
		/// The maximum amount of objects to keep in the cache
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when attempting to set the capacity to 0 or less</exception>
		/// <remarks>Changing the capacity of the cache frequently will degrade performance.
		/// If the cache size is decreased, items may be removed from the cache.</remarks>
		public int Capacity
		{
			get
			{
				lock(_cache)
					return _capacity;
			}

			set
			{
				if(1 > value)
					throw new ArgumentOutOfRangeException("value", "The cache must have a capacity of at least 1 item.");

				lock(_cache)
				{
					_capacity = value;
					_ghostSize = _capacity / 2;

					while(Count > _capacity)
					{// Cache decreased, remove items
						if(0 < _t1.Count)
							evictT1(); // Push one item from t1 to b1
						if(Count <= _capacity)
							break; // Done removing items
						if(0 < _t2.Count)
							evictT2(); // Push one item from t2 to b2
					}
				}
			}
		}

		/// <summary>
		/// Number of items in the cache
		/// </summary>
		public int Count
		{
			get
			{
				lock(_cache)
					return _t1.Count + _t2.Count;
			}
		}

		/// <summary>
		/// Checks if an object is in the cache
		/// </summary>
		/// <param name="key">Key that references the object</param>
		/// <returns>True if the object is cached in memory or false if it isn't</returns>
		/// <remarks>This should not be used in conjunction with GetItem() unless you lock the cache.</remarks>
		public bool Contains (TKey key)
		{
			lock(_cache)
				return _cache.ContainsKey(key);
		}

		/// <summary>
		/// Empties everything from the cache
		/// </summary>
		public void Clear ()
		{
			lock(_cache)
			{
				if(_dispose)
				{
					foreach(var value in _cache.Values)
						value.Dispose();
				}
				_cache.Clear();
				_t1.Clear();
				_t2.Clear();
				_b1.Clear();
				_b2.Clear();
			}
		}

		/// <summary>
		/// Retrieves an object from the cache
		/// </summary>
		/// <param name="key">Key that references the object</param>
		/// <param name="missFunc">Function to call if the object doesn't exist in the cache</param>
		/// <returns>The object that corresponds to the key</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="missFunc"/> is null</exception>
		/// <remarks>If the object doesn't exist in the cache, <paramref name="missFunc"/> will be called.
		/// Whatever is returned by that function will be saved in the cache and returned.</remarks>
		public TValue GetItem (TKey key, CacheMiss missFunc)
		{
			if(null == missFunc)
				throw new ArgumentNullException("missFunc", "The cache miss function can't be null.");

			TValue value;

			lock(_cache)
			{
				if(_cache.ContainsKey(key))
				{// Hooray! It's in the cache
					value = _cache[key];
					// Update T1 and T2
					if(!processT2(key))
						processT1(key);
				}

				else
				{// Booo. Cache miss
					value = missFunc(key);
					// Update B1 and B2
					if(!processB2(key))
						processB1(key);
					_cache.Add(key, value);
				}
			}

			return value;
		}

		#region Cache logic
		private void processT1 (TKey key)
		{
			var node = _t1.Find(key);
			if(null != node)
			{// Found in t1, move to t2
				_t1.Remove(node);
				_t2.AddFirst(key);
			}
		}

		private bool processT2 (TKey key)
		{
			var node = _t2.Find(key);
			if(null != node)
			{// Found in t2, shift to front
				_t2.Remove(node);
				_t2.AddFirst(key);
				return true;
			}
			return false;
		}

		private void processB1 (TKey key)
		{
			var node = _b1.Find(key);
			if(null != node)
			{// Found in b1, move to t2
				_b1.Remove(node);
				_t2.AddFirst(key);
			}
			else
			{// First time seeing, add to t1
				_t1.AddFirst(key);
				if(Count > _capacity)
					evictT2(); // Cache is full, push one item from t2 to b2
			}
		}

		private bool processB2 (TKey key)
		{
			var node = _b2.Find(key);
			if(null != node)
			{// Found in b2, move to t2
				_b2.Remove(node);
				_t2.AddFirst(key);
				if(Count > _capacity)
					evictT1(); // Cache is full, push one item from t1 to b1
				return true;
			}
			return false;
		}

		private void evictT1 ()
		{
			var node = _t1.Last;
			var item = _cache[node.Value];
			_cache.Remove(node.Value);
			_b1.AddFirst(node.Value);
			if(_b1.Count > _ghostSize)
				_b1.RemoveLast(); // b1 is full, evict the last item
			if(_dispose)
				item.Dispose();
		}

		private void evictT2 ()
		{
			var node = _t2.Last;
			var item = _cache[node.Value];
			_cache.Remove(node.Value);
			_b2.AddFirst(node.Value);
			if(_b2.Count > _ghostSize)
				_b2.RemoveLast(); // b2 is full, evict the last item
			if(_dispose)
				item.Dispose();
		}

		private void setSizes (int capacity)
		{
			_capacity  = capacity;
			_ghostSize = capacity / 2;
			if(1 == capacity % 2 || 1 > _ghostSize)
				++_ghostSize;
		}
		#endregion
	}
}
