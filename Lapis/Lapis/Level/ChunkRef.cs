using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a chunk
	/// </summary>
	/// <remarks>Chunk references are useful for when you want to reference a location, but don't care about what's there.
	/// A chunk reference doesn't contain any block information.
	/// However, a chunk reference won't cause the chunk data to become loaded in memory.</remarks>
	public class ChunkRef : IEquatable<ChunkRef>, IEquatable<Chunk>
	{
		private readonly int _cx, _cz;
		private readonly RealmRef _realm;

		/// <summary>
		/// Creates a new chunk reference
		/// </summary>
		/// <param name="realm">Realm that the chunk belongs to</param>
		/// <param name="cx">Chunk x-position</param>
		/// <param name="cz">Chunk z-position</param>
		internal ChunkRef (RealmRef realm, int cx, int cz)
		{
			if(ReferenceEquals(null, realm))
				throw new ArgumentNullException("realm", "The realm reference can't be null.");
			_realm = realm;
			_cx    = cx;
			_cz    = cz;
		}

		#region Properties
		/// <summary>
		/// X-position of the chunk within the realm
		/// </summary>
		public int ChunkX
		{
			get { return _cx; }
		}

		/// <summary>
		/// Z-position of the chunk within the realm
		/// </summary>
		public int ChunkZ
		{
			get { return _cz; }
		}

		/// <summary>
		/// Realm that the chunk belongs to
		/// </summary>
		public RealmRef Realm
		{
			get { return _realm; }
		}

		/// <summary>
		/// ID of the realm that the chunk belongs to
		/// </summary>
		public int RealmId
		{
			get { return _realm.RealmId; }
		}

		/// <summary>
		/// Worlds that the chunk is in
		/// </summary>
		public WorldRef World
		{
			get { return _realm.World; }
		}

		/// <summary>
		/// Name of the world that the chunk is in
		/// </summary>
		public string WorldName
		{
			get { return _realm.WorldName; }
		}
		#endregion

		#region Neighbors
		/// <summary>
		/// Gets the chunk neighboring to the north
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, z - 1)</remarks>
		public ChunkRef North
		{
			get { return GetRelativeChunk(0, -1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, z + 1)</remarks>
		public ChunkRef South
		{
			get { return GetRelativeChunk(0, +1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z + 0)</remarks>
		public ChunkRef East
		{
			get { return GetRelativeChunk(+1, 0); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z + 0)</remarks>
		public ChunkRef West
		{
			get { return GetRelativeChunk(-1, 0); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the north-east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z - 1)</remarks>
		public ChunkRef NorthEast
		{
			get { return GetRelativeChunk(+1, -1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the north-west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z - 1)</remarks>
		public ChunkRef NorthWest
		{
			get { return GetRelativeChunk(-1, -1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south-east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z + 1)</remarks>
		public ChunkRef SouthEast
		{
			get { return GetRelativeChunk(+1, +1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south-west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z + 1)</remarks>
		public ChunkRef SouthWest
		{
			get { return GetRelativeChunk(-1, +1); }
		}

		/// <summary>
		/// Gets a chunk relative to the current one
		/// </summary>
		/// <param name="cxOff">Chunk x-offset</param>
		/// <param name="czOff">Chunk z-offset</param>
		/// <returns>A chunk reference</returns>
		public ChunkRef GetRelativeChunk(int cxOff, int czOff)
		{
			if(0 == cxOff && 0 == czOff)
				return this; // Don't create another object
			var cx = _cx + cxOff;
			var cz = _cz + czOff;
			return new ChunkRef(_realm, cx, cz);
		}
		#endregion

		#region Conversion
		/// <summary>
		/// Forces a chunk to load (if it isn't loaded already) and returns the chunk object
		/// </summary>
		/// <param name="chunk">Chunk reference to de-reference</param>
		/// <returns>A loaded chunk or null if the chunk being referenced doesn't exist</returns>
		/// <remarks>Null will be returned if <paramref name="chunk"/> is null.
		/// The world and realm that the chunk belongs to will also be loaded (if it hasn't been already).
		/// If the chunk doesn't exist, it will be generated.</remarks>
		public static implicit operator Chunk (ChunkRef chunk)
		{
			if(!ReferenceEquals(null, chunk))
			{
				var realm = (Realm)chunk._realm; // This cast forces the realm to load
				return (ReferenceEquals(null, realm)) ? null : realm.GetChunk(chunk._cx, chunk._cz);
			}
			return null;
		}

		/// <summary>
		/// Creates a reference to a chunk from a loaded chunk
		/// </summary>
		/// <param name="chunk">Chunk to get a reference for</param>
		/// <returns>A reference to the chunk</returns>
		/// <remarks>Null will be returned if <paramref name="chunk"/> is null</remarks>
		public static implicit operator ChunkRef (Chunk chunk)
		{
			return (ReferenceEquals(null, chunk)) ? null : new ChunkRef(chunk.Realm, chunk.ChunkX, chunk.ChunkZ);
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Checks if two chunk references are equal to each other
		/// </summary>
		/// <param name="refA">First chunk reference</param>
		/// <param name="refB">Second chunk reference</param>
		/// <returns>True if the chunk references appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or they're from the same realm and have the same coordinate.</remarks>
		public static bool operator == (ChunkRef refA, ChunkRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return true;
			if(ReferenceEquals(null, refA))
				return false;
			if(ReferenceEquals(null, refB))
				return false;
			return refA._cx == refB._cx && refA._cz == refB._cz;
		}

		/// <summary>
		/// Checks if two chunk references are not equal to each other
		/// </summary>
		/// <param name="refA">First chunk reference</param>
		/// <param name="refB">Second chunk reference</param>
		/// <returns>True if the chunk references don't appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or they're from the same realm and have the same coordinate.</remarks>
		public static bool operator != (ChunkRef refA, ChunkRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return false;
			if(ReferenceEquals(null, refA))
				return true;
			if(ReferenceEquals(null, refB))
				return true;
			return refA._cx != refB._cx || refA._cz != refB._cz;
		}

		/// <summary>
		/// Checks if a chunk reference and chunk are equal
		/// </summary>
		/// <param name="chunkRef">Chunk reference</param>
		/// <param name="chunk">Chunk</param>
		/// <returns>True if the chunk reference and chunk appear to be equal</returns>
		/// <remarks><paramref name="chunkRef"/> and <paramref name="chunk"/> are considered equal if they're both null or they're from the same realm and have the same coordinate.</remarks>
		public static bool operator == (ChunkRef chunkRef, Chunk chunk)
		{
			if(ReferenceEquals(null, chunkRef))
				return ReferenceEquals(null, chunk);
			if(ReferenceEquals(null, chunk))
				return false;
			return chunkRef._cx == chunk.ChunkX && chunkRef._cz == chunk.ChunkZ;
		}

		/// <summary>
		/// Checks if a chunk reference and chunk are not equal
		/// </summary>
		/// <param name="chunkRef">Chunk reference</param>
		/// <param name="chunk">Chunk</param>
		/// <returns>True if the chunk reference and chunk don't appear to be equal</returns>
		/// <remarks><paramref name="chunkRef"/> and <paramref name="chunk"/> are considered equal if they're both null or they're from the same realm and have the same coordinate.</remarks>
		public static bool operator != (ChunkRef chunkRef, Chunk chunk)
		{
			if(ReferenceEquals(null, chunkRef))
				return !ReferenceEquals(null, chunk);
			if(ReferenceEquals(null, chunk))
				return true;
			return chunkRef._cx != chunk.ChunkX || chunkRef._cz != chunk.ChunkZ;
		}

		/// <summary>
		/// Checks if a chunk reference and chunk are equal
		/// </summary>
		/// <param name="chunk">Chunk</param>
		/// <param name="chunkRef">Chunk reference</param>
		/// <returns>True if the chunk reference and chunk appear to be equal</returns>
		/// <remarks><paramref name="chunkRef"/> and <paramref name="chunk"/> are considered equal if they're both null or they're from the same realm and have the same coordinate.</remarks>
		public static bool operator == (Chunk chunk, ChunkRef chunkRef)
		{
			return chunkRef == chunk;
		}

		/// <summary>
		/// Checks if a chunk reference and chunk are not equal
		/// </summary>
		/// <param name="chunk">Chunk</param>
		/// <param name="chunkRef">Chunk reference</param>
		/// <returns>True if the chunk reference and chunk don't appear to be equal</returns>
		/// <remarks><paramref name="chunkRef"/> and <paramref name="chunk"/> are considered equal if they're both null or they're from the same realm and have the same coordinate.</remarks>
		public static bool operator != (Chunk chunk, ChunkRef chunkRef)
		{
			return chunkRef != chunk;
		}

		/// <summary>
		/// Checks if the chunk reference is equal to another object
		/// </summary>
		/// <param name="obj">Object to check against</param>
		/// <returns>True if <paramref name="obj"/> is considered equal to the chunk reference</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a ChunkRef with the same world, realm, and coordinate, or a Chunk with the same world, realm, and coordinate.</remarks>
		public override bool Equals (object obj)
		{
			var chunkRef = obj as ChunkRef;
			if(!ReferenceEquals(null, chunkRef))
				return this == chunkRef;
			var chunk = obj as Chunk;
			if(!ReferenceEquals(null, chunk))
				return this == chunk;
			return false;
		}

		/// <summary>
		/// Compares the chunk reference against another chunk reference to check if they're equal
		/// </summary>
		/// <param name="other">Reference to compare against</param>
		/// <returns>True if the chunk references point to the same chunk</returns>
		public bool Equals (ChunkRef other)
		{
			if(!ReferenceEquals(null, other))
				return this == other;
			return false;
		}

		/// <summary>
		/// Checks if the chunk reference points to a chunk
		/// </summary>
		/// <param name="other">Chunk to compare against</param>
		/// <returns>True if the chunk reference refers to <paramref name="other"/></returns>
		public bool Equals (Chunk other)
		{
			if(!ReferenceEquals(null, other))
				return this == other;
			return false;
		}
		#endregion

		/// <summary>
		/// Generates a hash code from the object
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = _realm.GetHashCode();
			hash *= 37;
			hash ^= _cx;
			hash *= 37;
			hash ^= _cz;
			return hash;
		}

		/// <summary>
		/// Gets the string representation of the world reference
		/// </summary>
		/// <returns>A string representing the world</returns>
		public override string ToString ()
		{
			return _realm + " Chunk: " + _cx + ", " + _cz;
		}
	}
}
