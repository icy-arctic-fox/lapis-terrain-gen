using System;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Combines the results of two noise generators to produce a new noise value
	/// </summary>
	public class NoiseSelector : SmoothNoiseGenerator
	{
		private readonly NoiseGenerator _generatorA, _generatorB, _selector;
		private readonly NoiseSelectionMethod2D _method2D;
		private readonly NoiseSelectionMethod3D _method3D;
		private readonly double _cutoff;

		/// <summary>
		/// Creates a new noise selector using the blend method
		/// </summary>
		/// <param name="generatorA">First noise generator</param>
		/// <param name="generatorB">Second noise generator</param>
		/// <param name="selector">Noise generator used for getting the amount of blending from each generator</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="generatorA"/>, <paramref name="generatorB"/>, <paramref name="selector"/> is null</exception>
		public NoiseSelector (NoiseGenerator generatorA, NoiseGenerator generatorB, NoiseGenerator selector)
		{
			if(null == generatorA)
				throw new ArgumentNullException("generatorA", "The first noise generator can't be null.");
			if(null == generatorB)
				throw new ArgumentNullException("generatorB", "The second noise generator can't be null.");
			if(null == selector)
				throw new ArgumentNullException("selector", "The noise selector can't be null.");

			_generatorA = generatorA;
			_generatorB = generatorB;
			_selector = selector;

			_method2D = blendNoise;
			_method3D = blendNoise;
		}

		/// <summary>
		/// Creates a new noise selector using the blend method
		/// </summary>
		/// <param name="generatorA">First noise generator</param>
		/// <param name="generatorB">Second noise generator</param>
		/// <param name="selector">Noise generator used for getting the amount of blending from each generator</param>
		/// <param name="interpolator">Method used for interpolating the value from <paramref name="generatorA"/> and <paramref name="generatorB"/></param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="generatorA"/>, <paramref name="generatorB"/>, <paramref name="selector"/> is null</exception>
		public NoiseSelector (NoiseGenerator generatorA, NoiseGenerator generatorB, NoiseGenerator selector, NoiseInterpolator interpolator)
			: base(interpolator)
		{
			if(null == generatorA)
				throw new ArgumentNullException("generatorA", "The first noise generator can't be null.");
			if(null == generatorB)
				throw new ArgumentNullException("generatorB", "The second noise generator can't be null.");
			if(null == selector)
				throw new ArgumentNullException("selector", "The noise selector can't be null.");

			_generatorA = generatorA;
			_generatorB = generatorB;
			_selector = selector;

			_method2D = blendNoise;
			_method3D = blendNoise;
		}

		/// <summary>
		/// Creates a new noise selector using the selection method
		/// </summary>
		/// <param name="generatorA">First noise generator</param>
		/// <param name="generatorB">Second noise generator</param>
		/// <param name="selector">Noise generator used for selecting a value</param>
		/// <param name="cutoff">Cut-off value for selecting between output from <paramref name="generatorA"/> and <paramref name="generatorB"/>.
		/// If the value from the selector is less than the cut-off, then <paramref name="generatorA"/> will be used and vice-versa.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="generatorA"/>, <paramref name="generatorB"/>, <paramref name="selector"/> is null</exception>
		public NoiseSelector (NoiseGenerator generatorA, NoiseGenerator generatorB, NoiseGenerator selector, double cutoff = 0.0)
		{
			if(null == generatorA)
				throw new ArgumentNullException("generatorA", "The first noise generator can't be null.");
			if(null == generatorB)
				throw new ArgumentNullException("generatorB", "The second noise generator can't be null.");
			if(null == selector)
				throw new ArgumentNullException("selector", "The noise selector can't be null.");

			_generatorA = generatorA;
			_generatorB = generatorB;
			_selector   = selector;
			_cutoff     = cutoff;

			_method2D = selectNoise;
			_method3D = selectNoise;
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			var t = _selector.GenerateNoise(x, y);
			return _method2D(x, y, t);
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
			var t = _selector.GenerateNoise(x, y, z);
			return _method3D(x, y, z, t);
		}

		#region Selection methods
		/// <summary>
		/// Methods for selecting noise
		/// </summary>
		public enum SelectionMethod
		{
			Blend,

			Select
		}

		private delegate double NoiseSelectionMethod2D (double x, double y, double t);
		private delegate double NoiseSelectionMethod3D (double x, double y, double z, double t);

		private double blendNoise (double x, double y, double t)
		{
			var a = _generatorA.GenerateNoise(x, y);
			var b = _generatorB.GenerateNoise(x, y);
			return Interpolate(a, b, t);
		}

		private double blendNoise (double x, double y, double z, double t)
		{
			var a = _generatorA.GenerateNoise(x, y, z);
			var b = _generatorB.GenerateNoise(x, y, z);
			return Interpolate(a, b, t);
		}

		private double selectNoise (double x, double y, double t)
		{
			return (t < _cutoff) ? _generatorA.GenerateNoise(x, y) : _generatorB.GenerateNoise(x, y);
		}

		private double selectNoise (double x, double y, double z, double t)
		{
			return (t < _cutoff) ? _generatorA.GenerateNoise(x, y, z) : _generatorB.GenerateNoise(x, y, z);
		}
		#endregion
	}
}
