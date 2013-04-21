namespace Lapis.Blocks
{
	/// <summary>
	/// Textures that a slab can use
	/// </summary>
	public enum SlabTexture : byte
	{
		Stone = 0x0,

		Sandstone = 0x1,

		/// <summary>
		/// Wooden texture - not used anymore
		/// </summary>
		/// <remarks>This shows a wood slab, but it has stone properties.</remarks>
		Wood = 0x2,

		Cobblestone = 0x3,

		Brick = 0x4,

		StoneBrick = 0x5,

		NetherBrick = 0x6,

		Quartz = 0x7
	}
}
