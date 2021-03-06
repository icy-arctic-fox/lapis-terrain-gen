namespace Lapis.Blocks
{
	public class SnowCoverBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.SnowCover; }
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
			get { return 0.5f; }
		}
		#endregion

		/// <summary>
		/// Height of the snow
		/// </summary>
		public byte Height
		{
			get { return (byte)(_data & 0x7); }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return "Height: " + Height.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}

		/// <summary>
		/// Creates a new snow cover block
		/// </summary>
		public SnowCoverBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new snow cover block
		/// </summary>
		/// <param name="height">Height of the snow block (0-7)</param>
		public SnowCoverBlock (byte height)
			: base((byte)(height & 0x7))
		{
			// ...
		}
	}
}
