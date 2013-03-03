using System.Text;

namespace Lapis.Utility
{
	/// <summary>
	/// Utility class for converting strings (primarily to and from byte arrays)
	/// </summary>
	public static class StringManipulation
	{
		/// <summary>
		/// Converts a string to an array of bytes encoded as UTF-8
		/// </summary>
		/// <param name="value">String to convert</param>
		/// <returns>An array of bytes that contains the original string</returns>
		public static byte[] GetBytes (this string value)
		{
			var bytes = Encoding.UTF8.GetBytes(value);
			return bytes;
		}

		/// <summary>
		/// Converts a string to an array of bytes encoded as UTF-8.
		/// The length provided ensures that the byte array returned is exactly that length.
		/// </summary>
		/// <remarks>Extra available bytes are padded with null.
		/// If the string doesn't fit, it will be truncated.</remarks>
		/// <param name="value">String to convert</param>
		/// <param name="length">Number of bytes to allocate for the string</param>
		/// <returns>An array of bytes that contains the original string</returns>
		public static byte[] GetBytes (this string value, int length)
		{
			var temp  = Encoding.UTF8.GetBytes(value);
			var bytes = new byte[length];
			temp.Copy(bytes);
			return bytes;
		}

		/// <summary>
		/// Extracts a UTF-8 encoded string from an array of bytes
		/// </summary>
		/// <remarks>The extraction stops when null is found, or the end of the array is reached.</remarks>
		/// <param name="bytes">Array of bytes to pull the value from</param>
		/// <param name="offset">Offset into the array that the value starts at (default is 0)</param>
		/// <returns>The extracted value</returns>
		public static string ToUtf8String (this byte[] bytes, int offset = 0)
		{
			string value;
			if(0 < offset)
			{
				var temp = new byte[bytes.Length - offset];
				bytes.Copy(temp, offset);
				value = Encoding.UTF8.GetString(temp);
			}
			else
				value = Encoding.UTF8.GetString(bytes);
			value = value.TrimEnd('\0');
			return value;
		}
	}
}
