namespace Lapis.Blocks
{
	public class LeavesBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Leaves; }
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
			get { return 2; }
		}

		/// <summary>
		/// Whether or not the block diffuses light
		/// </summary>
		public override bool Diffuse
		{
			get { return true; }
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
			get { return 1f; }
		}
		#endregion

		/// <summary>
		/// Type of tree that the leaves belong to
		/// </summary>
		public TreeType TreeType
		{
			get { return (TreeType)(_data & 0x3); }
		}

		/// <summary>
		/// Whether or not the leaves were placed by a player
		/// </summary>
		/// <remarks>If the leaves were placed by a player, they won't decay.</remarks>
		public bool PlayerPlaced
		{
			get { return 0x4 == (_data & 0x4); }
		}

		/// <summary>
		/// Whether or not the leaves are about to decay
		/// </summary>
		/// <remarks>When true, the leaves should be checked if they need to decay.
		/// The flag is cleared once they have been checked and don't need to decay.</remarks>
		public bool PendingDecay
		{
			get { return 0x8 == (_data & 0x8); }
		}

		/// <summary>
		/// Creates a new leaves block
		/// </summary>
		public LeavesBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new leaves block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public LeavesBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new leaves block
		/// </summary>
		/// <param name="type">Type of tree that the leaves belong to</param>
		public LeavesBlock (TreeType type)
			: base((byte)type)
		{
			// ...
		}

		/// <summary>
		/// Creates a new leaves block
		/// </summary>
		/// <param name="type">Type of tree that the leaves belong to</param>
		/// <param name="playerPlaced">Whether or not the leaves were placed by a player - if true, the leaves won't decay</param>
		public LeavesBlock (TreeType type, bool playerPlaced)
			: base((byte)((byte)type | (playerPlaced ? 0x4 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Creates a new leaves block
		/// </summary>
		/// <param name="type">Type of tree that the leaves belong to</param>
		/// <param name="playerPlaced">Whether or not the leaves were placed by a player - if true, the leaves won't decay</param>
		/// <param name="pending">Whether or not the leaves need to be checked for decay</param>
		public LeavesBlock (TreeType type, bool playerPlaced, bool pending)
			: base((byte)((byte)type | (playerPlaced ? 0x4 : 0x0) | (pending ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
