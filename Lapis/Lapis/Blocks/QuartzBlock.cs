namespace Lapis.Blocks
{
	public class QuartzBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Quartz; }
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
		/// Design that appears on the quartz block
		/// </summary>
		public QuartzTexture Texture
		{
			get { return (QuartzTexture)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Texture.ToString(); }
		}

		/// <summary>
		/// Creates a new quartz block
		/// </summary>
		public QuartzBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new quartz block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public QuartzBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new quartz block
		/// </summary>
		/// <param name="texture">Design that appears on the block</param>
		public QuartzBlock (QuartzTexture texture)
			: base((byte)texture)
		{
			// ...
		}

		/// <summary>
		/// Designs that can appear on the quartz block
		/// </summary>
		public enum QuartzTexture : byte
		{
			/// <summary>
			/// Solid (default) block of quartz
			/// </summary>
			Solid = 0x0,

			/// <summary>
			/// Chiseled design
			/// </summary>
			Chiseled = 0x1,

			/// <summary>
			/// Pillar with lines going vertically
			/// </summary>
			Vertical = 0x2,

			/// <summary>
			/// Pillar with lines going vertically north and south
			/// </summary>
			NorthSouth = 0x3,

			/// <summary>
			/// Pillar with lines going horizontally east and west
			/// </summary>
			EastWest = 0x4
		}
	}
}
