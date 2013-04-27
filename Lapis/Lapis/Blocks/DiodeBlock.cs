namespace Lapis.Blocks
{
	public class DiodeBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Diode; }
		}

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return true; }
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
			get { return 15; }
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
			get { return 9; }
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
		/// Direction that the diode is pointing
		/// </summary>
		public DiodeOrientation Orientation
		{
			get { return (DiodeOrientation)(_data & 0x3); }
		}

		/// <summary>
		/// Delay that the diode is set to (1-4)
		/// </summary>
		public byte Delay
		{
			get { return (byte)(((_data >> 2) & 0x3) + 1); }
		}

		/// <summary>
		/// Creates a new diode block
		/// </summary>
		public DiodeBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diode block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public DiodeBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diode block
		/// </summary>
		/// <param name="orientation">Direction that the diode block is pointing</param>
		/// <param name="delay">Delay that the diode is set to (1-4)</param>
		public DiodeBlock (DiodeOrientation orientation, byte delay)
			: base((byte)((byte)orientation | (delay - 1) << 2))
		{
			// ...
		}

		/// <summary>
		/// Different directions that the diode can face
		/// </summary>
		public enum DiodeOrientation : byte
		{
			North = 0x0,
			East  = 0x1,
			South = 0x2,
			West  = 0x3
		}
	}
}
