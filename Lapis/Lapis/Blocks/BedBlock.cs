namespace Lapis.Blocks
{
	public class BedBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Bed; }
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
			get { return 1f; }
		}

		// TODO: Implement NBT data for 'Music'
		#endregion

		/// <summary>
		/// Direction that the bed is facing
		/// </summary>
		public BedOrientation Orientation
		{
			get { return (BedOrientation)(Data & 0x3); }
		}

		/// <summary>
		/// Whether or not the bed is occupied
		/// </summary>
		public bool Occupied
		{
			get { return (0x4 == (Data & 0x4)); }
		}

		/// <summary>
		/// True if the bed is the head block, false if it's the foot block
		/// </summary>
		public bool Head
		{
			get { return (0x8 == (Data & 0x8)); }
		}

		/// <summary>
		/// Creates a new bed block
		/// </summary>
		public BedBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new bed block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public BedBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new bed block
		/// </summary>
		/// <param name="orientation">Direction that the bed is facing</param>
		/// <param name="head">True if the block is the head of the bed, false if it's the foot</param>
		public BedBlock (BedOrientation orientation, bool head = true)
			: base((byte)((byte)orientation | (head ? 0x0 : 0x8)))
		{
			// ...
		}

		/// <summary>
		/// Creates a new bed block
		/// </summary>
		/// <param name="orientation">Direction that the bed is facing</param>
		/// <param name="head">True if the block is the head of the bed, false if it's the foot</param>
		/// <param name="occupied">Whether or not the bed is marked as occupied</param>
		public BedBlock (BedOrientation orientation, bool head, bool occupied = false)
			: base((byte)((byte)orientation | (head ? 0x0 : 0x8) | (occupied ? 0x4 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Direction that the bed can face
		/// </summary>
		public enum BedOrientation : byte
		{
			/// <summary>
			/// Head of the bed is pointing south
			/// </summary>
			South = 0x0,

			/// <summary>
			/// Head of the bed is pointing west
			/// </summary>
			West = 0x1,

			/// <summary>
			/// Head of the bed is pointing north
			/// </summary>
			North = 0x2,

			/// <summary>
			/// Head of the bed is pointing east
			/// </summary>
			East = 0x3
		}
	}
}
