using System;
using System.Collections.Generic;
using System.Linq;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// Generates noise values used for terrain generation
	/// </summary>
	/// <remarks>Noise generators always produce a value from -1 to 1.
	/// However, a post-processor (like RangePostProcessor or ClampPostProcessor) may change that.</remarks>
	public abstract class NoiseGenerator
	{
		/// <summary>
		/// Describes a method that generates noise given a 2D coordinate
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		public delegate double NoiseGenerator2D (double x, double y);

		/// <summary>
		/// Describes a method that generates noise given a 3D coordinate
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <param name="z">Z-position</param>
		/// <returns>A noise value</returns>
		public delegate double NoiseGenerator3D (double x, double y, double z);

		#region Pre-process
		private readonly List<INoisePreProcessor> _preProcessors = new List<INoisePreProcessor>();

		/// <summary>
		/// Adds a noise pre-processor
		/// </summary>
		/// <param name="processor">Noise pre-processor</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null</exception>
		/// <remarks>A pre-processor modifies the input coordinates of the noise generator.
		/// Order can matter when adding pre-processors.</remarks>
		public void AddPreProcess (INoisePreProcessor processor)
		{
			if(null == processor)
				throw new ArgumentNullException("processor", "The noise pre-processor can't be null.");
			_preProcessors.Add(processor);
		}

		private void preProcess (double xIn, double yIn, out double xOut, out double yOut)
		{
			xOut = xIn;
			yOut = yIn;
			foreach(var processor in _preProcessors)
			{
				processor.ProcessCoordinate(xIn, yIn, out xOut, out yOut);
				xIn = xOut;
				yIn = yOut;
			}
		}

		private void preProcess (double xIn, double yIn, double zIn, out double xOut, out double yOut, out double zOut)
		{
			xOut = xIn;
			yOut = yIn;
			zOut = zIn;
			foreach(var processor in _preProcessors)
			{
				processor.ProcessCoordinate(xIn, yIn, zIn, out xOut, out yOut, out zOut);
				xIn = xOut;
				yIn = yOut;
				zIn = zOut;
			}
		}
		#endregion

		#region Post-process
		private readonly List<INoisePostProcessor> _postProcessors = new List<INoisePostProcessor>();

		/// <summary>
		/// Adds a noise post-processor
		/// </summary>
		/// <param name="processor">Noise post-processor</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="processor"/> is null</exception>
		/// <remarks>A post-processor modifies the output of the noise generator.
		/// Order can matter when adding post-processors.</remarks>
		public void AddPostProcess (INoisePostProcessor processor)
		{
			if(null == processor)
				throw new ArgumentNullException("processor", "The noise post-processor can't be null.");
			_postProcessors.Add(processor);
		}

		private double postProcess (double noise)
		{
			return _postProcessors.Aggregate(noise, (current, processor) => processor.ProcessNoise(current));
		}
		#endregion

		#region Generation
		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value from -1 to 1</returns>
		public double GenerateNoise (double x, double y)
		{
			preProcess(x, y, out x, out y);
			var noise = Generate(x, y);
			noise = postProcess(noise);
			return noise;
		}

		/// <summary>
		/// Generates three-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <param name="z">Z-position</param>
		/// <returns>A noise value from -1 to 1</returns>
		public double GenerateNoise (double x, double y, double z)
		{
			preProcess(x, y, z, out x, out y, out z);
			var noise = Generate(x, y, z);
			noise = postProcess(noise);
			return noise;
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected abstract double Generate (double x, double y);

		/// <summary>
		/// Generates three-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <param name="z">Z-position</param>
		/// <returns>A noise value</returns>
		protected abstract double Generate (double x, double y, double z);
		#endregion
	}
}
