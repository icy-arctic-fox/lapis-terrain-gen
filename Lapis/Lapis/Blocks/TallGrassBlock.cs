namespace Lapis.Blocks
{
	public class TallGrassBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.TallGrass; }
		}

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return false; }
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
			get { return 0f; }
		}
		#endregion

		/// <summary>
		/// Type of shrub that is displayed
		/// </summary>
		public ShrubType Shrub
		{
			get { return (ShrubType)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Shrub.ToString(); }
		}

		/// <summary>
		/// Creates a new tall grass block
		/// </summary>
		public TallGrassBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tall grass block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public TallGrassBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tall grass block
		/// </summary>
		/// <param name="type">Type of shrub to display</param>
		public TallGrassBlock (ShrubType type = ShrubType.Grass)
			: base((byte)type)
		{
			// ...
		}

		/// <summary>
		/// Types of tall shrubs displayed for tall grass
		/// </summary>
		public enum ShrubType : byte
		{
			/// <summary>
			/// Identical to the Dead Bush block, but behaves like a fern
			/// </summary>
			/// <seealso cref="DeadBushBlock"/>
			DeadShrub = 0x0,

			Grass = 0x1,

			Fern = 0x2
		}
	}
}
