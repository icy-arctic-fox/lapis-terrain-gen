using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if !DEBUG
using Lapis.Utility;
#endif

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains an array of signed 32-bit integers
	/// </summary>
	public class IntArrayNode : Node, IEnumerable<int>
	{
		private int[] _data;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.IntArray</remarks>
		public override NodeType Type
		{
			get { return NodeType.IntArray; }
		}

		/// <summary>
		/// Value of the node represented as a string
		/// </summary>
		/// <remarks>This property produces the string: # integers</remarks>
		public override string StringValue
		{
			get { return _data.Length + " integers"; }
		}

		/// <summary>
		/// Raw integers contained in the node
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if an attempt is made to provide a null reference when updating the field</exception>
		/// <remarks>Updating the contents of this field will copy the integers.</remarks>
		public int[] Data
		{
			get { return _data; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The reference to the new set of integers can't be null.");

				lock(value)
				{
					_data = new int[value.Length];
					copy(value, _data);
				}
			}
		}

		/// <summary>
		/// Number of integers in the array
		/// </summary>
		public int Length
		{
			get { return _data.Length; }
		}

		/// <summary>
		/// Access to the underlying integer array
		/// </summary>
		/// <param name="index">Index of the integer to access</param>
		/// <returns>Integer value at the given index</returns>
		public int this[int index]
		{
			get { return _data[index]; }
			set { _data[index] = value; }
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
				_data = new int[data.Length];
				copy(data, _data);
			}
		}

		/// <summary>
		/// Duplicates the contents of the node and returns it
		/// </summary>
		/// <returns>A copy of the node</returns>
		/// <remarks>A deep copy of the node is performed.</remarks>
		public override Node CloneNode ()
		{
			return Duplicate();
		}

		/// <summary>
		/// Duplicates the contents of the node and returns it
		/// </summary>
		/// <returns>A copy of the node</returns>
		/// <remarks>A deep copy of the node is performed.</remarks>
		public IntArrayNode Duplicate ()
		{
			return new IntArrayNode(Name, _data);
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (BinaryWriter bw)
		{
			var length = _data.Length;
			bw.Write(length);
#if !DEBUG
			var ebw = bw as EndianBinaryWriter;
			if(null != ebw && needFlip(ebw.Endian))
				bw.Write(endianSpeedTrick());
			else
#endif
			foreach(var i in _data)
				bw.Write(i); // This is a costly operation when the endian is flipped
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>An integer array node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static IntArrayNode ReadPayload (BinaryReader br, string name)
		{
			var length = br.ReadInt32();
			var data   = new int[length];
#if !DEBUG
			var ebr = br as EndianBinaryReader;
			if(null != ebr && needFlip(ebr.Endian))
				endianSpeedTrick(ebr, data);
			else
#endif
			for(var i = 0; i < length; ++i)
				data[i] = br.ReadInt32();
			return new IntArrayNode(name, data);
		}

#if !DEBUG
		private static bool needFlip (Endian e)
		{
			return BitConverter.IsLittleEndian ^ Endian.Little == e;
		}

		/// <summary>
		/// Flips the endian in bulk for each integer quickly so that the binary writer doesn't have to do it individually
		/// </summary>
		/// <returns>An array of bytes containing the integers</returns>
		private byte[] endianSpeedTrick ()
		{
			var bytes = new byte[_data.Length * sizeof(int)];
			unsafe
			{
				fixed(int* pSrc = _data)
					fixed(byte* pDest = bytes)
					{
						var ps = pSrc;
						var pd = pDest;

						for(var i = 0; i < _data.Length; ++i)
						{
							var value = *ps;
							*(int*)pd = value.FlipEndian();
							++ps;
							pd += sizeof(int);
						}
					}
			}
			return bytes;
		}

		/// <summary>
		/// Reads the integers in bulk and then flips them so that the binary reader doesn't have to do it individually
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="data">Array to store the integers in</param>
		private static void endianSpeedTrick (BinaryReader br, int[] data)
		{
			var count = data.Length * sizeof(int);
			var bytes = br.ReadBytes(count);
			unsafe
			{
				fixed(byte* pSrc = bytes)
					fixed(int* pDest = data)
					{
						var ps = pSrc;
						var pd = pDest;

						for(var i = 0; i < data.Length; ++i)
						{
							var value = *(int*)ps;
							*pd = value.FlipEndian();
							ps += sizeof(int);
							++pd;
						}
					}
			}
		}
#endif
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
			sb.Append(_data.Length);
			sb.Append(" integers]\n");
		}

		/// <summary>
		/// Gets an enumerator that goes over the array of integers
		/// </summary>
		/// <returns>An enumerator</returns>
		public IEnumerator<int> GetEnumerator ()
		{
			return ((IEnumerable<int>)_data).GetEnumerator();
		}

		/// <summary>
		/// Gets an enumerator that goes over the array of integers
		/// </summary>
		/// <returns>An enumerator</returns>
		IEnumerator IEnumerable.GetEnumerator ()
		{
			return _data.GetEnumerator();
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

#if !DEBUG
			unsafe
			{
				fixed(int* pSrc = src, pDest = dest)
				{
					var ps = pSrc  + srcStart;
					var pd = pDest + destStart;

#if X64
					var stop = count / 2;
					for(var i = 0; i < stop; ++i)
					{
						*((long*)pd) = *((long*)ps);
						pd += 2;
						ps += 2;
					}
					if(1 == count % 2)
						*pd = *ps;
#else
					for(var i = 0; i < count; ++i)
					{
						*pd = *ps;
						++pd;
						++ps;
					}
#endif
				}
			}
#else
			Buffer.BlockCopy(src, srcStart, dest, destStart, count * sizeof(int));
#endif
		}
		#endregion
	}
}
