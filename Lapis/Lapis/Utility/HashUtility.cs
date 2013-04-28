using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Utility
{
	/// <summary>
	/// Various methods for generating hash values
	/// </summary>
	public static class HashUtility
	{
		/// <summary>
		/// Generates a hash from an array of bytes
		/// </summary>
		/// <param name="data">Array of bytes to hash</param>
		/// <returns>A hash code</returns>
		public static int GenerateHash (this byte[] data)
		{
			if(null == data || 0 >= data.Length)
				return 0;

			var length    = data.Length;
			var remainder = length & 3;
			length >>= 2;

			var hash = length;
			var pos  = 0;
			unchecked
			{
				for(; length > 0; --length)
				{
					hash += get16Bits(data, pos);
					var temp = (get16Bits(data, pos + sizeof(short)) << 11) ^ hash;
					hash  = (hash << 16) ^ temp;
					pos  += 2 * sizeof(short);
					hash += hash >> 11;
				}

				switch(remainder)
				{
					case 3:
						hash += get16Bits(data, pos);
						hash ^= hash << 16;
						hash ^= data[pos + 2] << 18;
						hash += hash >> 11;
						break;
					case 2:
						hash += get16Bits(data, pos);
						hash ^= hash << 11;
						hash += hash >> 17;
						break;
					case 1:
						hash += data[pos];
						hash ^= hash << 10;
						hash += hash >> 1;
						break;
				}

				hash ^= hash << 3;
				hash += hash >> 5;
				hash ^= hash << 4;
				hash += hash >> 17;
				hash ^= hash << 25;
				hash += hash >> 6;
			}

			return hash;
		}

		/// <summary>
		/// Generates a 64-bit hash from an array of bytes
		/// </summary>
		/// <param name="data">Array of bytes to hash</param>
		/// <returns>A hash code</returns>
		public static long GenerateLongHash (this byte[] data)
		{
			if(null == data || 0 >= data.Length)
				return 0;

			var length    = data.LongLength;
			var remainder = length & 3;
			length >>= 2;

			var hash = length;
			var pos  = 0;
			unchecked
			{
				for(; length > 0; --length)
				{
					hash += get16Bits(data, pos);
					var temp = (get16Bits(data, pos + sizeof(short)) << 11) ^ hash;
					hash  = (hash << 16) ^ temp;
					pos  += 2 * sizeof(short);
					hash += hash >> 11;
				}

				switch(remainder)
				{
					case 3:
						hash += get16Bits(data, pos);
						hash ^= hash << 16;
						hash ^= data[pos + 2] << 18;
						hash += hash >> 11;
						break;
					case 2:
						hash += get16Bits(data, pos);
						hash ^= hash << 11;
						hash += hash >> 17;
						break;
					case 1:
						hash += data[pos];
						hash ^= hash << 10;
						hash += hash >> 1;
						break;
				}

				hash ^= hash << 3;
				hash += hash >> 5;
				hash ^= hash << 4;
				hash += hash >> 17;
				hash ^= hash << 25;
				hash += hash >> 6;
			}

			return hash;
		}

		private static short get16Bits (byte[] data, int pos)
		{
			return BitConverter.ToInt16(data, pos);
		}

		/// <summary>
		/// Computes a hash from multiple objects
		/// </summary>
		/// <param name="objs">Objects to hash</param>
		/// <returns>A hash value</returns>
		public static int ComputeHash (params object[] objs)
		{
			var hash = 0;
			unchecked
			{
				for(var i = 0; i < objs.Length; ++i)
				{
					var obj = objs[i];
					hash <<= 7;
					hash  ^= (ReferenceEquals(null, obj) ? (int)0xffffffff : obj.GetHashCode());
				}
			}
			return hash;
		}
	}
}
