namespace Lapis.Blocks
{
	public class StoneSlabBlock : SlabBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.StoneSlab; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 30f; }
		}
		#endregion

		/// <summary>
		/// Type of stone slab
		/// </summary>
		public SlabTexture SlabType
		{
			get { return (SlabTexture)TextureData; }
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		public StoneSlabBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public StoneSlabBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		/// <param name="type">Type of stone slab</param>
		public StoneSlabBlock (SlabTexture type)
			: base((byte)type)
		{
			// ..
		}

		/// <summary>
		/// Creates a new stone slab block
		/// </summary>
		/// <param name="type">Type of stone slab</param>
		/// <param name="upper">Whether or not the slab is on the top-half of the block (upside-down)</param>
		public StoneSlabBlock (SlabTexture type, bool upper)
			: base((byte)type, upper)
		{
			// ...
		}
	}
}
