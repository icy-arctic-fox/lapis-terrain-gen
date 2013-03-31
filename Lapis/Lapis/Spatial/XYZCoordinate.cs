namespace Lapis.Spatial
{
	/// <summary>
	/// Coordinate containing a x, y, and z position
	/// </summary>
	public struct XYZCoordinate
	{
		private readonly int _x, _y, _z;

		/// <summary>
		/// X-position
		/// </summary>
		public int X
		{
			get { return _x; }
		}

		/// <summary>
		/// Y-position
		/// </summary>
		public int Y
		{
			get { return _y; }
		}

		/// <summary>
		/// Z-position
		/// </summary>
		public int Z
		{
			get { return _z; }
		}

		/// <summary>
		/// Creates a new coordinate
		/// </summary>
		/// <param name="x">X-position</param>
		/// <param name="y">Y-position</param>
		/// <param name="z">Z-position</param>
		public XYZCoordinate (int x, int y, int z)
		{
			_x = x;
			_z = z;
			_y = y;
		}

		/// <summary>
		/// Checks whether or not an object is equal to the coordinate
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if the object is considered equal or false if it isn't</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a XYZCoordinate, and has identical x, y, and z values</remarks>
		public override bool Equals (object obj)
		{
			if(null != obj)
			{
				if(obj is XYZCoordinate)
				{
					var coord = (XYZCoordinate)obj;
					return (coord._x == _x && coord._y == _y && coord._z == _z);
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
				hash ^= _y;
				hash *= 31;
				hash ^= _z;
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
			sb.Append(_y);
			sb.Append(", ");
			sb.Append(_z);
			sb.Append(')');
			return sb.ToString();
		}
	}
}
