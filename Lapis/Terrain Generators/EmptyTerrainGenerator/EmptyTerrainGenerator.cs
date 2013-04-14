using System;
using System.Collections.Generic;
using Lapis.Level.Data;
using Lapis.Level.Generation;
using Lapis.Level.Generation.Population;

namespace EmptyTerrainGenerator
{
	/// <summary>
	/// Generates empty chunks
	/// </summary>
	public class EmptyTerrainGenerator : ITerrainGenerator
	{
		#region Meta-data
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		/// <remarks>The name of this generator is "Empty".</remarks>
		public string Name
		{
			get { return "Empty"; }
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
		/// <remarks>The creator of this generator is Mike Miller &lt;dotMaiku@gmail.com&gt;</remarks>
		public string Author
		{
			get { return "Mike Miller <dotMaikU@gmail.com>"; }
		}

		/// <summary>
		/// Brief description of what the generator does
		/// </summary>
		public string Description
		{
			get { return "Generates empty chunks"; }
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
		public IEnumerable<IChunkPopulator> Populators
		{
			get { return null; }
		}

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="seed">Level seed used for generation (does nothing for this generator)</param>
		/// <param name="options">Options string used to customize the generator (does nothing for this generator)</param>
		public void Initialize (long seed, string options)
		{
			// ...
		}

		/// <summary>
		/// Generates a chunk at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>The generated chunk data</returns>
		public ChunkData GenerateChunk (int cx, int cz)
		{
			return new ChunkData(cx, cz) {
				TerrainPopulated = true
			};
		}
	}
}
