namespace Lapis.Level.Generation.Population
{
	/// <summary>
	/// Populates a chunk with an object
	/// </summary>
	/// <remarks>Populators may spill their contents over into other chunks.</remarks>
	public interface IChunkPopulator : IPlugin
	{
		/// <summary>
		/// Populates a chunk
		/// </summary>
		/// <param name="c">Chunk to populate</param>
		void PopulateChunk (Chunk c);
	}
}
