using System;
using Lapis.Blocks;

namespace Lapis.Level
{
	/// <summary>
	/// References a block within a chunk
	/// </summary>
	/// <remarks>Block references are useful for when you want to reference a location, but don't care about what's there.
	/// A block reference doesn't contain any block information.
	/// However, a block reference won't cause the chunk data to become loaded in memory.</remarks>
	public class BlockRef : IEquatable<BlockRef>, IEquatable<Block>
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
			if(ReferenceEquals(null, chunk))
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
		/// <summary>
		/// X-position of the block within the chunk
		/// </summary>
		public byte BlockX
		{
			get { return _bx; }
		}

		/// <summary>
		/// Y-position of the block within the chunk
		/// </summary>
		public byte BlockY
		{
			get { return _by; }
		}

		/// <summary>
		/// Z-position of the block within the chunk
		/// </summary>
		public byte BlockZ
		{
			get { return _bz; }
		}

		/// <summary>
		/// Global x-position of the block within the realm
		/// </summary>
		public int GlobalX
		{
			get { return _chunk.ChunkX * Level.Chunk.Size + _bx; }
		}

		/// <summary>
		/// Global z-position of the block within the realm
		/// </summary>
		public int GlobalZ
		{
			get { return _chunk.ChunkZ * Level.Chunk.Size + _bz; }
		}

		/// <summary>
		/// Chunk that the block is in
		/// </summary>
		public ChunkRef Chunk
		{
			get { return _chunk; }
		}

		/// <summary>
		/// X-position of the chunk that contains the block within the realm
		/// </summary>
		public int ChunkX
		{
			get { return _chunk.ChunkX; }
		}

		/// <summary>
		/// Z-position of the chunk that contains the block within the realm
		/// </summary>
		public int ChunkZ
		{
			get { return _chunk.ChunkZ; }
		}

		/// <summary>
		/// Realm that the block is a part of
		/// </summary>
		public RealmRef Realm
		{
			get { return _chunk.Realm; }
		}

		/// <summary>
		/// ID of the realm that the block is a part of
		/// </summary>
		public int RealmId
		{
			get { return _chunk.RealmId; }
		}

		/// <summary>
		/// World that the block belongs to
		/// </summary>
		public WorldRef World
		{
			get { return _chunk.World; }
		}

		/// <summary>
		/// Name of the world that the block belongs to
		/// </summary>
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

		#region Conversion
		/// <summary>
		/// Forces a chunk to load (if it isn't loaded already) and returns a block object from within it
		/// </summary>
		/// <param name="block">Block reference to de-reference</param>
		/// <returns>A loaded block or null if the block being referenced doesn't exist</returns>
		/// <remarks>Null will be returned if <paramref name="block"/> is null.
		/// The world, realm, and chunk that the block belongs to will also be loaded (if it hasn't been already).
		/// If the block doesn't exist, it will be generated.</remarks>
		public static implicit operator Block (BlockRef block)
		{
			if(!ReferenceEquals(null, block))
			{
				var chunk = (Chunk)block._chunk; // This cast forces the chunk to load
				return (ReferenceEquals(null, chunk)) ? null : chunk.GetBlock(block._bx, block._by, block._bz);
			}
			return null;
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Checks if two block references are equal to each other
		/// </summary>
		/// <param name="refA">First block reference</param>
		/// <param name="refB">Second block reference</param>
		/// <returns>True if the block references appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or they're from the same world, realm, and chunk and have the same coordinate.</remarks>
		public static bool operator == (BlockRef refA, BlockRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return true;
			if(ReferenceEquals(null, refA))
				return false;
			if(ReferenceEquals(null, refB))
				return false;
			return refA._bx == refB._bx && refA._by == refB._by && refA._bz == refB._bz;
		}

		/// <summary>
		/// Checks if two block references are not equal to each other
		/// </summary>
		/// <param name="refA">First block reference</param>
		/// <param name="refB">Second block reference</param>
		/// <returns>True if the block references don't appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or they're from the same world, realm, and chunk and have the same coordinate.</remarks>
		public static bool operator != (BlockRef refA, BlockRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return false;
			if(ReferenceEquals(null, refA))
				return true;
			if(ReferenceEquals(null, refB))
				return true;
			return refA._bx != refB._bx || refA._by != refB._by || refA._bz == refB._bz;
		}

		/// <summary>
		/// Checks if a block reference and block are equal
		/// </summary>
		/// <param name="blockRef">Block reference</param>
		/// <param name="block">Block</param>
		/// <returns>True if the block reference and block appear to be equal</returns>
		/// <remarks><paramref name="blockRef"/> and <paramref name="block"/> are considered equal if they're both null or contents from both blocks are the same.</remarks>
		public static bool operator == (BlockRef blockRef, Block block)
		{
			if(ReferenceEquals(null, blockRef))
				return ReferenceEquals(null, block);
			if(ReferenceEquals(null, block))
				return false;
			var b = (Block)blockRef; // Force block to load
			return block.Equals(b); // Compare contents
		}

		/// <summary>
		/// Checks if a block reference and block are not equal
		/// </summary>
		/// <param name="blockRef">Block reference</param>
		/// <param name="block">Block</param>
		/// <returns>True if the block reference and block don't appear to be equal</returns>
		/// <remarks><paramref name="blockRef"/> and <paramref name="block"/> are considered equal if they're both null or contents from both blocks are the same.</remarks>
		public static bool operator != (BlockRef blockRef, Block block)
		{
			if(ReferenceEquals(null, blockRef))
				return !ReferenceEquals(null, block);
			if(ReferenceEquals(null, block))
				return true;
			var b = (Block)blockRef; // Force block to load
			return !block.Equals(b); // Compare contents
		}

		/// <summary>
		/// Checks if a block reference and block are equal
		/// </summary>
		/// <param name="block">Block</param>
		/// <param name="blockRef">Block reference</param>
		/// <returns>True if the block reference and block appear to be equal</returns>
		/// <remarks><paramref name="blockRef"/> and <paramref name="block"/> are considered equal if they're both null or contents from both blocks are the same.</remarks>
		public static bool operator == (Block block, BlockRef blockRef)
		{
			return blockRef == block;
		}

		/// <summary>
		/// Checks if a block reference and block are not equal
		/// </summary>
		/// <param name="block">Block</param>
		/// <param name="blockRef">Block reference</param>
		/// <returns>True if the block reference and block don't appear to be equal</returns>
		/// <remarks><paramref name="blockRef"/> and <paramref name="block"/> are considered equal if they're both null or contents from both blocks are the same.</remarks>
		public static bool operator != (Block block, BlockRef blockRef)
		{
			return blockRef != block;
		}

		/// <summary>
		/// Checks if the block reference is equal to another object
		/// </summary>
		/// <param name="obj">Object to check against</param>
		/// <returns>True if <paramref name="obj"/> is considered equal to the block reference</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a BlockRef with the same world, realm, and coordinate, or a Block with the same world, realm, and coordinate.</remarks>
		public override bool Equals (object obj)
		{
			var blockRef = obj as BlockRef;
			if(!ReferenceEquals(null, blockRef))
				return this == blockRef;
			var block = obj as Block;
			if(!ReferenceEquals(null, block))
				return this == block;
			return false;
		}

		/// <summary>
		/// Compares the block reference against another block reference to check if they're equal
		/// </summary>
		/// <param name="other">Reference to compare against</param>
		/// <returns>True if the block references point to the same block</returns>
		public bool Equals (BlockRef other)
		{
			if(!ReferenceEquals(null, other))
				return this == other;
			return false;
		}

		/// <summary>
		/// Checks if the block data that the reference points to is the same
		/// </summary>
		/// <param name="other">Block to compare against</param>
		/// <returns>True if the realm reference refers to <paramref name="other"/></returns>
		public bool Equals (Block other)
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
			var hash = _chunk.GetHashCode();
			hash *= 37;
			hash ^= _bx;
			hash *= 37;
			hash ^= _by;
			hash *= 37;
			hash ^= _bz;
			return hash;
		}

		/// <summary>
		/// Gets the string representation of the world reference
		/// </summary>
		/// <returns>A string representing the world</returns>
		public override string ToString ()
		{
			return _chunk + " Block: " + _bx + ", " + _by + ", " + _bz;
		}
	}
}
