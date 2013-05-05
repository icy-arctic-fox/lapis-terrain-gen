using System;
using System.Collections.Generic;
using Lapis.Blocks;
using Lapis.Level;
using Lapis.Level.Data;
using Lapis.Level.Generation;
using Lapis.Level.Generation.Noise;
using Lapis.Level.Generation.Population;

namespace TestTerrainGenerator
{
	/// <summary>
	/// Generates empty chunks
	/// </summary>
	public class TestTerrainGenerator : ITerrainGenerator
	{
		#region Plugin properties
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		/// <remarks>The name of this generator is "Test".</remarks>
		public string Name
		{
			get { return "Test"; }
		}

		/// <summary>
		/// Version of the generator
		/// </summary>
		/// <remarks>The version of this generator is 1.</remarks>
		public int Version
		{
			get { return 1; }
		}

		/// <summary>
		/// Name of the person that wrote the terrain generator
		/// </summary>
		/// <remarks>The creator of this generator is Lapis MC</remarks>
		public string Author
		{
			get { return "Lapis MC"; }
		}

		/// <summary>
		/// Brief description of what the generator does
		/// </summary>
		public string Description
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
		/// List of chunk populators used to fill chunks after generation
		/// </summary>
		public ICollection<IChunkPopulator> Populators
		{
			get { return null; }
		}

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="seed">Level seed used for generation</param>
		/// <param name="options">Options string used to customize the generator (does nothing for this generator)</param>
		public void Initialize (long seed, string options)
		{
			_noise = new NoiseCombiner(new RidgedMultifractalNoiseGenerator(seed, RidgedMultifractalNoiseGenerator.DefaultOctaves, RidgedMultifractalNoiseGenerator.DefaultFrequency, MathConstants.Sqrt3),
				new RidgedMultifractalNoiseGenerator(unchecked(seed + 1), RidgedMultifractalNoiseGenerator.DefaultOctaves, RidgedMultifractalNoiseGenerator.DefaultFrequency, MathConstants.Sqrt3),
				NoiseCombiner.CombinationMethod.Multiply);
		}

		private NoiseGenerator _noise;
		private const double ScaleX = 1 / 72d;
		private const double ScaleY = 1 / 64d;
		private const double ScaleZ = 1 / 72d;

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
				{
					for(var bz = (byte)0; bz < Chunk.Size; ++bz)
					{
						var column = new BlockType[64];
						int gx, gz;
						LevelDataUtility.LocalToGlobalXZ(cx, cz, bx, bz, out gx, out gz);
						var x = gx * ScaleX;
						var z = gz * ScaleZ;
						for(var by = 0; by < column.Length; ++by)
						{
							var y = by * ScaleY;
							var noiseValue = _noise.GenerateNoise(x, y, z);
							if(noiseValue > 0d)
								column[by] = BlockType.Stone;
						}
						builder.FillColumn(bx, bz, column);
					}
				}
				return builder.GetChunkData();
			}
		}
	}
}
