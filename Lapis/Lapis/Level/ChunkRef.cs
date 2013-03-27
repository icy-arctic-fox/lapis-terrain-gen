using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a chunk
	/// </summary>
	/// <remarks>Chunk references are useful for when you want to reference a location, but don't care about what's there.
	/// A chunk reference doesn't contain any block information.
	/// However, a chunk reference won't cause the chunk data to become loaded in memory.</remarks>
	public class ChunkRef
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
			if(null == realm)
				throw new ArgumentNullException("realm", "The realm reference can't be null.");
			_realm = realm;
			_cx    = cx;
			_cz    = cz;
		}

		#region Properties
		public int ChunkX
		{
			get { return _cx; }
		}

		public int ChunkZ
		{
			get { return _cz; }
		}

		public RealmRef Realm
		{
			get { return _realm; }
		}

		public int RealmId
		{
			get { return _realm.RealmId; }
		}

		public WorldRef World
		{
			get { return _realm.World; }
		}

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
	}
}
