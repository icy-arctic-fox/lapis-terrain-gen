using System;

namespace Lapis.Blocks
{
	public class TripwireBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Tripwire; }
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
			get { return 0f; }
		}
		#endregion

		/// <summary>
		/// State that the tripwire is in
		/// </summary>
		public TripwireState State
		{
			get { return (TripwireState)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return State.ToString(); }
		}

		/// <summary>
		/// Creates a new tripwire block
		/// </summary>
		public TripwireBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tripwire block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public TripwireBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tripwire block
		/// </summary>
		/// <param name="state">State that the tripwire is in</param>
		public TripwireBlock (TripwireState state)
			: base((byte)(state & (TripwireState.Activated | TripwireState.Tripped)))
		{
			// ...
		}

		/// <summary>
		/// States that a tripwire can be in
		/// </summary>
		[Flags]
		public enum TripwireState : byte
		{
			/// <summary>
			/// The wire is tight and ready to be tripped
			/// </summary>
			Inactive = 0x0,

			/// <summary>
			/// The entire wire (not just this block) has been tripped
			/// </summary>
			Activated = 0x4,

			/// <summary>
			/// This particular block contains an object that tripped the wire
			/// </summary>
			Tripped = 0x1
		}
	}
}
