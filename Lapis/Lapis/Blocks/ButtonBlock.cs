namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for button blocks
	/// </summary>
	public abstract class ButtonBlock : Block, IRedstoneSourceBlock
	{
		#region Properties
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
		#endregion

		/// <summary>
		/// Direction that the button is facing
		/// </summary>
		public ButtonOrientation Orientation
		{
			get { return (ButtonOrientation)(Data & 0x7); }
		}

		/// <summary>
		/// Whether or not the button is pressed and providing a redstone current
		/// </summary>
		public bool Powered
		{
			get { return (0x8 == (Data & 0x8)); }
		}

		/// <summary>
		/// Strength of the redstone current given off by the button
		/// </summary>
		public byte CurrentStrength
		{
			get { return Powered ? (byte)15 : (byte)0; }
		}

		/// <summary>
		/// Creates a new button block
		/// </summary>
		protected ButtonBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new button block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		protected ButtonBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new button block
		/// </summary>
		/// <param name="orientation">Direction that the button is facing</param>
		protected ButtonBlock (ButtonOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new button block
		/// </summary>
		/// <param name="orientation">Direction that the button is facing</param>
		/// <param name="powered">True if the button is pressed</param>
		protected ButtonBlock (ButtonOrientation orientation, bool powered)
			: base((byte)((byte)orientation | (powered ? 0x8 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Different orientations that a button can face
		/// </summary>
		public enum ButtonOrientation : byte
		{
			East  = 0x1,
			West  = 0x2,
			South = 0x3,
			North = 0x4
		}
	}
}
