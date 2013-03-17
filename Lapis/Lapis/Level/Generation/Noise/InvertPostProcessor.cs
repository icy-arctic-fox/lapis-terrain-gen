namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Inverts the noise output from a generator
	/// </summary>
	public class InvertPostProcessor : INoisePostProcessor
	{
		/// <summary>
		/// Inverts output from a noise generator
		/// </summary>
		/// <param name="noise">Output value from the noise generator</param>
		/// <returns>An inverted noise value</returns>
		public double ProcessNoise (double noise)
		{
			return noise * -1;
		}
	}
}
