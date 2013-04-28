namespace Lapis.Blocks
{
	public class StoneButtonBlock : ButtonBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StoneButton; }
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
		/// Creates a new stone button block
		/// </summary>
		public StoneButtonBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone button block
		/// </summary>
		/// <param name="orientation">Direction that the button is facing</param>
		public StoneButtonBlock (ButtonOrientation orientation)
			: base(orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone button block
		/// </summary>
		/// <param name="orientation">Direction that the button is facing</param>
		/// <param name="powered">True if the button is pressed</param>
		public StoneButtonBlock (ButtonOrientation orientation, bool powered)
			: base(orientation, powered)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone button block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public StoneButtonBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
