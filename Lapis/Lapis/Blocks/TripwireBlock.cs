namespace Lapis.Blocks
{
	public class TripwireBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Tripwire; }
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
			get { return false; }
		}

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		public override int Opacity
		{
			get { return 0; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override int Luminance
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

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Creates a new tripwire block
		/// </summary>
		public TripwireBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tripwire block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public TripwireBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
