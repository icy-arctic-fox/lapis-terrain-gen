using System;
using Lapis.Level;
using Lapis.Level.Data;
using Lapis.Level.Generation;

namespace EmptyTerrainGenerator
{
	public class EmptyTerrainGenerator : ITerrainGenerator
	{
		public string GeneratorName
		{
			get { return "Empty Terrain Generator"; }
		}

		public int GeneratorVersion
		{
			get { return 1; }
		}

		public string GeneratorDescription
		{
			get { return String.Empty; }
		}

		public string GeneratorAuthor
		{
			get { return String.Empty; }
		}

		public string GeneratorOptions
		{
			get { return String.Empty; }
		}

		public void Initialize (string options)
		{
			// ...
		}

		public ChunkData GenerateChunk (int cx, int cz)
		{
			using(var builder = new ChunkBuilder(cx, cz))
			{
				builder.FillType(0, 30, 0, Chunk.Size, 1, Chunk.Size, Lapis.Blocks.BlockType.Sand);
				return builder.GetChunkData();
			}
		}
	}
}
