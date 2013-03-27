namespace Lapis.Level
{
	/// <summary>
	/// References a block within a chunk
	/// </summary>
	/// <remarks>Block references are useful for when you want to reference a location, but don't care about what's there.
	/// A block reference doesn't contain any block information.
	/// However, a block reference won't cause the chunk data to become loaded in memory.</remarks>
	public class BlockRef
	{
		private readonly byte _bx, _by, _bz;
		private readonly ChunkRef _chunk;
	}
}
