namespace Lapis.Blocks
{
	public class DisabledRedstoneTorchBlock : RedstoneTorchBlock
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
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 0; }
		}
		#endregion

		/// <summary>
		/// Whether or not the torch is powered and giving off a redstone current
		/// </summary>
		public override bool Powered
		{
			get { return false; }
		}

		/// <summary>
		/// Strength of the redstone current that the torch is giving off
		/// </summary>
		public override byte CurrentStrength
		{
			get { return 0; }
		}

		/// <summary>
		/// Creates a new disabled redstone torch block
		/// </summary>
		public DisabledRedstoneTorchBlock ()
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
			: base(orientation)
		{
			// ...
		}
	}
}
