using System;
using System.Collections.Generic;
using System.Threading;
using Lapis.Blocks;
using Lapis.Level.Data;
using Lapis.Spatial;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Provides an interface for creating structures made of blocks and then inserting them into chunks
	/// </summary>
	/// <remarks>This class is not thread-safe.</remarks>
	public class BlockCanvas : IDisposable
	{
		private readonly Dictionary<XYZCoordinate, Block> _blocks = new Dictionary<XYZCoordinate, Block>();

		private int _minX, _minY, _minZ, _maxX, _maxY, _maxZ;

		#region Properties
		/// <summary>
		/// Position of the minimum block along the z-axis
		/// </summary>
		public int MinimumX
		{
			get { return _minX; }
		}

		/// <summary>
		/// Position of the minimum block along the y-axis
		/// </summary>
		public int MinimumY
		{
			get { return _minY; }
		}

		/// <summary>
		/// Position of the minimum block along the z-axis
		/// </summary>
		public int MinimumZ
		{
			get { return _minZ; }
		}

		/// <summary>
		/// Position of the maximum block along the x-axis
		/// </summary>
		public int MaximumX
		{
			get { return _maxX; }
		}

		/// <summary>
		/// Position of the maximum block along the y-axis
		/// </summary>
		public int MaximumY
		{
			get { return _maxY; }
		}

		/// <summary>
		/// Position of the maximum block along the z-axis
		/// </summary>
		public int MaximumZ
		{
			get { return _maxZ; }
		}

		/// <summary>
		/// Length of the canvas along the x-axis
		/// </summary>
		public int Width
		{
			get { return (0 < _blocks.Count) ? _maxX - _minX + 1 : 0; }
		}

		/// <summary>
		/// Length of the canvas along the y-axis
		/// </summary>
		public int Height
		{
			get { return (0 < _blocks.Count) ? _maxY - _minY + 1 : 0; }
		}

		/// <summary>
		/// Length of the canvas along the z-axis
		/// </summary>
		public int Depth
		{
			get { return (0 < _blocks.Count) ? _maxZ - _minZ + 1 : 0; }
		}
		#endregion

		#region Editing
		/// <summary>
		/// Sets a block on the canvas
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <param name="z">Z-position</param>
		/// <param name="block">Block information to set</param>
		public void SetBlock (int x, int y, int z, Block block)
		{
			// TODO: Add support for tile blocks (blocks that contain NBT data)
			throw new NotImplementedException();
		}

		#region Block brush
		// TODO: Add support for block brushes
		#endregion

		/// <summary>
		/// Applies the canvas to one or more chunks
		/// </summary>
		/// <param name="c">Chunk to apply the canvas to</param>
		/// <param name="xOff">Offset along the x-axis</param>
		/// <param name="yOff">Offset along the y-axis</param>
		/// <param name="zOff">Offset along the z-axis</param>
		/// <remarks>Any blocks outside the bounds of the world (y &lt; 0 or y &gt; 255) are ignored.</remarks>
		public void Apply (Chunk c, int xOff = 0, int yOff = 0, int zOff = 0)
		{
			var chunks = constructChunkList(c, xOff, yOff, zOff);
			lock(c)
			{
				foreach(var item in chunks)
				{// Go through each chunk that is affected
					var chunk  = item.Key;
					var blocks = item.Value;

					if(chunk != c)
					{// Give up the lock to prevent deadlocking
						Monitor.Pulse(c);
						Monitor.Exit(c);
					}

					lock(chunk)
					{
						foreach(var tuple in blocks)
						{// Apply all of the actions to the chunk
							var coord = tuple.Item1;
							var block = tuple.Item2;

							var bx = (byte)coord.X;
							var by = (byte)coord.Y;
							var bz = (byte)coord.Z;
							chunk.SetBlock(bx, by, bz, block);
						}
					}

					if(chunk != c)
						Monitor.Enter(c);
				}
			}
		}

		private Dictionary<Chunk, List<Tuple<XYZCoordinate, Block>>> constructChunkList (Chunk c, int xOff, int yOff, int zOff)
		{
			var chunks = new Dictionary<Chunk, List<Tuple<XYZCoordinate, Block>>>();
			foreach(var item in _blocks)
			{
				var coord = item.Key;
				var block = item.Value;

				var bx = coord.X + xOff;
				var by = coord.Y + yOff;
				var bz = coord.Z + zOff;

				var sy = by / Chunk.SectionHeight;

				if(0 <= sy && Chunk.SectionCount > sy)
				{// Block is in bounds
					var cxOff = bx / Chunk.Size;
					var czOff = bz / Chunk.Size;

					if(0 > bx)
					{
						--cxOff;
						bx -= cxOff * Chunk.Size;
					}
					if(0 > bz)
					{
						--czOff;
						bz -= czOff * Chunk.Size;
					}

					var chunk = c.GetRelativeChunk(cxOff, czOff);
					List<Tuple<XYZCoordinate, Block>> blocks;
					if(chunks.ContainsKey(chunk))
						blocks = chunks[chunk];
					else
						chunks[chunk] = blocks = new List<Tuple<XYZCoordinate, Block>>();
					var chunkCoord = new XYZCoordinate(bx, by, bz);
					var tuple = new Tuple<XYZCoordinate, Block>(chunkCoord, block);
					blocks.Add(tuple);
				}
			}
			return chunks;
		}

		/// <summary>
		/// Clears the contents of the canvas
		/// </summary>
		public void Clear ()
		{
			_blocks.Clear();
			_minX = 0;
			_minY = 0;
			_minZ = 0;
			_maxX = 0;
			_maxY = 0;
			_maxZ = 0;
		}
		#endregion

		#region Disposable
		/// <summary>
		/// Disposes the contents of the object
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Cleans up resources held by the object
		/// </summary>
		/// <param name="disposing">True if child variables should be cleaned up</param>
		protected virtual void Dispose (bool disposing)
		{
			if(disposing)
				_blocks.Clear();
		}

		~BlockCanvas ()
		{
			Dispose(false);
		}
		#endregion
	}
}
