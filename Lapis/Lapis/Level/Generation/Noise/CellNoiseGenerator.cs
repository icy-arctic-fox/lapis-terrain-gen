using System;

// TODO: Optimize this algorithm

namespace Lapis.Level.Generation.Noise
{
	/// <summary>
	/// A noise generator that produces random, cell-like noise (Worley)
	/// </summary>
	public class CellNoiseGenerator : SmoothNoiseGenerator
	{
		private const int MaxOffset = 256;

		private readonly DistanceMethod2D _distanceMethod2D;
		private readonly DistanceMethod3D _distanceMethod3D;
		private readonly ValueCombinationMethod _combinationMethod;

		private readonly int _seed;
		private readonly int _xOff, _yOff, _zOff;

		/// <summary>
		/// Creates a new cell noise generator
		/// </summary>
		/// <param name="seed">Random seed</param>
		public CellNoiseGenerator (long seed, DistanceMethod method = DistanceMethod.Euclidian, CombinationMethod combination = CombinationMethod.D1)
		{
			// Unlike Java, .NET C# only supports 32-bit seeds
			// Split it up to achieve a similar random range
			var seed1 = (int)(seed & 0xffffffff);
			_seed    = (int)((seed >> 32) & 0xffffffff);

			var rng = new Random(seed1);
			_xOff = rng.Next(MaxOffset);
			_yOff = rng.Next(MaxOffset);
			_zOff = rng.Next(MaxOffset);

			switch(method)
			{
			case DistanceMethod.Manhattan:
				_distanceMethod2D = manhattanDistance;
				_distanceMethod3D = manhattanDistance;
				break;
			case DistanceMethod.Chebyshev:
				_distanceMethod2D = chebyshevDistance;
				_distanceMethod3D = chebyshevDistance;
				break;
			default:
				_distanceMethod2D = euclidianDistance;
				_distanceMethod3D = euclidianDistance;
				break;
			}

			switch(combination)
			{
			case CombinationMethod.D2MinusD1:
				_combinationMethod = d2MinusD1Combination;
				break;
			case CombinationMethod.D3MinusD1:
				_combinationMethod = d3MinusD1Combination;
				break;
			default:
				_combinationMethod = d1Combination;
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
			return Generate(x, y, 0); // TODO: Possibly optimize for 2D
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

			//Initialize values in distance array to large values
			var distanceArray = new double[3];
			for(var i = 0; i < distanceArray.Length; i++)
				distanceArray[i] = Double.MaxValue;

			// Determine which cube the evaluation point is in
			var evalCubeX = Floor(x);
			var evalCubeY = Floor(y);
			var evalCubeZ = Floor(z);

			for(var i = -1; i < 2; ++i)
				for(var j = -1; j < 2; ++j)
					for(var k = -1; k < 2; ++k)
					{// Check each neighboring cube for evaluation points
						var cubeX = evalCubeX + i;
						var cubeY = evalCubeY + j;
						var cubeZ = evalCubeZ + k;

						// Generate a reproducible random number generator for the cube
						var prevRandom = lcgRandom(hash(_seed, cubeX, cubeY, cubeZ));

						// Determine how many feature points are in the cube
						var featureCount = probabilityLookup(prevRandom);

						// Randomly place the feature points in the cube
						for(uint l = 0; l < featureCount; ++l)
						{
							prevRandom = lcgRandom(prevRandom);
							var dX     = (double)prevRandom / 0x100000000;
							var fX     = dX + cubeX;

							prevRandom = lcgRandom(prevRandom);
							var dY     = (double)prevRandom / 0x100000000;
							var fY     = dY + cubeY;

							prevRandom = lcgRandom(prevRandom);
							var dZ     = (double)prevRandom / 0x100000000;
							var fZ     = dZ + cubeZ;

							// Find the feature point closest to the evaluation point.
							// This is done by inserting the distances to the feature points into a sorted list.
							insert(distanceArray, _distanceMethod3D(x, y, z, fX, fY, fZ));
						}
						// Check the neighboring cubes to ensure their are no closer evaluation points.
						// This is done by repeating the steps for each neighboring cube
					}

			// TODO: Verify output range is -1 to 1
			var value = _combinationMethod(distanceArray);
			value = value * 4.0 - 1.0;
			if(-1.0 > value)
				value = 0.0;
			else if(1.0 < value)
				value = 1.0;
			return value;
		}

		#region Distance methods
		/// <summary>
		/// Method used for calculating the distance between two feature points
		/// </summary>
		public enum DistanceMethod
		{
			Euclidian,

			Manhattan,

			Chebyshev
		}

		private delegate double DistanceMethod2D(double x1, double y1, double x2, double y2);
		private delegate double DistanceMethod3D (double x1, double y1, double z1, double x2, double y2, double z2);

		private static double euclidianDistance (double x1, double y1, double x2, double y2)
		{
			var x = (x1 - x2) * (x1 - x2);
			var y = (y1 - y2) * (y1 - y2);
			return x + y;
		}

		private static double euclidianDistance (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			var x = (x1 - x2) * (x1 - x2);
			var y = (y1 - y2) * (y1 - y2);
			var z = (z1 - z2) * (z1 - z2);
			return x + y + z;
		}

		private static double manhattanDistance (double x1, double y1, double x2, double y2)
		{
			var x = Math.Abs(x1 - x2);
			var y = Math.Abs(y1 - y2);
			return x + y;
		}

		private static double manhattanDistance (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			var x = Math.Abs(x1 - x2);
			var y = Math.Abs(y1 - y2);
			var z = Math.Abs(z1 - z2);
			return x + y + z;
		}

		private static double chebyshevDistance (double x1, double y1, double x2, double y2)
		{
			var x = Math.Abs(x1 - x2);
			var y = Math.Abs(y1 - y2);
			return Math.Max(x, y);
		}

		private static double chebyshevDistance (double x1, double y1, double z1, double x2, double y2, double z2)
		{
			var x = Math.Abs(x1 - x2);
			var y = Math.Abs(y1 - y2);
			var z = Math.Abs(z1 - z2);
			return Math.Max(x, Math.Max(y, z));
		}
		#endregion

		#region Combination methods
		/// <summary>
		/// Method used for combining values
		/// </summary>
		public enum CombinationMethod
		{
			D1,

			D2MinusD1,

			D3MinusD1
		}

		private delegate double ValueCombinationMethod(double[] values);

		private static double d1Combination (double[] values)
		{
			return values[0];
		}

		private static double d2MinusD1Combination (double[] values)
		{
			return values[1] - values[0];
		}

		private static double d3MinusD1Combination (double[] values)
		{
			return values[2] - values[0];
		}
		#endregion

		#region Utility methods
		private const uint OffsetBasis = 2166136261;
		private const uint FnvPrime    = 16777619;

		private static uint hash (int seed, int i, int j, int k)
		{
			long hash;
			unchecked
			{
				hash  = OffsetBasis ^ seed;
				hash *= FnvPrime;
				hash ^= i;
				hash *= FnvPrime;
				hash ^= j;
				hash *= FnvPrime;
				hash ^= k;
				hash *= FnvPrime;
			}
			return (uint)(hash & 0xffffffff);
		}

		private static uint lcgRandom (uint prevValue)
		{
			return (uint)unchecked((1103515245 * prevValue + 12345) % 0x100000000);
		}

		private static uint probabilityLookup (uint value)
		{
			if(value < 393325350)  return 1;
			if(value < 1022645910) return 2;
			if(value < 1861739990) return 3;
			if(value < 2700834071) return 4;
			if(value < 3372109335) return 5;
			if(value < 3819626178) return 6;
			if(value < 4075350088) return 7;
			if(value < 4203212043) return 8;
			return 9;
		}

		private static void insert (double[] arr, double value)
		{
			for(var i = arr.Length - 1; i >= 0; --i)
			{
				if(value > arr[i])
					break;

				var temp = arr[i];
				arr[i]   = value;

				if(i + 1 < arr.Length)
					arr[i + 1] = temp;
			}
		}
		#endregion
	}
}
