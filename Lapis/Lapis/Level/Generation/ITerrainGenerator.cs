using Lapis.Level.Data;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Generates chunks for a realm
	/// </summary>
	/// <remarks>Terrain generators may require additional configuration and parameters when they're created.
	/// After a generator is set up, a combination of the version and options string can produce the same generator later.</remarks>
	public interface ITerrainGenerator
	{
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		string GeneratorName { get; }

		/// <summary>
		/// Version of the generator
		/// </summary>
		/// <remarks>This property is used for consistency (backwards compatibility) when a generator's version changes.
		/// It helps to use the same generator version so that there aren't chunk alignment issues (sheer cliffs).</remarks>
		int GeneratorVersion { get; }

		/// <summary>
		/// Name of the person that wrote the terrain generator
		/// </summary>
		string GeneratorAuthor { get; }

		/// <summary>
		/// Brief description of what the generator does
		/// </summary>
		string GeneratorDescription { get; }

		/// <summary>
		/// Options string used to customize the generator
		/// </summary>
		/// <remarks>This property can be fed into Initialize() to get the same generation settings.</remarks>
		string GeneratorOptions { get; }

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="options">Options string used to customize the generator</param>
		void Initialize (string options);

		/// <summary>
		/// Generates a chunk at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>The generated chunk data</returns>
		ChunkData GenerateChunk (int cx, int cz);
	}
}
