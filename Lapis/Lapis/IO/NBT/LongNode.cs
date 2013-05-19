﻿using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a long integer (64 bits)
	/// </summary>
	public class LongNode : Node, IEquatable<LongNode>, IEquatable<long>, IComparable<LongNode>, IComparable<long>
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

		/// <summary>
		/// Duplicates the contents of the node and returns it
		/// </summary>
		/// <returns>A copy of the node</returns>
		public override Node CloneNode ()
		{
			return Duplicate();
		}

		/// <summary>
		/// Duplicates the contents of the node and returns it
		/// </summary>
		/// <returns>A copy of the node</returns>
		public LongNode Duplicate ()
		{
			return new LongNode(Name, _value);
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

		/// <summary>
		/// Compares the node against another node to check if they're equal
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>True if the nodes are equal</returns>
		public override bool Equals (Node other)
		{
			if(base.Equals(other))
				return (_value == ((LongNode)other)._value);
			return false;
		}

		/// <summary>
		/// Compares the node against another node to check if they're equal
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>True if the nodes are equal</returns>
		public bool Equals (LongNode other)
		{
			if(base.Equals(other))
				return (_value == other._value);
			return false;
		}

		/// <summary>
		/// Compares the node against a value to check if they're equal
		/// </summary>
		/// <param name="other">Other value to compare against</param>
		/// <returns>True if the node's value and <paramref name="other"/> are equal</returns>
		public bool Equals (long other)
		{
			return (_value == other);
		}

		/// <summary>
		/// Compares the node against another node
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>Less than 0 if the node is less than <paramref name="other"/>,
		/// 0 if the nodes are equal,
		/// or greater than 1 if the node is greater than <paramref name="other"/></returns>
		public override int CompareTo (Node other)
		{
			var val = base.CompareTo(other);
			return (0 == val) ? _value.CompareTo(((LongNode)other)._value) : val;
		}

		/// <summary>
		/// Compares the node against another node
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>Less than 0 if the node is less than <paramref name="other"/>,
		/// 0 if the nodes are equal,
		/// or greater than 1 if the node is greater than <paramref name="other"/></returns>
		public int CompareTo (LongNode other)
		{
			var val = base.CompareTo(other);
			return (0 == val) ? _value.CompareTo(other._value) : val;
		}

		/// <summary>
		/// Compares the node against another value
		/// </summary>
		/// <param name="other">Other value to compare against</param>
		/// <returns>Less than 0 if the node's value is less than <paramref name="other"/>,
		/// 0 if the node's value and <paramref name="other"/> are equal,
		/// or greater than 1 if the node's value is greater than <paramref name="other"/></returns>
		public int CompareTo (long other)
		{
			return _value.CompareTo(other);
		}
	}
}
