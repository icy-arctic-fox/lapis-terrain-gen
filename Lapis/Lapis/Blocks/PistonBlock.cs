namespace Lapis.Blocks
{
	public class PistonBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Piston; }
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
			get { return 2.5f; }
		}
		#endregion

		/// <summary>
		/// Direction that the piston is facing
		/// </summary>
		public PistonOrientation Orientation
		{
			get { return (PistonOrientation)(_data & 0x7); }
		}

		/// <summary>
		/// Whether or not the piston is extended
		/// </summary>
		public bool Extended
		{
			get { return (0x8 == (_data & 0x8)); }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return (Extended ? "Extended " : "Contracted ") + Orientation.ToString(); }
		}

		/// <summary>
		/// Creates a new piston block
		/// </summary>
		public PistonBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new piston block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public PistonBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new piston block
		/// </summary>
		/// <param name="orientation">Direction that the piston is facing</param>
		public PistonBlock (PistonOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new piston block
		/// </summary>
		/// <param name="orientation">Direction that the piston is facing</param>
		/// <param name="extended">Whether or not the piston is extended</param>
		public PistonBlock (PistonOrientation orientation, bool extended)
			: base((byte)((byte)orientation | (extended ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
