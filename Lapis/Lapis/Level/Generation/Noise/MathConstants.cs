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

		/// <summary>
		/// 2 Pi
		/// </summary>
		public const double Pi2 = Math.PI * 2d;

		/// <summary>
		/// Half Pi
		/// </summary>
		public const double HalfPi = Math.PI / 2d;

		/// <summary>
		/// Square root of 3
		/// </summary>
		public const double Sqrt3 = 1.7320508075688772935274463415059;

		/// <summary>
		/// F skewing factor for 2D
		/// </summary>
		public const double F2 = 0.5 * (Sqrt3 - 1.0);

		/// <summary>
		/// G skewing factor for 2D
		/// </summary>
		public const double G2 = (3.0 - Sqrt3) / 6.0;

		/// <summary>
		/// F skewing factor for 3D
		/// </summary>
		public const double F3 = 1 / 3;

		/// <summary>
		/// G skewing factor for 3D
		/// </summary>
		public const double G3 = 1 / 6;
	}
}
