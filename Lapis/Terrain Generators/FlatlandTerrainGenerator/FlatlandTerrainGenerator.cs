using System;
using Lapis.Blocks;
using Lapis.Level;
using Lapis.Level.Data;
using Lapis.Level.Generation;

namespace FlatlandTerrainGenerator
{
	/// <summary>
	/// Generates empty chunks
	/// </summary>
	public class FlatlandTerrainGenerator : ITerrainGenerator
	{
		#region Meta data
		/// <summary>
		/// Name of the terrain generator
		/// </summary>
		/// <remarks>The name of this generator is "Flatland Terrain Generator".</remarks>
		public string GeneratorName
		{
			get { return "Flatland Terrain Generator"; }
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
			get { return "Generates completely flat and identical terrain"; }
		}
		#endregion

		private readonly BlockType[] _column = new BlockType[Chunk.Height];

		/// <summary>
		/// Options string used to customize the generator
		/// </summary>
		/// <remarks>This string has the format: BlockID[xCount][,BlockID[xCount]]...</remarks>
		public string GeneratorOptions
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="options">Options string used to customize the generator</param>
		public void Initialize (string options)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Generates a chunk at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>The generated chunk data</returns>
		public ChunkData GenerateChunk (int cx, int cz)
		{
			throw new NotImplementedException();
		}
	}
}
