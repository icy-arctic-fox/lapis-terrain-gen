namespace Lapis.Blocks
{
	public class StoneBrickStairsBlock : StairsBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StoneBrickStairs; }
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
			get { return 30f; }
		}
		#endregion

		/// <summary>
		/// Creates a new stone brick stairs block
		/// </summary>
		public StoneBrickStairsBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone brick stairs block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public StoneBrickStairsBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone brick stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		public StoneBrickStairsBlock (StairsOrientation orientation)
			: base(orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone brick stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		/// <param name="upsideDown">Whether or not the stairs are upside-down</param>
		public StoneBrickStairsBlock (StairsOrientation orientation, bool upsideDown)
			: base(orientation, upsideDown)
		{
			// ...
		}
	}
}
