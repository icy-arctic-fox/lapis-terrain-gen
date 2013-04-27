namespace Lapis.Blocks
{
	public class GateBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Gate; }
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
			get { return true; }
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
			get { return 15f; }
		}
		#endregion

		/// <summary>
		/// Direction that the gate is facing
		/// </summary>
		public GateOrientation Orientation
		{
			get { return (GateOrientation)Data; }
		}

		/// <summary>
		/// Whether or not the gate is open
		/// </summary>
		public bool Open
		{
			get { return (0x4 == (Data & 0x4)); }
		}

		/// <summary>
		/// Creates a new gate block
		/// </summary>
		public GateBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gate block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public GateBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gate block
		/// </summary>
		/// <param name="orientation">Direction that the gate is facing</param>
		public GateBlock (GateOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gate block
		/// </summary>
		/// <param name="orientation">Direction that the gate is facing</param>
		/// <param name="open">Whether or not the gate is open</param>
		public GateBlock (GateOrientation orientation, bool open)
			: base((byte)((byte)orientation | (open ? 0x4 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Direction that the gate is facing
		/// </summary>
		/// <remarks>The direction can easily be found by using the direction that the doors point when opened.</remarks>
		public enum GateOrientation : byte
		{
			South = 0x0,
			West  = 0x1,
			North = 0x2,
			East  = 0x3
		}
	}
}
