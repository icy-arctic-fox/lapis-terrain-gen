using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces curvy line-like smooth noise
	/// </summary>
	/// <remarks>An octave is a set a noise.
	/// Consecutive octaves can be stacked (or added) together to give more detail.
	/// Persistence determines how much each additional octave affects the final result.</remarks>
	public class RidgedMultifractalNoiseGenerator : SmoothNoiseGenerator
	{
		private const int MaxOffset = 256;

		public const int DefaultOctaves       = 8;
		public const double DefaultFrequency  = 2.0;
		public const double DefaultLacunarity = 2.0;

		private readonly int _octaves, _seed;
		private readonly double _frequency, _lacunarity;
		private readonly int _xOff, _yOff, _zOff;
		private readonly CurveMethod _curve;

		private readonly double[] _spectralWeights;

		// TODO: Add more constructors

		/// <summary>
		/// Creates a new ridged multi-fractal noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		/// <param name="octaves">Number of times to stack noise</param>
		/// <param name="frequency">Rate at which points should change per octave</param>
		/// <param name="lacunarity">Multiplier for <paramref name="frequency"/> after each octave</param>
		/// <param name="quality">Quality of the computation when interpolating between points</param>
		public RidgedMultifractalNoiseGenerator (long seed, int octaves = DefaultOctaves, double frequency = DefaultFrequency, double lacunarity = DefaultLacunarity, NoiseQuality quality = NoiseQuality.Standard)
		{
			// Unlike Java, .NET C# only supports 32-bit seeds
			// Split it up to achieve a similar random range
			var seed1 = (int)(seed & 0xffffffff);
			_seed     = (int)((seed >> 32) & 0xffffffff);

			var rng = new Random(seed1);
			_xOff = rng.Next(MaxOffset);
			_yOff = rng.Next(MaxOffset);
			_zOff = rng.Next(MaxOffset);

			_octaves    = octaves;
			_frequency  = frequency;
			_lacunarity = lacunarity;

			switch(quality)
			{
			case NoiseQuality.Standard:
				_curve = SCurve3;
				break;
			case NoiseQuality.Best:
				_curve = SCurve5;
				break;
			default:
				_curve = null;
				break;
			}

			const double h = 1d;

			// Pre-compute weights
			_spectralWeights = new double[octaves];
			var freq = 1d;
			for(var i = 0; i < octaves; ++i)
			{
				_spectralWeights[i] = Math.Pow(freq, -h);
				freq *= lacunarity;
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
			var value  = 0d;
			var weight = 1d;

			const double offset = 1d;
			const double gain   = 2d;

			x += _xOff;
			y += _yOff;
			x *= _frequency;
			y *= _frequency;

			for(var octave = 0; octave < _octaves; ++octave)
			{
				var seed   = unchecked(_seed + octave);
				var signal = calculate(seed, x, y);

				if(signal < 0)
					signal = -1d * signal;
				signal  = offset - signal;
				signal *= signal;
				signal *= weight;

				weight = signal * gain;
				if(1d < weight)
					weight = 1d;
				else if(0d > weight)
					weight = 0d;

				value += signal * _spectralWeights[octave];

				x *= _lacunarity;
				y *= _lacunarity;
			}

			return (value * 1.25) - 1d;
		}

		private double calculate (int seed, double x, double y)
		{
			return calculate(seed, x, y, 0); // TODO: Possibly optimize 2D generation
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
			var value  = 0d;
			var weight = 1d;

			const double offset = 1d;
			const double gain   = 2d;

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

				if(signal < 0d)
					signal = -1d * signal;
				signal  = offset - signal;
				signal *= signal;
				signal *= weight;

				weight = signal * gain;
				if(1d < weight)
					weight = 1d;
				else if(0d > weight)
					weight = 0d;

				value += signal * _spectralWeights[octave];

				x *= _lacunarity;
				y *= _lacunarity;
				z *= _lacunarity;
			}

			return (value * 1.25) - 1d;
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
			n0  = Noise(seed, x0, y0, z0);
			n1  = Noise(seed, x1, y0, z0);
			ix0 = Interpolate(n0, n1, xs);
			n0  = Noise(seed, x0, y1, z0);
			n1  = Noise(seed, x1, y1, z0);
			ix1 = Interpolate(n0, n1, xs);
			iy0 = Interpolate(ix0, ix1, ys);
			n0  = Noise(seed, x0, y0, z1);
			n1  = Noise(seed, x1, y0, z1);
			ix0 = Interpolate(n0, n1, xs);
			n0  = Noise(seed, x0, y1, z1);
			n1  = Noise(seed, x1, y1, z1);
			ix1 = Interpolate(n0, n1, xs);
			iy1 = Interpolate(ix0, ix1, ys);
			return Interpolate(iy0, iy1, zs);
		}
	}
}
