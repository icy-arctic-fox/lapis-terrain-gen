namespace Lapis.Blocks
{
	public class MelonStemBlock : CropBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.MelonStem; }
		}
		#endregion

		/// <summary>
		/// Creates a new melon stem block
		/// </summary>
		public MelonStemBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new melon stem block
		/// </summary>
		/// <param name="stage">Age or stage that the stem is in (from 0 to 7)</param>
		public MelonStemBlock (byte stage)
			: base(stage)
		{
			// ...
		}
	}
}
