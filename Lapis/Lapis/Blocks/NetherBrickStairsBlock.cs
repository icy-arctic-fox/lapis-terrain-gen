namespace Lapis.Blocks
{
	public class NetherBrickStairsBlock : StairsBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.NetherBrickStairs; }
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

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Creates a new nether brick stairs block
		/// </summary>
		public NetherBrickStairsBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new nether brick stairs block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public NetherBrickStairsBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new nether brick stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		protected NetherBrickStairsBlock (StairsOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new nether brick stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		/// <param name="upsideDown">Whether or not the stairs are upside-down</param>
		protected NetherBrickStairsBlock (StairsOrientation orientation, bool upsideDown)
			: base((byte)((byte)orientation | (upsideDown ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
