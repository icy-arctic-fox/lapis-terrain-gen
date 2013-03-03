using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// Container for NBT nodes
	/// </summary>
	public class Tree
	{
		private readonly CompoundNode container;
		private readonly Node root;

		/// <summary>
		/// Root node of the NBT structure
		/// </summary>
		public Node Root
		{
			get { return root; }
		}

		/// <summary>
		/// Creates a new NBT structure
		/// </summary>
		/// <param name="root">Root node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="root"/> is null</exception>
		public Tree (Node root)
		{
			if(null == root)
				throw new ArgumentNullException("root", "The root node can't be null.");

			container = new CompoundNode(string.Empty);
			this.root = root;
			container.Add(this.root);
		}

		/// <summary>
		/// Writes the NBT structure to a stream using a binary writer
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (System.IO.BinaryWriter bw)
		{
			if(null == bw)
				throw new ArgumentNullException("The stream writer can't be null.");

			container.WriteToStream(bw);
		}

		/// <summary>
		/// Reads an NBT structure from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>An NBT structure</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the structure of the data is unexpected</exception>
		public static Tree ReadFromStream (System.IO.BinaryReader br)
		{
			if(null == br)
				throw new ArgumentNullException("The stream reader can't be null.");

			Node node = Node.ReadFromStream(br);
			if(node.Type != NodeType.Compound)
				throw new FormatException("The container for the NBT structure is not a compound node.");

			CompoundNode container = (CompoundNode)node;
			if(container.Count != 1)
				throw new FormatException("The container for the NBT structure should have only one item in it.");

			Node root = null;
			foreach(Node n in container)
			{// Only get the first item
				root = n;
				break;
			}

			return new Tree(root);
		}

		/// <summary>
		/// Generates a string representation of the NBT structure
		/// </summary>
		/// <returns>A string</returns>
		public override string ToString ()
		{
			return root.ToString();
		}
	}
}
