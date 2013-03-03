using System;
using Lapis.Utility;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains a UTF-8 string
	/// </summary>
	public class StringNode : Node
	{
		private string value;

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
			get { return value; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The new string to put in the node can't be null.");

				this.value = value;
			}
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
			if(value.Length > short.MaxValue)
				throw new ArgumentOutOfRangeException("value.Length", "The string to put in the node can't be longer than " + short.MaxValue + " characters.");

			this.value = value;
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			short length = (short)value.Length;
			byte[] temp  = value.GetBytes(length);

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
			short length = br.ReadInt16();
			string value = br.ReadBytes(length).ToUTF8String();
			return new StringNode(name, value);
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
