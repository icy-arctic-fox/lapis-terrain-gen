namespace Lapis.Blocks
{
	public class EnderChestBlock : ChestBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.EnderChest; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 7; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 3000f; }
		}

		// TODO: Implement NBT data for 'EnderChest'
		#endregion

		/// <summary>
		/// Creates a new (empty) ender chest block
		/// </summary>
		public EnderChestBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new (empty) ender chest block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public EnderChestBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new (empty) ender chest block
		/// </summary>
		/// <param name="orientation">Direction that the chest is facing</param>
		/// <remarks>The orientation of the chest can't be Up or Down.</remarks>
		public EnderChestBlock (BlockOrientation orientation)
			: base(orientation)
		{
			// ...
		}
	}
}
