using System;

namespace Lapis.Level.Generation.Population
{
	/// <summary>
	/// Populates a chunk with sky light
	/// </summary>
	/// <remarks>Air blocks above the column height are filled with full brightness (15).
	/// Blocks below under-hangs effectively become the minimum horizontal distance to the sky subtracted from the maximum brightness.
	/// The algorithm for lighting goes as follows:
	/// <list type="ordered">
	/// <item>From the top of the chunk to the highest non-air block, set the sky light to 15.
	/// Chunks that are completely empty can be skipped since they won't be saved anyways.</item>
	/// <item>For non-air blocks, subtract the block's opacity from the current light level (15 for the top-most air block) and apply it to the current block.</item>
	/// <item>When the bottom of the chunk or the light level is 0, stop.</item>
	/// <item>While traversing down the column, check the surrounding columns.
	/// Calculate the light level as: Current column's light level - 1 - neighboring block's opacity.</item>
	/// <item>If the neighboring block has a lower light level, set it to the calculated value - do NOT add it.</item>
	/// <item>Repeat the horizontal movement along columns until the light level is 0 or a column with a higher light level is found.</item>
	/// </list></remarks>
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
				c.ClearSkyLight();

				for(var bx = (byte)0; bx < Chunk.Size; ++bx)
					for(var bz = (byte)0; bz < Chunk.Size; ++bz)
					{
						var blockHeight = c.GetHighestBlockAt(bx, bz); // Index of the highest non-air block

						if(blockHeight >= 0 || blockHeight < Chunk.Height - 1) // TODO: Should we be filling and empty column? If so, remove the first condition.
						{// Quickly fill air blocks
							var sectionCount = blockHeight / Chunk.SectionHeight + 1; // Number of chunk sections that contain blocks
							var maxHeight    = sectionCount * Chunk.SectionHeight; // Height (+1) of the highest air block to fill
							for(var by = maxHeight; by > blockHeight; --by)
								c.SetSkyLight(bx, (byte)by, bz, Chunk.FullBrightness); // TODO: Add a method like SetSpan(startIndex, endIndex) to NibbleArray
						}

						// Reduce amount of sky light through semi-transparent blocks
						var light = Chunk.FullBrightness;
						for(var by = blockHeight; by >= 0 && light > 0; --by)
						{
							var block = c.GetBlock(bx, (byte)by, bz);
							light = (byte)Math.Max(0, light - block.Opacity);
							c.AddSkyLight(bx, (byte)by, bz, light);
						}
					}

				// TODO: Account for diffuse blocks

				// TODO: Add fading (for things like overhangs)
				// Fading should go horizontal only for sky light, block light goes in all directions (verify this)
			}
		}
	}
}
