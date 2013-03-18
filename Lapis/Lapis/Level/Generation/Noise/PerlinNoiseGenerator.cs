using System;
using Lapis.Utility;

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces random, smooth noise
	/// </summary>
	public class PerlinNoiseGenerator : SmoothNoiseGenerator
	{
		private const int ByteLimit = byte.MaxValue + 1;

		private readonly int _xOff, _yOff, _zOff;
		private readonly byte[] _perm = new byte[ByteLimit * 2]; // We use 256 * 2 so we don't have to loop around

		/// <summary>
		/// Creates a new constant value noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		public PerlinNoiseGenerator (long seed)
		{
			// Unlike Java, .NET C# only supports 32-bit seeds
			// Split it up to achieve a similar random range
			var seed1 = (int)(seed & 0xffffffff);
			var seed2 = (int)((seed >> 32) & 0xffffffff);

			var rng = new Random(seed1);
			_xOff = rng.Next(ByteLimit);
			_yOff = rng.Next(ByteLimit);
			_zOff = rng.Next(ByteLimit);

			rng = new Random(seed2);
			for(var i = 0; i < ByteLimit; ++i)
				_perm[i] = (byte)rng.Next(ByteLimit);
			for(var i = 0; i < ByteLimit; ++i)
			{// Shuffle values and fill all 512 (last 256) elements
				var pos  = rng.Next(ByteLimit - i) + i;
				var prev = _perm[i];

				_perm[i] = _perm[pos];
				_perm[pos] = prev;
				_perm[ByteLimit + i] = _perm[i];
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
			var inX = x + _xOff;
			var inY = y + _yOff;
			var inZ = z + _zOff;

			var floorX = Floor(inX);
			var floorY = Floor(inY);
			var floorZ = Floor(inZ);

			var cubeX = floorX & byte.MaxValue;
			var cubeY = floorY & byte.MaxValue;
			var cubeZ = floorZ & byte.MaxValue;

			inX -= floorX;
			inY -= floorY;
			inZ -= floorZ;

			var fadeX = Fade(inX);
			var fadeY = Fade(inY);
			var fadeZ = Fade(inZ);

			var hashA  = _perm[cubeX] + cubeY;
			var hashAa = _perm[hashA] + cubeZ;
			var hashAb = _perm[hashA + 1] + cubeZ;
			var hashB  = _perm[cubeX + 1] + cubeZ;
			var hashBa = _perm[hashB] + cubeZ;
			var hashBb = _perm[hashB + 1] + cubeZ;

			var a = Interpolate(
						 Interpolate(
							 Grad(_perm[hashAa], inX, inY, inZ),
							Grad(_perm[hashBa], inX - 1, inY, inZ), fadeX),
						Interpolate(
							 Grad(_perm[hashAb], inX, inY - 1, inZ),
							Grad(_perm[hashBb], inX - 1, inY - 1, inZ), fadeX), fadeY);
			var b = Interpolate(
						 Interpolate(
							 Grad(_perm[hashAa + 1], inX, inY, inZ - 1),
							Grad(_perm[hashBa + 1], inX - 1, inY, inZ - 1), fadeX),
						Interpolate(
							 Grad(_perm[hashAb + 1], inX, inY - 1, inZ - 1),
							Grad(_perm[hashBb + 1], inX - 1, inY - 1, inZ - 1), fadeX), fadeY);
			return Interpolate(a, b, fadeZ);
		}
	}
}
