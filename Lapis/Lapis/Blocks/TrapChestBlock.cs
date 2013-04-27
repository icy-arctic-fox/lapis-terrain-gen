namespace Lapis.Blocks
{
	public class TrapChestBlock : ChestBlock, IRedstoneSourceBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.TrapChest; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}
		#endregion

		/// <summary>
		/// Whether or not the chest is giving off a redstone current
		/// </summary>
		public bool Powered
		{
			get { return false; }
		}

		/// <summary>
		/// Strength of the redstone current that the chest is giving off
		/// </summary>
		public byte CurrentStrength
		{
			get { return 0; }
		}

		/// <summary>
		/// Creates a new (empty) trap chest block
		/// </summary>
		public TrapChestBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new (empty) trap chest block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public TrapChestBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new (empty) trap chest block
		/// </summary>
		/// <param name="orientation">Direction that the chest is facing</param>
		/// <remarks>The orientation of the chest can't be Up or Down.</remarks>
		public TrapChestBlock (BlockOrientation orientation)
			: base(orientation)
		{
			// ...
		}
	}
}
