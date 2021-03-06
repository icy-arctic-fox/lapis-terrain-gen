namespace Lapis.Blocks
{
	public class DoubleStoneSlabBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.DoubleStoneSlab; }
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
			get { return 30f; }
		}
		#endregion

		/// <summary>
		/// Type of stone slab
		/// </summary>
		public SlabTexture SlabType
		{
			get { return (SlabTexture)(_data & 0x7); }
		}

		/// <summary>
		/// When true, the "top" texture will be displayed on all sides of the block
		/// </summary>
		public bool AlternateTexture
		{
			get { return (0x8 == (_data & 0x8)); }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get
			{
				var str = SlabType.ToString();
				if(AlternateTexture)
					str += " Alternate";
				return str;
			}
		}

		/// <summary>
		/// Creates a new double stone slab block
		/// </summary>
		public DoubleStoneSlabBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new double stone slab block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public DoubleStoneSlabBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new double stone slab block
		/// </summary>
		/// <param name="type">Type of stone slab</param>
		public DoubleStoneSlabBlock (SlabTexture type)
			: base((byte)type)
		{
			// ..
		}

		/// <summary>
		/// Creates a new double stone slab block
		/// </summary>
		/// <param name="type">Type of stone slab</param>
		/// <param name="texture">If true, the block will have the "top" texture on all sides of the block</param>
		public DoubleStoneSlabBlock (SlabTexture type, bool texture)
			: base((byte)((byte)type | (texture ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
