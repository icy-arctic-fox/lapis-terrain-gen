using System;
using System.Collections;
using System.Collections.Generic;

namespace Lapis.Utility
{
	/// <summary>
	/// Saves items in memory so that they can be quickly accessed later instead of reloading
	/// </summary>
	/// <typeparam name="TKey">Type used to reference cached objects</typeparam>
	/// <typeparam name="TValue">Type of object to cache (must implement IDisposable)</typeparam>
	/// <remarks>This class uses the Low Inter-reference Recency Set cache technique.
	/// A description of this algorithm can be found here: http://en.wikipedia.org/wiki/LIRS_caching_algorithm
	/// Items that are no longer cached will be disposed (have Dispose() called on them) if the disposed flag is set.</remarks>
	public class Cache<TKey, TValue> : IDictionary<TKey, TValue> where TValue : class, IDisposable
	{
		#region Preset Constants
		/// <summary>
		/// Default number of items to contain in the entire cache
		/// </summary>
		private const int DefaultCacheSize = 64;

		/// <summary>
		/// Percentage of the cache that is dedicated to hot entries
		/// </summary>
		private const float HotRate = 0.99f;
		#endregion

		/// <summary>
		/// Describes a method that returns an item to insert into the cache given a key
		/// </summary>
		/// <param name="key">Key that represents the item to cache</param>
		/// <returns>Value of the item to cache</returns>
		public delegate TValue CacheMiss (TKey key);

		private readonly Dictionary<TKey, CacheEntry> _cacheEntries;
		private readonly CacheEntry _header;

		private readonly int _maxHotSize, _maxSize;
		private volatile int _hotSize, _size;
		private readonly bool _disposeEntries;

		/// <summary>
		/// Creates a new cache with the default maximum number of entries
		/// </summary>
		/// <param name="dispose">Flag determining whether or not cached items are forcefully disposed when evicted from the cache</param>
		public Cache (bool dispose = true)
		{
			_maxSize        = DefaultCacheSize;
			_maxHotSize     = calculateMaxHotSize(_maxSize);
			_cacheEntries   = new Dictionary<TKey, CacheEntry>(_maxSize);
			_header         = new CacheEntry(this);
			_disposeEntries = dispose;
		}

		/// <summary>
		/// Creates a new cache
		/// </summary>
		/// <param name="maxSize">Maximum number of items to keep in the cache</param>
		/// <param name="dispose">Flag determining whether or not cached items are forcefully disposed when evicted from the cache</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="maxSize"/> is less than 1</exception>
		public Cache (int maxSize, bool dispose = true)
		{
			if(1 > maxSize)
				throw new ArgumentOutOfRangeException("maxSize", "The maximum size of the cache must be at least 1.");

			_maxSize        = maxSize;
			_maxHotSize     = calculateMaxHotSize(_maxSize);
			_cacheEntries   = new Dictionary<TKey, CacheEntry>(_maxSize);
			_header         = new CacheEntry(this);
			_disposeEntries = dispose;
		}

		/// <summary>
		/// Maximum number of items to keep in the cache
		/// </summary>
		public int Capacity
		{
			get { return _maxSize; }
		}

		#region Dictionary access
		/// <summary>
		/// Checks if an item is in the cache
		/// </summary>
		/// <param name="key">Key that represents that item</param>
		/// <returns>True if the cache contains the item or false if it doesn't</returns>
		public bool ContainsKey (TKey key)
		{
			lock(_cacheEntries)
				return _cacheEntries.ContainsKey(key);
		}

		/// <summary>
		/// Forces an item to be added to the cache
		/// </summary>
		/// <param name="key">Key that represents the item</param>
		/// <param name="value">Value of the item itself</param>
		/// <exception cref="ArgumentException">Thrown if an item already exists in the cache with the same key as <paramref name="key"/></exception>
		public void Add (TKey key, TValue value)
		{
			lock(_cacheEntries)
			{
				var entry = new CacheEntry(this, key, value);
				if(_cacheEntries.ContainsKey(key))
					throw new ArgumentException("The item specified by the key already exists in the cache.", "key");
				_cacheEntries[key] = entry;
			}
		}

		/// <summary>
		/// Forces an item to be removed from the cache
		/// </summary>
		/// <param name="key">Key that represents the item</param>
		/// <returns>True if the item existed and was removed or false if it didn't exist to start with</returns>
		public bool Remove (TKey key)
		{
			CacheEntry entry;
			lock(_cacheEntries)
				if(_cacheEntries.TryGetValue(key, out entry))
				{
					entry.Remove();
					return true;
				}
			return false;
		}

		/// <summary>
		/// Attempts to get a value without raising an exception
		/// </summary>
		/// <param name="key">Key that represents the item</param>
		/// <param name="value">Updated to contain the value of the cached item if it exists</param>
		/// <returns>True if the item exists in the cache and <paramref name="value"/> was set or false if the item doesn't exist in the cache</returns>
		public bool TryGetValue (TKey key, out TValue value)
		{
			lock(_cacheEntries)
			{
				CacheEntry entry;
				if(_cacheEntries.TryGetValue(key, out entry))
				{
					value = entry.Value;
					if(entry.IsResident)
						entry.Hit();
					else
						entry.Miss();
					return true;
				}
				value = default(TValue);
				return false;
			}
		}

		/// <summary>
		/// Access to items in the cache
		/// </summary>
		/// <param name="key">Key that represents the cached item</param>
		/// <returns>The item contained in the cache</returns>
		/// <exception cref="KeyNotFoundException">Thrown if an item represented by <paramref name="key"/> does not exist in the cache</exception>
		public TValue this [TKey key]
		{
			get
			{
				lock(_cacheEntries)
				{
					// This could throw a KeyNotFoundException
					var entry = _cacheEntries[key];

					if(entry.IsResident)
						entry.Hit();
					else
						entry.Miss();
					return entry.Value;
				}
			}

			set
			{
				lock(_cacheEntries)
				{
					var entry = new CacheEntry(this, key, value);
					CacheEntry prev;
					if(_cacheEntries.TryGetValue(key, out prev))
						prev.Remove();
					_cacheEntries[key] = entry;
				}
			}
		}

		/// <summary>
		/// Gets an item from the cache.
		/// If the item doesn't exist, a cache miss function will be called to get the item.
		/// The item will then be stored in the cache and returned.
		/// </summary>
		/// <param name="key">Key that represents the cached item</param>
		/// <param name="missFunc">Function that will get the item if it isn't in the cache</param>
		/// <returns>Value contained in the cache</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="missFunc"/> is null</exception>
		public TValue GetItem (TKey key, CacheMiss missFunc)
		{
			if(null == missFunc)
				throw new ArgumentNullException("missFunc", "The cache miss function can't be null.");

			lock(_cacheEntries)
			{
				TValue value;
				if(TryGetValue(key, out value))
					return value; // Already exists in cache
				// Doesn't exist
				var item = missFunc(key);
				return this[key] = item;
			}
		}

		/// <summary>
		/// Collection of keys for the items contained in the cache
		/// </summary>
		public ICollection<TKey> Keys
		{
			get
			{
				lock(_cacheEntries)
					return _cacheEntries.Keys;
			}
		}

		/// <summary>
		/// Collection of values for the items contained in the cache
		/// </summary>
		public ICollection<TValue> Values
		{
			get
			{
				lock(_cacheEntries)
				{
					var list = new List<TValue>(_cacheEntries.Count);
					foreach(var item in _cacheEntries.Values)
						list.Add(item.Value);
					return list;
				}
			}
		}

		/// <summary>
		/// Gets an enumerator to iterate over the items contained in the cache
		/// </summary>
		/// <returns>An enumerator</returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator ()
		{
			lock(_cacheEntries)
			{
				var list = new List<KeyValuePair<TKey, TValue>>(_cacheEntries.Count);
				foreach(var entry in _cacheEntries)
					list.Add(new KeyValuePair<TKey, TValue>(entry.Key, entry.Value.Value));
				return list.GetEnumerator();
			}
		}

		/// <summary>
		/// Gets an enumerator to iterate over the items contained in the cache
		/// </summary>
		/// <returns>An enumerator</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Forces an item to be added to the cache
		/// </summary>
		/// <param name="item">Key and value for the item to add to the cache</param>
		/// <exception cref="ArgumentException">Thrown if an item already exists in the cache with the same key provided by <paramref name="item"/></exception>
		public void Add (KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		/// <summary>
		/// Removes all items from the cache
		/// </summary>
		public void Clear ()
		{
			lock(_cacheEntries)
			{
				var items = new CacheEntry[_cacheEntries.Count];
				_cacheEntries.Values.CopyTo(items, 0);
				for(var i = 0; i < items.Length; ++i)
					items[i].Remove();
			}
		}

		/// <summary>
		/// Checks if a item is contained in the cache
		/// </summary>
		/// <param name="item">Key and value of the item to look for in the cache</param>
		/// <returns>True if the item is cached or false if it isn't</returns>
		public bool Contains (KeyValuePair<TKey, TValue> item)
		{
			lock(_cacheEntries)
				return _cacheEntries.ContainsKey(item.Key) && _cacheEntries[item.Key].Value.Equals(item.Value);
		}

		/// <summary>
		/// Copies the contents of the cache to an array
		/// </summary>
		/// <param name="array">Array to store the cached items in</param>
		/// <param name="arrayIndex">Index to start storing items at in <paramref name="array"/></param>
		public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes an item from the cache
		/// </summary>
		/// <param name="item">Key and value of the item to remove</param>
		/// <returns>True if the item was in the cache and was removed or false if it didn't exist</returns>
		public bool Remove (KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Current number of items contained in the cache
		/// </summary>
		public int Count
		{
			get { return _size; }
		}

		/// <summary>
		/// Whether or not the cache is read-only
		/// </summary>
		/// <remarks>This value is always false</remarks>
		public bool IsReadOnly
		{
			get { return false; }
		}
		#endregion

		#region Stack and queue
		private CacheEntry StackTop
		{
			get
			{
				var top = _header.NextInStack;
				return (_header == top) ? null : top;
			}
		}

		private CacheEntry StackBottom
		{
			get
			{
				var bottom = _header.PreviousInStack;
				return (_header == bottom) ? null : bottom;
			}
		}

		private CacheEntry QueueFront
		{
			get
			{
				var front = _header.NextInQueue;
				return (_header == front) ? null : front;
			}
		}

		private CacheEntry QueueEnd
		{
			get
			{
				var end = _header.PreviousInQueue;
				return (_header == end) ? null : end;
			}
		}
		#endregion

		#region Utility methods
		private static int calculateMaxHotSize (int maxSize)
		{
			var result = (int)(HotRate * maxSize);
			return (maxSize == result) ? maxSize - 1 : result;
		}

		private void pruneStack ()
		{
			var bottom = StackBottom;
			while(null != bottom && EntryStatus.Hot != bottom.Status)
			{
				bottom.RemoveFromStack();
				if(EntryStatus.NonResident == bottom.Status)
					_cacheEntries.Remove(bottom.Key);
				bottom = StackBottom;
			}
		}
		#endregion

		/// <summary>
		/// States that cache entries can be in
		/// </summary>
		private enum EntryStatus
		{
			/// <summary>
			/// Resident LIRS, in stack, never in queue
			/// </summary>
			Hot,

			/// <summary>
			/// Resident HIRS, always in queue, sometimes in stack
			/// </summary>
			Cold,

			/// <summary>
			/// Non-resident HIRS, may be in stack, but never in queue
			/// </summary>
			NonResident
		}

		private class CacheEntry
		{
			private readonly Cache<TKey, TValue> _parent;
			private readonly TKey _key;

			private EntryStatus _status = EntryStatus.NonResident;

			/// <summary>
			/// Default entry containing a key and value
			/// </summary>
			/// <param name="parent">Parent cache</param>
			/// <param name="key">Cached item's key</param>
			/// <param name="value">Cached item's value</param>
			public CacheEntry (Cache<TKey, TValue> parent, TKey key, TValue value)
			{
				_parent = parent;
				_key    = key;
				Value   = value;
				Miss();
			}

			/// <summary>
			/// Creates the cache entry header
			/// </summary>
			/// <param name="parent">Parent cache</param>
			public CacheEntry (Cache<TKey, TValue> parent)
			{
				_parent = parent;
				_key    = default(TKey);
				Value   = null;

				PreviousInStack = this;
				NextInStack     = this;
				PreviousInQueue = this;
				NextInQueue     = this;
			}

			/// <summary>
			/// Key that represents the cached item
			/// </summary>
			public TKey Key
			{
				get { return _key; }
			}

			/// <summary>
			/// Value of the item in the cache
			/// </summary>
			public TValue Value { get; private set; }

			/// <summary>
			/// Current status of the entry
			/// </summary>
			public EntryStatus Status
			{
				get { return _status; }
			}

			/// <summary>
			/// Whether or not the entry is resident in the cache
			/// </summary>
			public bool IsResident
			{
				get { return EntryStatus.NonResident != _status; }
			}

			/// <summary>
			/// Whether or not this entry is in the stack
			/// </summary>
			private bool InStack
			{
				get { return null != NextInStack; }
			}

			/// <summary>
			/// Whether or not this entry is in the queue
			/// </summary>
			private bool InQueue
			{
				get { return null != NextInQueue; }
			}

			public CacheEntry PreviousInStack, NextInStack;
			public CacheEntry PreviousInQueue, NextInQueue;

			#region Hit
			/// <summary>
			/// Records a cache hit
			/// </summary>
			public void Hit ()
			{
				switch(_status)
				{
				case EntryStatus.Hot:
					hotHit();
					break;
				case EntryStatus.Cold:
					coldHit();
					break;
				default:
					throw new InvalidOperationException("Can't record a cache hit for a non-resident entry.");
				}
			}

			private void hotHit ()
			{
				var onBottom = (_parent.StackBottom == this);
				moveToStackTop();
				if(onBottom)
					_parent.pruneStack();
			}

			private void coldHit ()
			{
				var inStack = InStack;
				moveToStackTop();

				if(inStack)
				{
					markHot();
					RemoveFromQueue();
					_parent.StackBottom.migrateToQueue();
					_parent.pruneStack();
				}

				else
					moveToQueueEnd();
			}
			#endregion

			#region Miss
			/// <summary>
			/// Records a cache miss
			/// </summary>
			public void Miss ()
			{
				if(_parent._hotSize < _parent._maxHotSize)
					warmupMiss();
				else
					fullMiss();
				++_parent._size;
			}

			private void warmupMiss ()
			{
				markHot();
				moveToStackTop();
			}

			private void fullMiss ()
			{
				if(_parent._size >= _parent._maxSize)
					_parent.QueueFront.evict();
				var inStack = InStack;
				moveToStackTop();

				if(inStack)
				{
					markHot();
					_parent.StackBottom.migrateToQueue();
					_parent.pruneStack();
				}
				else
					markCold();
			}
			#endregion

			#region Mark status
			private void markHot ()
			{
				if(EntryStatus.Hot != _status)
					++_parent._hotSize;
				_status = EntryStatus.Hot;
			}

			private void markCold ()
			{
				if(EntryStatus.Hot == _status)
					--_parent._hotSize;
				_status = EntryStatus.Cold;
				moveToQueueEnd();
			}

			private void markNonResident ()
			{
				switch(_status)
				{
				case EntryStatus.Hot:
					--_parent._hotSize;
					--_parent._size;
					break;
				case EntryStatus.Cold:
					--_parent._size;
					break;
				}
				_status = EntryStatus.NonResident;
			}
			#endregion

			#region Stack and Queue
			private void tempRemoveFromStack ()
			{
				if(InStack)
				{
					PreviousInStack.NextInStack = NextInStack;
					NextInStack.PreviousInStack = PreviousInStack;
				}
			}

			public void RemoveFromStack ()
			{
				tempRemoveFromStack();
				PreviousInStack = null;
				NextInStack     = null;
			}

			private void addToStackBefore (CacheEntry entry)
			{
				PreviousInStack = entry.PreviousInStack;
				NextInStack     = entry;
				PreviousInStack.NextInStack = this;
				NextInStack.PreviousInStack = this;
			}

			private void moveToStackTop ()
			{
				tempRemoveFromStack();
				addToStackBefore(_parent._header.NextInStack);
			}

			private void moveToStackBottom ()
			{
				tempRemoveFromStack();
				addToStackBefore(_parent._header);
			}

			private void tempRemoveFromQueue ()
			{
				if(InQueue)
				{
					PreviousInQueue.NextInQueue = NextInQueue;
					NextInQueue.PreviousInQueue = PreviousInQueue;
				}
			}

			public void RemoveFromQueue ()
			{
				tempRemoveFromQueue();
				PreviousInQueue = null;
				NextInQueue     = null;
			}

			private void addToQueueBefore (CacheEntry entry)
			{
				PreviousInQueue = entry.PreviousInQueue;
				NextInQueue     = entry;
				PreviousInQueue.NextInQueue = this;
				NextInQueue.PreviousInQueue = this;
			}

			private void moveToQueueEnd ()
			{
				tempRemoveFromQueue();
				addToQueueBefore(_parent._header);
			}

			private void migrateToQueue ()
			{
				RemoveFromStack();
				markCold();
			}

			private void migrateToStack ()
			{
				RemoveFromQueue();
				if(!InStack)
					moveToStackBottom();
				markHot();
			}
			#endregion

			private void evict ()
			{
				RemoveFromQueue();
				RemoveFromStack();
				_parent._cacheEntries.Remove(_key);
				markNonResident();
				if(_parent._disposeEntries)
					Value.Dispose();
				Value = null;
			}

			/// <summary>
			/// Removes the entry from the cache
			/// </summary>
			public void Remove ()
			{
				var wasHot = (EntryStatus.Hot == _status);
				evict();

				if(wasHot)
				{
					var end = _parent.QueueEnd;
					if(null != end)
						end.migrateToStack();
				}
			}

			public override string  ToString()
			{
 				return _key + " = " + Value + " [" + _status + "]";
			}
		}
	}
}
