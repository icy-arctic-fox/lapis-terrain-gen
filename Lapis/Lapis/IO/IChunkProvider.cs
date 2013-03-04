using Lapis.Level.Data;

namespace Lapis.IO
{
	/// <summary>
	/// Retrieves and stores chunk data
	/// </summary>
	public interface IChunkProvider
	{
		/// <summary>
		/// Retrieves the raw chunk data at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>Chunk data or null if the chunk doesn't exist</returns>
		ChunkData GetChunk (int cx, int cz);

		/// <summary>
		/// Stores the raw chunk data at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <param name="data">Chunk data</param>
		void PutChunk (int cx, int cz, ChunkData data);

		/// <summary>
		/// Checks if a chunk exists (has been generated)
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>True if the chunk exists or false if it doesn't</returns>
		bool ChunkExists (int cx, int cz);
	}
}
