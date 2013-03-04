using System;
using Lapis.IO;

namespace Lapis.Utility
{
	/// <summary>
	/// Utility class for manipulating arrays of bytes
	/// </summary>
	public static class ByteArrayUtility
	{
		#region Extration
		/// <summary>
		/// Extracts a short integer from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static short ToShort (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			short value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(short)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToInt16(temp, 0);
			}
			else
				value = BitConverter.ToInt16(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts an unsigned short integer from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static ushort ToUShort (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			ushort value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(ushort)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToUInt16(temp, 0);
			}
			else
				value = BitConverter.ToUInt16(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts an integer from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static int ToInt (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			int value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(int)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToInt32(temp, 0);
			}
			else
				value = BitConverter.ToInt32(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts an unsigned integer from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static uint ToUInt (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			uint value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(uint)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToUInt32(temp, 0);
			}
			else
				value = BitConverter.ToUInt32(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts a long integer from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static long ToLong (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			long value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(long)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToInt64(temp, 0);
			}
			else
				value = BitConverter.ToInt64(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts an unsigned integer from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static ulong ToULong (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			ulong value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(ulong)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToUInt64(temp, 0);
			}
			else
				value = BitConverter.ToUInt64(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts a floating-point value (single precision) from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static float ToFloat (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			float value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(float)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToSingle(temp, 0);
			}
			else
				value = BitConverter.ToSingle(bytes, offset);
			return value;
		}

		/// <summary>
		/// Extracts a floating-point value (double precision) from an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <param name="e">Endianness that the value is in</param>
		/// <returns>The extracted value</returns>
		public static double ToDouble (this byte[] bytes, int offset = 0, Endian e = Endian.Big)
		{
			double value;
			if(needFlip(e))
			{
				var temp = new byte[sizeof(double)];
				Copy(bytes, temp, offset);
				Reverse(temp);
				value = BitConverter.ToDouble(temp, 0);
			}
			else
				value = BitConverter.ToDouble(bytes, offset);
			return value;
		}
		#endregion

		#region Insertion
		/// <summary>
		/// Converts a short integer and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, short value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts an unsigned short integer and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, ushort value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts an integer and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, int value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts an unsigned integer and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, uint value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts a long integer and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, long value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts an unsigned long integer and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, ulong value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts a floating-point value (single precision) and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, float value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}

		/// <summary>
		/// Converts a floating-point value (double precision) and inserts it into an array of bytes
		/// </summary>
		/// <param name="bytes">Array of bytes to store the value in</param>
		/// <param name="value">Value to convert</param>
		/// <param name="offset">Offset into the array to put the value at (default is 0)</param>
		/// <param name="e">Endianness (default is big-endian)</param>
		public static void Insert (this byte[] bytes, double value, int offset = 0, Endian e = Endian.Big)
		{
			var temp = BitConverter.GetBytes(value);
			if(needFlip(e))
				Reverse(temp);
			Copy(temp, bytes, 0, offset);
		}
		#endregion

		/// <summary>
		/// Copies bytes from one array to another
		/// </summary>
		/// <remarks>A negative count will copy bytes until the end of either array is reached.</remarks>
		/// <param name="src">Array to copy bytes from</param>
		/// <param name="dest">Array to put bytes into</param>
		/// <param name="srcStart">Starting position in the source array (default is 0)</param>
		/// <param name="destStart">Starting position in the destination array (default is 0)</param>
		/// <param name="count">Number of bytes to copy (default is -1)</param>
		public static void Copy (this byte[] src, byte[] dest, int srcStart = 0, int destStart = 0, int count = -1)
		{
			if(0 > count)	// Calculate where to stop
				count = Math.Min(src.Length - srcStart, dest.Length - destStart);

#if !DEBUG
			unsafe
			{
				fixed(byte* pSrc = src, pDest = dest)
				{
					var ps = pSrc + srcStart;
					var pd = pDest + destStart;
#if X64
					var stop = count / 8;
#else
					var stop = count / 4;
#endif
					for(var i = 0; i < stop; ++i)
					{
#if X64
						*((long*)pd) = *((long*)ps);
						pd += 8;
						ps += 8;
					}
					stop = count % 8;
#else
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}
					stop = count % 4;
#endif
					for(var i = 0; i < stop; ++i)
					{
						*pd = *ps;
						++pd;
						++ps;
					}
				}
			}
#else
			Buffer.BlockCopy(src, srcStart, dest, destStart, count);
#endif
		}

		/// <summary>
		/// Reverses an array of bytes.
		/// A new array is not created, the one provided is reversed in itself.
		/// </summary>
		/// <remarks>This can effectively flip the endianness of some types.</remarks>
		/// <param name="bytes">Array to reverse</param>
		public static void Reverse (this byte[] bytes)
		{
			var end = bytes.Length;
			var mid = end / 2;
			for(int i = 0, j = end - 1; i < mid; ++i, --j)
			{
				var temp = bytes[i];
				bytes[i] = bytes[j];
				bytes[j] = temp;
			}
		}

		#region Nibble
		/// <summary>
		/// Gets a nibble (4 bits) from an array of bytes
		/// </summary>
		/// <param name="data">Array of bytes</param>
		/// <param name="index">Index of the nibble</param>
		/// <returns>The value of the nibble at the given index</returns>
		public static byte GetNibble (this byte[] data, int index)
		{
			if(null == data)
				throw new ArgumentNullException("data", "The byte array can't be null.");
			if(0 > index || index >= data.Length * 2)
				throw new IndexOutOfRangeException("The index of the nibble is out of range.");

			var pos = index / 2;
			var val = data[pos];
			if(0 == (index % 2)) // First half of the byte
				val &= 0x0f;
			else // Second half of the byte
				val >>= 4;
			return val;
		}

		/// <summary>
		/// Stores a nibble (4 bits) into an array of bytes
		/// </summary>
		/// <param name="data">Array of bytes</param>
		/// <param name="index">Index of the nibble</param>
		/// <param name="value">Value of the nibble to store</param>
		public static void SetNibble (this byte[] data, int index, byte value)
		{
			if(null == data)
				throw new ArgumentNullException("data", "The byte array can't be null.");
			if(0 > index || index >= data.Length * 2)
				throw new IndexOutOfRangeException("The index of the nibble is out of range.");

			value  &= 0x0f;
			var pos = index / 2;
			var val = data[pos];
			if(0 == (index % 2)) // First half of the byte
				val = (byte)((val & 0xf0) | value);
			else // Second half of the byte
				val = (byte)((value << 4) | val & 0x0f);
			data[pos] = val;
		}
		#endregion

		/// <summary>
		/// Checks if the bytes need to be flipped based on the desired endian and the system's architecture.
		/// </summary>
		/// <param name="e">Desired endian</param>
		/// <returns>True if the bytes need to be flipped or false if they should be flipped</returns>
		private static bool needFlip (Endian e)
		{
			return ((BitConverter.IsLittleEndian && Endian.Big == e) ||
				(!BitConverter.IsLittleEndian && Endian.Little == e));
		}
	}
}
