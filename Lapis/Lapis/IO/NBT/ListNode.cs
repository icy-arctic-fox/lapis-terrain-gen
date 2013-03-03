using System;
using System.Collections.Generic;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a collection of other nodes (each child node is the same type)
	/// </summary>
	public class ListNode : Node, ICollection<Node>
	{
		private readonly List<Node> nodes;
		private readonly NodeType type;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.List</remarks>
		public override NodeType Type
		{
			get { return NodeType.List; }
		}

		/// <summary>
		/// The node type of each element in the list
		/// </summary>
		public NodeType ElementType
		{
			get { return type; }
		}

		/// <summary>
		/// Node contained at an index
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if attempting to set the new node to null</exception>
		/// <exception cref="InvalidCastException">Thrown if the new node's type does not match the type of the existing nodes</exception>
		public Node this[int index]
		{
			get { return nodes[index]; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The new node to store in the list can't be null.");
				if(value.Type != type)
					throw new InvalidCastException("The new node does not match the type of the existing nodes in the list. Expected: " + type + ", got: " + value.Type);

				nodes[index] = value;
			}
		}

		/// <summary>
		/// Creates a new list node that is empty
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="type">Type of nodes in the list</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public ListNode (string name, NodeType type)
			: base(name)
		{
			this.nodes = new List<Node>();
			this.type = type;
		}

		/// <summary>
		/// Creates a new list node with contents
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="type">Type of nodes in the list</param>
		/// <param name="nodes">Nodes to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nodes"/> is null</exception>
		/// <exception cref="InvalidCastException">Thrown if one of the nodes in <paramref name="nodes"/> does not match the type provided by <paramref name="type"/></exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public ListNode (string name, NodeType type, IEnumerable<Node> nodes)
			: base(name)
		{
			if(null == nodes)
				throw new ArgumentNullException("nodes", "The list of nodes can't be null.");

			this.nodes = new List<Node>();
			this.type = type;

			lock(nodes)
			{
				foreach(Node node in nodes)
				{
					if(node.Type != type)
						throw new InvalidCastException("One of the nodes provided doesn't match the expected node type. Expected: " + type + ", got: " + node.Type);
					this.nodes.Add(node);
				}
			}
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			bw.Write((byte)type);
			bw.Write(nodes.Count);
			foreach(Node node in nodes)
				node.WritePayload(bw);
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>A list node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static ListNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			NodeType type = (NodeType)br.ReadByte();
			int count     = br.ReadInt32();

			ListNode list = new ListNode(name, type);
			NodePayloadReader reader = DecodeNodeType(type);
			for(int i = 0; i < count; ++i)
			{
				Node node = reader(br, name);
				list.Add(node);
			}
			return list;
		}
		#endregion

		#region Collection
		/// <summary>
		/// Adds a node to the end of the list
		/// </summary>
		/// <param name="item">Node to add</param>
		/// <exception cref="ArgumentNullException">Thrown if attempting to set the new value to null</exception>
		/// <exception cref="InvalidCastException">Thrown if the new node's type does not match the type of the existing nodes</exception>
		public void Add (Node item)
		{
			if(null == item)
				throw new ArgumentNullException("item", "The new node being added can't be null.");
			if(item.Type != type)
				throw new InvalidCastException("The new node's type doesn't match the other nodes' type. Expected: " + type + ", got: " + item.Type);

			nodes.Add(item);
		}

		/// <summary>
		/// Removes all nodes from the list
		/// </summary>
		public void Clear ()
		{
			nodes.Clear();
		}

		/// <summary>
		/// Checks if the list contains a certain node
		/// </summary>
		/// <param name="item">Node to look for</param>
		/// <returns>True if the list contains the node or false if it doesn't</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null</exception>
		/// <remarks>This method checks for references, not actual node values.</remarks>
		public bool Contains (Node item)
		{
			if(null == item)
				throw new ArgumentNullException("item", "The item to search for can't be null.");

			return nodes.Contains(item);
		}

		/// <summary>
		/// Copies the nodes in the list to an array
		/// </summary>
		/// <param name="array">Array to copy the nodes to</param>
		/// <param name="arrayIndex">Index to start placing nodes at in the array</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
		public void CopyTo (Node[] array, int arrayIndex)
		{
			if(null == array)
				throw new ArgumentNullException("array", "The array to store the nodes in can't be null.");

			nodes.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Number of items in the list
		/// </summary>
		public int Count
		{
			get { return nodes.Count; }
		}

		/// <summary>
		/// Whether or not this collection is read-only
		/// </summary>
		/// <remarks>This is always false.</remarks>
		public bool IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes a node from the list
		/// </summary>
		/// <param name="item">Node to remove</param>
		/// <returns>True if the node was found and removed or false if it wasn't found</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null</exception>
		/// <remarks>This method checks for references, not actual node values.</remarks>
		public bool Remove (Node item)
		{
			if(null == item)
				throw new ArgumentNullException("item", "The node to remove can't be null.");

			return nodes.Remove(item);
		}

		/// <summary>
		/// Gets an enumerator for the list of nodes
		/// </summary>
		/// <returns>An enumerator</returns>
		public IEnumerator<Node> GetEnumerator ()
		{
			return nodes.GetEnumerator();
		}

		/// <summary>
		/// Gets an enumerator for the list of nodes
		/// </summary>
		/// <returns>An enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return nodes.GetEnumerator();
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
			sb.Append(nodes.Count);
			sb.Append(" entries of type ");
			sb.Append(ElementType);
			sb.Append('\n');
			sb.Append(StringIndent, depth);
			sb.Append("{\n");

			++depth;
			foreach(Node node in nodes)
				node.ToString(sb, depth);

			sb.Append(StringIndent, --depth);
			sb.Append("}\n");
		}
	}
}
