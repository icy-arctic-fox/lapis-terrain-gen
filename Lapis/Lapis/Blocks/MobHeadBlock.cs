namespace Lapis.Blocks
{
	public class MobHeadBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.MobHead; }
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
			get { return 0; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 5f; }
		}

		// TODO: Implement NBT data for 'Skull'
		#endregion

		/// <summary>
		/// Position that the mob head is in
		/// </summary>
		public MobHeadPosition Position
		{
			get { return (MobHeadPosition)_data; }
		}

		/// <summary>
		/// Creates a new mob head block
		/// </summary>
		public MobHeadBlock ()
			: base((byte)MobHeadPosition.Floor)
		{
			// ...
		}

		/// <summary>
		/// Creates a new mob head block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public MobHeadBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new mob head block
		/// </summary>
		/// <param name="position">Position of the mob head</param>
		public MobHeadBlock (MobHeadPosition position)
			: base((byte)position)
		{
			// ...
		}

		/// <summary>
		/// Positions that the mob head can have
		/// </summary>
		public enum MobHeadPosition : byte
		{
			/// <summary>
			/// On the floor
			/// </summary>
			/// <remarks>The rotation data is stored in the NBT data.</remarks>
			Floor = 0x1,

			/// <summary>
			/// Against a wall, facing north
			/// </summary>
			North = 0x2,

			/// <summary>
			/// Against a wall, facing south
			/// </summary>
			South = 0x3,

			/// <summary>
			/// Against a wall, facing east
			/// </summary>
			East = 0x4,

			/// <summary>
			/// Against a wall, facing west
			/// </summary>
			West = 0x5
		}
	}
}
