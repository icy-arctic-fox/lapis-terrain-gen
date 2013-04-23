namespace Lapis.Blocks
{
	public class FarmlandBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Farmland; }
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
			get { return 3f; }
		}

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Amount of moisture in the block
		/// </summary>
		public byte Moisture
		{
			get { return Data; }
		}

		/// <summary>
		/// Whether or not the block is dry
		/// </summary>
		public bool Dry
		{
			get { return 0 == Data; }
		}

		/// <summary>
		/// Creates a new farmland block
		/// </summary>
		public FarmlandBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new farmland block
		/// </summary>
		/// <param name="moisture">Amount of moisture in the farmland</param>
		/// <remarks>When <paramref name="moisture"/> is 0, that means it is completely dry.
		/// Values from 1 to 8 can be given based on how far away from water the block is.</remarks>
		public FarmlandBlock (byte moisture)
			: base(moisture)
		{
			// ...
		}
	}
}
