namespace Lapis.Blocks
{
	public class StoneSlabBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StoneSlab; }
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
			get { return 30f; }
		}

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Type of stone slab
		/// </summary>
		public SlabTexture SlabType
		{
			get { return (SlabTexture)(Data & 0x7); }
		}

		/// <summary>
		/// Whether or not the slab is on the top-half of the block
		/// </summary>
		public bool UpsideDown
		{
			get { return (0x8 == (Data & 0x8)); }
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		public StoneSlabBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public StoneSlabBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		/// <param name="type">Type of stone slab</param>
		public StoneSlabBlock (SlabTexture type)
			: base((byte)type)
		{
			// ..
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		/// <param name="type">Type of stone slab</param>
		/// <param name="upper">Whether or not the slab is on the top-half of the block (upside-down)</param>
		public StoneSlabBlock (SlabTexture type, bool upper)
			: base((byte)((byte)type | (upper ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
