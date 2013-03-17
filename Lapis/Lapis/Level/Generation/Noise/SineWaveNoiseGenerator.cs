using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that generates values that appear along a sine wave
	/// </summary>
	public class SineWaveNoiseGenerator : NoiseGenerator
	{
		private readonly NoiseGenerator2D _2DMethod;
		private readonly NoiseGenerator3D _3DMethod;

		/// <summary>
		/// Creates a new sine wave noise generator
		/// </summary>
		/// <param name="method">Method used for calculating the sine wave</param>
		public SineWaveNoiseGenerator (SineWaveMethod method = SineWaveMethod.Normal)
		{
			switch(method)
			{
			case SineWaveMethod.Ribbon:
				_2DMethod = generateRibbon;
				_3DMethod = generateRibbon;
				break;
			case SineWaveMethod.Ripple:
				_2DMethod = generateRipple;
				_3DMethod = generateRipple;
				break;
			default:
				_2DMethod = generateNormal;
				_3DMethod = generateNormal;
				break;
			}
		}

		#region Normal method
		private static double generateNormal (double x, double y)
		{
			return Math.Sin(x) * Math.Sin(y);
		}

		private static double generateNormal (double x, double y, double z)
		{
			return Math.Sin(x) * Math.Sin(y) * Math.Sin(z);
		}
		#endregion

		#region Ribbon method
		private static double generateRibbon (double x, double y)
		{
			return Math.Sin(x * y);
		}

		private static double generateRibbon (double x, double y, double z)
		{
			return Math.Sin(x * y * z);
		}
		#endregion

		#region Ripple method
		private static double generateRipple (double x, double y)
		{
			return Math.Sin(Math.Sqrt((x * x) + (y * y)));
		}

		private static double generateRipple (double x, double y, double z)
		{
			return Math.Sin(Math.Sqrt((x * x) + (y * y) + (z * z)));
		}
		#endregion

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			return _2DMethod(x, y);
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
			return _3DMethod(x, y, z);
		}

		/// <summary>
		/// Methods of output for a multi-dimensional sine wave
		/// </summary>
		public enum SineWaveMethod
		{
			/// <summary>
			/// Egg carton looking appearance
			/// </summary>
			/// <remarks>Uses the equation: n = sin(x) * sin(y)
			/// A preview of this type can be found here: http://www.wolframalpha.com/input/?i=sin%28x%29+*+sin%28y%29 </remarks>
			Normal,

			/// <summary>
			/// Ribbon appearance with sharp diagonals and smooth inner-regions
			/// </summary>
			/// <remarks>Uses the equation: n = sin(x * y)
			/// A preview of this type can be found here: http://www.wolframalpha.com/input/?i=sin%28x*y%29 </remarks>
			Ribbon,

			/// <summary>
			/// Water ripple appearance
			/// </summary>
			/// <remarks>Uses the equation: n = sin(sqrt(x^2 + y^2))
			/// A preview of this type can be found here: http://www.wolframalpha.com/input/?i=sin%28sqrt%28x^2+%2B+y^2%29%29 </remarks>
			Ripple
		}
	}
}
