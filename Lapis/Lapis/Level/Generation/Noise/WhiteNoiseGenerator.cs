using Lapis.Utility;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces random, unsmooth noise
	/// </summary>
	public class WhiteNoiseGenerator : NoiseGenerator
	{
		private readonly long _seed;

		/// <summary>
		/// Creates a new white noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		public WhiteNoiseGenerator (long seed)
		{
			_seed = seed;
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			var bytes = new byte[sizeof(double) * 2 + sizeof(long)];
			bytes.Insert(x);
			bytes.Insert(y, sizeof(double));
			bytes.Insert(_seed, sizeof(double) * 2);
			var n = bytes.GenerateHash();
			return (1d - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824d);
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
			var bytes = new byte[sizeof(double) * 3 + sizeof(long)];
			bytes.Insert(x);
			bytes.Insert(y, sizeof(double));
			bytes.Insert(z, sizeof(double) * 2);
			bytes.Insert(_seed, sizeof(double) * 3);
			var n = bytes.GenerateHash();
			return (1d - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824d);
		}
	}
}
