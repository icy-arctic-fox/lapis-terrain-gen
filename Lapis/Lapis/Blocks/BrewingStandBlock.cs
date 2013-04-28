using System;

namespace Lapis.Blocks
{
	public class BrewingStandBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.BrewingStand; }
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
			get { return 1; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 2.5f; }
		}
		// TODO: Implement NBT data for 'Cauldron'
		#endregion

		/// <summary>
		/// Slots that have potions in them
		/// </summary>
		public PotionPlacement SlotsFilled
		{
			get { return (PotionPlacement)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return SlotsFilled.ToString(); }
		}

		/// <summary>
		/// Creates a new brewing stand block
		/// </summary>
		public BrewingStandBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new brewing stand block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public BrewingStandBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new brewing stand block
		/// </summary>
		/// <param name="slots">Slots that contain potions</param>
		public BrewingStandBlock (PotionPlacement slots)
			: base((byte)slots)
		{
			// ...
		}

		/// <summary>
		/// Slots that potions are in
		/// </summary>
		[Flags]
		public enum PotionPlacement : byte
		{
			/// <summary>
			/// No potions are in the brewing stand
			/// </summary>
			Empty = 0x0,

			/// <summary>
			/// A potion is in the slot pointing east
			/// </summary>
			Middle = 0x1,

			/// <summary>
			/// A potion is in the slot pointing south-west
			/// </summary>
			Left = 0x2,

			/// <summary>
			/// A potion is in the slot pointing north-west
			/// </summary>
			Right = 0x4
		}
	}
}
