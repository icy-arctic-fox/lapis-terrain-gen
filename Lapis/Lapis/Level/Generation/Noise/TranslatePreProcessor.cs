namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Translates a coordinate by adding or subtracting from it
	/// </summary>
	public class TranslatePreProcessor : INoisePreProcessor
	{
		private readonly double _x, _y, _z;

		/// <summary>
		/// Creates a new translation pre-processor
		/// </summary>
		/// <param name="xDist">Distance along the x-axis to translate the coordinate</param>
		/// <param name="yDist">Distance along the y-axis to translate the coordinate</param>
		/// <param name="zDist">Distance along the z-axis to translate the coordinate</param>
		public TranslatePreProcessor (double xDist, double yDist, double zDist = 0d)
		{
			_x = xDist;
			_y = yDist;
			_z = zDist;
		}

		/// <summary>
		/// Translates an input coordinate
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="xOut">Translated x-value</param>
		/// <param name="yOut">Translated y-value</param>
		public void ProcessCoordinate (double xIn, double yIn, out double xOut, out double yOut)
		{
			xOut = xIn + _x;
			yOut = yIn + _y;
		}

		/// <summary>
		/// Translates an input coordinate
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="zIn">Original input z-value</param>
		/// <param name="xOut">Translated x-value</param>
		/// <param name="yOut">Translated y-value</param>
		/// <param name="zOut">Translated z-value</param>
		public void ProcessCoordinate (double xIn, double yIn, double zIn, out double xOut, out double yOut, out double zOut)
		{
			xOut = xIn + _x;
			yOut = yIn + _y;
			zOut = zIn + _z;
		}
	}
}
