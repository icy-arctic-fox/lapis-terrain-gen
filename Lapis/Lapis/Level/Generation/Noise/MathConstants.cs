using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Pre-calculated mathematical values used for faster computations
	/// </summary>
	public static class MathConstants
	{
		/// <summary>
		/// Degrees to radians ratio
		/// </summary>
		public const double DegreesToRadians = Math.PI / 180d;

		/// <summary>
		/// Radians to degrees ratio
		/// </summary>
		public const double RadiansToDegrees = 180d / Math.PI;
	}
}
