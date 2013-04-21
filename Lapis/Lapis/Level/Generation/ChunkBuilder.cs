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
		private readonly BlockType[][] _types;

		/// <summary>
		/// Creates a new chunk builder
		/// </summary>
		/// <param name="cx">X-position of the chunk coordinate</param>
		/// <param name="cz">Z-position of the chunk coordinate</param>
		public ChunkBuilder (int cx, int cz)
		{
			_data  = new ChunkData(cx, cz);
			_types = _data.BlockTypes; // Cache the arrays so we don't have to generate them again (from ChunkData)
		}

		/// <summary>
		/// Retrieves the constructed chunk data
		/// </summary>
		/// <returns>Chunk data</returns>
		public ChunkData GetChunkData ()
		{
			// Mark the sections that we touched as modified
			var sectionCount = _data.HeightMap.Maximum / Chunk.SectionHeight;
			for(var i = 0; i <= sectionCount; ++i)
				_data.Sections[i].MarkAsModified();

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
			fillBlock(bx, by, bz, xCount, yCount, zCount, _types, type);
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
			FillColumn(bx, bz, type, yOff, (byte)(Chunk.Height - yOff));
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
			var yEnd = (byte)(yOff + yCount - 1);
			const int step = Chunk.Size * Chunk.Size;

			byte sectionStart, sectionEnd;
			var startIndex = ChunkData.CalculateSectionIndex(bx, yOff, bz, out sectionStart);
			var endIndex   = ChunkData.CalculateSectionIndex(bx, yEnd, bz, out sectionEnd);

			if(sectionEnd > sectionStart)
			{// Mutliple sections
				for(var index = startIndex; index < ChunkSectionData.SectionLength; index += step)
					_types[sectionStart][index] = type;
				var baseIndex = ChunkData.CalculateBlockIndex(bx, 0, bz);
				for(var section = sectionStart + 1; section < sectionEnd; ++section)
					for(var index = baseIndex; index < Chunk.SectionHeight; index += step)
						_types[section][index] = type;
				for(var index = baseIndex; index < endIndex; index += step)
					_types[sectionEnd][index] = type;
			}

			else
			{// Single section
				for(var index = startIndex; index < endIndex; index += step)
					_types[sectionStart][index] = type;
			}
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

			BlockType[] column;
			if(repeat)
			{
				column = new BlockType[Chunk.Height];
				for(var i = yOff; i < column.Length; ++i)
					column[i] = types[i % types.Length];
			}
			else
			{
				column = new BlockType[Math.Min(Chunk.Height, types.Length + yOff)];
				var y = 0;
				for(var i = yOff; i < column.Length && y < types.Length; ++i, ++y)
					column[i] = types[y]; // TODO: This could be optimized
			}

			setSectionSpan(bx, bz, _types, yOff, column);
			_data.HeightMap[bx, bz] = column.FindHighestBlock() + 1;
		}
		#endregion

		#region Utility
		private static void fillBlock<T> (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, T[][] data, T value)
		{
			checkBounds(bx, by, bz, xCount, yCount, zCount);

			var xEnd = (byte)(bx + xCount);
			var yEnd = (byte)(by + yCount);
			var zEnd = (byte)(bz + zCount);

			byte sectionStart, sectionEnd;
			var startIndex = ChunkData.CalculateSectionIndex(bx, by, bz, out sectionStart);
			var endIndex   = ChunkData.CalculateSectionIndex(xEnd, yEnd, zEnd, out sectionEnd);

			if(sectionEnd > sectionStart)
			{// Mutliple sections
				for(var y = by; y < Chunk.SectionHeight; ++y)
					for(var z = bz; z < zEnd; ++z)
						for(int x = bx, index = ChunkData.CalculateBlockIndex(bx, y, z); x < xEnd; ++x, ++index)
							data[sectionStart][index] = value;
				var baseIndex = ChunkData.CalculateBlockIndex(bx, 0, bz);
				for(var section = sectionStart + 1; section < sectionEnd; ++section)
					for(var y = (byte)0; y < Chunk.SectionHeight; ++y)
						for(var z = bz; z < zEnd; ++z)
							for(int x = bx, index = ChunkData.CalculateBlockIndex(bx, y, z); x < xEnd; ++x, ++index)
								data[section][index] = value;
				for(var y = (byte)0; y < yEnd; ++y)
					for(var z = bz; z < zEnd; ++z)
						for(int x = bx, index = ChunkData.CalculateBlockIndex(bx, y, z); x < xEnd; ++x, ++index)
							data[sectionEnd][index] = value;
			}

			else
			{// Single section
				for(int index = startIndex, y = by; y < yEnd; ++y)
					for(var z = bz; z < zEnd; ++z)
						for(var x = bx; x < xEnd; ++x, ++index)
							data[sectionStart][index] = value;
			}
		}

		private static void fillBlockNibble (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, NibbleArray[] blocks, byte value)
		{
			throw new NotImplementedException();
		}

		private static void setSectionSpan<T> (byte bx, byte bz, T[][] sections, byte yOff, T[] column)
		{
			var yEnd = (byte)(yOff + column.Length - 1);
			const int step = Chunk.Size * Chunk.Size;

			byte sectionStart, sectionEnd;
			var startIndex = ChunkData.CalculateSectionIndex(bx, yOff, bz, out sectionStart);
			var endIndex   = ChunkData.CalculateSectionIndex(bx, yEnd, bz, out sectionEnd);

			var y = 0;
			if(sectionEnd > sectionStart)
			{// Multiple sections
				for(int by = yOff, index = startIndex; by < Chunk.SectionHeight; ++by, index += step)
					sections[sectionStart][index] = column[y++];
				var baseIndex = ChunkData.CalculateBlockIndex(bx, 0, bz);
				for(var section = sectionStart + 1; section < sectionEnd; ++section)
					for(int by = 0, index = baseIndex; by < Chunk.SectionHeight; ++by, index += step)
						sections[section][index] = column[y++];
				for(var index = baseIndex; index < endIndex; index += step)
					sections[sectionEnd][index] = column[y++];
			}

			else
			{// Single section
				for(var index = startIndex; index < endIndex; index += step)
					sections[sectionStart][index] = column[y++];
			}
		}

		private static void updateHeightMap (HeightData heightMap, byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, bool reduce)
		{
			var xEnd = bx + xCount;
			var zEnd = bz + zCount;

			int height;
			if(reduce)
			{// Air blocks (lower height if needed)
				height = (0 == yCount) ? by + 1 : by + yCount;
				for(var x = bx; x < xEnd; ++x)
					for(var z = bz; z < zEnd; ++z)
						if(height < heightMap[x, z])
							heightMap[x, z] = height;
			}

			else
			{// Solid blocks (raise height if needed)
				height = (0 == by) ? 0 : by;
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
