namespace Lapis.Blocks
{
	/// <summary>
	/// Positioning of a torch
	/// </summary>
	public enum TorchOrientation : byte
	{
		/// <summary>
		/// Pointing east
		/// </summary>
		East = 0x1,

		/// <summary>
		/// Pointing west
		/// </summary>
		West = 0x2,

		/// <summary>
		/// Pointing south
		/// </summary>
		South = 0x3,

		/// <summary>
		/// Pointing north
		/// </summary>
		North = 0x4,

		/// <summary>
		/// On the floor (or on top of a block) pointing up
		/// </summary>
		Floor = 0x5,

		/// <summary>
		/// Unknown position
		/// </summary>
		/// <remarks>Documentation calls this value "in the ground", but that doesn't make sense for a torch.</remarks>
		Unknown = 0x6
	}
}
