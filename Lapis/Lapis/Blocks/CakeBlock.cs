namespace Lapis.Blocks
{
	public class CakeBlock : Block
	{
		/// <summary>
		/// Total number of pieces of cake
		/// </summary>
		public const byte TotalPieces = 6;

		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Cake; }
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
			get { return 2.5f; }
		}

		// TODO: Implement meta-data values
		#endregion

		/// <summary>
		/// Number of cake pieces eaten (0-5)
		/// </summary>
		public byte PiecesEaten
		{
			get { return Data; }
		}

		/// <summary>
		/// Number of cake pieces left
		/// </summary>
		public byte PiecesLeft
		{
			get { return (byte)(TotalPieces - PiecesEaten); }
		}

		/// <summary>
		/// Creates a new cake block
		/// </summary>
		public CakeBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new cake block
		/// </summary>
		/// <param name="piecesEaten">Number of cake pieces eaten (0-5)</param>
		public CakeBlock (byte piecesEaten)
			: base(piecesEaten)
		{
			// ...
		}
	}
}
