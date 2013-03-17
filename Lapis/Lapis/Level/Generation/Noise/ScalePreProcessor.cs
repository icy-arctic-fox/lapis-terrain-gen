namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Scales a coordinate by multiplying or dividing it
	/// </summary>
	public class ScalePreProcessor : INoisePreProcessor
	{
		private readonly double _x, _y, _z;

		/// <summary>
		/// Creates a new scaling pre-processor
		/// </summary>
		/// <param name="scale">Amount to scale along each axis</param>
		public ScalePreProcessor (double scale)
		{
			_x = scale;
			_y = scale;
			_z = scale;
		}

		/// <summary>
		/// Creates a new scaling pre-processor
		/// </summary>
		/// <param name="xScale">Amount to scale along the x-axis</param>
		/// <param name="yScale">Amount to scale along the y-axis</param>
		/// <param name="zScale">Amount to scale along the z-axis</param>
		public ScalePreProcessor (double xScale, double yScale, double zScale = 1d)
		{
			_x = xScale;
			_y = yScale;
			_z = zScale;
		}

		/// <summary>
		/// Scales an input coordinate
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="xOut">Scaled x-value</param>
		/// <param name="yOut">Scaled y-value</param>
		public void ProcessCoordinate (double xIn, double yIn, out double xOut, out double yOut)
		{
			xOut = xIn * _x;
			yOut = yIn * _y;
		}

		/// <summary>
		/// Scales an input coordinate
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="zIn">Original input z-value</param>
		/// <param name="xOut">Scaled x-value</param>
		/// <param name="yOut">Scaled y-value</param>
		/// <param name="zOut">Scaled z-value</param>
		public void ProcessCoordinate (double xIn, double yIn, double zIn, out double xOut, out double yOut, out double zOut)
		{
			xOut = xIn * _x;
			yOut = yIn * _y;
			zOut = zIn * _z;
		}
	}
}
