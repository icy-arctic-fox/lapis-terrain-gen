using System.Collections.Generic;
using Lapis.Level.Data;
using Lapis.Level.Generation.Population;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Generates chunks for a realm
	/// </summary>
	/// <remarks>Terrain generators may require additional configuration and parameters when they're created.
	/// After a generator is set up, a combination of the version and options string can produce the same generator later.</remarks>
	public interface ITerrainGenerator : IPlugin
	{
		/// <summary>
		/// Options string used to customize the generator
		/// </summary>
		/// <remarks>This property can be fed into Initialize() to get the same generation settings.</remarks>
		string GeneratorOptions { get; }

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="seed">Level seed used for generation</param>
		/// <param name="options">Options string used to customize the generator</param>
		/// <remarks>The realm is responsible for initializing the generator (calling this method).</remarks>
		void Initialize (long seed, string options);

		/// <summary>
		/// Generates a chunk at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>The generated chunk data</returns>
		ChunkData GenerateChunk (int cx, int cz);

		/// <summary>
		/// List of chunk populators used to fill chunks after generation
		/// </summary>
		/// <remarks>This property can return null if there aren't any populators.</remarks>
		IEnumerable<IChunkPopulator> Populators { get; }
	}
}
