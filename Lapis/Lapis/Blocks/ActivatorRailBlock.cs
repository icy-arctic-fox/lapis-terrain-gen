namespace Lapis.Blocks
{
	public class ActivatorRailBlock : UncurvedRailBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.ActivatorRail; }
		}
		#endregion

		/// <summary>
		/// Creates a new activator rail block
		/// </summary>
		public ActivatorRailBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new activator rail block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public ActivatorRailBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new activator rail block
		/// </summary>
		/// <param name="orientation">Direction that the rail is facing</param>
		public ActivatorRailBlock (RailOrientation orientation)
			: base(orientation)
		{
			// ...
		}
	}
}
