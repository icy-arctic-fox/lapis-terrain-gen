using System;

namespace Lapis.Blocks
{
	public class TripwireHookBlock : Block, IRedstoneSourceBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.TripwireHook; }
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
		/// Direction that the hook is facing
		/// </summary>
		public TripwireHookOrientation Orientation
		{
			get { return (TripwireHookOrientation)(_data & 0x3); }
		}

		/// <summary>
		/// State that the hook is in
		/// </summary>
		public TripwireHookState State
		{
			get { return (TripwireHookState)(_data & 0xc); }
		}

		/// <summary>
		/// Whether or not the hook is providing redstone current
		/// </summary>
		public bool Powered
		{
			get { return State.HasFlag(TripwireHookState.Tripped); }
		}

		/// <summary>
		/// Strength of the redstone current given off by the hook
		/// </summary>
		public byte CurrentStrength
		{
			get { return Powered ? (byte)15 : (byte)0; }
		}

		/// <summary>
		/// Creates a new tripwire hook block
		/// </summary>
		public TripwireHookBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tripwire hook block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public TripwireHookBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tripwire hook block
		/// </summary>
		/// <param name="orientation">Direction that the hook is facing</param>
		public TripwireHookBlock (TripwireHookOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new tripwire hook block
		/// </summary>
		/// <param name="orientation">Direction that the hook is facing</param>
		/// <param name="state">State that the hook is in</param>
		public TripwireHookBlock (TripwireHookOrientation orientation, TripwireHookState state)
			: base((byte)((byte)orientation | (byte)state))
		{
			// ...
		}

		/// <summary>
		/// Directions that the tripwire hook can face
		/// </summary>
		public enum TripwireHookOrientation : byte
		{
			South = 0x0,
			West  = 0x1,
			North = 0x2,
			East  = 0x3
		}

		/// <summary>
		/// States that the tripwire hook can be in
		/// </summary>
		/// <remarks>Normally when a tripwire is triggered, Connected and Tripped are used.</remarks>
		[Flags]
		public enum TripwireHookState : byte
		{
			/// <summary>
			/// No string is attached to the hook (in the up position)
			/// </summary>
			Inactive = 0x0,

			/// <summary>
			/// The hook has string attached ready to be triggered (in the middle position)
			/// </summary>
			Connected = 0x4,

			/// <summary>
			/// The hook has been triggered/activated (in the down position)
			/// </summary>
			Tripped = 0x8
		}
	}
}
