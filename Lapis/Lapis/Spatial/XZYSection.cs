using System;

namespace Lapis.Spatial
{
	/// <summary>
	/// A coordinate that references a chunk section within a realm
	/// </summary>
	public struct XZYSection
	{
		private readonly int _x, _z;
		private readonly byte _y;

		/// <summary>
		/// X-position of the chunk within the realm
		/// </summary>
		public int ChunkX
		{
			get { return _x; }
		}

		/// <summary>
		/// Z-position of the chunk within the realm
		/// </summary>
		public int ChunkZ
		{
			get { return _z; }
		}

		/// <summary>
		/// The section within the chunk
		/// </summary>
		/// <remarks>The section index goes from 0 to 15 - 0 being y = 0 (bedrock) and y = 15 being the top-most.</remarks>
		public byte Section
		{
			get { return _y; }
		}

		/// <summary>
		/// Creates a new coordinate
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="z">Z-position</param>
		/// <param name="y">Index of the chunk section</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="y"/> is outside the allowed bounds (0 - 15)</exception>
		public XZYSection (int x, int z, byte y)
		{
			if(Level.Chunk.SectionCount <= y)
				throw new ArgumentOutOfRangeException("y", "The index of the chunk section is out of bounds.");

			_x = x;
			_z = z;
			_y = y;
		}

		/// <summary>
		/// Checks whether or not an object is equal to the coordinate
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if the object is considered equal or false if it isn't</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a XZYSection, and has identical x, z, and y-index values</remarks>
		public override bool Equals (object obj)
		{
			if(null != obj)
			{
				if(obj is XZYSection)
				{
					var coord = (XZYSection)obj;
					return (coord._x == _x && coord._z == _z && coord._y == _y);
				}
			}
			return false;
		}

		/// <summary>
		/// Generates a hash code from the coordinate
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = 17;
			unchecked
			{
				hash ^= _x;
				hash *= 31;
				hash ^= _z;
				hash *= 31;
				hash ^= _y;
			}
			return hash;
		}

		/// <summary>
		/// Generates a string from the coordinate
		/// </summary>
		/// <returns>A string containing the coordinate</returns>
		/// <remarks>The string will be formatted as: (X, Z)</remarks>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append('(');
			sb.Append(_x);
			sb.Append(", ");
			sb.Append(_z);
			sb.Append(") Section #");
			sb.Append(_y);
			return sb.ToString();
		}
	}
}
