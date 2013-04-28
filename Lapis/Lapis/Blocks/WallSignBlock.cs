using Lapis.IO.NBT;

namespace Lapis.Blocks
{
	public class WallSignBlock : SignBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.WallSign; }
		}
		#endregion

		/// <summary>
		/// Side of the wall that the sign is attached to
		/// </summary>
		public BlockOrientation Orientation
		{
			get { return (BlockOrientation)_data; }
		}

		/// <summary>
		/// Creates a new wall sign block
		/// </summary>
		public WallSignBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wall sign block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		/// <param name="tileData">Node that contains the tile entity data</param>
		public WallSignBlock (byte data, Node tileData)
			: base(data, tileData)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wall sign block
		/// </summary>
		/// <param name="orientation">Side of the wall that the sign is attached to</param>
		/// <remarks>The orientation of the sign can't be Up or Down.</remarks>
		public WallSignBlock (BlockOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wall sign block with text on it
		/// </summary>
		/// <param name="orientation">Side of the wall that the sign is attached to</param>
		/// <param name="line1">First line of text</param>
		/// <param name="line2">Second line of text</param>
		/// <param name="line3">Third line of text</param>
		/// <param name="line4">Last line of text</param>
		public WallSignBlock (BlockOrientation orientation, string line1, string line2, string line3, string line4)
			: base((byte)orientation, line1, line2, line3, line4)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wall sign block with text on it
		/// </summary>
		/// <param name="line1">First line of text</param>
		/// <param name="line2">Second line of text</param>
		/// <param name="line3">Third line of text</param>
		/// <param name="line4">Last line of text</param>
		public WallSignBlock (string line1, string line2, string line3, string line4)
			: base(0, line1, line2, line3, line4)
		{
			// ...
		}
	}
}
