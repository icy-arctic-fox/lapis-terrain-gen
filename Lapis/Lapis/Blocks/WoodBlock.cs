using System;

namespace Lapis.Blocks
{
	public class WoodBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Wood; }
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
			get { return 10f; }
		}
		#endregion

		/// <summary>
		/// Type of tree that the wood belongs to
		/// </summary>
		public TreeType TreeType
		{
			get { return (TreeType)(_data & 0x3); }
		}

		/// <summary>
		/// Direction that the wood is facing
		/// </summary>
		public WoodOrientation Orientation
		{
			get { return (WoodOrientation)(_data & 0xc); }
		}

		/// <summary>
		/// Block data value as a string
		/// </summary>
		public string DataString
		{
			get { return String.Join(" ", Orientation.ToString(), TreeType.ToString()); }
		}

		/// <summary>
		/// Creates a new wood block
		/// </summary>
		public WoodBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public WoodBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood block
		/// </summary>
		/// <param name="type">Type of tree that the wood belongs to</param>
		public WoodBlock (TreeType type)
			: base((byte)type)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood block
		/// </summary>
		/// <param name="type">Type of tree that the wood belongs to</param>
		/// <param name="orientation">Orientation of the wood</param>
		public WoodBlock (TreeType type, WoodOrientation orientation)
			: base((byte)((byte)type | (byte)orientation))
		{
			// ...
		}

		/// <summary>
		/// Direction that the wood is oriented
		/// </summary>
		public enum WoodOrientation : byte
		{
			/// <summary>
			/// Standard orientation, pointing up and down
			/// </summary>
			Vertical = 0x0,

			/// <summary>
			/// Wood is on its side, pointing east and west
			/// </summary>
			EastWest = 0x4,

			/// <summary>
			/// Wood is on its side, pointing north and south
			/// </summary>
			NorthSouth = 0x8,

			/// <summary>
			/// No orientation - bark is all 6 sides of the block
			/// </summary>
			Directionless = EastWest | NorthSouth
		}
	}
}
