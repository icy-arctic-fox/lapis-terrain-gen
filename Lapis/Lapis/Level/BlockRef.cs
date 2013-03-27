using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a block within a chunk
	/// </summary>
	/// <remarks>Block references are useful for when you want to reference a location, but don't care about what's there.
	/// A block reference doesn't contain any block information.
	/// However, a block reference won't cause the chunk data to become loaded in memory.</remarks>
	public class BlockRef
	{
		private readonly byte _bx, _by, _bz;
		private readonly ChunkRef _chunk;

		/// <summary>
		/// Creates a new block reference
		/// </summary>
		/// <param name="chunk"></param>
		/// <param name="bx"></param>
		/// <param name="by"></param>
		/// <param name="bz"></param>
		internal BlockRef (ChunkRef chunk, byte bx, byte by, byte bz)
		{
			if(null == chunk)
				throw new ArgumentNullException("chunk", "The chunk reference can't be null.");
		}

		public byte BlockX
		{
			get { return _bx; }
		}

		public byte BlockY
		{
			get { return _by; }
		}

		public byte BlockZ
		{
			get { return _bz; }
		}

		public int GlobalX
		{
			get { throw new NotImplementedException(); }
		}

		public int GlobalZ
		{
			get { throw new NotImplementedException(); }
		}

		public ChunkRef Chunk
		{
			get { return _chunk; }
		}

		public int ChunkX
		{
			get { return _chunk.ChunkX; }
		}

		public int ChunkZ
		{
			get { return _chunk.ChunkZ; }
		}

		public RealmRef Realm
		{
			get { return _chunk.Realm; }
		}

		public int RealmId
		{
			get { return _chunk.RealmId; }
		}

		public WorldRef World
		{
			get { return _chunk.World; }
		}

		public string WorldName
		{
			get { return _chunk.WorldName; }
		}
	}
}
