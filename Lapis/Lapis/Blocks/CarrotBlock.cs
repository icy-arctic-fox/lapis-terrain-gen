namespace Lapis.Blocks
{
	public class CarrotBlock : CropBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Carrot; }
		}
		#endregion

		/// <summary>
		/// Creates a new carrot block
		/// </summary>
		public CarrotBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new carrot block
		/// </summary>
		/// <param name="stage">Age or stage that the crop is in (from 0 to 7)</param>
		public CarrotBlock (byte stage)
			: base(stage)
		{
			// ...
		}
	}
}
