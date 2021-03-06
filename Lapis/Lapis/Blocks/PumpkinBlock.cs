namespace Lapis.Blocks
{
	public class PumpkinBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Pumpkin; }
		}

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return true; }
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
			get { return false; }
		}

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		public override byte Opacity
		{
			get { return 15; }
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
		#endregion

		/// <summary>
		/// Direction that the pumpkin is facing
		/// </summary>
		public PumpkinOrientation Orientation
		{
			get { return (PumpkinOrientation)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Orientation.ToString(); }
		}

		/// <summary>
		/// Creates a new pumpkin block
		/// </summary>
		public PumpkinBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new pumpkin block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public PumpkinBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new pumpkin block
		/// </summary>
		/// <param name="orientation">Direction that the pumpkin is facing</param>
		public PumpkinBlock (PumpkinOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Different directions that a pumpkin can face
		/// </summary>
		public enum PumpkinOrientation : byte
		{
			South = 0x0,
			West  = 0x1,
			North = 0x2,
			East  = 0x3,

			/// <summary>
			/// The jack-o-lantern face won't appear on any sides of the pumpkin
			/// </summary>
			None  = 0x4
		}
	}
}
