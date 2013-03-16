using System;

namespace Lapis.Utility
{
	/// <summary>
	/// Utility class for flipping the endian of values
	/// </summary>
	public static class EndianUtility
	{
		/// <summary>
		/// Flips the endian for a short integer
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static short FlipEndian (this short value)
		{
			var newValue = (short)((value & 0xff) << 8);
			newValue    |= (short)((value >> 8) & 0xff);
			return newValue;
		}

		/// <summary>
		/// Flips the endian for an unsigned short integer
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static ushort FlipEndian (this ushort value)
		{
			var newValue = (ushort)((value & 0xff) << 8);
			newValue    |= (ushort)((value >> 8) & 0xff);
			return newValue;
		}

		/// <summary>
		/// Flips the endian for an integer
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static int FlipEndian (this int value)
		{
			var newValue = (value >> 24) & 0xff;
			newValue    |= (value >> 8) & 0xff00;
			newValue    |= (value & 0xff00) << 8;
			newValue    |= (value & 0xff) << 24;
			return newValue;
		}

		/// <summary>
		/// Flips the endian for an unsigned integer
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static uint FlipEndian (this uint value)
		{
			var newValue = (value >> 24) & 0xff;
			newValue    |= (value >> 8) & 0xff00;
			newValue    |= (value & 0xff00) << 8;
			newValue    |= (value & 0xff) << 24;
			return newValue;
		}

		/// <summary>
		/// Flips the endian for an long integer
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static long FlipEndian (this long value)
		{
			var newValue = (value >> 56) & 0xff;
			newValue    |= (value >> 40) & 0xff00;
			newValue    |= (value >> 24) & 0xff0000;
			newValue    |= (value >> 8) & 0xff000000;
			newValue    |= (value & 0xff000000) << 8;
			newValue    |= (value & 0xff0000) << 24;
			newValue    |= (value & 0xff00) << 40;
			newValue    |= (value & 0xff) << 56;
			return newValue;
		}

		/// <summary>
		/// Flips the endian for an unsigned long integer
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static ulong FlipEndian (this ulong value)
		{
			var newValue = (value >> 56) & 0xff;
			newValue    |= (value >> 40) & 0xff00;
			newValue    |= (value >> 24) & 0xff0000;
			newValue    |= (value >> 8) & 0xff000000;
			newValue    |= (value & 0xff000000) << 8;
			newValue    |= (value & 0xff0000) << 24;
			newValue    |= (value & 0xff00) << 40;
			newValue    |= (value & 0xff) << 56;
			return newValue;
		}

		/// <summary>
		/// Flips the endian for a single-precision floating-point number
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static float FlipEndian (this float value)
		{
			float newValue;
#if !DEBUG
			unsafe
			{
				var src  = (byte*)(&value + 3);
				var dest = (byte*)(&newValue);
				*dest = *src;
				dest++;
				src++;
				*dest = *src;
				dest++;
				src++;
				*dest = *src;
				dest++;
				src++;
				*dest = *src;
			}
#else
			var bytes = BitConverter.GetBytes(value);
			bytes.Reverse();
			newValue = BitConverter.ToSingle(bytes, 0);
#endif
			return newValue;
		}

		/// <summary>
		/// Flips the endian for a double-precision floating-point number
		/// </summary>
		/// <param name="value">Value to flip</param>
		/// <returns>The flipped value</returns>
		public static double FlipEndian (this double value)
		{
			var temp = BitConverter.DoubleToInt64Bits(value);
			temp = FlipEndian(temp);
			var newValue = BitConverter.Int64BitsToDouble(temp);
			return newValue;
		}
	}
}
