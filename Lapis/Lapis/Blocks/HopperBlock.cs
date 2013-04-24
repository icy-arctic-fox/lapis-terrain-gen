namespace Lapis.Blocks
{
	public class HopperBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Hopper; }
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
			get { return 15f; }
		}

		// TODO: Implement meta-data values

		// TODO: Implement NBT data for 'Hopper'
		#endregion

		/// <summary>
		/// Direction that the hopper is pointed
		/// </summary>
		public BlockOrientation Orientation
		{
			get { return (BlockOrientation)Data; }
		}

		/// <summary>
		/// Creates a new hopper block
		/// </summary>
		public HopperBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new hopper block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public HopperBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new hopper block
		/// </summary>
		/// <param name="orientation">Direction that the hopper is pointing</param>
		public HopperBlock (BlockOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}
	}
}
