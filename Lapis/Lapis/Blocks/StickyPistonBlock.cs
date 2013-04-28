namespace Lapis.Blocks
{
	public class StickyPistonBlock : PistonBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StickyPiston; }
		}
		#endregion

		/// <summary>
		/// Creates a new sticky piston block
		/// </summary>
		public StickyPistonBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new sticky piston block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public StickyPistonBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sticky piston block
		/// </summary>
		/// <param name="orientation">Direction that the piston is facing</param>
		public StickyPistonBlock (PistonOrientation orientation)
			: base(orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new sticky piston block
		/// </summary>
		/// <param name="orientation">Direction that the piston is facing</param>
		/// <param name="extended">Whether or not the piston is extended</param>
		public StickyPistonBlock (PistonOrientation orientation, bool extended)
			: base(orientation, extended)
		{
			// ...
		}
	}
}
