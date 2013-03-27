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
		/// <param name="chunk">Reference to the chunk that the block is in</param>
		/// <param name="bx">Block x-position</param>
		/// <param name="by">Block y-position</param>
		/// <param name="bz">Block z-position</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="chunk"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bx"/> or <paramref name="bz"/> are outside the bounds of the chunk</exception>
		internal BlockRef (ChunkRef chunk, byte bx, byte by, byte bz)
		{
			if(null == chunk)
				throw new ArgumentNullException("chunk", "The chunk reference can't be null.");
			if(Level.Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The block x-position must be inside the bounds of the chunk.");
			if(Level.Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The block z-position must be inside the bounds of the chunk.");

			_chunk = chunk;
			_bx    = bx;
			_by    = by;
			_bz    = bz;
		}

		#region Properties
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
		#endregion


		#region Neighbors
		/// <summary>
		/// Gets the block neighboring to the north
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, y + 0, z - 1)</remarks>
		public BlockRef North
		{
			get { return GetRelativeBlock(0, 0, -1); }
		}

		/// <summary>
		/// Gets the block neighboring to the south
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, y + 0, z + 1)</remarks>
		public BlockRef South
		{
			get { return GetRelativeBlock(0, 0, +1); }
		}

		/// <summary>
		/// Gets the block neighboring to the east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, y + 0, z + 0)</remarks>
		public BlockRef East
		{
			get { return GetRelativeBlock(+1, 0, 0); }
		}

		/// <summary>
		/// Gets the block neighboring to the west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, y + 0, z + 0)</remarks>
		public BlockRef West
		{
			get { return GetRelativeBlock(-1, 0, 0); }
		}

		/// <summary>
		/// Gets the block neighboring above
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, y + 1, z + 0)</remarks>
		public BlockRef Above
		{
			get { return GetRelativeBlock(0, +1, 0); }
		}

		/// <summary>
		/// Gets the block neighboring below
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, y - 0, z + 0)</remarks>
		public BlockRef Below
		{
			get { return GetRelativeBlock(0, -1, 0); }
		}

		/// <summary>
		/// Gets a block relative to the current one
		/// </summary>
		/// <param name="bxOff">Block x-offset</param>
		/// <param name="byOff">Block y-offset</param>
		/// <param name="bzOff">Block z-offset</param>
		/// <returns>A block reference or null if the block is outside the y-bounds</returns>
		/// <remarks>The offset can go outside the bounds of the chunk.</remarks>
		public BlockRef GetRelativeBlock (int bxOff, int byOff, int bzOff)
		{
			if(0 == bxOff && 0 == byOff && 0 == bzOff)
				return this; // Don't create another object

			var by = _by + byOff;
			if(by >= 0 && by < Level.Chunk.Height)
			{// Y-position is valid
				var xRel  = _bx + bxOff;
				var zRel  = _bz + bzOff;
				var bx    = xRel % Level.Chunk.Size;
				var bz    = zRel % Level.Chunk.Size;
				var cxRel = xRel / Level.Chunk.Size;
				var czRel = zRel / Level.Chunk.Size;
				if(bx < 0)
					bx += Level.Chunk.Size;
				if(bz < 0)
					bz += Level.Chunk.Size;
				if(xRel < 0)
					--cxRel;
				if(zRel < 0)
					--czRel;
				var chunk = _chunk.GetRelativeChunk(cxRel, czRel);
				return new BlockRef(chunk, (byte)bx, (byte)by, (byte)bz);
			}
			return null; // Y-position is invalid
		}
		#endregion
	}
}
