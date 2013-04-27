namespace Lapis.Blocks
{
	/// <summary>
	/// Textures that can appear on a giant mushroom block
	/// </summary>
	public enum MushroomTexture : byte
	{
		/// <summary>
		/// Pores on all sides of the block
		/// </summary>
		Fleshy = 0x0,

		/// <summary>
		/// Corner piece with the cap texture on the top, north, and west sides of the block
		/// </summary>
		TopNorthWest = 0x1,

		/// <summary>
		/// Side piece with the cap texture on the top and north sides of the block
		/// </summary>
		TopNorth = 0x2,

		/// <summary>
		/// Corner piece with the cap texture on the top, north, and east sides of the block
		/// </summary>
		TopNorthEast = 0x3,

		/// <summary>
		/// Side piece with the cap texture on the top and west sides of the block
		/// </summary>
		TopWest = 0x4,

		/// <summary>
		/// Top piece with the cap texture only on the top side of block
		/// </summary>
		Top = 0x5,

		/// <summary>
		/// Side piece with the cap texture on the top and east sides of the block
		/// </summary>
		TopEast = 0x6,

		/// <summary>
		/// Corner piece with the cap texture on the top, south, and west sides of the block
		/// </summary>
		TopSouthWest = 0x7,

		/// <summary>
		/// Side piece with the cap texture on the top and south sides of the block
		/// </summary>
		TopSouth = 0x8,

		/// <summary>
		/// Corner piece with the cap texture on the top, south, and east sides of the block
		/// </summary>
		TopSouthEast = 0x9,

		/// <summary>
		/// Stem block with the pore texture on the top and bottom and the stem texture on the four sides
		/// </summary>
		Stem = 0xa,

		/// <summary>
		/// Cap texture on all six sides of the block
		/// </summary>
		AllCap = 0xe,

		/// <summary>
		/// Stem texture on all six sides of the block
		/// </summary>
		AllStem = 0xf
	}
}
