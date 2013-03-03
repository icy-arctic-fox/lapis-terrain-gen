using System;
using System.IO;
using System.Text;
using Lapis.Utility;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// Base class for all NBT node types.
	/// This class also provides functions for reading and writing nodes.
	/// </summary>
	public abstract class Node
	{
		/// <summary>
		/// Character to use for indenting in ToString()
		/// </summary>
		protected internal const char StringIndent = ' ';

		private readonly string name;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This property can be used to check a node before casting.</remarks>
		public abstract NodeType Type { get; }

		/// <summary>
		/// Name of the node
		/// </summary>
		/// <remarks>Node names cannot be null and cannot be changed after being set.</remarks>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		/// Creates a new node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		protected internal Node (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the node can't be null.");
			if(name.Length > short.MaxValue)
				throw new ArgumentOutOfRangeException("name.Length", "The name of the node can't be longer than " + short.MaxValue + " characters.");

			this.name = name;
		}

		#region Serialization
		/// <summary>
		/// Represents a function that reads the payload of a node from a stream and constructs a node
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>The constructed node</returns>
		internal delegate Node NodePayloadReader (BinaryReader br, string name);

		/// <summary>
		/// Writes the node to a stream using a binary writer
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		internal void WriteToStream (BinaryWriter bw)
		{
			if(null == bw)
				throw new ArgumentNullException("bw", "The stream writer can't be null.");

			writeHeader(bw);
			WritePayload(bw);
		}

		/// <summary>
		/// Writes the header portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		private void writeHeader (BinaryWriter bw)
		{
			byte type    = (byte)Type;
			short length = (short)name.Length;
			byte[] temp  = name.GetBytes(length);

			bw.Write(type);
			bw.Write(length);
			bw.Write(temp);
		}

		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal abstract void WritePayload (BinaryWriter bw);

		/// <summary>
		/// Reads a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>A node read from a stream or null if it is an "End" node</returns>
		internal static Node ReadFromStream (BinaryReader br)
		{
			if(null == br)
				throw new ArgumentNullException("The stream reader can't be null.");

			// Read header
			NodeType type = (NodeType)br.ReadByte();
			if(NodeType.End == type)
				return null;
			short length  = br.ReadInt16();
			string name   = br.ReadBytes(length).ToUTF8String();

			// Read payload
			NodePayloadReader reader = DecodeNodeType(type);
			Node node = reader(br, name);

			return node;
		}

		/// <summary>
		/// Retrieves the respective payload reader function for a node type
		/// </summary>
		/// <param name="type">Node type</param>
		/// <returns>A payload reader</returns>
		internal static NodePayloadReader DecodeNodeType (NodeType type)
		{
			switch(type)
			{
			case NodeType.Byte:
				return ByteNode.ReadPayload;
			case NodeType.Short:
				return ShortNode.ReadPayload;
			case NodeType.Int:
				return IntNode.ReadPayload;
			case NodeType.Long:
				return LongNode.ReadPayload;
			case NodeType.Float:
				return FloatNode.ReadPayload;
			case NodeType.Double:
				return DoubleNode.ReadPayload;
			case NodeType.ByteArray:
				return ByteArrayNode.ReadPayload;
			case NodeType.String:
				return StringNode.ReadPayload;
			case NodeType.List:
				return ListNode.ReadPayload;
			case NodeType.Compound:
				return CompoundNode.ReadPayload;
			case NodeType.IntArray:
				return IntArrayNode.ReadPayload;
			default:
				throw new InvalidDataException("The NBT node type '" + type + "' is invalid.");
			}
		}
		#endregion

		/// <summary>
		/// Gets a string representation of the node structure
		/// </summary>
		/// <returns>A string</returns>
		public override string ToString ()
		{
			StringBuilder sb = new StringBuilder();
			ToString(sb, 0);
			string result = sb.ToString();
			return result;
		}

		/// <summary>
		/// Recursive method that appends the node's string form to a builder
		/// </summary>
		/// <param name="sb">Builder that contains the node strings</param>
		/// <param name="depth">Depth into the node structure (number of times to indent)</param>
		/// <remarks>This is far more optimal than calling the default ToString() multiple times.</remarks>
		protected internal abstract void ToString (StringBuilder sb, int depth);
	}
}
