namespace Lapis.Blocks
{
	public class AnvilBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Anvil; }
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
			get { return true; }
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
			get { return 6000f; }
		}
		#endregion

		/// <summary>
		/// Direction that the anvil is facing
		/// </summary>
		public AnvilOrientation Orientation
		{
			get { return (AnvilOrientation)(_data & 0x3); }
		}

		/// <summary>
		/// Amount of damage the anvil has (0-2)
		/// </summary>
		/// <remarks>0 is pristine (new) anvil,
		/// 1 is a slightly damaged anvil,
		/// and 2 is a very damaged anvil (about to break).</remarks>
		public byte Damage
		{
			get { return (byte)((_data >> 2) & 0x3); }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Orientation + " Damage: " + Damage + "/2"; }
		}

		/// <summary>
		/// Creates a new anvil block
		/// </summary>
		public AnvilBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new anvil block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public AnvilBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new anvil block
		/// </summary>
		/// <param name="orientation">Direction that the anvil is facing</param>
		public AnvilBlock (AnvilOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}

		/// <summary>
		/// Creates a new anvil block
		/// </summary>
		/// <param name="orientation">Direction that the anvil is facing</param>
		/// <param name="damage">Amount of damage the anvil has (0-2)</param>
		public AnvilBlock (AnvilOrientation orientation, byte damage)
			: base((byte)((byte)orientation | ((damage & 0x3) << 2)))
		{
			// ...
		}

		/// <summary>
		/// Directions that an anvil can face
		/// </summary>
		public enum AnvilOrientation : byte
		{
			NorthSouth = 0x0,
			EastWest   = 0x1
		}
	}
}
