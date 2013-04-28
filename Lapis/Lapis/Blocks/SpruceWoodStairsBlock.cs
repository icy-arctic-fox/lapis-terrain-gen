namespace Lapis.Blocks
{
	public class SpruceWoodStairsBlock : StairsBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.SpruceWoodStairs; }
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
		/// Creates a new spruce wood stairs block
		/// </summary>
		public SpruceWoodStairsBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new spruce wood stairs block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public SpruceWoodStairsBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new spruce wood stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		public SpruceWoodStairsBlock (StairsOrientation orientation)
			: base(orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new spruce wood stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		/// <param name="upsideDown">Whether or not the stairs are upside-down</param>
		public SpruceWoodStairsBlock (StairsOrientation orientation, bool upsideDown)
			: base(orientation, upsideDown)
		{
			// ...
		}
	}
}
