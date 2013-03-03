using System;
using Lapis.Utility;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains an array of signed 32-bit integers
	/// </summary>
	public class IntArrayNode : Node
	{
		private int[] data;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.IntArray</remarks>
		public override NodeType Type
		{
			get { return NodeType.IntArray; }
		}

		/// <summary>
		/// Raw integers contained in the node
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if an attempt is made to provide a null reference when updating the field</exception>
		/// <remarks>Updating the contents of this field will copy the integers.</remarks>
		public int[] Data
		{
			get { return data; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The reference to the new set of integers can't be null.");

				lock(value)
				{
					data = new int[value.Length];
					copy(value, this.data);
				}
			}
		}

		/// <summary>
		/// Creates a new integer array node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="data">Data to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		/// <remarks>The integers passed in will be copied.</remarks>
		public IntArrayNode (string name, int[] data)
			: base(name)
		{
			if(null == data)
				throw new ArgumentNullException("data", "The reference to the integer array can't be null.");

			lock(data)
			{
				this.data = new int[data.Length];
				copy(data, this.data);
			}
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			int length = data.Length;
			bw.Write(length);
			foreach(int i in data)
				bw.Write(i);
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>An integer array node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static IntArrayNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			int length = br.ReadInt32();
			int[] data = new int[length];
			for(int i = 0; i < length; ++i)
				data[i] = br.ReadInt32();
			return new IntArrayNode(name, data);
		}
		#endregion

		/// <summary>
		/// Recursive method that appends the node's string form to a builder
		/// </summary>
		/// <param name="sb">Builder that contains the node strings</param>
		/// <param name="depth">Depth into the node structure (number of times to indent)</param>
		protected internal override void ToString (System.Text.StringBuilder sb, int depth)
		{
			sb.Append(StringIndent, depth);
			sb.Append(Type);
			sb.Append("(\"");
			sb.Append(Name);
			sb.Append("\"): [");
			sb.Append(data.Length);
			sb.Append(" integers]\n");
		}

		#region Utility
		/// <summary>
		/// Copies integers from one array to another.
		/// </summary>
		/// <remarks>A negative count will copy bytes until the end of either array is reached.</remarks>
		/// <param name="src">Array to copy integers from</param>
		/// <param name="dest">Array to put integers into</param>
		/// <param name="srcStart">Starting position in the source array (default is 0)</param>
		/// <param name="destStart">Starting position in the destination array (default is 0)</param>
		/// <param name="count">Number of integers to copy (default is -1)</param>
		private static void copy (int[] src, int[] dest, int srcStart = 0, int destStart = 0, int count = -1)
		{
			if(0 > count)	// Calculate where to stop
				count = Math.Min(src.Length - srcStart, dest.Length - destStart);
			int end = destStart + count;

#if !DEBUG
			unsafe
			{
				fixed(int* pSrc = src, pDest = dest)
				{
					int* ps = pSrc + srcStart;
					int* pd = pDest + destStart;
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
			Buffer.BlockCopy(src, srcStart, dest, destStart, count * sizeof(int));
#endif
		}
		#endregion
	}
}
