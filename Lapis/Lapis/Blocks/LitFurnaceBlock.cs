namespace Lapis.Blocks
{
	public class LitFurnaceBlock : FurnaceBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.LitFurnace; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 13; }
		}
		#endregion

		/// <summary>
		/// Creates a new (empty) lit furnace block
		/// </summary>
		public LitFurnaceBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new (empty) lit furnace block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public LitFurnaceBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new (empty) lit furnace block
		/// </summary>
		/// <param name="orientation">Direction that the furnace is facing</param>
		/// <remarks>The orientation of the furnace can't be Up or Down.</remarks>
		public LitFurnaceBlock (BlockOrientation orientation)
			: base(orientation)
		{
			// ...
		}
	}
}
