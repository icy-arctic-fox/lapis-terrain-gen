using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a single byte
	/// </summary>
	public class ByteNode : Node, IEquatable<ByteNode>, IEquatable<byte>, IComparable<ByteNode>, IComparable<byte>
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
		public ByteNode Duplicate ()
		{
			return new ByteNode(Name, _value);
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

		/// <summary>
		/// Compares the node against another node to check if they're equal
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>True if the nodes are equal</returns>
		public override bool Equals (Node other)
		{
			if(base.Equals(other))
				return (_value == ((ByteNode)other)._value);
			return false;
		}

		/// <summary>
		/// Compares the node against another node to check if they're equal
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>True if the nodes are equal</returns>
		public bool Equals (ByteNode other)
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
		public bool Equals (byte other)
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
			return (0 == val) ? _value.CompareTo(((ByteNode)other)._value) : val;
		}

		/// <summary>
		/// Compares the node against another node
		/// </summary>
		/// <param name="other">Other node to compare against</param>
		/// <returns>Less than 0 if the node is less than <paramref name="other"/>,
		/// 0 if the nodes are equal,
		/// or greater than 1 if the node is greater than <paramref name="other"/></returns>
		public int CompareTo (ByteNode other)
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
		public int CompareTo (byte other)
		{
			return _value.CompareTo(other);
		}
	}
}
