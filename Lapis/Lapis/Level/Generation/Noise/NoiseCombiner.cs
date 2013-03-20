using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Combines the results of two noise generators to produce a new noise value
	/// </summary>
	public class NoiseCombiner : NoiseGenerator
	{
		private readonly NoiseGenerator _generatorA, _generatorB;
		private readonly NoiseCombinationMethod _method;
		private readonly bool _clamp;

		/// <summary>
		/// Creates a new noise combiner
		/// </summary>
		/// <param name="generatorA">First noise generator</param>
		/// <param name="generatorB">Second noise generator</param>
		/// <param name="method">Method to use when combining noise</param>
		/// <param name="clamp">Whether or not to clamp the output values to -1 to 1</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="generatorA"/> or <paramref name="generatorB"/> is null</exception>
		public NoiseCombiner (NoiseGenerator generatorA, NoiseGenerator generatorB, CombinationMethod method = CombinationMethod.Add, bool clamp = true)
		{
			if(null == generatorA)
				throw new ArgumentNullException("generatorA", "The first noise generator can't be null.");
			if(null == generatorB)
				throw new ArgumentNullException("generatorB", "The second noise generator can't be null.");

			_generatorA = generatorA;
			_generatorB = generatorB;
			_clamp      = clamp;

			switch(method)
			{
			case CombinationMethod.Subtract:
				_method = subtractNoise;
				break;
			case CombinationMethod.Multiply:
				_method = multiplyNoise;
				break;
			case CombinationMethod.Average:
				_method = averageNoise;
				break;
			case CombinationMethod.Min:
				_method = minNoise;
				break;
			case CombinationMethod.Max:
				_method = maxNoise;
				break;
			default:
				_method = addNoise;
				break;
			}
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			var a = _generatorA.GenerateNoise(x, y);
			var b = _generatorB.GenerateNoise(x, y);
			var noise = _method(a, b);
			if(_clamp)
			{
				if(-1.0 > noise)
					noise = -1.0;
				else if(1.0 < noise)
					noise = 1.0;
			}
			return noise;
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
			var a = _generatorA.GenerateNoise(x, y, z);
			var b = _generatorB.GenerateNoise(x, y, z);
			var noise = _method(a, b);
			if(_clamp)
			{
				if(-1.0 > noise)
					noise = -1.0;
				else if(1.0 < noise)
					noise = 1.0;
			}
			return noise;
		}

		#region Combination methods
		/// <summary>
		/// Methods for combining noise together
		/// </summary>
		public enum CombinationMethod
		{
			Add,

			Subtract,

			Multiply,

			Average,

			Min,

			Max
		}

		private delegate double NoiseCombinationMethod (double a, double b);

		private static double addNoise (double a, double b)
		{
			return a + b;
		}

		private static double subtractNoise (double a, double b)
		{
			return a - b;
		}

		private static double multiplyNoise (double a, double b)
		{
			return a * b;
		}

		private static double averageNoise (double a, double b)
		{
			return (a + b) / 2;
		}

		private static double minNoise (double a, double b)
		{
			return Math.Min(a, b);
		}

		private static double maxNoise (double a, double b)
		{
			return Math.Max(a, b);
		}
		#endregion
	}
}
