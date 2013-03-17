namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Modifies the output value from a noise generator
	/// </summary>
	public interface INoisePostProcessor
	{
		/// <summary>
		/// Manipulates output from a noise generator
		/// </summary>
		/// <param name="noise">Output value from the noise generator</param>
		/// <returns>A modified noise value</returns>
		double ProcessNoise (double noise);
	}
}
