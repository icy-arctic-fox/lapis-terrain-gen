using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public string Name
		{
			get { return "Flatland Terrain Generator"; }
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
			get { return "Generates completely flat and identical terrain"; }
		}
		#endregion

		#region Presets
		/// <summary>
		/// Classic flatlands with grass on top of a couple layers of dirt
		/// </summary>
		public const string ClassicFlat = "7,2x3,2;1";

		/// <summary>
		/// Mostly stone world with a bit of dirt and grass on top
		/// </summary>
		public const string TunnelersDream = "7,230x1,5x3,2";

		/// <summary>
		/// Ocean with a sandy floor
		/// </summary>
		public const string WaterWorld = "7,5x1,5x3,5x12,90x9";

		/// <summary>
		/// Typical vanilla Minecraft overworld, but completely flat
		/// </summary>
		public const string StandardOverworld = "7,59x1,3x3,2";

		/// <summary>
		/// Typical vanilla Minecraft overworld, but completely flat and covered in snow
		/// </summary>
		public const string SnowOverworld = "7,59x1,3x3,2,78";

		/// <summary>
		/// Flat sandy desert
		/// </summary>
		public const string Desert = "7,3x1,52x24,8x12";

		/// <summary>
		/// Flatland area that is ideal for redstone contraptions
		/// </summary>
		public const string RedstoneCanvas = "7,3x1,52x24";
		#endregion

		private readonly BlockType[] _column = new BlockType[Chunk.Height];

		/// <summary>
		/// Options string used to customize the generator
		/// </summary>
		/// <remarks>This string has the format: [Countx]BlockID[,[Countx]BlockID]...
		/// The order is arranged from bottom (0) to top (256).</remarks>
		public string GeneratorOptions
		{
			get
			{
				var parts = new List<Tuple<BlockType, int>>();
				for(var i = 0; i < _column.Length;)
				{
					var type = _column[i];
					int count;
					for(count = 0; i < _column.Length && type == _column[i]; ++i, ++count) {}
					parts.Add(new Tuple<BlockType, int>(type, count));
				}

				var index = parts.Count - 1;
				var last  = parts[index];
				if(BlockType.Air == last.Item1)
					parts.RemoveAt(index);

				var sb = new StringBuilder();
				foreach(var part in parts)
				{
					sb.Append(part.Item2);
					sb.Append('x');
					sb.Append(part.Item1);
					sb.Append(',');
				}
				if(parts.Count > 0) // Remove trailing comma
					sb.Remove(sb.Length - 1, 1);
				return sb.ToString();
			}
		}

		/// <summary>
		/// Initializes the generator to use settings from an option string
		/// </summary>
		/// <param name="options">Options string used to customize the generator</param>
		public void Initialize (string options)
		{
			if(null != options)
			{
				var pos  = 0;
				var sets = options.Split(',')
					.Select(part => part.Split('x'))
					.Where(set => set.Length > 0);
				foreach(var set in sets)
				{
					byte typeValue;
					if(byte.TryParse(set[0], out typeValue))
					{
						byte count = 1;
						if(set.Length > 1)
							if(byte.TryParse(set[1], out count))
							{// Annoyingly, the vanilla Minecraft flatlands generator does things more difficult than the need to be
								var temp  = count;
								count     = typeValue;
								typeValue = temp;
							}
						var type = (BlockType)typeValue;
						switch(set.Length)
						{
						case 1:
							if(_column.Length > pos)
								_column[pos++] = type;
							break;
						case 2:
							for(var i = 0; i < count && pos < _column.Length; ++i, ++pos)
								_column[pos] = type;
							break;
						}
					}
				}
			}
		}

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
						builder.FillColumn(bx, bz, _column);
				// TODO: Light chunk
				return builder.GetChunkData();
			}
		}

		/// <summary>
		/// Sets the blocks used at a specific level
		/// </summary>
		/// <param name="by">Y-offset of the level/block to set</param>
		/// <param name="count">Number of levels/blocks to set (should be at least 1)</param>
		/// <param name="type">Type to set to the blocks to</param>
		public void SetLevel (byte by, byte count, BlockType type)
		{
			var yEnd = by + count;
			if(_column.Length <= yEnd)
				yEnd = _column.Length - 1;
			for(var y = by; y < yEnd; ++y)
				_column[y] = type;
		}
	}
}
