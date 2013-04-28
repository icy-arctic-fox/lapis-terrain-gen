namespace Lapis.Blocks
{
	public class GlowingRedstoneOreBlock : RedstoneOreBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.GlowingRedstoneOre; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 9; }
		}
		#endregion

		/// <summary>
		/// Creates a new glowing redstone ore block
		/// </summary>
		public GlowingRedstoneOreBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new glowing redstone ore block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		protected GlowingRedstoneOreBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
