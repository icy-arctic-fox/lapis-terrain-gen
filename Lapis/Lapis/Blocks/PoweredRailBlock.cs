namespace Lapis.Blocks
{
	public class PoweredRailBlock : UncurvedRailBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.PoweredRail; }
		}
		#endregion

		/// <summary>
		/// Creates a new powered rail block
		/// </summary>
		public PoweredRailBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new powered rail block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public PoweredRailBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new powered rail block
		/// </summary>
		/// <param name="orientation">Direction that the rail is facing</param>
		public PoweredRailBlock (RailOrientation orientation)
			: base(orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new powered rail block
		/// </summary>
		/// <param name="orientation">Direction that the rail is facing</param>
		/// <param name="powered">Whether or not the rail is powered</param>
		public PoweredRailBlock (RailOrientation orientation, bool powered)
			: base((byte)((byte)orientation | (powered ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
