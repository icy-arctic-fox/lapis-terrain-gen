using System;
using System.Collections.Generic;
using Lapis.Blocks;
using Lapis.Level;
using Lapis.Level.Data;
using Lapis.Level.Generation;
using Lapis.Level.Generation.Noise;
using Lapis.Level.Generation.Population;

namespace IslandsTerrainGenerator
{
	/// <summary>
	/// Generates a vast ocean with lots of islands
	/// </summary>
	public class IslandsTerrainGenerator : ITerrainGenerator
	{
		#region Meta-data
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		/// <remarks>The name of this generator is "Islands".</remarks>
		public string Name
		{
			get { return "Islands"; }
		}

		/// <summary>
		/// Version of the generator
		/// </summary>
		/// <remarks>The version of this generator is 2.</remarks>
		public int Version
		{
			get { return 2; }
		}

		/// <summary>
		/// Name of the person that wrote the terrain generator
		/// </summary>
		/// <remarks>The creator of this generator is Michael Miller &lt;icy.arctic.fox@gmail.com&gt;</remarks>
		public string Author
		{
			get { return "Michael Miller <icy.arctic.fox@gmail.com>"; }
		}

		/// <summary>
		/// Brief description of what the generator does
		/// </summary>
		public string Description
		{
			get { return "Generates a vast ocean with lots of islands"; }
		}
		#endregion

		private SimplexNoiseGenerator _bedrockNoise;
		private PerlinNoiseGenerator _stoneNoise;
		private PerlinNoiseGenerator _dirtNoise;
		private PerlinNoiseGenerator _sandNoise;
		private PerlinNoiseGenerator _surfaceNoise;

		/// <summary>
		/// Options string used to customize the generator
		/// </summary>
		/// <remarks>This property is empty because there is nothing to configure for this generator.</remarks>
		public string GeneratorOptions
		{
			get { return String.Empty; }
		}

		/// <summary>
		/// List of chunk populators used to fill chunks after generation
		/// </summary>
		public ICollection<IChunkPopulator> Populators
		{
			get { return new[] { new SkyLightPopulator() }; }
		}

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="seed">Level seed used for generation</param>
		/// <param name="options">Options string used to customize the generator (does nothing for this generator)</param>
		public void Initialize (long seed, string options)
		{
			_bedrockNoise = new SimplexNoiseGenerator(seed);
			_bedrockNoise.AddPostProcess(new RangePostProcessor(1, 3));

			_stoneNoise = new PerlinNoiseGenerator(unchecked(seed + 1));
			_stoneNoise.AddPostProcess(new RangePostProcessor(20, 40));

			_dirtNoise = new PerlinNoiseGenerator(unchecked(seed + 2));
			_dirtNoise.AddPostProcess(new RangePostProcessor(3, 7));

			_sandNoise = new PerlinNoiseGenerator(unchecked(seed + 3));
			_sandNoise.AddPostProcess(new RangePostProcessor(2, 5));
		}

		private const double Scale = 1 / 128.0; // TODO: Make customizable
		private const byte SeaLevel = 64; // TODO: Make customizable
		private const byte Raise = 2; // TODO: Make customizable
		// TODO: Add additional properties

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

						var bedrockHeight = (int)_bedrockNoise.GenerateNoise(x, z);
						var stoneHeight   = (int)_stoneNoise.GenerateNoise(x, z);
						var dirtHeight    = (int)_dirtNoise.GenerateNoise(x, z);
						var sandHeight    = (int)_sandNoise.GenerateNoise(x, z);

						var pos = 0;
						for(var i = 0; i < bedrockHeight && pos < Chunk.Height; ++i, ++pos)
							column[i] = BlockType.Bedrock;
						for(var i = 0; i < stoneHeight && pos < Chunk.Height; ++i, ++pos)
							column[i] = BlockType.Stone;
						for(var i = 0; i < dirtHeight && pos < Chunk.Height; ++i, ++pos)
							column[i] = BlockType.Dirt;
						for(var i = 0; i < sandHeight && pos < Chunk.Height; ++i, ++pos)
							column[i] = BlockType.Sand;

						if(pos < SeaLevel)
						{// Ocean
							for(var i = pos; i < SeaLevel; ++i)
								column[i] = BlockType.Water;
							// TODO: Set biome type to Ocean
						}

						else
						{//Island
							// TODO: Set biome type to Beach
						}

						builder.FillColumn(bx, bz, column);
					}
				return builder.GetChunkData();
			}
		}
	}
}
