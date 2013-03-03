using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Utility
{
	/// <summary>
	/// Utility class for manipulating arrays of bytes
	/// </summary>
	public static class ByteArrayUtility
	{
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
			int end = destStart + count;

#if !DEBUG
			unsafe
			{
				fixed(byte* pSrc = src, pDest = dest)
				{
					byte* ps = pSrc + srcStart;
					byte* pd = pDest + destStart;
#if X64
					int stop = count / 8;
#else
					int stop = count / 4;
#endif
					for(int i = 0; i < stop; ++i)
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
					for(int i = 0; i < stop; ++i)
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
	}
}
