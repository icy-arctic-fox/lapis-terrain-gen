using System;
using System.Linq;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// Container for NBT nodes
	/// </summary>
	public class Tree : ICloneable
	{
		private readonly CompoundNode _container;
		private readonly Node _root;

		/// <summary>
		/// Root node of the NBT structure
		/// </summary>
		public Node Root
		{
			get { return _root; }
		}

		/// <summary>
		/// Creates a new NBT tree structure
		/// </summary>
		/// <param name="root">Root node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="root"/> is null</exception>
		public Tree (Node root)
		{
			if(null == root)
				throw new ArgumentNullException("root", "The root node can't be null.");

			_container = new CompoundNode(String.Empty);
			_root = root;
			_container.Add(_root);
		}

		/// <summary>
		/// Creates a duplicate of the tree
		/// </summary>
		/// <returns>A copy of the tree</returns>
		/// <remarks>A deep copy is performed on the tree.</remarks>
		public object Clone ()
		{
			return CloneTree();
		}

		/// <summary>
		/// Creates a duplicate of the tree
		/// </summary>
		/// <returns>A copy of the tree</returns>
		/// <remarks>A deep copy is performed on the tree.</remarks>
		public Tree CloneTree ()
		{
			return new Tree(_root.CloneNode());
		}

		#region Serialization
		/// <summary>
		/// Writes the NBT tree structure to a stream using a binary writer
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (System.IO.BinaryWriter bw)
		{
			if(null == bw)
				throw new ArgumentNullException("bw", "The stream writer can't be null.");

			_container.WriteToStream(bw);
		}

		/// <summary>
		/// Reads an NBT tree structure from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>An NBT tree structure</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the structure of the data is unexpected</exception>
		public static Tree ReadFromStream (System.IO.BinaryReader br)
		{
			if(null == br)
				throw new ArgumentNullException("br", "The stream reader can't be null.");

			var node = Node.ReadFromStream(br);
			if(node.Type != NodeType.Compound)
				throw new FormatException("The container for the NBT structure is not a compound node.");

			var container = (CompoundNode)node;
			if(container.Count != 1)
				throw new FormatException("The container for the NBT structure should have only one item in it.");

			var root = container.FirstOrDefault();
			return new Tree(root);
		}
		#endregion

		/// <summary>
		/// Generates a string representation of the NBT tree structure
		/// </summary>
		/// <returns>A string</returns>
		public override string ToString ()
		{
			return _root.ToString();
		}
	}
}
