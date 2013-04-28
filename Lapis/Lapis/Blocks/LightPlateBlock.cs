namespace Lapis.Blocks
{
	public class LightPlateBlock : PlateBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.LightPlate; }
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
		/// Creates a new light plate block
		/// </summary>
		public LightPlateBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new light plate block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public LightPlateBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new light plate block
		/// </summary>
		/// <param name="powered">Whether or not the plate is pressed and giving off power</param>
		public LightPlateBlock (bool powered)
			: base(powered)
		{
			// ...
		}
	}
}
