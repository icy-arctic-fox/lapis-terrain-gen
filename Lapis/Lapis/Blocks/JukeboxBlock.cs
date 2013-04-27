namespace Lapis.Blocks
{
	public class JukeboxBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Jukebox; }
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
			get { return 0; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 30f; }
		}

		// TODO: Implement NBT data for 'RecordPlayer'
		#endregion

		/// <summary>
		/// Contents of the jukebox
		/// </summary>
		public JukeboxRecord Contents
		{
			get { return (JukeboxRecord)Data; }
		}

		/// <summary>
		/// Creates a new jukebox block
		/// </summary>
		public JukeboxBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new jukebox block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public JukeboxBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new jukebox block
		/// </summary>
		/// <param name="contents">Contents of the jukebox</param>
		public JukeboxBlock (JukeboxRecord contents)
			: base((byte)contents)
		{
			// ...
		}

		/// <summary>
		/// Record contained in the jukebox
		/// </summary>
		public enum JukeboxRecord : byte
		{
			/// <summary>
			/// No record in the jukebox
			/// </summary>
			None = 0x0,

			/// <summary>
			/// Gold disc - 13
			/// </summary>
			Gold = 0x1,

			/// <summary>
			/// Green disc - cat
			/// </summary>
			Green = 0x2,

			/// <summary>
			/// Orange disc - blocks
			/// </summary>
			Orange = 0x3,

			/// <summary>
			/// Red disc - chirp
			/// </summary>
			Red = 0x4,

			/// <summary>
			/// Lime green disc - far
			/// </summary>
			Lime = 0x5,

			/// <summary>
			/// Purple disc - mall
			/// </summary>
			Purple = 0x6,

			/// <summary>
			/// Violet disc - mellohi
			/// </summary>
			Violet = 0x7,

			/// <summary>
			/// Black disc - stal
			/// </summary>
			Black = 0x8,

			/// <summary>
			/// White disc - strad
			/// </summary>
			White = 0x9,

			/// <summary>
			/// Sea green disc - ward
			/// </summary>
			Sea = 0xa,

			/// <summary>
			/// Broken disc - 11
			/// </summary>
			Broken = 0xb,

			/// <summary>
			/// Blue disc - wait
			/// </summary>
			Blue = 0xc
		}
	}
}
