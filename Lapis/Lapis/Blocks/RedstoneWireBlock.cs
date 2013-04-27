namespace Lapis.Blocks
{
	public class RedstoneWireBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.RedstoneWire; }
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
		/// Strength of the current in the redstone wire
		/// </summary>
		public byte Strength
		{
			get { return _data; }
		}

		/// <summary>
		/// Creates a new redstone wire block
		/// </summary>
		public RedstoneWireBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new redstone wire block
		/// </summary>
		/// <param name="strength">Strength of the current in the redstone wire</param>
		public RedstoneWireBlock (byte strength)
			: base(strength)
		{
			// ...
		}
	}
}
