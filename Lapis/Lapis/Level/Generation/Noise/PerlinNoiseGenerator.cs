using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces random, smooth noise
	/// </summary>
	/// <remarks>An octave is a set a noise.
	/// Consecutive octaves can be stacked (or added) together to give more detail.
	/// Persistence determines how much each additional octave affects the final result.</remarks>
	public class PerlinNoiseGenerator : SmoothNoiseGenerator
	{
		private const int MaxOffset = 256;

		private const int XNoisePrime    = 1619;
		private const int YNoisePrime    = 31337;
		private const int ZNoisePrime    = 263;
		private const int SeedNoisePrime = 1013;

		public const int DefaultOctaves        = 8;
		public const double DefaultPersistence = 0.35;
		public const double DefaultFrequency   = 2.0;
		public const double DefaultLacunarity  = 2.0;

		private readonly int _octaves, _seed;
		private readonly double _persistence, _frequency, _lacunarity;
		private readonly int _xOff, _yOff, _zOff;
		private readonly CurveMethod _curve;

		/// <summary>
		/// Creates a new constant value noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		/// <param name="octaves">Number of times to stack noise</param>
		/// <param name="persistence">How much each octave should affect the final result</param>
		public PerlinNoiseGenerator (long seed, int octaves = DefaultOctaves, double persistence = DefaultPersistence, double frequency = DefaultFrequency, double lacunarity = DefaultLacunarity, NoiseQuality quality = NoiseQuality.Best)
			: base(SineInterpolator)
		{
			// Unlike Java, .NET C# only supports 32-bit seeds
			// Split it up to achieve a similar random range
			var seed1 = (int)(seed & 0xffffffff);
			_seed     = (int)((seed >> 32) & 0xffffffff);

			var rng = new Random(seed1);
			_xOff = rng.Next(MaxOffset);
			_yOff = rng.Next(MaxOffset);
			_zOff = rng.Next(MaxOffset);

			_octaves     = octaves;
			_persistence = persistence;
			_frequency   = frequency;
			_lacunarity  = lacunarity;

			switch(quality)
			{
			case NoiseQuality.Standard:
				_curve = sCurve3;
				break;
			case NoiseQuality.Best:
				_curve = sCurve5;
				break;
			default:
				_curve = null;
				break;
			}
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			var value       = 0d;
			var persistence = 1d;
			var normal      = 0d;

			x += _xOff;
			y += _yOff;
			x *= _frequency;
			y *= _frequency;

			for(var octave = 0; octave < _octaves; ++octave)
			{
				var seed   = unchecked(_seed + octave);
				var signal = calculate(seed, x, y);
				value     += signal * persistence;
				normal    += persistence;

				x *= _lacunarity;
				y *= _lacunarity;
				persistence *= _persistence;
			}

			value /= normal;
			return value;
		}

		private double calculate (int seed, double x, double y)
		{
			return calculate(seed, x, y, 0);
			throw new NotImplementedException();
		}

		/// <summary>
		/// Generates three-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <param name="z">Z-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y, double z)
		{
			var value       = 0d;
			var persistence = 1d;
			var normal      = 0d;

			x += _xOff;
			y += _yOff;
			z += _zOff;
			x *= _frequency;
			y *= _frequency;
			z *= _frequency;

			for(var octave = 0; octave < _octaves; ++octave)
			{
				var seed   = unchecked(_seed + octave);
				var signal = calculate(seed, x, y, z);
				value     += signal * persistence;
				normal    += persistence;

				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;
				persistence *= _persistence;
			}

			value /= normal;
			if(-1 > value || 1 < value)
				Console.WriteLine(value);
			return value;
		}

		private double calculate (int seed, double x, double y, double z)
		{
			var x0 = Floor(x);
			var y0 = Floor(y);
			var z0 = Floor(z);
			var x1 = x0 + 1;
			var y1 = y0 + 1;
			var z1 = z0 + 1;

			var xs = x - x0;
			var ys = y - y0;
			var zs = z - z0;
			if(null != _curve)
			{// Use better quality
				xs = _curve(xs);
				ys = _curve(ys);
				zs = _curve(zs);
			}

			double n0, n1, ix0, ix1, iy0, iy1;
			n0  = noise(seed, x0, y0, z0);
			n1  = noise(seed, x1, y0, z0);
			ix0 = Interpolate(n0, n1, xs);
			n0  = noise(seed, x0, y1, z0);
			n1  = noise(seed, x1, y1, z0);
			ix1 = Interpolate(n0, n1, xs);
			iy0 = Interpolate(ix0, ix1, ys);
			n0  = noise(seed, x0, y0, z1);
			n1  = noise(seed, x1, y0, z1);
			ix0 = Interpolate(n0, n1, xs);
			n0  = noise(seed, x0, y1, z1);
			n1  = noise(seed, x1, y1, z1);
			ix1 = Interpolate(n0, n1, xs);
			iy1 = Interpolate(ix0, ix1, ys);
			return Interpolate(iy0, iy1, zs);
		}

		private static double noise (int seed, int x, int y, int z)
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

		#region Noise quality
		/// <summary>
		/// Quality (smoothness) of the produced noise
		/// </summary>
		public enum NoiseQuality
		{
			/// <summary>
			/// Fastest calculation, but produces sharper noise
			/// </summary>
			Fast,

			/// <summary>
			/// Balance of quality and speed
			/// </summary>
			Standard,

			/// <summary>
			/// Slowest calculation, but produces smoother noise
			/// </summary>
			Best
		}

		private delegate double CurveMethod (double a);

		private static double sCurve3 (double a)
		{
			return a * a * (3.0 - 2.0 * a);
		}

		private static double sCurve5 (double a)
		{
			var a3 = a * a * a;
			var a4 = a3 * a;
			var a5 = a4 * a;
			return (6.0 * a5) - (15.0 * a4) + (10.0 * a3);
		}
		#endregion
	}
}
