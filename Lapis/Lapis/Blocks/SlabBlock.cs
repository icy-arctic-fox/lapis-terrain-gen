namespace Lapis.Blocks
{
	/// <summary>
	/// Base block class for slabs
	/// </summary>
	public abstract class SlabBlock : Block, IDataBlock
	{
		#region Properties
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
		#endregion

		/// <summary>
		/// Value of the texture shown on the slab as a byte
		/// </summary>
		protected byte TextureData
		{
			get { return (byte)(_data & 0x7); }
		}

		/// <summary>
		/// Whether or not the slab is on the top-half of the block
		/// </summary>
		public bool TopHalf
		{
			get { return (0x8 == (_data & 0x8)); }
		}

		public virtual string DataString
		{
			get { return TopHalf ? "Top" : "Bottom"; }
		}

		/// <summary>
		/// Creates a new slab block
		/// </summary>
		protected SlabBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new slab block
		/// </summary>
		/// <param name="textureData">Value for the slab texture as a byte</param>
		protected SlabBlock (byte textureData)
			: base(textureData)
		{
			// ..
		}

		/// <summary>
		/// Creates a new slab block
		/// </summary>
		/// <param name="textureData">Value for the slab texture as a byte</param>
		/// <param name="upper">Whether or not the slab is on the top-half of the block (upside-down)</param>
		protected SlabBlock (byte textureData, bool upper)
			: base((byte)(textureData | (upper ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
