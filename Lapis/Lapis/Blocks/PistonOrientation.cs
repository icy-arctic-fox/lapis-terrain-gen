namespace Lapis.Blocks
{
	/// <summary>
	/// Directions that a piston can face
	/// </summary>
	public enum PistonOrientation : byte
	{
		/// <summary>
		/// Head or extension is facing down
		/// </summary>
		Down = 0x0,

		/// <summary>
		/// Head or extension is facing up
		/// </summary>
		Up = 0x1,

		/// <summary>
		/// Head or extension is facing north
		/// </summary>
		North = 0x2,

		/// <summary>
		/// Head or extension is facing south
		/// </summary>
		South = 0x3,

		/// <summary>
		/// Head or extension is facing west
		/// </summary>
		West = 0x4,

		/// <summary>
		/// Head or extension is facing east
		/// </summary>
		East = 0x5
	}
}
