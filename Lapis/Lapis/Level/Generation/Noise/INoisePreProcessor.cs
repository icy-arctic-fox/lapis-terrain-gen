namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Modifies the input coordinates to a noise generator
	/// </summary>
	public interface INoisePreProcessor
	{
		/// <summary>
		/// Processes a coordinate that will be fed to a noise generator
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="xOut">Modified output x-value</param>
		/// <param name="yOut">Modified output y-value</param>
		void ProcessCoordinate (double xIn, double yIn, out double xOut, out double yOut);

		/// <summary>
		/// Processes a coordinate that will be fed to a noise generator
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="zIn">Original input z-value</param>
		/// <param name="xOut">Modified output x-value</param>
		/// <param name="yOut">Modified output y-value</param>
		/// <param name="zOut">Modified output z-value</param>
		void ProcessCoordinate (double xIn, double yIn, double zIn, out double xOut, out double yOut, out double zOut);
	}
}
