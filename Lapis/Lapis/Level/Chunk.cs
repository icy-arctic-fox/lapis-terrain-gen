using System;
using Lapis.Blocks;
using Lapis.Level.Data;

namespace Lapis.Level
{
	/// <summary>
	/// A manageable collection of blocks within a realm
	/// </summary>
	public sealed class Chunk : IDisposable
	{
		#region Constants
		/// <summary>
		/// Number of blocks in size of a chunk section
		/// </summary>
		public const int SectionHeight = 16;

		/// <summary>
		/// Number of sections that are stacked to make one chunk column
		/// </summary>
		public const int SectionCount = 16;

		/// <summary>
		/// Number of blocks high a chunk is
		/// </summary>
		public const int Height = SectionHeight * SectionCount;

		/// <summary>
		/// Length and width of a chunk (in blocks)
		/// </summary>
		public const int Size = 16;

		/// <summary>
		/// Value for a fully lit block
		/// </summary>
		public const byte FullBrightness = 15;

		/// <summary>
		/// Value for a completely dark block
		/// </summary>
		public const byte NoBrightness = 0;
		#endregion

		private readonly Realm _realm;
		private readonly ChunkData _data;

		/// <summary>
		/// Creates a chunk from chunk data
		/// </summary>
		/// <param name="realm">Realm that the chunk belongs to</param>
		/// <param name="data">Chunk data</param>
		internal Chunk (Realm realm, ChunkData data)
		{
			_realm = realm;
			_data  = data;
		}

		#region Properties
		/// <summary>
		/// World that the chunk is in
		/// </summary>
		public World World
		{
			get { return _realm.World; }
		}

		/// <summary>
		/// Realm that the chunk is in
		/// </summary>
		public Realm Realm
		{
			get { return _realm; }
		}

		/// <summary>
		/// Block information from the chunk
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Block information</returns>
		public Block this[byte bx, byte by, byte bz]
		{
			get { return GetBlock(bx, by, bz); }
			set { SetBlock(bx, by, bz, value); }
		}

		/// <summary>
		/// X-position of the chunk within the realm
		/// </summary>
		public int ChunkX
		{
			get { return _data.ChunkX; }
		}

		/// <summary>
		/// Z-position of the chunk within the realm
		/// </summary>
		public int ChunkZ
		{
			get { return _data.ChunkZ; }
		}
		#endregion

		/// <summary>
		/// Gets a reference to a block within the chunk
		/// </summary>
		/// <param name="bx">X-position of the block within the chunk</param>
		/// <param name="by">Y-position of the block within the chunk</param>
		/// <param name="bz">Z-position of the block within the chunk</param>
		/// <returns>A block reference</returns>
		/// <remarks>Block references are useful for when you want to reference a location, but don't care about what's there.
		/// A block reference doesn't contain any block information.
		/// However, a block reference won't cause the chunk data to become loaded in memory.</remarks>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bx"/> or <paramref name="bz"/> are outside the bounds of the chunk</exception>
		public BlockRef GetBlockReference (byte bx, byte by, byte bz)
		{
			return new BlockRef(this, bx, by, bz);
		}

		#region Chunk data
		#region Blocks
		// TODO: When implementing block tiles (blocks that require additional NBT data), use an interface/abstract class and expose a method to retrieve that data

		/// <summary>
		/// Gets the values of a block at the given coordinates in the chunk
		/// </summary>
		/// <param name="bx">X-position of the block within the chunk</param>
		/// <param name="by">Y-position of the block within the chunk</param>
		/// <param name="bz">Z-position of the block within the chunk</param>
		/// <returns>Block information</returns>
		/// <remarks>The block information is cloned from the position within the chunk.
		/// This means that editing the block information (it should be read-only anyways) will have no effect on the chunk.
		/// To update the chunk, use SetBlock().</remarks>
		public Block GetBlock (byte bx, byte by, byte bz)
		{
			BlockType type;
			byte data;
			lock(this)
			{// TODO: This could be faster
				type = _data.GetBlockType(bx, by, bz);
				data = _data.GetBlockData(bx, by, bz);
			}
			return Block.Create(type, data);
		}

		/// <summary>
		/// Sets the values of a block at the given coordinates in the chunk
		/// </summary>
		/// <param name="bx">X-position of the block within the chunk</param>
		/// <param name="by">Y-position of the block within the chunk</param>
		/// <param name="bz">Z-position of the block within the chunk</param>
		/// <param name="block">Block information to store</param>
		/// <remarks>If you need to get and set multiple blocks and there concurrent threads, lock the chunk object.
		/// This class guarantees that blocks will not get corrupt when multiple threads call SetBlock simultaneously,
		/// but it cannot guarantee that nothing will happen to the state between a GetBlock() and SetBlock() call.</remarks>
		public void SetBlock (byte bx, byte by, byte bz, Block block)
		{
			var blockInfo = new BlockInformation(block.Type, block.Data, 0, block.Luminance);
			lock(this)
			{
				_data.SetBlock(bx, by, bz, blockInfo);
				updateHeightMap(bx, bz);
			}
		}

		/// <summary>
		/// Sets the values of a block at the given coordinates in the chunk
		/// </summary>
		/// <param name="bx">X-position of the block within the chunk</param>
		/// <param name="by">Y-position of the block within the chunk</param>
		/// <param name="bz">Z-position of the block within the chunk</param>
		/// <param name="block">Block information to store</param>
		/// <remarks>If you need to get and set multiple blocks and there concurrent threads, lock the chunk object.
		/// This class guarantees that blocks will not get corrupt when multiple threads call SetBlock simultaneously,
		/// but it cannot guarantee that nothing will happen to the state between a GetBlock() and SetBlock() call.</remarks>
		public void SetBlock (byte bx, byte by, byte bz, BlockInformation block)
		{
			lock(this)
			{
				_data.SetBlock(bx, by, bz, block);
				updateHeightMap(bx, bz);
			}
		}
		#endregion

		#region Sky light
		/// <summary>
		/// Gets the amount of sky light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Amount of sky light from 0 to 15</returns>
		public byte GetSkyLight (byte bx, byte by, byte bz)
		{
			lock(this)
				return _data.GetSkyLight(bx, by, bz);
		}

		/// <summary>
		/// Sets the amount of sky light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="amount">Amount of sky light from 0 to 15</param>
		public void SetSkyLight (byte bx, byte by, byte bz, byte amount)
		{
			lock(this)
				_data.SetSkyLight(bx, by, bz, amount);
		}

		/// <summary>
		/// Adds some sky light to a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="amount">Amount of sky light to add</param>
		/// <remarks>This method reduces the number of calls for updating lighting - it is ideal for fading light values.
		/// The sky light value will be capped at maximum brightness so it won't overflow (wrap around).</remarks>
		public void AddSkyLight (byte bx, byte by, byte bz, byte amount)
		{
			lock(this)
			{
				var light = _data.GetSkyLight(bx, by, bz);
				light = (byte)Math.Min(light + amount, FullBrightness);
				_data.SetSkyLight(bx, by, bz, light);
			}
		}
		#endregion

		#region Block light
		/// <summary>
		/// Gets the amount of block light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Amount of block light from 0 to 15</returns>
		public byte GetBlockLight (byte bx, byte by, byte bz)
		{
			lock(this)
				return _data.GetBlockLight(bx, by, bz);
		}

		/// <summary>
		/// Sets the amount of block light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="amount">Amount of block light from 0 to 15</param>
		public void SetBlockLight (byte bx, byte by, byte bz, byte amount)
		{
			lock(this)
				_data.SetBlockLight(bx, by, bz, amount);
		}

		/// <summary>
		/// Adds some block light to a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="amount">Amount of block light to add</param>
		/// <remarks>This method reduces the number of calls for updating lighting - it is ideal for fading light values.
		/// The block light value will be capped at maximum brightness so it won't overflow (wrap around).</remarks>
		public void AddBlockLight (byte bx, byte by, byte bz, byte amount)
		{
			lock(this)
			{
				var light = _data.GetBlockLight(bx, by, bz);
				light = (byte)Math.Min(light + amount, FullBrightness);
				_data.SetBlockLight(bx, by, bz, light);
			}
		}
		#endregion

		/// <summary>
		/// Gets the highest block at a position within the chunk
		/// </summary>
		/// <param name="bx">X-position</param>
		/// <param name="bz">Z-position</param>
		/// <returns>The y-position of the highest block</returns>
		/// <remarks>The lowest value returned by this method is 0.
		/// This method gets the highest non-air block in a column.
		/// If a grass block is the highest at y=64, then the value returned will be 64.</remarks>
		public int GetHighestBlockAt (byte bx, byte bz)
		{
			lock(this)
				return _data.HeightMap[bx, bz];
		}

		private void updateHeightMap (byte bx, byte bz)
		{
			int y;
			for(y = Height - 1; y > 0; --y)
				if(_data.GetBlockType(bx, (byte)y, bz) != BlockType.Air)
					break;
			_data.HeightMap[bx, bz] = y;
		}

		/// <summary>
		/// Creates a snapshot of the chunk that contains all of the block information within the chunk
		/// </summary>
		/// <returns>A copy of the underlying block chunk data</returns>
		/// <remarks>A snapshot is useful for getting the full contents of a chunk without affecting it.
		/// This can also be useful in multi-threaded situations.</remarks>
		public ChunkData GetSnapshot ()
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Neighbors
		/// <summary>
		/// Gets the chunk neighboring to the north
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, z - 1)</remarks>
		public Chunk North
		{
			get { return GetRelativeChunk(0, -1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, z + 1)</remarks>
		public Chunk South
		{
			get { return GetRelativeChunk(0, +1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z + 0)</remarks>
		public Chunk East
		{
			get { return GetRelativeChunk(+1, 0); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z + 0)</remarks>
		public Chunk West
		{
			get { return GetRelativeChunk(-1, 0); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the north-east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z - 1)</remarks>
		public Chunk NorthEast
		{
			get { return GetRelativeChunk(+1, -1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the north-west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z - 1)</remarks>
		public Chunk NorthWest
		{
			get { return GetRelativeChunk(-1, -1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south-east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z + 1)</remarks>
		public Chunk SouthEast
		{
			get { return GetRelativeChunk(+1, +1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south-west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z + 1)</remarks>
		public Chunk SouthWest
		{
			get { return GetRelativeChunk(-1, +1); }
		}

		/// <summary>
		/// Gets a chunk relative to the current one
		/// </summary>
		/// <param name="cxOff">Chunk x-offset</param>
		/// <param name="czOff">Chunk z-offset</param>
		/// <returns>A chunk</returns>
		/// <remarks>The chunk returned might not be populated.</remarks>
		public Chunk GetRelativeChunk (int cxOff, int czOff)
		{
			return _realm.GetChunk(ChunkX + cxOff, ChunkZ + czOff);
		}
		#endregion

		/// <summary>
		/// Forces the chunk to save its contents to disk
		/// </summary>
		public void Save ()
		{
			lock(this)
				_realm.SaveChunk(ChunkX, ChunkZ, _data);
		}

		#region Cleanup
		/// <summary>
		/// Disposes the chunk and removes its contents from memory
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Finalizer - simply calls Dispose()
		/// </summary>
		~Chunk ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes the chunk and removes its contents from memory
		/// </summary>
		/// <param name="disposing">True if we should clean up all of our own resources (code called Dispose) or false if we should only cleanup ourselves (GC called Dispose)</param>
		private void Dispose (bool disposing)
		{
			// It appears that _data is always alive when entering this method.
			// TODO: The chunk should be saved when entering this method (and if there were changes).
			if(!disposing)
				_realm.FreeChunk(ChunkX, ChunkZ);
		}
		#endregion
	}
}
