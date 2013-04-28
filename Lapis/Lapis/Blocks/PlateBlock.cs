namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for pressure plate blocks
	/// </summary>
	public abstract class PlateBlock : Block, IRedstoneSourceBlock, IDataBlock
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
		/// True if the plate is pressed and providing power
		/// </summary>
		public bool Powered
		{
			get { return 0x0 != _data; }
		}

		/// <summary>
		/// Strength of the redstone current that the plate is giving off
		/// </summary>
		public virtual byte CurrentStrength
		{
			get { return Powered ? (byte)15 : (byte)0; }
		}

		/// <summary>
		/// Block data value as a string
		/// </summary>
		public string DataString
		{
			get { return Powered ? "Pressed" : "Unpressed"; }
		}

		/// <summary>
		/// Creates a new plate block
		/// </summary>
		protected PlateBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new plate block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		protected PlateBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new plate block
		/// </summary>
		/// <param name="powered">Whether or not the plate is pressed and giving off power</param>
		protected PlateBlock (bool powered)
			: base(powered ? (byte)0x1 : (byte)0x0)
		{
			// ...
		}
	}
}
