namespace Lapis.Blocks
{
	public class StonePlateBlock : PlateBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StonePlate; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 2.5f; }
		}
		#endregion

		/// <summary>
		/// Creates a new stone plate block
		/// </summary>
		public StonePlateBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone plate block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public StonePlateBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone plate block
		/// </summary>
		/// <param name="powered">Whether or not the plate is pressed and giving off power</param>
		public StonePlateBlock (bool powered)
			: base(powered)
		{
			// ...
		}
	}
}
