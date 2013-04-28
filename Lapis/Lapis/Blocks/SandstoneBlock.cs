namespace Lapis.Blocks
{
	public class SandstoneBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Sandstone; }
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
			get { return 4f; }
		}
		#endregion

		/// <summary>
		/// Texture that appears on the sandstone block
		/// </summary>
		public SandstoneTexture Texture
		{
			get { return (SandstoneTexture)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Texture.ToString(); }
		}

		/// <summary>
		/// Creates a new sandstone block
		/// </summary>
		public SandstoneBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sandstone block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public SandstoneBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sandstone block
		/// </summary>
		/// <param name="texture">Texture that appears on the sandstone block</param>
		public SandstoneBlock (SandstoneTexture texture)
			: base((byte)texture)
		{
			// ...
		}

		/// <summary>
		/// Different types of sandstone textures
		/// </summary>
		public enum SandstoneTexture : byte
		{
			Normal = 0x0,
			Chiseled = 0x1,
			Smooth = 0x2
		}
	}
}
