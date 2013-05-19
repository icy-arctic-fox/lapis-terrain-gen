using System;
using Lapis.Utility;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a UTF-8 string
	/// </summary>
	public class StringNode : Node, IEquatable<StringNode>, IEquatable<string>, IComparable<StringNode>, IComparable<string>
	{
		private string _value;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.String</remarks>
		public override NodeType Type
		{
			get { return NodeType.String; }
		}

		/// <summary>
		/// Value contained in the node
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if attempting to set the new value to null</exception>
		public string Value
		{
			get { return _value; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The new string to put in the node can't be null.");

				_value = value;
			}
		}

		/// <summary>
		/// Value of the node represented as a string
		/// </summary>
		/// <remarks>This property is pointless, but required.</remarks>
		public override string StringValue
		{
			get { return _value; }
		}

		/// <summary>
		/// Creates a new string node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="value">Value to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is longer than allowed</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public StringNode (string name, string value)
			: base(name)
		{
			if(null == value)
				throw new ArgumentNullException("value", "The string to put in the node can't be null.");
			if(value.Length > Int16.MaxValue)
				throw new ArgumentOutOfRangeException("value", "The string to put in the node can't be longer than " + Int16.MaxValue + " characters.");

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
		public StringNode Duplicate ()
		{
			return new StringNode(Name, _value);
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			var length = (short)_value.Length;
			var temp   = _value.GetBytes(length);

			bw.Write(length);
			bw.Write(temp);
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>A string node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static StringNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var length = br.ReadInt16();
			var value  = br.ReadBytes(length).ToUtf8String();
			return new StringNode(name, value);
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
				return (_value == ((StringNode)other)._value);
			return false;
		}

		/// <summary>
		/// Compares the node against another node to check if they're equal
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>True if the nodes are equal</returns>
		public bool Equals (StringNode other)
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
		public bool Equals (string other)
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
			return (0 == val) ? String.Compare(_value, ((StringNode)other)._value, StringComparison.Ordinal) : val;
		}

		/// <summary>
		/// Compares the node against another node
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>Less than 0 if the node is less than <paramref name="other"/>,
		/// 0 if the nodes are equal,
		/// or greater than 1 if the node is greater than <paramref name="other"/></returns>
		public int CompareTo (StringNode other)
		{
			var val = base.CompareTo(other);
			return (0 == val) ? String.Compare(_value, other._value, StringComparison.Ordinal) : val;
		}

		/// <summary>
		/// Compares the node against another value
		/// </summary>
		/// <param name="other">Other value to compare against</param>
		/// <returns>Less than 0 if the node's value is less than <paramref name="other"/>,
		/// 0 if the node's value and <paramref name="other"/> are equal,
		/// or greater than 1 if the node's value is greater than <paramref name="other"/></returns>
		public int CompareTo (string other)
		{
			return String.Compare(_value, other, StringComparison.Ordinal);
		}
	}
}
