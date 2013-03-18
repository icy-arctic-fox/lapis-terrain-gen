using System;
using Lapis.Utility;
using System.Linq;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces random, smooth noise
	/// </summary>
	public class PerlinNoiseGenerator : SmoothNoiseGenerator
	{
		private const int ByteSize = byte.MaxValue + 1;

		private readonly int _octaves;
		private readonly int _xOff, _yOff, _zOff;
		private readonly int[] _p;

		/// <summary>
		/// Creates a new constant value noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		public PerlinNoiseGenerator (int octaves, long seed)
		{
			// Unlike Java, .NET C# only supports 32-bit seeds
			// Split it up to achieve a similar random range
			var seed1 = (int)(seed & 0xffffffff);
			var seed2 = (int)((seed >> 32) & 0xffffffff);

			var rng = new Random(seed1);
			_xOff = rng.Next(ByteSize);
			_yOff = rng.Next(ByteSize);
			_zOff = rng.Next(ByteSize);

			_octaves   = octaves;
			var length = (int)Math.Pow(2, _octaves);
			_p = new int[length * 2];
			seedPermutation(seed2, length);
		}

		private void seedPermutation (int seed, int length)
		{
			var perm = Enumerable.Range(0, length).ToArray();
			var rng  = new Random(seed);
			for(var i = 0; i < perm.Length; ++i)
			{
				var swap   = rng.Next(perm.Length);
				var temp   = perm[i];
				perm[i]    = perm[swap];
				perm[swap] = temp;
			}

			for(var i =0; i < perm.Length; ++i)
				_p[perm.Length + i] = _p[i] = perm[i];
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			throw new NotImplementedException();
		}

		private double noise (double xIn, double yIn)
		{
			throw new NotImplementedException();
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
			var result = 0d;
			var octave = 1;

			var newX = x + _xOff;
			var newY = y + _yOff;
			var newZ = z + _zOff;

			for(var i = 0; i < _octaves; ++i)
			{
				var finalX = newX * octave;
				var finalY = newY * octave;
				var finalZ = newZ * octave;

				var value = noise(finalX, finalY, finalZ);
				result   += value / octave;
				octave   *= 2;
			}

			result = 1d - 2 * result;
			return result;
		}

		private double noise (double x, double y, double z)
		{
			var halfLength = _p.Length;
			var cx = Floor(x) % halfLength;
			var cy = Floor(y) % halfLength;
			var cz = Floor(z) % halfLength;

			if(cx < 0)
				cx += halfLength;
			if(cy < 0)
				cy += halfLength;
			if(cz < 0)
				cz += halfLength;

			var u = Fade(x);
			var v = Fade(y);
			var w = Fade(z);

			// Hash coordinates of the corners
			int a  = _p[cx] + cy,
				 aa = _p[a] + cz,
				 ab = _p[a + 1] + cz,
				 b  = _p[cx + 1] + cy,
				 ba = _p[b] + cz,
				 bb = _p[b + 1] + cz;

			var p1 = Interpolate(
				Grad(_p[aa], x, y, z),
				Grad(_p[ba], x - 1, y, z),
				u);
			var p2 = Interpolate(
				Grad(_p[ab], x, y - 1, z),
				Grad(_p[bb], x - 1, y - 1, z),
				u);
			var p3 = Interpolate(
				Grad(_p[aa + 1], x, y, z - 1),
				Grad(_p[ba + 1], x - 1, y, z - 1),
				u);
			var p4 = Interpolate(
				Grad(_p[ab + 1], x, y - 1, z - 1),
				Grad(_p[bb + 1], x - 1, y - 1, z - 1),
				u);
			p1 = Interpolate(p1, p2, v);
			p2 = Interpolate(p3, p4, v);
			return Interpolate(p1, p2, w);
		}
	}
}
