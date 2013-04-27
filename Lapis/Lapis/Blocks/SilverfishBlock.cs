namespace Lapis.Blocks
{
	public class SilverfishBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Silverfish; }
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
			get { return 3.75f; }
		}

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Texture that appears on the silverfish block
		/// </summary>
		public SilverfishBlockTexture Texture
		{
			get { return (SilverfishBlockTexture)Data; }
		}

		/// <summary>
		/// Creates a new silverfish block
		/// </summary>
		public SilverfishBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new silverfish block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public SilverfishBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new silverfish block
		/// </summary>
		/// <param name="texture">Texture that appears on the silverfish block</param>
		public SilverfishBlock (SilverfishBlockTexture texture)
			: base((byte)texture)
		{
			// ...
		}

		/// <summary>
		/// Textures that a silverfish block can have
		/// </summary>
		public enum SilverfishBlockTexture : byte
		{
			/// <summary>
			/// Smooth stone appearance
			/// </summary>
			Stone = 0x0,

			/// <summary>
			/// Cobblestone appearance
			/// </summary>
			Cobblestone = 0x1,

			/// <summary>
			/// Smooth stone brick appearance
			/// </summary>
			StoneBrick = 0x2
		}
	}
}
