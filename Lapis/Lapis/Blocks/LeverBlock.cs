namespace Lapis.Blocks
{
	public class LeverBlock : Block, IRedstoneSourceBlock, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Lever; }
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
			get { return 2.5f; }
		}
		#endregion

		/// <summary>
		/// Direction that the lever is facing
		/// </summary>
		public LeverOrientation Orientation
		{
			get { return (LeverOrientation)(_data & 0x7); }
		}

		/// <summary>
		/// Whether or not the lever is providing power
		/// </summary>
		public bool Powered
		{
			get { return (0x8 == (_data & 0x8)); }
		}

		/// <summary>
		/// Strength of the redstone current that the block provides
		/// </summary>
		public byte CurrentStrength
		{
			get { return 15; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Orientation.ToString() + (Powered ? " On" : " Off"); }
		}

		/// <summary>
		/// Creates a new lever block
		/// </summary>
		public LeverBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new lever block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public LeverBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new lever block
		/// </summary>
		/// <param name="orientation">Direction that the lever is facing</param>
		public LeverBlock (LeverOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new lever block
		/// </summary>
		/// <param name="orientation">Direction that the lever is facing</param>
		/// <param name="powered">Whether or not the lever is switched on and providing power</param>
		public LeverBlock (LeverOrientation orientation, bool powered)
			: base((byte)((byte)orientation | (powered ? 0x8 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Directions that a lever can be facing
		/// </summary>
		/// <remarks>These directions are the direction that the handle points when turned off.</remarks>
		public enum LeverOrientation : byte
		{
			/// <summary>
			/// Upside-down and pointing east
			/// </summary>
			CelingEast = 0x0,

			/// <summary>
			/// Horizontally on a wall and pointing east
			/// </summary>
			WallEast = 0x1,

			/// <summary>
			/// Horizontally on a wall and pointing west
			/// </summary>
			WallWest = 0x2,

			/// <summary>
			/// Vertically on a wall and pointing down
			/// </summary>
			WallDown = 0x3,

			/// <summary>
			/// Vertically on a wall and pointing up
			/// </summary>
			WallNorth = 0x4,

			/// <summary>
			/// On the ground and pointing south
			/// </summary>
			GroundSouth = 0x5,

			/// <summary>
			/// On the ground and pointing east
			/// </summary>
			GroundEast = 0x6,

			/// <summary>
			/// Upside-down and pointing south
			/// </summary>
			CeilingSouth = 0x7
		}
	}
}
