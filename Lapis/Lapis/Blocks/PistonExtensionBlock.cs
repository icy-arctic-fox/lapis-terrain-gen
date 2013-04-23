namespace Lapis.Blocks
{
	public class PistonExtensionBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.PistonExtension; }
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

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Direction that the piston extension is facing
		/// </summary>
		public PistonOrientation Orientation
		{
			get { return (PistonOrientation)(Data & 0x7); }
		}

		/// <summary>
		/// Whether or not the piston extension is sticky (belongs to a sticky piston)
		/// </summary>
		public bool Sticky
		{
			get { return (0x8 == (Data & 0x8)); }
		}

		/// <summary>
		/// Creates a new piston extension block
		/// </summary>
		public PistonExtensionBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new piston extension block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public PistonExtensionBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new piston extension block
		/// </summary>
		/// <param name="orientation">Direction that the piston is facing</param>
		public PistonExtensionBlock (PistonOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new piston extension block
		/// </summary>
		/// <param name="orientation">Direction that the piston is facing</param>
		/// <param name="sticky">Whether or not the piston extension is sticky (belongs to a sticky piston)</param>
		public PistonExtensionBlock (PistonOrientation orientation, bool sticky)
			: base((byte)((byte)orientation | (sticky ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
