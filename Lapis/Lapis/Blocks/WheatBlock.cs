namespace Lapis.Blocks
{
	public class WheatBlock : CropBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Wheat; }
		}
		#endregion

		/// <summary>
		/// Creates a new wheat block
		/// </summary>
		public WheatBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wheat block
		/// </summary>
		/// <param name="stage">Age or stage that the crop is in (from 0 to 7)</param>
		public WheatBlock (byte stage)
			: base(stage)
		{
			// ...
		}
	}
}
