namespace Lapis.Blocks
{
	public class RailBlock : RailBaseBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Rail; }
		}
		#endregion

		/// <summary>
		/// Direction that the rail is facing
		/// </summary>
		public CurvedRailOrientation Orientation
		{
			get { return (CurvedRailOrientation)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public override string DataString
		{
			get { return Orientation.ToString(); }
		}

		/// <summary>
		/// Creates a new rail block
		/// </summary>
		public RailBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new rail block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public RailBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new rail block
		/// </summary>
		/// <param name="orientation">Direction that the rail block is facing</param>
		public RailBlock (CurvedRailOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}
	}
}
