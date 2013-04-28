namespace Lapis.Blocks
{
	public class WoolBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Wool; }
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
			get { return true; }
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
			get { return 4f; }
		}
		#endregion

		/// <summary>
		/// Color of the wool
		/// </summary>
		public WoolColor Color
		{
			get { return (WoolColor)_data; }
		}

		/// <summary>
		/// Block data value as a string
		/// </summary>
		public string DataString
		{
			get { return Color.ToString(); }
		}

		/// <summary>
		/// Creates a new wool block
		/// </summary>
		public WoolBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wool block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public WoolBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wool block
		/// </summary>
		/// <param name="color">Wool color</param>
		public WoolBlock (WoolColor color)
			: base((byte)color)
		{
			// ...
		}

		/// <summary>
		/// Colors of wool available
		/// </summary>
		public enum WoolColor : byte
		{
			White     = 0x0,
			Orange    = 0x1,
			Magenta   = 0x2,
			LightBlue = 0x3,
			Yellow    = 0x4,
			Lime      = 0x5,
			Pink      = 0x6,
			Gray      = 0x7,
			LightGray = 0x8,
			Cyan      = 0x9,
			Purple    = 0xa,
			Blue      = 0xb,
			Brown     = 0xc,
			Green     = 0xd,
			Red       = 0xe,
			Black     = 0xf
		}
	}
}
