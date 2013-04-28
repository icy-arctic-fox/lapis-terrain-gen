namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for all growable crop blocks
	/// </summary>
	public abstract class CropBlock : Block, IDataBlock
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
			get { return 0f; }
		}
		#endregion

		/// <summary>
		/// Stage of growth that the crop is in (from 0 to 7)
		/// </summary>
		public byte Stage
		{
			get { return (byte)(_data & 0x7); }
		}

		/// <summary>
		/// Whether or not the crop is fully grown
		/// </summary>
		public bool FullyGrown
		{
			get { return _data >= 0x7; }
		}

		/// <summary>
		/// Block data value as a string
		/// </summary>
		public string DataString
		{
			get { return Stage.ToString(System.Globalization.CultureInfo.InvariantCulture) + "/7"; }
		}

		/// <summary>
		/// Creates a new crop block
		/// </summary>
		protected CropBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new crop block
		/// </summary>
		/// <param name="stage">Age or stage that the crop is in (from 0 to 7)</param>
		protected CropBlock (byte stage)
			: base(stage)
		{
			// ...
		}
	}
}
