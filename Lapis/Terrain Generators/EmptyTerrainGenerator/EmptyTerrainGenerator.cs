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
			var data = new ChunkData(cx, cz);
			for(var bx = (byte)0; bx < Chunk.Size; ++bx)
				for(var bz = (byte)0; bz < Chunk.Size; ++bz)
					data.SetBlockType(bx, (byte)30, bz, Lapis.Blocks.BlockType.Glass);
			return data;
		}
	}
}
