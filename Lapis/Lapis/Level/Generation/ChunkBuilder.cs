using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public void FillType (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount, BlockType type)
		{
			throw new NotImplementedException();
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
			var sectionEnd   = yEnd / Chunk.SectionHeight;

			var subBy1 = (byte)(by % Chunk.Size);
			var subBy2 = (byte)(yEnd % Chunk.Size);

			var index = ChunkData.CalculateIndex(bx, by, bz);
			// Blocks are organized as YZX
			if(sectionStart == sectionEnd)
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
			throw new NotImplementedException();
		}

		private static void checkBounds (byte bx, byte by, byte bz, byte xCount, byte yCount, byte zCount)
		{
			if(Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The x-position of the block must be less than " + Chunk.Size);
			if(Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The z-position of the block must be less than " + Chunk.Size);
			if(Chunk.Size <= bx + xCount)
				throw new ArgumentOutOfRangeException("xCount", "The total block count can't extend outside the chunk.");
			if(Chunk.Height <= by + yCount)
				throw new ArgumentOutOfRangeException("yCount", "The total block count can't extend outside the chunk.");
			if(Chunk.Size <= bz + zCount)
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
