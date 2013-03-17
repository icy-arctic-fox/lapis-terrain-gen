using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Rotates a coordinate around a point
	/// </summary>
	public class RotatePreProcessor : INoisePreProcessor
	{
		private readonly double _x1, _x2, _x3, _y1, _y2, _y3, _z1, _z2, _z3;

		/// <summary>
		/// Creates a new rotation pre-processor
		/// </summary>
		/// <param name="xAngle">Amount to rotate along the x-axis (in degrees)</param>
		/// <param name="yAngle">Amount to rotate along the y-axis (in degrees)</param>
		/// <param name="zAngle">Amount to rotate along the z-axis (in degrees)</param>
		public RotatePreProcessor (double xAngle, double yAngle, double zAngle = 0d)
		{
			// Pre-compute these values for the next calculations
			var xSin = Math.Sin(xAngle * MathConstants.DegreesToRadians);
			var ySin = Math.Sin(yAngle * MathConstants.DegreesToRadians);
			var zSin = Math.Sin(zAngle * MathConstants.DegreesToRadians);
			var xCos = Math.Cos(xAngle * MathConstants.DegreesToRadians);
			var yCos = Math.Cos(yAngle * MathConstants.DegreesToRadians);
			var zCos = Math.Cos(zAngle * MathConstants.DegreesToRadians);

			// Store these calculations instead of angles for faster computation later
			_x1 = ySin * xSin * zSin + yCos * zCos;
			_y1 = xCos * zSin;
			_z1 = ySin * zCos - yCos * xSin * zSin;
			_x2 = ySin * xSin * zCos - yCos * zSin;
			_y2 = xCos * zCos;
			_z2 = -yCos * xSin * zCos - ySin * zSin;
			_x3 = -ySin * xCos;
			_y3 = xSin;
			_z3 = yCos * xCos;
		}

		/// <summary>
		/// Rotates an input coordinate
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="xOut">Rotated x-value</param>
		/// <param name="yOut">Rotated y-value</param>
		public void ProcessCoordinate (double xIn, double yIn, out double xOut, out double yOut)
		{
			xOut = (_x1 * xIn) + (_y1 * yIn);
			yOut = (_x2 * xIn) + (_y2 * yIn);
		}

		/// <summary>
		/// Rotates an input coordinate
		/// </summary>
		/// <param name="xIn">Original input x-value</param>
		/// <param name="yIn">Original input y-value</param>
		/// <param name="zIn">Original input z-value</param>
		/// <param name="xOut">Rotated x-value</param>
		/// <param name="yOut">Rotated y-value</param>
		/// <param name="zOut">Rotated z-value</param>
		public void ProcessCoordinate (double xIn, double yIn, double zIn, out double xOut, out double yOut, out double zOut)
		{
			xOut = (_x1 * xIn) + (_y1 * yIn) + (_z1 * zIn);
			yOut = (_x2 * xIn) + (_y2 * yIn) + (_z2 * zIn);
			zOut = (_x3 * xIn) + (_y3 * yIn) + (_z3 * zIn);
		}
	}
}
