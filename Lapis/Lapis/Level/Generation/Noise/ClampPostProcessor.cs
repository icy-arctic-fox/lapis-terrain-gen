using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Clamps the bounds of noise output from a generator
	/// </summary>
	/// <remarks>This post-processor guarantees that a noise value will be kept between an upper and lower bound.
	/// It will raise the value to the lower bound if it is below it,
	/// and lower the value to the upper bound if it above it.</remarks>
	public class ClampPostProcessor : INoisePostProcessor
	{
		private readonly double _lower, _upper;

		/// <summary>
		/// Creates a new clamp post-processor
		/// </summary>
		/// <param name="lower">Lower bound</param>
		/// <param name="upper">Upper bound</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="lower"/> is greater than <paramref name="upper"/></exception>
		public ClampPostProcessor (double lower, double upper)
		{
			if(lower > upper)
				throw new ArgumentOutOfRangeException("lower", "The lower bound must be less than the upper bound.");

			_lower = lower;
			_upper = upper;
		}

		/// <summary>
		/// Clamps output from a noise generator
		/// </summary>
		/// <param name="noise">Output value from the noise generator</param>
		/// <returns>A clamped noise value</returns>
		public double ProcessNoise (double noise)
		{
			if(_lower > noise)
				return _lower;
			if(_upper < noise)
				return _upper;
			return noise;
		}
	}
}
