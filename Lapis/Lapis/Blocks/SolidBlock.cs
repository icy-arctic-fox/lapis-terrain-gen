namespace Lapis.Blocks
{
	/// <summary>
	/// A solid block that cannot be passed through
	/// </summary>
	public abstract class SolidBlock : Block
	{
		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		/// <remarks>This value is true for all block types that inherit this class.</remarks>
		public override bool IsSolid
		{
			get { return true; }
		}

		/// <summary>
		/// Creates a new solid block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		protected SolidBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
