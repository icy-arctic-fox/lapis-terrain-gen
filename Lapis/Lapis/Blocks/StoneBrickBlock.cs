namespace Lapis.Blocks
{
	public class StoneBrickBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StoneBrick; }
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
		/// Texture that appears on the block
		/// </summary>
		public StoneBrickTexture Texture
		{
			get { return (StoneBrickTexture)_data; }
		}

		/// <summary>
		/// Creates a new stone brick block
		/// </summary>
		public StoneBrickBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone brick block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public StoneBrickBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone brick block
		/// </summary>
		/// <param name="texture">Texture that appears on the block</param>
		public StoneBrickBlock (StoneBrickTexture texture)
			: base((byte)texture)
		{
			// ...
		}

		/// <summary>
		/// Types of textures that can appear on the stone brick block
		/// </summary>
		public enum StoneBrickTexture : byte
		{
			Normal   = 0x0,
			Mossy    = 0x1,
			Cracked  = 0x2,
			Chiseled = 0x3
		}
	}
}
