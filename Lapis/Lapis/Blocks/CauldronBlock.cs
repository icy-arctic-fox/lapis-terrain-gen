namespace Lapis.Blocks
{
	public class CauldronBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Cauldron; }
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
			get { return 10f; }
		}
		#endregion

		/// <summary>
		/// Water level contained in the cauldron
		/// </summary>
		public byte WaterLevel
		{
			get { return _data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return "Water: " + WaterLevel; }
		}

		/// <summary>
		/// Creates a new cauldron block
		/// </summary>
		public CauldronBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new cauldron block
		/// </summary>
		/// <param name="level">Water level contained in the cauldron (0-3)</param>
		public CauldronBlock (byte level)
			: base((byte)(level & 0x3))
		{
			// ...
		}
	}
}
