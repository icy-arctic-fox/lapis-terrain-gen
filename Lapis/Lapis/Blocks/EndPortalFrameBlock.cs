namespace Lapis.Blocks
{
	public class EndPortalFrameBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.EndPortalFrame; }
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
			get { return 1; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 18000000f; }
		}
		#endregion

		/// <summary>
		/// Direction that the portal frame block is facing
		/// </summary>
		public PortalFrameOrientation Orientation
		{
			get { return (PortalFrameOrientation)(_data & 0x3); }
		}

		/// <summary>
		/// Whether or not the block contains an eye of ender
		/// </summary>
		public bool ContainsEye
		{
			get { return (0x4 == (_data & 0x4)); }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Orientation + (ContainsEye ? " Eye" : " Empty"); }
		}

		/// <summary>
		/// Creates a new end portal frame block
		/// </summary>
		public EndPortalFrameBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new end portal frame block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public EndPortalFrameBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new end portal block
		/// </summary>
		/// <param name="orientation">Direction that the portal block is facing</param>
		/// <param name="eye">True if there is an eye of ender in the block</param>
		public EndPortalFrameBlock (PortalFrameOrientation orientation, bool eye)
			: base((byte)((byte)orientation | (eye ? 0x4 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Directions that a portal frame block can be facing
		/// </summary>
		/// <remarks>All portal frame blocks in the frame must point to the center for the portal to work.</remarks>
		public enum PortalFrameOrientation : byte
		{
			South = 0x0,
			West  = 0x1,
			North = 0x2,
			East  = 0x3
		}
	}
}
