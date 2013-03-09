using System;
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
			return new ChunkData(cx, cz);
		}
	}
}
