namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that always returns the same value
	/// </summary>
	public class ConstantValueNoiseGenerator : NoiseGenerator
	{
		private readonly double _value;

		/// <summary>
		/// Creates a new constant value noise generator
		/// </summary>
		/// <param name="value">Value to always return for noise</param>
		public ConstantValueNoiseGenerator (double value)
		{
			_value = value;
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			return _value;
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
			return _value;
		}
	}
}
