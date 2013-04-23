namespace Lapis.Blocks
{
	public class PotatoBlock : CropBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Potato; }
		}
		#endregion

		/// <summary>
		/// Creates a new potato block
		/// </summary>
		public PotatoBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new potato block
		/// </summary>
		/// <param name="stage">Age or stage that the crop is in (from 0 to 7)</param>
		public PotatoBlock (byte stage)
			: base(stage)
		{
			// ...
		}
	}
}
