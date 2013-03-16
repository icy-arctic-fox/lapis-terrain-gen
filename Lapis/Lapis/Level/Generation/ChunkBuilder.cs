using System;
using Lapis.Blocks;
using Lapis.Level.Data;
using Lapis.Utility;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Helper class for constructing chunk data
	/// </summary>
	/// <remarks>Use the methods in the class to generate a chunk.
	/// When finished, call GetChunkData() to get the constructed chunk data.</remarks>
	public class ChunkBuilder : IDisposable
	{
		private readonly ChunkData _data;

		/// <summary>
		/// Creates a new chunk builder
		/// </summary>
		/// <param name="cx">X-position of the chunk coordinate</param>
		/// <param name="cz">Z-position of the chunk coordinate</param>
		public ChunkBuilder (int cx, int cz)
		{
			_data = new ChunkData(cx, cz);
		}

		/// <summary>
		/// Retrieves the constructed chunk data
		/// </summary>
		/// <returns>Chunk data</returns>
		public ChunkData GetChunkData ()
		{
			return _data;
		}

		#region Builder methods
		/// <summary>
		/// Fills a rectangular region within the chunk
		/// </summary>
		/// <param name="bx">Position to start at along the x-axis</param>
		/// <param name="by">Position to start at along the y-axis</param>
		/// <param name="bz">Position to start at along the z-axis</param>
		/// <param name="xCount">Number of blocks along the x-axis to fill</param>
		/// <param name="yCount">Number of blocks along the y-axis to fill</param>
		/// <param name="zCount">Number of blocks along the z-axis to fill</param>
		/// <param name="type">Block type to fill the region with</param>
		/// <remarks>The chunk height map will be updated after filling blocks.</remarks>
		public void FillType (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, BlockType type)
		{
			fillBlock(bx, by, bz, xCount, yCount, zCount, _data.BlockTypes, type);
			var reduce = BlockType.Air == type; // Move the height map down if we're filling with air
			updateHeightMap(_data.HeightMap, bx, by, bz, xCount, yCount, zCount, reduce);
		}

		/// <summary>
		/// Fills a single column within a chunk (1x1x256)
		/// </summary>
		/// <param name="bx">X-position of the column</param>
		/// <param name="bz">Z-position of the column</param>
		/// <param name="type">Block type to fill the region with</param>
		/// <param name="yOff">Y-offset to start at</param>
		public void FillColumn (byte bx, byte bz, BlockType type, byte yOff = 0)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fills a single column within a chunk (1x1x256)
		/// </summary>
		/// <param name="bx">X-position of the column</param>
		/// <param name="bz">Z-position of the column</param>
		/// <param name="type">Block type to fill the region with</param>
		/// <param name="yOff">Y-offset to start at</param>
		/// <param name="yCount">Number of blocks to fill</param>
		public void FillColumn (byte bx, byte bz, BlockType type, byte yOff, byte yCount)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Fills a single column within a chunk (1x1x256)
		/// </summary>
		/// <param name="bx">X-position of the column</param>
		/// <param name="bz">Z-position of the column</param>
		/// <param name="types">Array of types to fill the column with</param>
		/// <param name="yOff">Y-offset to start at</param>
		/// <param name="repeat">Whether or not to repeat the contents of <paramref name="types"/></param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="types"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the length of <paramref name="types"/> is less than 1</exception>
		public void FillColumn (byte bx, byte bz, BlockType[] types, byte yOff = 0, bool repeat = false)
		{
			checkBounds(bx, bz);
			if(null == types)
				throw new ArgumentNullException("types", "The array of block types can't be null.");
			if(0 >= types.Length)
				throw new ArgumentOutOfRangeException("types", "The array of block types must have at least 1 element.");

			var column = new BlockType[Chunk.Height];
			if(repeat)
				for(var i = yOff; i < column.Length; ++i)
					column[i] = types[i % types.Length];
			else
			{
				var y = 0;
				for(var i = yOff; i < column.Length && y < types.Length; ++i, ++y)
					column[i] = types[y]; // TODO: This could be optimized
			}

			setSectionSpan(bx, bz, _data.BlockTypes, yOff, column);
			_data.HeightMap[bx, bz] = column.FindHighestBlock();
		}
		#endregion

		#region Utility
		private static void fillBlock<T> (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, T[][] data, T value)
		{
			checkBounds(bx, by, bz, xCount, yCount, zCount);

			var xEnd = bx + xCount;
			var yEnd = by + yCount;
			var zEnd = bz + zCount;

			var sectionStart = by / Chunk.SectionHeight;
			var sectionEnd   = (yEnd - 1) / Chunk.SectionHeight;

			var subBy1 = (byte)(by % Chunk.Size);
			var subBy2 = (byte)(yEnd % Chunk.Size);

			var index = ChunkData.CalculateIndex(bx, subBy1, bz);
			// Blocks are organized as YZX
			if(sectionStart <= sectionEnd)
			{// Region is contained in a single section
				for(var y = subBy1; y < subBy2; ++y)
					for(var z = bz; z < zEnd; ++z)
						for(var x = bx; x < xEnd; ++x)
							data[sectionStart][index++] = value;
			}

			else
			{// Region spans multiple sections
				for(var y = subBy1; y < Chunk.SectionHeight; ++y)
					for(var z = bz; z < zEnd; ++z)
						for(var x = bx; x < xEnd; ++x)
							data[sectionStart][index++] = value;
				index = ChunkData.CalculateIndex(bx, 0, bz);
				for(var section = sectionStart + 1; section < sectionEnd; ++section)
					for(var y = 0; y < Chunk.SectionHeight; ++y)
						for(var z = bz; z < zEnd; ++z)
							for(var x = bx; x < xEnd; ++x)
								data[section][index++] = value;
				index = ChunkData.CalculateIndex(bx, 0, bz);
				for(var y = 0; y < yEnd; ++y)
					for(var z = bz; z < zEnd; ++z)
						for(var x = bx; x < xEnd; ++x)
							data[sectionEnd][index++] = value;
			}
		}

		private static void fillBlockNibble (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, byte[][] blocks, byte value)
		{
			checkBounds(bx, by, bz, xCount, yCount, zCount);

			var section = by / Chunk.Size;
			var secBy   = (byte)(by % Chunk.Size);
			var index   = ChunkData.CalculateIndex(bx, secBy, bz);

			blocks[section].SetNibble(index, value);
		}

		private static void setSectionSpan<T> (byte bx, byte bz, T[][] sections, byte yOff, T[] column)
		{
			var yEnd = yOff + column.Length;
			const int step = Chunk.SectionHeight * Chunk.SectionHeight;

			var sectionStart = yOff / Chunk.SectionHeight;
			var sectionEnd   = (yEnd - 1) / Chunk.SectionHeight;

			var subBy1 = yOff % Chunk.SectionHeight;
			var subBy2 = yEnd % Chunk.SectionHeight;

			var startIndex = ChunkData.CalculateIndex(bx, (byte)subBy1, bz);
			var index = startIndex;

			if(sectionStart >= sectionEnd)
			{// Column is contained in a single section
				for(var y = subBy1; y < subBy2; ++y, index += step)
					sections[sectionStart][index] = column[y];
			}

			else
			{// Column spans multiple sections
				var y = subBy1;
				for(; y < Chunk.SectionHeight; index += step)
					sections[sectionStart][index] = column[y++];
				for(var section = sectionStart + 1; section < sectionEnd; ++section)
				{
					index = startIndex;
					for(var i = 0; i < Chunk.SectionHeight; ++i, index += step)
						sections[section][index] = column[y++];
				}
				for(index = startIndex; y < yEnd; index += step)
					sections[sectionEnd][index] = column[y++];
			}
		}

		private static void updateHeightMap (HeightData heightMap, byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, bool reduce)
		{
			var xEnd = bx + xCount;
			var zEnd = bz + zCount;

			int height;
			if(reduce)
			{// Air blocks (lower height if needed)
				height = (0 == yCount) ? by : by + yCount - 1;
				for(var x = bx; x < xEnd; ++x)
					for(var z = bz; z < zEnd; ++z)
						if(height < heightMap[x, z])
							heightMap[x, z] = height;
			}

			else
			{// Solid blocks (raise height if needed)
				height = (0 == by) ? 0 : by - 1;
				for(var x = bx; x < xEnd; ++x)
					for(var z = bz; z < zEnd; ++z)
						if(height > heightMap[x, z])
							heightMap[x, z] = height;
			}
		}

		private static void checkBounds (byte bx, byte bz)
		{
			if(Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The x-position of the block must be less than " + Chunk.Size);
			if(Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The z-position of the block must be less than " + Chunk.Size);
		}

		private static void checkBounds (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount)
		{
			checkBounds(bx, bz);
			if(Chunk.Size < bx + xCount)
				throw new ArgumentOutOfRangeException("xCount", "The total block count can't extend outside the chunk.");
			if(Chunk.Height < by + yCount)
				throw new ArgumentOutOfRangeException("yCount", "The total block count can't extend outside the chunk.");
			if(Chunk.Size < bz + zCount)
				throw new ArgumentOutOfRangeException("zCount", "The total block count can't extend outside the chunk.");
		}
		#endregion

		#region Disposable
		/// <summary>
		/// Disposes the contents of the builder
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Cleans up resources held by the builder
		/// </summary>
		/// <param name="disposing">True if child variables should be cleaned up</param>
		protected virtual void Dispose (bool disposing)
		{
			// ...
		}

		~ChunkBuilder ()
		{
			Dispose(false);
		}
		#endregion
	}
}
