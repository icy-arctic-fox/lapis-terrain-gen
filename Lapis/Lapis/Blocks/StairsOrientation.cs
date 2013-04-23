namespace Lapis.Blocks
{
	/// <summary>
	/// Different directions that stairs can face
	/// </summary>
	public enum StairsOrientation : byte
	{
		/// <summary>
		/// Ascending east
		/// </summary>
		East = 0x0,

		/// <summary>
		/// Ascending west
		/// </summary>
		West = 0x1,

		/// <summary>
		/// Ascending south
		/// </summary>
		South = 0x2,

		/// <summary>
		/// Ascending north
		/// </summary>
		North = 0x3
	}
}
