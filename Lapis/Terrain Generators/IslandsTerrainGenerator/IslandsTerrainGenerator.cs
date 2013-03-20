using System;
using Lapis.Blocks;
using Lapis.Level;
using Lapis.Level.Data;
using Lapis.Level.Generation;
using Lapis.Level.Generation.Noise;

namespace IslandsTerrainGenerator
{
	/// <summary>
	/// Generates a vast ocean with lots of islands
	/// </summary>
	public class IslandsTerrainGenerator : ITerrainGenerator
	{
		#region Meta data
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		/// <remarks>The name of this generator is "Islands Terrain Generator".</remarks>
		public string GeneratorName
		{
			get { return "Islands Terrain Generator"; }
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
			get { return "Generates a vast ocean with lots of islands"; }
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
			var floorGenerator = new PerlinNoiseGenerator(0);
			floorGenerator.AddPostProcess(new RangePostProcessor(-1.0, -0.7));
			var surfaceGenerator = new PerlinNoiseGenerator(1);
			surfaceGenerator.AddPreProcess(new ScalePreProcessor(1.0 / 5.0));
			surfaceGenerator.AddPostProcess(new RangePostProcessor(-0.6, 0.5));
			_noise = new NoiseCombiner(floorGenerator, surfaceGenerator);
			_noise.AddPostProcess(new RangePostProcessor(20, Chunk.Height));
		}

		private NoiseGenerator _noise;
		private const double Scale = 1 / 128.0;
		private const byte SeaLevel = 64;

		/// <summary>
		/// Generates a chunk at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>The generated chunk data</returns>
		public ChunkData GenerateChunk (int cx, int cz)
		{
			using(var builder = new ChunkBuilder(cx, cz))
			{
				for(var bx = (byte)0; bx < Chunk.Size; ++bx)
					for(var bz = (byte)0; bz < Chunk.Size; ++bz)
					{
						var column = new BlockType[Chunk.Height];

						int gx, gz;
						LevelDataUtility.LocalToGlobalXZ(cx, cz, bx, bz, out gx, out gz);
						var x = gx * Scale;
						var z = gz * Scale;
						var height = _noise.GenerateNoise(x, z, 0);
						var by = (byte)height;

						for(var i = by; i < SeaLevel; ++i)
							column[i] = BlockType.Water;

						var sandStart = (byte)Math.Max(1, height - 5);
						var dirtStart = (byte)Math.Max(sandStart - 1, sandStart - 7);
						for(var i = (byte)1; i < dirtStart; ++i)
							column[i] = BlockType.Stone;
						for(var i = dirtStart; i < sandStart; ++i)
							column[i] = BlockType.Dirt;
						for(var i = sandStart; i < by; ++i)
							column[i] = BlockType.Sand;

						if(4 < by - SeaLevel)
						{
							for(var i = dirtStart; i < by - 1; ++i)
								column[i] = BlockType.Dirt;
							column[by - 1] = BlockType.Grass;
						}

						column[0] = BlockType.Bedrock;
						builder.FillColumn(bx, bz, column);
					}
				return builder.GetChunkData();
			}
		}
	}
}
