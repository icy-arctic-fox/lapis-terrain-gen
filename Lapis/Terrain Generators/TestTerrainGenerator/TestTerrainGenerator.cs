using System;
using Lapis.Level;
using Lapis.Level.Data;
using Lapis.Level.Generation;
using Lapis.Level.Generation.Noise;

namespace TestTerrainGenerator
{
	/// <summary>
	/// Generates empty chunks
	/// </summary>
	public class TestTerrainGenerator : ITerrainGenerator
	{
		#region Meta data
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		/// <remarks>The name of this generator is "Test Terrain Generator".</remarks>
		public string GeneratorName
		{
			get { return "Test Terrain Generator"; }
		}

		/// <summary>
		/// Version of the generator
		/// </summary>
		/// <remarks>The version of this generator is 1.</remarks>
		public int GeneratorVersion
		{
			get { return 1; }
		}

		/// <summary>
		/// Name of the person that wrote the terrain generator
		/// </summary>
		/// <remarks>The creator of this generator is Mike Miller &lt;dotMaiku@gmail.com&gt;</remarks>
		public string GeneratorAuthor
		{
			get { return "Mike Miller <dotMaikU@gmail.com>"; }
		}

		/// <summary>
		/// Brief description of what the generator does
		/// </summary>
		public string GeneratorDescription
		{
			get { return "Generator used for testing functionality"; }
		}
		#endregion

		/// <summary>
		/// Options string used to customize the generator
		/// </summary>
		/// <remarks>This property is empty because there is nothing to configure for this generator.</remarks>
		public string GeneratorOptions
		{
			get { return String.Empty; }
		}

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="options">Options string used to customize the generator (does nothing for this generator)</param>
		public void Initialize (string options)
		{
			// ...
			_noise.AddPostProcess(new RangePostProcessor(0, Chunk.Height));
		}

		private readonly NoiseGenerator _noise = new SimplexNoiseGenerator(0);
		private const double Scale = 1 / 256d;


		/// <summary>
		/// Generates a chunk at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>The generated chunk data</returns>
		public ChunkData GenerateChunk (int cx, int cz)
		{
			var data = new ChunkData(cx, cz);
			for(var bx = (byte)0; bx < Chunk.Size; ++bx)
				for(var bz = (byte)0; bz < Chunk.Size; ++bz)
				{
					int gx, gz;
					LevelDataUtility.LocalToGlobalXZ(cx, cz, bx, bz, out gx, out gz);
					var x = gx * Scale;
					var z = gz * Scale;
					var height = _noise.GenerateNoise(x, z, 0);
					for(var by = (byte)(height - 1); by > 0; --by)
						data.SetBlockType(bx, by, bz, Lapis.Blocks.BlockType.Dirt);
					if(height < 64)
					{
						for(var by = (byte)height; by < height + 3; ++by)
							data.SetBlockType(bx, by, bz, Lapis.Blocks.BlockType.Sand);
						for(var by = (byte)(height + 3); by < 65; ++by)
							data.SetBlockType(bx, by, bz, Lapis.Blocks.BlockType.Water);
						height = 64;
					}
					else
					{
						data.SetBlockType(bx, (byte)height, bz, height > 220 ? Lapis.Blocks.BlockType.Snow : Lapis.Blocks.BlockType.Grass);
					}
					data.SetBlockType(bx, 0, bz, Lapis.Blocks.BlockType.Bedrock);
					data.HeightMap[bx, bz] = (byte)height;
				}
			return data;
		}
	}
}
