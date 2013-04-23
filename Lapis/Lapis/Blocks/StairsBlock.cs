namespace Lapis.Blocks
{
	/// <summary>
	/// Base for all stair block types
	/// </summary>
	public abstract class StairsBlock : Block
	{
		#region Properties
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
		/// Direction that the stairs are facing
		/// </summary>
		public StairsOrientation Orientation
		{
			get { return (StairsOrientation)(Data & 0x7); }
		}

		/// <summary>
		/// Whether or not the stairs are upside-down
		/// </summary>
		public bool UpsideDown
		{
			get { return (0x8 == (Data & 0x8)); }
		}

		/// <summary>
		/// Creates a new stairs block
		/// </summary>
		protected StairsBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stairs block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		protected StairsBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		protected StairsBlock (StairsOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stairs block
		/// </summary>
		/// <param name="orientation">Direction that the stairs are facing</param>
		/// <param name="upsideDown">Whether or not the stairs are upside-down</param>
		protected StairsBlock (StairsOrientation orientation, bool upsideDown)
			: base((byte)((byte)orientation | (upsideDown ? 0x8 : 0x0)))
		{
			// ...
		}
	}
}
