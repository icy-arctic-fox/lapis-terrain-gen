using System;

namespace Lapis.Level.Generation.Population
{
	/// <summary>
	/// Populates a chunk with block light
	/// </summary>
	public class BlockLightPopulator : IChunkPopulator
	{
		#region Plugin properties
		/// <summary>
		/// Name of the chunk populator
		/// </summary>
		/// <remarks>The name of this populator is "Block Light Populator"</remarks>
		public string PluginName
		{
			get { return "Block Light Populator"; }
		}

		/// <summary>
		/// Version of the chunk populator
		/// </summary>
		/// <remarks>The version of this populator is 1</remarks>
		public int PluginVersion
		{
			get { return 1; }
		}

		/// <summary>
		/// Description of what the chunk populator does
		/// </summary>
		public string PluginDescription
		{
			get { return "Fills chunks with block light"; }
		}

		/// <summary>
		/// Creator of the chunk populator
		/// </summary>
		/// <remarks>The creator of this populator is Lapis MC</remarks>
		public string PluginCreator
		{
			get { return "Lapis MC"; }
		}
		#endregion

		/// <summary>
		/// Populates a chunk
		/// </summary>
		/// <param name="c">Chunk to populate</param>
		public void PopulateChunk (Chunk c)
		{
			LightChunk(c);
		}

		/// <summary>
		/// Lights a chunk with block light
		/// </summary>
		/// <param name="c">Chunk to light up</param>
		/// <remarks>This method can be used to recalculate block light for a chunk.</remarks>
		public static void LightChunk (Chunk c)
		{
			throw new NotImplementedException();
		}
	}
}
