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
		/// <remarks>The creator of this generator is Michael Miller &lt;icy.arctic.fox@gmail.com&gt;</remarks>
		public string GeneratorAuthor
		{
			get { return "Michael Miller <icy.arctic.fox@gmail.com>"; }
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
					sb.Append(part.Item1);
					sb.Append('x');
					sb.Append(part.Item2);
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
						var type = (BlockType)typeValue;
						uint count = 1;
						if(set.Length > 1)
							uint.TryParse(set[1], out count);
						switch(count)
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
				return builder.GetChunkData();
			}
		}
	}
}
