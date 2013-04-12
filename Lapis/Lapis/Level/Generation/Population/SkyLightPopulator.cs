using System;

namespace Lapis.Level.Generation.Population
{
	/// <summary>
	/// Populates a chunk with sky light
	/// </summary>
	public class SkyLightPopulator : IChunkPopulator
	{
		#region Meta-data
		/// <summary>
		/// Name of the chunk populator
		/// </summary>
		/// <remarks>The name of this populator is "Sky Light Populator"</remarks>
		public string Name
		{
			get { return "Sky Light Populator"; }
		}

		/// <summary>
		/// Version of the chunk populator
		/// </summary>
		/// <remarks>The version of this populator is 1</remarks>
		public int Version
		{
			get { return 1; }
		}

		/// <summary>
		/// Description of what the chunk populator does
		/// </summary>
		public string Description
		{
			get { return "Fills chunks with sky light"; }
		}

		/// <summary>
		/// Creator of the chunk populator
		/// </summary>
		/// <remarks>The creator of this populator is Mike Miller &lt;dotMaiku@gmail.com&gt;</remarks>
		public string Author
		{
			get { return "Mike Miller <dotMaiku@gmail.com>"; }
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
		/// Lights a chunk with sky light
		/// </summary>
		/// <param name="c">Chunk to light up</param>
		/// <remarks>This method can be used to recalculate sky light for a chunk.</remarks>
		public static void LightChunk (Chunk c)
		{
			lock(c)
			{
				// TODO: Blank out all sky light

				for(var bx = (byte)0; bx < Chunk.Size; ++bx)
					for(var bz = (byte)0; bz < Chunk.Size; ++bz)
					{
						// Quickly fill air blocks
						var height = c.GetHighestBlockAt(bx, bz);
						for(var by = Chunk.Height - 1; by > height; --by)
							c.SetSkyLight(bx, (byte)by, bz, Chunk.FullBrightness); // TODO: Add a method like SetSpan(startIndex, endIndex) to NibbleArray

						// Reduce amount of sky light through semi-transparent blocks
						var light = Chunk.FullBrightness;
						for(var by = height; by >= 0 && light > 0; --by)
						{
							var block = c.GetBlock(bx, (byte)by, bz);
							light = (byte)Math.Max(0, light - block.Opacity);
							c.AddSkyLight(bx, (byte)by, bz, light);
						}
					}

				// TODO: Add fading (for things like overhangs)
				// Fading should go horizontal only for sky light, block light goes in all directions (verify this)
			}
		}
	}
}
