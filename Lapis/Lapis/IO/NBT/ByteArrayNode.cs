using System;
using Lapis.Utility;

namespace Lapis.IO.NBT
{
	/// <summary>
	/// A node that contains an array of bytes
	/// </summary>
	public class ByteArrayNode : Node
	{
		private byte[] _data;

		/// <summary>
		/// The type of node
		/// </summary>
		/// <remarks>This is always NodeType.ByteArray</remarks>
		public override NodeType Type
		{
			get { return NodeType.ByteArray; }
		}

		/// <summary>
		/// Value of the node represented as a string
		/// </summary>
		/// <remarks>This property produces the string: # bytes</remarks>
		public override string StringValue
		{
			get { return _data.Length + " bytes"; }
		}

		/// <summary>
		/// Raw bytes contained in the node
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if an attempt is made to provide a null reference when updating the field</exception>
		/// <remarks>Updating the contents of this field will copy the bytes.</remarks>
		public byte[] Data
		{
			get { return _data; }
			set
			{
				if(null == value)
					throw new NullReferenceException("The reference to the new set of bytes can't be null.");

				lock(value)
				{
					_data = new byte[value.Length];
					value.Copy(_data);
				}
			}
		}

		/// <summary>
		/// Creates a new byte array node
		/// </summary>
		/// <param name="name">Name of the node</param>
		/// <param name="data">Data to put in the node</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		/// <remarks>The bytes passed in will be copied.</remarks>
		public ByteArrayNode (string name, byte[] data)
			: base(name)
		{
			if(null == data)
				throw new ArgumentNullException("data", "The reference to the byte array can't be null.");

			lock(data)
			{
				_data = new byte[data.Length];
				data.Copy(_data);
			}
		}

		#region Serialization
		/// <summary>
		/// Writes just the payload portion of the node to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		protected internal override void WritePayload (System.IO.BinaryWriter bw)
		{
			var length = _data.Length;
			bw.Write(length);
			bw.Write(_data);
		}

		/// <summary>
		/// Reads the payload of a node from a stream using a binary reader
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <param name="name">Name to give the node</param>
		/// <returns>A byte array node read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="name"/> is longer than allowed</exception>
		internal static ByteArrayNode ReadPayload (System.IO.BinaryReader br, string name)
		{
			var length = br.ReadInt32();
			var data   = br.ReadBytes(length);
			return new ByteArrayNode(name, data);
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
			sb.Append("\"): [");
			sb.Append(_data.Length);
			sb.Append(" bytes]\n");
		}
	}
}
