namespace Lapis.Level
{
	/// <summary>
	/// Types of modes that the player may be in
	/// </summary>
	public enum GameMode : byte
	{
		/// <summary>
		/// Players must survive, build, and craft on their own
		/// </summary>
		Survival = 0,

		/// <summary>
		/// Players can place and destroy blocks at will
		/// </summary>
		Creative = 1,

		/// <summary>
		/// Players cannot place or destroy blocks
		/// </summary>
		Adventure = 2
	}
}
