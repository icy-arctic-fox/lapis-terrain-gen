namespace Lapis.Blocks
{
	public class CocoaPlantBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.CocoaPlant; }
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
			get { return 15f; }
		}
		#endregion

		/// <summary>
		/// Direction that the cocoa plant is facing
		/// </summary>
		public CocoaPlantOrientation Orientation
		{
			get { return (CocoaPlantOrientation)(_data & 0x3); }
		}

		/// <summary>
		/// Size of the cocoa plant
		/// </summary>
		/// <remarks>0 is small, 1 is medium, and 2 is large (fully grown).</remarks>
		public byte Size
		{
			get { return (byte)(_data >> 2); }
		}

		/// <summary>
		/// Whether or not the plant is fully grown
		/// </summary>
		public bool FullyGrown
		{
			get { return Size >= 2; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get
			{
				if(Size == 0)
					return "Small " + Orientation;
				if(Size == 2)
					return "Medium " + Orientation;
				return "Large " + Orientation;
			}
		}

		/// <summary>
		/// Creates a new cocoa plant block
		/// </summary>
		public CocoaPlantBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new cocoa plant block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public CocoaPlantBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new cocoa plant block
		/// </summary>
		/// <param name="orientation">Direction that the plant is facing</param>
		public CocoaPlantBlock (CocoaPlantOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new cocoa plant block
		/// </summary>
		/// <param name="orientation">Direction that the plant is facing</param>
		/// <param name="size">Size of the plant (0-2, 2 being the largest)</param>
		public CocoaPlantBlock (CocoaPlantOrientation orientation, byte size)
			: base((byte)((byte)orientation | ((size & 0x3) << 2)))
		{
			// ...
		}

		/// <summary>
		/// Directions that the cocoa plants cant face (side of the tree it is on) 
		/// </summary>
		public enum CocoaPlantOrientation : byte
		{
			North = 0x0,
			East  = 0x1,
			South = 0x2,
			West  = 0x3
		}
	}
}
