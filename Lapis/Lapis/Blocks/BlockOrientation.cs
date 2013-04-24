namespace Lapis.Blocks
{
	/// <summary>
	/// Common directional values for some blocks
	/// </summary>
	/// <remarks>Most blocks have their own orientation scheme.
	/// Blocks that use these orientation values might allow only a subset.
	/// These value correspond to the direction that the block is facing (unless specified otherwise by the block).</remarks>
	public enum BlockOrientation : byte
	{
		Down  = 0x0,
		Up    = 0x1,
		North = 0x2,
		South = 0x3,
		West  = 0x4,
		East  = 0x5
	}
}
