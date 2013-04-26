namespace Lapis.Blocks
{
	public class DisabledRedstoneTorchBlock : Block, IRedstoneSourceBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.DisabledRedstoneTorch; }
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

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Whether or not the torch is powered and giving off a redstone current
		/// </summary>
		public bool Powered
		{
			get { return false; }
		}

		/// <summary>
		/// Strength of the redstone current that the torch is giving off
		/// </summary>
		public byte CurrentStrength
		{
			get { return 0; }
		}

		/// <summary>
		/// Positioning of the torch
		/// </summary>
		public TorchOrientation Orientation
		{
			get { return (TorchOrientation)Data; }
		}

		/// <summary>
		/// Creates a new disabled redstone torch block
		/// </summary>
		public DisabledRedstoneTorchBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new disabled redstone torch block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public DisabledRedstoneTorchBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new disabled redstone torch block
		/// </summary>
		/// <param name="orientation">Position of the torch</param>
		public DisabledRedstoneTorchBlock (TorchOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}
	}
}
