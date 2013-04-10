using System;
using Lapis.Blocks;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Utility methods for manipulating level data
	/// </summary>
	public static class LevelDataUtility
	{
		#region Block types
		/// <summary>
		/// Converts an array of block types to an array of bytes
		/// </summary>
		/// <param name="types">Array of block types</param>
		/// <returns>Array of bytes</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null</exception>
		public static byte[] GetBytes (this BlockType[] types)
		{
			if(null == types)
				throw new ArgumentNullException("types", "The array of block types can't be null.");

			var count = types.Length;
			var bytes = new byte[count];

#if !DEBUG
			unsafe
			{
				fixed(BlockType* pSrc = types)
				fixed(byte* pDest = bytes)
				{
					var ps = pSrc;
					var pd = pDest;
#if X64
					var stop = count / sizeof(long);
#else
					var stop = count / sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += sizeof(long);
						ps += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = (byte)(*ps);
						++pd;
						++ps;
					}
				}
			}
#else
			for(var i = 0; i < count; ++i)
				bytes[i] = (byte)types[i];
#endif
			return bytes;
		}

		/// <summary>
		/// Converts an array of bytes to an array of block types
		/// </summary>
		/// <param name="bytes">Array of bytes</param>
		/// <returns>Array of block types</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is null</exception>
		public static BlockType[] ToBlockTypes (this byte[] bytes)
		{
			if(null == bytes)
				throw new ArgumentNullException("bytes", "The array of bytes can't be null.");

			var count = bytes.Length;
			var types = new BlockType[count];

#if !DEBUG
			unsafe
			{
				fixed(byte* pSrc = bytes)
				fixed(BlockType* pDest = types)
				{
					var ps = pSrc;
					var pd = pDest;
#if X64
					var stop = count / sizeof(long);
#else
					var stop = count / sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += sizeof(long);
						ps += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = *((int*)ps);
						pd += sizeof(int);
						ps += sizeof(int);
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = (BlockType)(*ps);
						++pd;
						++ps;
					}
				}
			}
#else
			for(var i = 0; i < count; ++i)
				types[i] = (BlockType)bytes[i];
#endif
			return types;
		}

		/// <summary>
		/// Fills an array of block type data with one type
		/// </summary>
		/// <param name="types">Array of block type data to fill</param>
		/// <param name="fillType">Block type to fill the array with</param>
		public static void Fill (this BlockType[] types, BlockType fillType)
		{
			if(null == types)
				throw new ArgumentNullException("types", "The array of block types can't be null.");

			var count = types.Length;

#if !DEBUG
			unsafe
			{
				fixed(BlockType* pDest = types)
				{
					var pd = pDest;
#if X64
					long fillData = (byte)fillType;
					var stop      = count / sizeof(long);
					long data     = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24) |
						(fillData << 32) | (fillData << 40) | (fillData << 48) | (fillData << 56);
#else
					int fillData = (byte)fillType;
					var stop     = count / sizeof(int);
					var data     = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = data;
						pd += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = data;
						pd += sizeof(int);
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = fillType;
						++pd;
					}
				}
			}
#else
			for(var i = 0; i < count; ++i)
				types[i] = fillType;
#endif
		}
		#endregion

		#region Biome types
		/// <summary>
		/// Converts an array of biome types to an array of bytes
		/// </summary>
		/// <param name="types">Array of biome types</param>
		/// <returns>Array of bytes</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null</exception>
		public static byte[] GetBytes (this BiomeType[] types)
		{
			if(null == types)
				throw new ArgumentNullException("types", "The array of biome types can't be null.");

			var count = types.Length;
			var bytes = new byte[count];

#if !DEBUG
			unsafe
			{
				fixed(BiomeType* pSrc = types)
				fixed(byte* pDest = bytes)
				{
					var ps = pSrc;
					var pd = pDest;
#if X64
					var stop = count / sizeof(long);
#else
					var stop = count / sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += sizeof(long);
						ps += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = *((int*)ps);
						pd += sizeof(int);
						ps += sizeof(int);
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = (byte)*ps;
						++pd;
						++ps;
					}
				}
			}
#else
			for(var i = 0; i < count; ++i)
				bytes[i] = (byte)types[i];
#endif

			return bytes;
		}

		/// <summary>
		/// Converts an array of bytes to an array of biome types
		/// </summary>
		/// <param name="bytes">Array of bytes</param>
		/// <returns>Array of biome types</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is null</exception>
		public static BiomeType[] ToBiomeTypes (this byte[] bytes)
		{
			if(null == bytes)
				throw new ArgumentNullException("bytes", "The array of bytes can't be null.");

			var count = bytes.Length;
			var types = new BiomeType[count];

#if !DEBUG
			unsafe
			{
				fixed(byte* pSrc = bytes)
				fixed(BiomeType* pDest = types)
				{
					var ps = pSrc;
					var pd = pDest;
#if X64
					var stop = count / sizeof(long);
#else
					var stop = count / sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += sizeof(long);
						ps += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = *((int*)ps);
						pd += sizeof(int);
						ps += sizeof(int);
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = (BiomeType)(*ps);
						++pd;
						++ps;
					}
				}
			}
#else
			for(var i = 0; i < count; ++i)
				types[i] = (BiomeType)bytes[i];
#endif

			return types;
		}

		/// <summary>
		/// Fills an array of biome data with one type
		/// </summary>
		/// <param name="types">Array of biome data to fill</param>
		/// <param name="fillType">Biome type to fill the array with</param>
		public static void Fill (this BiomeType[] types, BiomeType fillType)
		{
			if(null == types)
				throw new ArgumentNullException("types", "The array of biome types can't be null.");

			var count = types.Length;

#if !DEBUG
			unsafe
			{
				fixed(BiomeType* pDest = types)
				{
					var pd = pDest;
#if X64
					long fillData = (byte)fillType;
					var stop      = count / sizeof(long);
					long data     = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24) |
						(fillData << 32) | (fillData << 40) | (fillData << 48) | (fillData << 56);
#else
					int fillData = (byte)fillType;
					var stop     = count / sizeof(int);
					var data     = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24);
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = data;
						pd += sizeof(long);
					}
					stop = count % sizeof(long);
#else
						*((int*)pd) = data;
						pd += sizeof(int);
					}
					stop = count % sizeof(int);
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = fillType;
						++pd;
					}
				}
			}
#else
			for(var i = 0; i < count; ++i)
				types[i] = fillType;
#endif
		}
		#endregion

		/// <summary>
		/// Finds the highest non-air block in a column
		/// </summary>
		/// <param name="column">Column of blocks</param>
		/// <returns>The y-value of the highest non-air block</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="column"/> is null</exception>
		public static int FindHighestBlock (this BlockType[] column)
		{
			if(null == column)
				throw new ArgumentNullException("column", "The column of block types can't be null.");

			for(var i = column.Length - 1; i > 0; --i)
				if(BlockType.Air != column[i])
					return i; // TODO: This could be optimized
			return 0;
		}

		/// <summary>
		/// Converts a local block coordinate to a global coordinate
		/// </summary>
		/// <param name="cx">X-position of the containing chunk</param>
		/// <param name="cz">Z-position of the containing chunk</param>
		/// <param name="bx">X-position inside the chunk</param>
		/// <param name="bz">Z-position inside the chunk</param>
		/// <param name="gx">X-position of the global block coordinate</param>
		/// <param name="gz">Z-position of the global block coordinate</param>
		public static void LocalToGlobalXZ (int cx, int cz, int bx, int bz, out int gx, out int gz)
		{
			gx = cx * Chunk.Size + bx;
			gz = cz * Chunk.Size + bz;
		}
	}
}
