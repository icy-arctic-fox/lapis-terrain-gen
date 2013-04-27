namespace Lapis.Blocks
{
	public class WoodSlabBlock : SlabBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.WoodSlab; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return true; }
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
		/// Type of wooden slab
		/// </summary>
		public TreeType SlabType
		{
			get { return (TreeType)TextureData; }
		}

		/// <summary>
		/// Creates a new wood slab block
		/// </summary>
		public WoodSlabBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood slab block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public WoodSlabBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood slab block
		/// </summary>
		/// <param name="type">Type of wooden slab</param>
		public WoodSlabBlock (TreeType type)
			: base((byte)type)
		{
			// ..
		}

		/// <summary>
		/// Creates a new wood slab block
		/// </summary>
		/// <param name="type">Type of wooden slab</param>
		/// <param name="upper">Whether or not the slab is on the top-half of the block (upside-down)</param>
		public WoodSlabBlock (TreeType type, bool upper)
			: base((byte)type, upper)
		{
			// ...
		}
	}
}
