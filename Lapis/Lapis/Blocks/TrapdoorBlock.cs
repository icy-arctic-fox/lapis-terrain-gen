namespace Lapis.Blocks
{
	public class TrapdoorBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Trapdoor; }
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
		/// True if the trapdoor is on the top half of the block,
		/// false if it is on the bottom half of the block
		/// </summary>
		public bool Upper
		{
			get { return (0x8 == (_data & 0x8)); }
		}

		/// <summary>
		/// Whether or not the trapdoor is open
		/// </summary>
		public bool Open
		{
			get { return (0x4 == (_data & 0x4)); }
		}

		/// <summary>
		/// Wall that the trapdoor is attached to
		/// </summary>
		public TrapdoorOrientation Orientation
		{
			get { return (TrapdoorOrientation)(_data & 0x3); }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return (Upper ? "Upper " : "Lower ") + (Open ? "Open " : "Closed ") + Orientation.ToString(); }
		}

		/// <summary>
		/// Creates a new trapdoor block
		/// </summary>
		public TrapdoorBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new trapdoor block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public TrapdoorBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new trapdoor block
		/// </summary>
		/// <param name="orientation">Wall that the trapdoor is attached to</param>
		/// <param name="topHalf">True if the trapdoor is on the top half of the block</param>
		/// <param name="open">Whether or not the trapdoor is open</param>
		public TrapdoorBlock (TrapdoorOrientation orientation, bool topHalf, bool open)
			: base((byte)((byte)orientation | (open ? 0x4 : 0x0) | (topHalf ? 0x8 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Wall that the trapdoor is attached to
		/// </summary>
		public enum TrapdoorOrientation : byte
		{
			South = 0x0,
			North = 0x1,
			East  = 0x2,
			West  = 0x3
		}
	}
}
