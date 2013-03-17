using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Utility
{
	/// <summary>
	/// An array that gives access to half-byte (4-bit) values
	/// </summary>
	public class NibbleArray
	{
		private readonly byte[] _data;
		private readonly int _count;

		/// <summary>
		/// Creates a new nibble array
		/// </summary>
		/// <param name="length">Number of nibbles in the array (not bytes)</param>
		public NibbleArray (int length)
		{
			if(0 > length)
				throw new ArgumentOutOfRangeException("length", "The length of the array can't be negative.");

			_count = length;
			var byteCount = length / 2;
			if(1 == length%2)
				++byteCount;
			_data = new byte[byteCount];
		}

		/// <summary>
		/// Number of nibbles in the array
		/// </summary>
		public int Length
		{
			get { return _count; }
		}

		public int ByteCount
		{
			get { return _data.Length; }
		}
	}
}
