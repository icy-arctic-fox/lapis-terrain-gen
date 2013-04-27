namespace Lapis.Blocks
{
	public class NetherWartBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.NetherWart; }
		}

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return false; }
		}

		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		public override bool Physics
		{
			get { return false; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		public override byte Opacity
		{
			get { return 0; }
		}

		/// <summary>
		/// Whether or not the block diffuses light
		/// </summary>
		public override bool Diffuse
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 0; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 0f; }
		}
		#endregion

		/// <summary>
		/// Stage of growth the nether wart is in (0-3)
		/// </summary>
		public byte Stage
		{
			get { return Data; }
		}

		/// <summary>
		/// Creates a new nether wart block
		/// </summary>
		public NetherWartBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new nether wart block
		/// </summary>
		/// <param name="stage">Stage of growth the nether wart is in (0-3)</param>
		public NetherWartBlock (byte stage)
			: base((byte)(stage & 0x3))
		{
			// ...
		}
	}
}
