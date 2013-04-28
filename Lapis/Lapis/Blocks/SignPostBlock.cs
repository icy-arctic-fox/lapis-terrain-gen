using Lapis.IO.NBT;

namespace Lapis.Blocks
{
	public class SignPostBlock : SignBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.SignPost; }
		}
		#endregion

		/// <summary>
		/// Direction that the sign is facing
		/// </summary>
		public SignOrientation Orientation
		{
			get { return (SignOrientation)_data; }
		}

		/// <summary>
		/// Creates a new sign post block
		/// </summary>
		public SignPostBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sign post block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		/// <param name="tileData">Node that contains the tile entity data</param>
		public SignPostBlock (byte data, Node tileData)
			: base(data, tileData)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sign post block
		/// </summary>
		/// <param name="orientation">Direction that the sign is facing</param>
		public SignPostBlock (SignOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sign post block with text on it
		/// </summary>
		/// <param name="orientation">Direction that the sign is facing</param>
		/// <param name="line1">First line of text</param>
		/// <param name="line2">Second line of text</param>
		/// <param name="line3">Third line of text</param>
		/// <param name="line4">Last line of text</param>
		public SignPostBlock (SignOrientation orientation, string line1, string line2, string line3, string line4)
			: base((byte)orientation, line1, line2, line3, line4)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sign post block with text on it
		/// </summary>
		/// <param name="line1">First line of text</param>
		/// <param name="line2">Second line of text</param>
		/// <param name="line3">Third line of text</param>
		/// <param name="line4">Last line of text</param>
		public SignPostBlock (string line1, string line2, string line3, string line4)
			: base(0, line1, line2, line3, line4)
		{
			// ...
		}

		/// <summary>
		/// Directions that the sign post can face
		/// </summary>
		public enum SignOrientation : byte
		{
			South          = 0x0,
			SouthSouthWest = 0x1,
			SouthWest      = 0x2,
			WestSouthWest  = 0x3,
			West           = 0x4,
			WestNorthWest  = 0x5,
			NorthWest      = 0x6,
			NorthNorthWest = 0x7,
			North          = 0x8,
			NorthNorthEast = 0x9,
			NorthEast      = 0xa,
			EastNorthEast  = 0xb,
			East           = 0xc,
			EastSouthEast  = 0xd,
			SouthEast      = 0xe,
			SouthSouthEast = 0xf
		}
	}
}
