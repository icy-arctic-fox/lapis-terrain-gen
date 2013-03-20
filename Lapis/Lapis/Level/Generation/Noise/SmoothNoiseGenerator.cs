using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Base class for smooth noise generators - contains useful common methods
	/// </summary>
	public abstract class SmoothNoiseGenerator : NoiseGenerator
	{
		private readonly NoiseInterpolator _interpolator;

		/// <summary>
		/// Creates a new smooth noise generator
		/// </summary>
		/// <remarks>Linear interpolation will be used for Lerp.</remarks>
		protected SmoothNoiseGenerator ()
		{
			_interpolator = LinearInterpolator;
		}

		/// <summary>
		/// Creates a new smooth noise generator
		/// </summary>
		/// <param name="interpolator">Interpolation method</param>
		protected SmoothNoiseGenerator (NoiseInterpolator interpolator)
		{
			if(null == interpolator)
				throw new ArgumentNullException("interpolator", "The noise interpolator can't be null.");
			_interpolator = interpolator;
		}

		#region Interpolators
		/// <summary>
		/// Describes a method that is used for interpolating a value between two points
		/// </summary>
		/// <param name="a">Fist point</param>
		/// <param name="b">Second point</param>
		/// <param name="x">Distance from point <paramref name="a"/> to point <paramref name="b"/> to interpolate the value for (0 to 1)</param>
		/// <returns>An interpolated value between point <paramref name="a"/> and point <paramref name="b"/></returns>
		public delegate double NoiseInterpolator (double a, double b, double x);

		/// <summary>
		/// Linearly interpolates a value between two points
		/// </summary>
		/// <param name="a">Fist point</param>
		/// <param name="b">Second point</param>
		/// <param name="x">Distance from point <paramref name="a"/> to point <paramref name="b"/> to interpolate the value for (0 to 1)</param>
		/// <returns>An interpolated value between point <paramref name="a"/> and point <paramref name="b"/></returns>
		public static double LinearInterpolator (double a, double b, double x)
		{
			return (b - a) * x + a;
		}

		/// <summary>
		/// Smoothly interpolates a value between two points using sine
		/// </summary>
		/// <param name="a">Fist point</param>
		/// <param name="b">Second point</param>
		/// <param name="x">Distance from point <paramref name="a"/> to point <paramref name="b"/> to interpolate the value for (0 to 1)</param>
		/// <returns>An interpolated value between point <paramref name="a"/> and point <paramref name="b"/></returns>
		/// <remarks>This is slower than linear interpolation, but gives a nice round and smooth appearance between points.</remarks>
		public static double SineInterpolator (double a, double b, double x)
		{
			var y = Math.Sin(x * MathConstants.HalfPi);
			return (b - a) * y + a;
		}
		#endregion

		#region Smoothing utility methods
		/// <summary>
		/// Fast floor calculation
		/// </summary>
		/// <param name="value">Value to floor</param>
		/// <returns>An integer</returns>
		protected static int Floor (double value)
		{
			return (value >= 0) ? (int)value : (int)value - 1;
		}

		protected static double Fade (double t)
		{
			return t * t * t * (t * (t * 6 - 15) + 10);
		}

		protected static double Grad (int hash, double x)
		{
			var h = hash & 15;
			var grad = 1.0 + (h & 7);
			if(0 != (h & 8))
				grad = -grad;
			return grad * x;
		}

		protected static double Grad (int hash, double x, double y)
		{
			var h = hash & 7;
			var u = (h < 4) ? x : y;
			var v = (h < 4) ? y : x;
			return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? 2d * v : -2d * v);
		}

		protected static double Grad (int hash, double x, double y, double z)
		{
			var h = hash & 15;
			var u = (h < 8) ? x : y;
			var v = (h < 4) ? y : (12 == h || 14 == h) ? x : z;
			return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
		}

		protected static double Grad (int hash, double x, double y, double z, double t)
		{
			var h = hash & 31;
			var u = (h < 24) ? x : y;
			var v = (h < 16) ? y : z;
			var w = (h < 8)  ? z : t;
			return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
		}

		/// <summary>
		/// Interpolates a value between two points
		/// </summary>
		/// <param name="a">First point</param>
		/// <param name="b">Second point</param>
		/// <param name="x">Distance from point <paramref name="a"/> to point <paramref name="b"/> to interpolate the value for (0 to 1)</param>
		/// <returns>An interpolated value between point <paramref name="a"/> and point <paramref name="b"/></returns>
		protected double Interpolate (double a, double b, double x)
		{
			return _interpolator(a, b, x);
		}
		#endregion

		private const int XNoisePrime    = 1619;
		private const int YNoisePrime    = 31337;
		private const int ZNoisePrime    = 263;
		private const int SeedNoisePrime = 1013;

		/// <summary>
		/// Generates a reproducable noise value from -1 to 1
		/// </summary>
		/// <param name="seed">Seed</param>
		/// <param name="x">X value</param>
		/// <param name="y">Y value</param>
		/// <param name="z">Z value</param>
		/// <returns>A noise value from -1 to 1</returns>
		protected static double Noise (int seed, int x, int y, int z)
		{
			unchecked
			{
				var n = (XNoisePrime * x) +
						(YNoisePrime * y) +
						(ZNoisePrime * z) +
						(SeedNoisePrime * seed) &
						0x7fffffff;
				n = (n >> 13) ^ n;
				var noise = (n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;
				return 1 - (noise / 1073741824.0);
			}
		}
	}
}
