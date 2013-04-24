namespace Lapis.Blocks
{
	public class DetectorRailBlock : UncurvedRailBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.DetectorRail; }
		}
		#endregion

		/// <summary>
		/// Creates a new detector rail block
		/// </summary>
		public DetectorRailBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new detector rail block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public DetectorRailBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new detector rail block
		/// </summary>
		/// <param name="orientation">Direction that the rail is facing</param>
		public DetectorRailBlock (RailOrientation orientation)
			: base(orientation)
		{
			// ...
		}
	}
}
