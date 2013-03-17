using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Changes the output range of noise from a generator
	/// </summary>
	/// <remarks>This post-processor will expand and shrink an output range.
	/// It also clamps to the range boundaries (after scaling).
	/// Note: This processor expects the input range to be from -1 to 1.
	/// Input values outside that range may have undesired results.</remarks>
	public class RangePostProcessor : INoisePostProcessor
	{
		private readonly double _lower, _upper, _range;

		/// <summary>
		/// Creates a new range post-processor
		/// </summary>
		/// <param name="lower">Lower bound</param>
		/// <param name="upper">Upper bound</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="lower"/> is greater than <paramref name="upper"/></exception>
		public RangePostProcessor (double lower, double upper)
		{
			if(lower > upper)
				throw new ArgumentOutOfRangeException("lower", "The lower bound must be less than the upper bound.");

			_lower = lower;
			_upper = upper;
			_range = (_upper - _lower + 1d) / 2d; // +1 to make bounds inclusive and /2 assuming original -1 to 1 range
		}

		/// <summary>
		/// Changes the output range from a noise generator
		/// </summary>
		/// <param name="noise">Output value from the noise generator</param>
		/// <returns>A scaled noise value</returns>
		public double ProcessNoise (double noise)
		{
			var value = noise * _range;
			if(_lower > value)
				return _lower;
			if(_upper < value)
				return _upper;
			return value;
		}
	}
}
