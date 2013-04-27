namespace Lapis.Blocks
{
	public class WoodStairsBlock : StairsBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.WoodStairs; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return true; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 15f; }
		}
		#endregion

		/// <summary>
		/// Creates a new wood stairs block
		/// </summary>
		public WoodStairsBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood stairs block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public WoodStairsBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		public WoodStairsBlock (StairsOrientation orientation)
			: base(orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		/// <param name="upsideDown">Whether or not the stairs are upside-down</param>
		public WoodStairsBlock (StairsOrientation orientation, bool upsideDown)
			: base(orientation, upsideDown)
		{
			// ...
		}
	}
}
