namespace Lapis.Blocks
{
	public class InactiveComparatorBlock : ComparatorBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.InactiveComparator; }
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
		/// Creates a new inactive comparator block
		/// </summary>
		public InactiveComparatorBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new inactive comparator block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public InactiveComparatorBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
