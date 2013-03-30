using System;

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
		/// Creates a new nibble array from an array of bytes
		/// </summary>
		/// <param name="data">Array of bytes to get nibbles from</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
		public NibbleArray (byte[] data)
		{
			if(null == data)
				throw new ArgumentNullException("data", "The byte array can't be null.");

			_count = data.Length * 2;
			_data  = new byte[data.Length];
			data.Copy(_data);
		}

		/// <summary>
		/// Number of nibbles in the array
		/// </summary>
		public int Length
		{
			get { return _count; }
		}

		/// <summary>
		/// Number of bytes that the array uses
		/// </summary>
		public int ByteCount
		{
			get { return _data.Length; }
		}

		/// <summary>
		/// Nibble values contained in the array
		/// </summary>
		/// <param name="index">Index of the nibble</param>
		/// <returns>A nibble value</returns>
		public byte this[int index]
		{
			get
			{
				if(0 > index || index >= _data.Length * 2)
					throw new IndexOutOfRangeException("The index of the nibble is out of range.");

				var pos = index / 2;
				var val = _data[pos];
				if(0 == (index % 2)) // First half of the byte
					val &= 0x0f;
				else // Second half of the byte
					val >>= 4;
				return val;
			}

			set
			{
				if(0 > index || index >= _data.Length * 2)
					throw new IndexOutOfRangeException("The index of the nibble is out of range.");

				value &= 0x0f;
				var pos = index / 2;
				var val = _data[pos];
				if(0 == (index % 2)) // First half of the byte
					val = (byte)((val & 0xf0) | value);
				else // Second half of the byte
					val = (byte)((value << 4) | val & 0x0f);
				_data[pos] = val;
			}
		}

		/// <summary>
		/// Gets the contents of the array as an array of bytes
		/// </summary>
		/// <returns>A byte array</returns>
		public byte[] ToByteArray ()
		{
			var bytes = new byte[_data.Length];
			_data.Copy(bytes);
			return bytes;
		}
	}
}
