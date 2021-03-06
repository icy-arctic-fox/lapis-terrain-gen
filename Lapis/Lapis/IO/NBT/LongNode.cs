﻿using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a long integer (64 bits)
	/// </summary>
	public class LongNode : Node
	{
		private long _value;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.Long</remarks>
		public override NodeType Type
		{
			get { return NodeType.Long; }
		}

		/// <summary>
		/// Value contained in the node
		/// </summary>
		public long Value
		{
			get { return _value; }
			set { _value = value; }
		}

		/// <summary>
		/// Value of the node represented as a string
		/// </summary>
		public override string StringValue
		{
			get { return _value.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}

		/// <summary>
		/// Creates a new long integer node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="value">Value to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public LongNode (string name, long value)
			: base(name)
		{
			_value = value;
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(_value);
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>A long integer node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static LongNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var value = br.ReadInt64();
			return new LongNode(name, value);
		}
		#endregion
	}
}
