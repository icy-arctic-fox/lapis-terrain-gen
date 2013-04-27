namespace Lapis.Blocks
{
	public class FlowerPotBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.FlowerPot; }
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
		/// Contents of the flower pot
		/// </summary>
		public FlowerPotContents Contents
		{
			get { return (FlowerPotContents)Data;}
		}

		/// <summary>
		/// Creates a new flower pot block
		/// </summary>
		public FlowerPotBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new flower pot block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public FlowerPotBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new flower pot block
		/// </summary>
		/// <param name="contents">Contents of the flower pot</param>
		public FlowerPotBlock (FlowerPotContents contents)
			: base((byte)contents)
		{
			// ...
		}

		/// <summary>
		/// Contents of a flower pot
		/// </summary>
		public enum FlowerPotContents : byte
		{
			/// <summary>
			/// Nothing in the flower pot
			/// </summary>
			Empty = 0x0,

			/// <summary>
			/// Red flower
			/// </summary>
			Rose = 0x1,

			/// <summary>
			/// Yellow flower
			/// </summary>
			Dandelion = 0x2,

			/// <summary>
			/// Sapling from an oak tree
			/// </summary>
			OakSapling = 0x3,

			/// <summary>
			/// Sapling from a spruce tree
			/// </summary>
			SpruceSapling = 0x4,

			/// <summary>
			/// Sapling from a birch tree
			/// </summary>
			BirchSapling = 0x5,

			/// <summary>
			/// Sapling from a jungle tree
			/// </summary>
			JungleTreeSapling = 0x6,

			/// <summary>
			/// Red mushroom
			/// </summary>
			RedMushroom = 0x7,

			/// <summary>
			/// Brown mushroom
			/// </summary>
			BrownMushroom = 0x8,

			/// <summary>
			/// Small cactus
			/// </summary>
			Cactus = 0x9,

			/// <summary>
			/// Dead bush from the desert
			/// </summary>
			DeadBush = 0xa,

			/// <summary>
			/// Small fern
			/// </summary>
			Fern = 0xb
		}
	}
}
