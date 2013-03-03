using System;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a double-precision floating point value (64 bits)
	/// </summary>
	public class DoubleNode : Node
	{
		private double value;

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
			get { return value; }
			set { this.value = value; }
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
			this.value = value;
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
		/// <returns>A double-precision floating point node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static DoubleNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			double value = br.ReadDouble();
			return new DoubleNode(name, value);
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
