using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a single byte
	/// </summary>
	public class ByteNode : Node
	{
		private byte value;

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
			get { return value; }
			set { this.value = value; }
		}

		/// <summary>
		/// Boolean value of the node
		/// </summary>
		/// <remarks>Non-zero is true.</remarks>
		public bool BooleanValue
		{
			get { return value != 0; }
			set { this.value = (byte)(value ? 1 : 0); }
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
			this.value = value;
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
			this.value = (byte)(value ? 1 : 0);
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write(value);
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
			byte value = br.ReadByte();
			return new ByteNode(name, value);
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
			sb.Append("\"): ");
			sb.Append(value);
			sb.Append('\n');
		}
	}
}
