using System;
using System.Collections.Generic;
using System.Linq;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a collection of other nodes (each child node can be a different type)
	/// </summary>
	public class CompoundNode : Node, ICollection<Node>
	{
		private readonly Dictionary<string, Node> _nodes;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.Compound</remarks>
		public override NodeType Type
		{
			get { return NodeType.Compound; }
		}

		/// <summary>
		/// Value of the node represented as a string
		/// </summary>
		/// <remarks>This property produces the string: # elements</remarks>
		public override string StringValue
		{
			get { return _nodes.Count + " elements"; }
		}

		/// <summary>
		/// Node represented by a name
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if attempting to set the new node to null</exception>
		public Node this[string name]
		{
			get { return _nodes[name]; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The new node to store in the list can't be null.");

				_nodes[name] = value;
			}
		}

		/// <summary>
		/// Creates a new compound node that is empty
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		public CompoundNode (string name)
			: base(name)
		{
			_nodes = new Dictionary<string, Node>();
		}

		/// <summary>
		/// Creates a new compound node with contents
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="nodes">Nodes to put in the compound</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="nodes"/> is null</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		/// <exception cref="ArgumentException">Thrown if one of the nodes in <paramref name="nodes"/> is null or a has a duplicate name</exception>
		public CompoundNode (string name, IEnumerable<Node> nodes)
			: base(name)
		{
			if(null == nodes)
				throw new ArgumentNullException("nodes", "The list of nodes can't be null.");

			_nodes = new Dictionary<string, Node>();

			var enumerable = nodes as IList<Node> ?? nodes.ToList();
			lock(enumerable)
			{
				foreach(var node in enumerable)
				{
					if(null == node)
						throw new ArgumentException("One or more nodes in the collection are null.");
					if(_nodes.ContainsKey(node.Name))
						throw new ArgumentException("The node named " + node.Name + " already exists.");
					_nodes.Add(node.Name, node);
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
			foreach(var node in _nodes.Values)
				node.WriteToStream(bw);
			bw.Write((byte)NodeType.End);
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>A compound node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static CompoundNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var compound = new CompoundNode(name);
			Node node;
			while(null != (node = ReadFromStream(br)))
				compound.Add(node);
			return compound;
		}
		#endregion

		#region Collection
		/// <summary>
		/// Adds a node to the compound
		/// </summary>
		/// <param name="item">Node to add</param>
		/// <exception cref="ArgumentNullException">Thrown if attempting to set the new value to null</exception>
		public void Add (Node item)
		{
			if(null == item)
				throw new ArgumentNullException("item", "The new node being added can't be null.");

			_nodes.Add(item.Name, item);
		}

		/// <summary>
		/// Removes all nodes from the compound
		/// </summary>
		public void Clear ()
		{
			_nodes.Clear();
		}

		/// <summary>
		/// Checks if the compound contains a certain node
		/// </summary>
		/// <param name="item">Node to look for</param>
		/// <returns>True if the compound contains the node or false if it doesn't</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null</exception>
		/// <remarks>This method checks for references, not actual node values.</remarks>
		public bool Contains (Node item)
		{
			if(null == item)
				throw new ArgumentNullException("item", "The item to search for can't be null.");

			return _nodes.ContainsValue(item);
		}

		/// <summary>
		/// Checks if the compound contains a node by a certain name
		/// </summary>
		/// <param name="name">Name of the node to look for</param>
		/// <returns>True if the compound contains the node or false if it doesn't</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public bool Contains (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the item to search for can't be null.");

			return _nodes.ContainsKey(name);
		}

		/// <summary>
		/// Copies the nodes in the compound to an array
		/// </summary>
		/// <param name="array">Array to copy the nodes to</param>
		/// <param name="arrayIndex">Index to start placing nodes at in the array</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="array"/> is null</exception>
		public void CopyTo (Node[] array, int arrayIndex)
		{
			if(null == array)
				throw new ArgumentNullException("array", "The array to store the nodes in can't be null.");

			_nodes.Values.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Number of items in the compound
		/// </summary>
		public int Count
		{
			get { return _nodes.Count; }
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
		/// Removes a node from the compound
		/// </summary>
		/// <param name="item">Node to remove</param>
		/// <returns>True if the node was found and removed or false if it wasn't found</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="item"/> is null</exception>
		/// <remarks>This method checks for node names, not by reference or value.</remarks>
		public bool Remove (Node item)
		{
			if(null == item)
				throw new ArgumentNullException("item", "The node to remove can't be null.");

			return _nodes.Remove(item.Name);
		}

		/// <summary>
		/// Removes a node from the compound
		/// </summary>
		/// <param name="name">Name of the node to remove</param>
		/// <returns>True if the node was found and removed or false if it wasn't found</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public bool Remove (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the node to remove can't be null.");

			return _nodes.Remove(name);
		}

		/// <summary>
		/// Gets an enumerator for the collection of nodes
		/// </summary>
		/// <returns>An enumerator</returns>
		public IEnumerator<Node> GetEnumerator ()
		{
			return _nodes.Values.GetEnumerator();
		}

		/// <summary>
		/// Gets an enumerator for the collection of nodes
		/// </summary>
		/// <returns>An enumerator</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return _nodes.Values.GetEnumerator();
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
			sb.Append(_nodes.Count);
			sb.Append(" entries\n");
			sb.Append(StringIndent, depth);
			sb.Append("{\n");

			++depth;
			foreach(var node in _nodes.Values)
				node.ToString(sb, depth);

			sb.Append(StringIndent, --depth);
			sb.Append("}\n");
		}
	}
}
