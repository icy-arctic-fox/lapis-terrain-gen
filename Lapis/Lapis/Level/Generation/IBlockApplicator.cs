using System.Collections.Generic;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// An object that applies a region of blocks to one or more chunks
	/// </summary>
	public interface IBlockApplicator
	{
		/// <summary>
		/// Gets the chunks affected by the region
		/// </summary>
		/// <param name="origin">Origin chunk</param>
		/// <param name="xOff">Offset along the x-axis at the origin chunk's x = 0</param>
		/// <param name="yOff">Offset along the y-axis at the origin chunk's y = 0</param>
		/// <param name="zOff">Offset along the z-axis at the origin chunk's z = 0</param>
		/// <returns>A collection of chunks that are affected</returns>
		/// <remarks>The object part of key-value pair allows extra data to be stored and passed back in to ApplyToChunk.
		/// This is ideal when ApplyToChunk would have to recalculate or reprocess values that were already done in this method.</remarks>
		IEnumerable<KeyValuePair<ChunkRef, object>> GetAffectedChunks (ChunkRef origin, int xOff, int yOff, int zOff);

		/// <summary>
		/// Applies the region of blocks to a chunk
		/// </summary>
		/// <param name="c">Chunk to apply the blocks to</param>
		/// <param name="origin">Origin chunk</param>
		/// <param name="xOff">Offset along the x-axis from the origin chunk's x = 0</param>
		/// <param name="yOff">Offset along the y-axis from the origin chunk's y = 0</param>
		/// <param name="zOff">Offset along the z-axis from the origin chunk's z = 0</param>
		/// <param name="extra">Extra data returned from GetAffectedChunks</param>
		void ApplyToChunk (Chunk c, ChunkRef origin, int xOff, int yOff, int zOff, object extra);
	}
}
