using System;
using System.Linq;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces random, smooth noise
	/// </summary>
	/// <remarks>An octave is a set a noise.
	/// Consecutive octaves can be stacked (or added) together to give more detail.
	/// Persistence determines how much each additional octave affects the final result.</remarks>
	public class SimplexNoiseGenerator : SmoothNoiseGenerator
	{
		private const int MaxOffset = 512;
		private const int PermSize  = 256;

		private readonly int[] _perm = new int[PermSize * 2];
		private readonly double _xOff, _yOff, _zOff;

		/// <summary>
		/// Creates a new simplex noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		public SimplexNoiseGenerator (long seed)
		{
			// Unlike Java, .NET C# only supports 32-bit seeds
			// Split it up to achieve a similar random range
			var seed1 = (int)(seed & 0xffffffff);
			var seed2 = (int)((seed >> 32) & 0xffffffff);

			var rng = new Random(seed1);
			_xOff = rng.Next(MaxOffset);
			_yOff = rng.Next(MaxOffset);
			_zOff = rng.Next(MaxOffset);

			SeedPermutation(seed2, _perm);
		}

		private static void SeedPermutation (int seed, int[] perm)
		{
			var src = Enumerable.Range(0, PermSize).ToArray();
			var rng = new Random(seed);
			for(var i = 0; i < src.Length; ++i)
			{
				var swap  = rng.Next(src.Length);
				var temp  = src[i];
				src[i]    = src[swap];
				src[swap] = temp;
			}

			for(var i = 0; i < src.Length; ++i)
				perm[src.Length + i] = perm[i] = src[i];
		}

		/// <summary>
		/// Generates two-dimensional noise
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <returns>A noise value</returns>
		protected override double Generate (double x, double y)
		{
			x += _xOff;
			y += _yOff;

			double n0, n1, n2; // Noise contributions from the three corners

			// Skew the input space to determine which simplex cell we're in
			var s  = (x + y) * MathConstants.F2; // Hairy factor for 2D
			var xs = x + s;
			var ys = y + s;
			var i  = Floor(xs);
			var j  = Floor(ys);

			var t  = (i + j) * MathConstants.G2;
			var u0 = i - t; // Unskew the cell origin back to (x,y) space
			var v0 = j - t;
			var x0 = x - u0; // The x,y distances from the cell origin
			var y0 = y - v0;

			// For the 2D case, the simplex shape is an equilateral triangle.
			// Determine which simplex we are in.
			int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
			if (x0 > y0)
			{// lower triangle, XY order: (0,0)->(1,0)->(1,1)
				i1 = 1;
				j1 = 0;
			}
			else
			{// upper triangle, YX order: (0,0)->(0,1)->(1,1)
				i1 = 0;
				j1 = 1;
			}

			// A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
			// a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
			// c = (3-sqrt(3))/6

			var x1 = x0 - i1 + MathConstants.G2; // Offsets for middle corner in (x,y) unskewed coords
			var y1 = y0 - j1 + MathConstants.G2;
			var x2 = x0 - 1.0f + 2.0f * MathConstants.G2; // Offsets for last corner in (x,y) unskewed coords
			var y2 = y0 - 1.0f + 2.0f * MathConstants.G2;

			// Wrap the integer indices at half of the permutation length, to avoid indexing _perm[] out of bounds
			var halfLength = _perm.Length / 2;
			var ii = i % halfLength;
			var jj = j % halfLength;

			// Calculate the contribution from the three corners
			var t0 = 0.5 - x0 * x0 - y0 * y0;
			if(t0 < 0.0)
				n0 = 0.0;
			else
			{
				t0 *= t0;
				n0  = t0 * t0 * Grad(_perm[ii + _perm[jj]], x0, y0);
			}

			var t1 = 0.5f - x1 * x1 - y1 * y1;
			if(t1 < 0.0)
				n1 = 0.0;
			else
			{
				t1 *= t1;
				n1  = t1 * t1 * Grad(_perm[ii + i1 + _perm[jj + j1]], x1, y1);
			}

			var t2 = 0.5 - x2 * x2 - y2 * y2;
			if(t2 < 0.0)
				n2 = 0.0f;
			else
			{
				t2 *= t2;
				n2  = t2 * t2 * Grad(_perm[ii + 1 + _perm[jj + 1]], x2, y2);
			}

			// Add contributions from each corner to get the final noise value.
			// The result is scaled to return values in the interval [-1,1].
			return 40.0 * (n0 + n1 + n2); // TODO: The scale factor is preliminary!
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
			x += _xOff;
			y += _yOff;
			z += _zOff;

			double n0, n1, n2, n3; // Noise contributions from the four corners

			// Skew the input space to determine which simplex cell we're in
			var s  = (x + y + z) * MathConstants.F3; // Very nice and simple skew factor for 3D
			var xs = x + s;
			var ys = y + s;
			var zs = z + s;
			var i  = Floor(xs);
			var j  = Floor(ys);
			var k  = Floor(zs);

			var t  = (i + j + k) * MathConstants.G3;
			var u0 = i - t; // Unskew the cell origin back to (x,y,z) space
			var v0 = j - t;
			var w0 = k - t;
			var x0 = x - u0; // The x,y,z distances from the cell origin
			var y0 = y - v0;
			var z0 = z - w0;

			// For the 3D case, the simplex shape is a slightly irregular tetrahedron.
			// Determine which simplex we are in.
			int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
			int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

			/* This code would benefit from a backport from the GLSL version! */
			if(x0 >= y0)
			{
				if(y0 >= z0)
				{// XYZ order
					i1 = 1; j1 = 0; k1 = 0;
					i2 = 1; j2 = 1; k2 = 0;
				}
				else if(x0 >= z0)
				{// XZY order
					i1 = 1; j1 = 0; k1 = 0;
					i2 = 1; j2 = 0; k2 = 1;
				}
				else
				{// ZXY order
					i1 = 0; j1 = 0; k1 = 1;
					i2 = 1; j2 = 0; k2 = 1;
				}
			}
			else
			{// x0 < y0
				if(y0 < z0)
				{// ZYX order
					i1 = 0; j1 = 0; k1 = 1;
					i2 = 0; j2 = 1; k2 = 1;
				}
				else if(x0 < z0)
				{// YZX order
					i1 = 0; j1 = 1; k1 = 0;
					i2 = 0; j2 = 1; k2 = 1;
				}
				else
				{// YXZ order
					i1 = 0; j1 = 1; k1 = 0;
					i2 = 1; j2 = 1; k2 = 0;
				}
			}

			// A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
			// a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
			// a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
			// c = 1/6.

			var x1 = x0 - i1 + MathConstants.G3; // Offsets for second corner in (x,y,z) coords
			var y1 = y0 - j1 + MathConstants.G3;
			var z1 = z0 - k1 + MathConstants.G3;
			var x2 = x0 - i2 + 2.0 * MathConstants.G3; // Offsets for third corner in (x,y,z) coords
			var y2 = y0 - j2 + 2.0 * MathConstants.G3;
			var z2 = z0 - k2 + 2.0 * MathConstants.G3;
			var x3 = x0 - 1.0 + 3.0 * MathConstants.G3; // Offsets for last corner in (x,y,z) coords
			var y3 = y0 - 1.0 + 3.0 * MathConstants.G3;
			var z3 = z0 - 1.0 + 3.0 * MathConstants.G3;

			// Wrap the integer indices at half of the permutation length, to avoid indexing _perm[] out of bounds
			var halfLength = _perm.Length / 2;
			var ii = i % halfLength;
			var jj = j % halfLength;
			var kk = k % halfLength;

			// Calculate the contribution from the four corners
			var t0 = 0.6 - x0 * x0 - y0 * y0 - z0 * z0;
			if(t0 < 0.0)
				n0 = 0.0;
			else
			{
				t0 *= t0;
				n0  = t0 * t0 * Grad(_perm[ii + _perm[jj + _perm[kk]]], x0, y0, z0);
			}

			var t1 = 0.6 - x1 * x1 - y1 * y1 - z1 * z1;
			if(t1 < 0.0)
				n1 = 0.0;
			else
			{
				t1 *= t1;
				n1  = t1 * t1 * Grad(_perm[ii + i1 + _perm[jj + j1 + _perm[kk + k1]]], x1, y1, z1);
			}

			var t2 = 0.6 - x2 * x2 - y2 * y2 - z2 * z2;
			if(t2 < 0.0)
				n2 = 0.0;
			else
			{
				t2 *= t2;
				n2  = t2 * t2 * Grad(_perm[ii + i2 + _perm[jj + j2 + _perm[kk + k2]]], x2, y2, z2);
			}

			var t3 = 0.6 - x3 * x3 - y3 * y3 - z3 * z3;
			if(t3 < 0.0)
				n3 = 0.0;
			else
			{
				t3 *= t3;
				n3  = t3 * t3 * Grad(_perm[ii + 1 + _perm[jj + 1 + _perm[kk + 1]]], x3, y3, z3);
			}

			// Add contributions from each corner to get the final noise value.
			// The result is scaled to stay just inside [-1,1]
			return 32.0 * (n0 + n1 + n2 + n3); // TODO: The scale factor is preliminary!
		}
	}
}
