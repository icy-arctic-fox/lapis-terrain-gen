namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for all rail types
	/// </summary>
	/// <remarks>All rail blocks derive from this, so a block can be quickly checked if it is a rail by using this type.</remarks>
	public abstract class RailBaseBlock : Block, IDataBlock
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
			get { return 3.5f; }
		}

		#endregion

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public abstract string DataString { get; }

		/// <summary>
		/// Creates a new rail block
		/// </summary>
		protected RailBaseBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new rail block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		protected RailBaseBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
