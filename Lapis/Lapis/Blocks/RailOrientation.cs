namespace Lapis.Blocks
{
	/// <summary>
	/// Directions that all rail types can face
	/// </summary>
	public enum RailOrientation : byte
	{
		/// <summary>
		/// Flat track going north and south
		/// </summary>
		NorthSouth = 0x0,

		/// <summary>
		/// Flat track going west and east
		/// </summary>
		WestEast = 0x1,

		/// <summary>
		/// Sloped track ascending east
		/// </summary>
		AscendingEast = 0x2,

		/// <summary>
		/// Sloped track ascending west
		/// </summary>
		AscendingWest = 0x3,

		/// <summary>
		/// Sloped track ascending north
		/// </summary>
		AscendingNorth = 0x4,

		/// <summary>
		/// Sloped track ascending south
		/// </summary>
		AscendingSouth = 0x5
	}

	/// <summary>
	/// Directions that curved rails can face
	/// </summary>
	public enum CurvedRailOrientation : byte
	{
		/// <summary>
		/// Flat track going north and south
		/// </summary>
		NorthSouth = 0x0,

		/// <summary>
		/// Flat track going west and east
		/// </summary>
		WestEast = 0x1,

		/// <summary>
		/// Sloped track ascending east
		/// </summary>
		AscendingEast = 0x2,

		/// <summary>
		/// Sloped track ascending west
		/// </summary>
		AscendingWest = 0x3,

		/// <summary>
		/// Sloped track ascending north
		/// </summary>
		AscendingNorth = 0x4,

		/// <summary>
		/// Sloped track ascending south
		/// </summary>
		AscendingSouth = 0x5,

		/// <summary>
		/// North-west corner piece - connects east and south
		/// </summary>
		NorthWestCorner = 0x6,

		/// <summary>
		/// North-east corner piece - connects west and south
		/// </summary>
		NorthEastCorner = 0x7,

		/// <summary>
		/// South-east corner piece - connects west and north
		/// </summary>
		SouthEastCorner = 0x8,

		/// <summary>
		/// South-west corner piece - connects east and north
		/// </summary>
		SouthWestCorner = 0x9
	}
}
