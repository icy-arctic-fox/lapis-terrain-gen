namespace Lapis.Blocks
{
	public class PumpkinStemBlock : CropBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.PumpkinStem; }
		}
		#endregion

		/// <summary>
		/// Creates a new pumpkin stem block
		/// </summary>
		public PumpkinStemBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new pumpkin stem block
		/// </summary>
		/// <param name="stage">Age or stage that the stem is in (from 0 to 7)</param>
		public PumpkinStemBlock (byte stage)
			: base(stage)
		{
			// ...
		}
	}
}
