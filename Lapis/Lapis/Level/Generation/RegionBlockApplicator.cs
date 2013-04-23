using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lapis.Blocks;
using Lapis.IO.NBT;
using Lapis.Utility;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Tracks and applies a region of blocks to chunks
	/// </summary>
	/// <remarks>This type of applicator is ideal for large objects with lots of blocks.
	/// However, the region is a fixed size at creation.</remarks>
	public abstract class RegionBlockApplicator : IBlockApplicator
	{
		private readonly int _length, _width, _height;
		private readonly BlockType[] _blockTypes;
		private readonly NibbleArray _blockData;
		private readonly Dictionary<int, Node> _tileEntities = new Dictionary<int, Node>();

		/// <summary>
		/// Size of the region along the x-axis
		/// </summary>
		public int Length
		{
			get { return _length; }
		}

		/// <summary>
		/// Size of the region along the y-axis
		/// </summary>
		public int Height
		{
			get { return _height; }
		}

		/// <summary>
		/// Size of the region along the z-axis
		/// </summary>
		public int Width
		{
			get { return _width; }
		}

		/// <summary>
		/// Generates a collection of chunks that are affected by the region
		/// </summary>
		/// <param name="origin">Origin chunk</param>
		/// <param name="xOff">Offset along to x-axis inside the origin chunk to start at</param>
		/// <param name="yOff">Offset along to y-axis inside the origin chunk to start at</param>
		/// <param name="zOff">Offset along to z-axis inside the origin chunk to start at</param>
		/// <returns>A collection of affected chunks</returns>
		public IEnumerable<KeyValuePair<ChunkRef, object>> GetAffectedChunks (ChunkRef origin, int xOff, int yOff, int zOff)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Applies the part (or all) of the region to a chunk
		/// </summary>
		/// <param name="c">Chunk to apply blocks to</param>
		/// <param name="origin">Origin chunk</param>
		/// <param name="xOff">Offset along to x-axis inside the origin chunk to start at</param>
		/// <param name="yOff">Offset along to y-axis inside the origin chunk to start at</param>
		/// <param name="zOff">Offset along to z-axis inside the origin chunk to start at</param>
		/// <param name="extra">Extra pre-processed information from the GetAffectedChunks method</param>
		public void ApplyToChunk (Chunk c, ChunkRef origin, int xOff, int yOff, int zOff, object extra)
		{
			throw new NotImplementedException();
		}
	}
}
