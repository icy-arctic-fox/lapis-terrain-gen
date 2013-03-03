﻿using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a double-precision floating point value (64 bits)
	/// </summary>
	public class DoubleNode : Node
	{
		private double _value;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.Double</remarks>
		public override NodeType Type
		{
			get { return NodeType.Double; }
		}

		/// <summary>
		/// Value contained in the node
		/// </summary>
		public double Value
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
		/// Creates a new double-precision floating point node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="value">Value to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public DoubleNode (string name, double value)
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
		/// <returns>A double-precision floating point node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static DoubleNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var value = br.ReadDouble();
			return new DoubleNode(name, value);
		}
		#endregion
	}
}
