﻿using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a single byte
	/// </summary>
	public class ByteNode : Node
	{
		private byte _value;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.Byte</remarks>
		public override NodeType Type
		{
			get { return NodeType.Byte; }
		}

		/// <summary>
		/// Value contained in the node
		/// </summary>
		public byte Value
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
		/// Boolean value of the node
		/// </summary>
		/// <remarks>Non-zero is true.</remarks>
		public bool BooleanValue
		{
			get { return _value != 0; }
			set { _value = (byte)(value ? 1 : 0); }
		}

		/// <summary>
		/// Creates a new byte node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="value">Value to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public ByteNode (string name, byte value)
			: base(name)
		{
			_value = value;
		}

		/// <summary>
		/// Creates a new byte node from a boolean value
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="value">Value to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public ByteNode (string name, bool value)
			: base(name)
		{
			_value = (byte)(value ? 1 : 0);
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
		/// <returns>A byte node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static ByteNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var value = br.ReadByte();
			return new ByteNode(name, value);
		}
		#endregion
	}
}
