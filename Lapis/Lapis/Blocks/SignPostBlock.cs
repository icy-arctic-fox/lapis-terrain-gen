namespace Lapis.Blocks
{
	public class SignPostBlock : Block
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

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return false; }
		}

		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		public override bool Physics
		{
			get { return false; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return true; }
		}

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		public override byte Opacity
		{
			get { return 0; }
		}

		/// <summary>
		/// Whether or not the block diffuses light
		/// </summary>
		public override bool Diffuse
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 0; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 5f; }
		}

		// TODO: Implement meta-data values

		// TODO: Implement NBT data for 'Sign'
		#endregion

		/// <summary>
		/// Direction that the sign is facing
		/// </summary>
		public SignOrientation Orientation
		{
			get { return (SignOrientation)Data; }
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
		public SignPostBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sign post block
		/// </summary>
		/// <param name="orientation">Additional meta-data for the block</param>
		public SignPostBlock (SignOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Directions that the sign post can face
		/// </summary>
		public enum SignOrientation : byte
		{
			South = 0x0,

			SouthSouthWest = 0x1,

			SouthWest = 0x2,

			WestSouthWest = 0x3,

			West = 0x4,

			WestNorthWest = 0x5,

			NorthWest = 0x6,

			NorthNorthWest = 0x7,

			North = 0x8,

			NorthNorthEast = 0x9,

			NorthEast = 0xa,

			EastNorthEast = 0xb,

			East = 0xc,

			EastSouthEast = 0xd,

			SouthEast = 0xe,

			SouthSouthEast = 0xf
		}
	}
}
