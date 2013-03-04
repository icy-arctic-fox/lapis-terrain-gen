namespace Lapis.Spatial
{
	/// <summary>
	/// A coordinate that contains an X and Z value
	/// </summary>
	public struct XZCoordinate
	{
		private readonly int _x, _z;

		/// <summary>
		/// X-position
		/// </summary>
		public int X
		{
			get { return _x; }
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
		/// <param name="z">Z-position</param>
		public XZCoordinate (int x, int z)
		{
			_x = x;
			_z = z;
		}

		/// <summary>
		/// Checks whether or not an object is equal to the coordinate
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if the object is considered equal or false if it isn't</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a XZCoordinate, and has identical x and z values</remarks>
		public override bool Equals (object obj)
		{
			if(null != obj)
			{
				if(obj is XZCoordinate)
				{
					var coord = (XZCoordinate)obj;
					return (coord._x == _x && coord._z == _z);
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
			hash = hash * 31 + _x;
			hash = hash * 31 + _z;
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
			sb.Append(')');
			return sb.ToString();
		}
	}
}
