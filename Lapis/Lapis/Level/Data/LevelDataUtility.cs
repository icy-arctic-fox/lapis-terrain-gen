using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lapis.Blocks;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Utility methods for manipulating level data
	/// </summary>
	public static class LevelDataUtility
	{
		/// <summary>
		/// Converts an array of block types to an array of bytes.
		/// </summary>
		/// <param name="types">Array of block types</param>
		/// <returns>Array of bytes</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null</exception>
		public static byte[] ToBytes (this BlockType[] types)
		{
			if(null == types)
				throw new ArgumentNullException("types", "The array of block types can't be null.");

			int count    = types.Length;
			byte[] bytes = new byte[count];

#if !DEBUG
			unsafe
			{
				fixed(BlockType* pSrc = types)
				fixed(byte* pDest = bytes)
				{
					BlockType* ps = pSrc;
					byte*      pd = pDest;
#if X64
					int stop = count / 8;
#else
					int stop = count / 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += 8;
						ps += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}
					stop = count % 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
						*pd = (byte)(*ps);
						++pd;
						++ps;
					}
				}
			}
#else
			for(int i = 0; i < count; ++i)
				bytes[i] = (byte)types[i];
#endif
			return bytes;
		}

		/// <summary>
		/// Converts an array of bytes to an array of block types.
		/// </summary>
		/// <param name="bytes">Array of bytes</param>
		/// <returns>Array of block types</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is null</exception>
		public static BlockType[] ToBlockTypes (this byte[] bytes)
		{
			if(null == bytes)
				throw new ArgumentNullException("bytes", "The array of bytes can't be null.");

			int count         = bytes.Length;
			BlockType[] types = new BlockType[count];

#if !DEBUG
			unsafe
			{
				fixed(byte* pSrc = bytes)
				fixed(BlockType* pDest = types)
				{
					byte*      ps = pSrc;
					BlockType* pd = pDest;
#if X64
					int stop = count / 8;
#else
					int stop = count / 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += 8;
						ps += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}
					stop = count % 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
						*pd = (BlockType)(*ps);
						++pd;
						++ps;
					}
				}
			}
#else
			for(int i = 0; i < count; ++i)
				types[i] = (BlockType)bytes[i];
#endif
			return types;
		}

		/// <summary>
		/// Converts an array of biome types to an array of bytes.
		/// </summary>
		/// <param name="types">Array of biome types</param>
		/// <returns>Array of bytes</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null</exception>
		public static byte[] ToBytes (this BiomeType[] types)
		{
			if(null == types)
				throw new ArgumentNullException("types", "The array of biome types can't be null.");

			int count    = types.Length;
			byte[] bytes = new byte[count];

#if !DEBUG
			unsafe
			{
				fixed(BiomeType* pSrc = types)
				fixed(byte* pDest = bytes)
				{
					BiomeType* ps = pSrc;
					byte*      pd = pDest;
#if X64
					int stop = count / 8;
#else
					int stop = count / 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += 8;
						ps += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}
					stop = count % 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
						*pd = (byte)*ps;
						++pd;
						++ps;
					}
				}
			}
#else
			for(int i = 0; i < count; ++i)
				bytes[i] = (byte)types[i];
#endif

			return bytes;
		}

		/// <summary>
		/// Converts an array of bytes to an array of biome types.
		/// </summary>
		/// <param name="bytes">Array of bytes</param>
		/// <returns>Array of biome types</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bytes"/> is null</exception>
		public static BiomeType[] ToBiomeTypes (this byte[] bytes)
		{
			if(null == bytes)
				throw new ArgumentNullException("bytes", "The array of bytes can't be null.");

			int count         = bytes.Length;
			BiomeType[] types = new BiomeType[count];

#if !DEBUG
			unsafe
			{
				fixed(byte* pSrc = bytes)
				fixed(BiomeType* pDest = types)
				{
					byte*      ps = pSrc;
					BiomeType* pd = pDest;
#if X64
					int stop = count / 8;
#else
					int stop = count / 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += 8;
						ps += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}
					stop = count % 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
						*pd = (BiomeType)(*ps);
						++pd;
						++ps;
					}
				}
			}
#else
			for(int i = 0; i < count; ++i)
				types[i] = (BiomeType)bytes[i];
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

			int count    = types.Length;

#if !DEBUG
			unsafe
			{
				fixed(BlockType* pDest = types)
				{
					BlockType* pd = pDest;
#if X64
					long fillData = (byte)fillType;
					int stop = count / 8;
					long data = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24) |
						(fillData << 32) | (fillData << 40) | (fillData << 48) | (fillData << 56);
#else
					int fillData = (byte)fillType;
					int stop = count / 4;
					int data = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24);
#endif
					for(int i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = data;
						pd += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = data;
						pd += 4;
					}
					stop = count % 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
						*pd = fillType;
						++pd;
					}
				}
			}
#else
			for(int i = 0; i < count; ++i)
				types[i] = fillType;
#endif
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

			int count = types.Length;

#if !DEBUG
			unsafe
			{
				fixed(BiomeType* pDest = types)
				{
					BiomeType* pd = pDest;
#if X64
					long fillData = (byte)fillType;
					int stop = count / 8;
					long data = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24) |
						(fillData << 32) | (fillData << 40) | (fillData << 48) | (fillData << 56);
#else
					int fillData = (byte)fillType;
					int stop = count / 4;
					int data = fillData | (fillData << 8) | (fillData << 16) | (fillData << 24);
#endif
					for(int i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = data;
						pd += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = data;
						pd += 4;
					}
					stop = count % 4;
#endif
					for(int i = 0; i < stop; ++i)
					{
						*pd = fillType;
						++pd;
					}
				}
			}
#else
			for(int i = 0; i < count; ++i)
				types[i] = fillType;
#endif
		}
	}
}
