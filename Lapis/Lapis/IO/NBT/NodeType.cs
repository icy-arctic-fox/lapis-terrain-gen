namespace Lapis.IO.NBT
{
	/// <summary>
	/// Types of NBT nodes and their values
	/// </summary>
	public enum NodeType : byte
	{
		/// <summary>
		/// Marks the end of NBT structure
		/// </summary>
		/// <remarks>This is not an actual node, but a representation of one.</remarks>
		End = 0,

		/// <summary>
		/// A node that contains a single byte
		/// </summary>
		Byte = 1,

		/// <summary>
		/// A node that contains a short integer (16 bits)
		/// </summary>
		Short = 2,

		/// <summary>
		/// A node that contains an integer (32 bits)
		/// </summary>
		Int = 3,

		/// <summary>
		/// A node that contains a long integer (64 bits)
		/// </summary>
		Long = 4,

		/// <summary>
		/// A node that contains a single-precision floating point value (32 bits)
		/// </summary>
		Float = 5,

		/// <summary>
		/// A node that contains a double-precision floating point value (64 bits)
		/// </summary>
		Double = 6,

		/// <summary>
		/// A node that contains an array of bytes
		/// </summary>
		ByteArray = 7,

		/// <summary>
		/// A node that contains a UTF-8 string
		/// </summary>
		String = 8,

		/// <summary>
		/// A node that contains a collection of other nodes (each child node is the same type)
		/// </summary>
		List = 9,

		/// <summary>
		/// A node that contains a collection of other nodes (each child node can be a different type)
		/// </summary>
		Compound = 10,

		/// <summary>
		/// A node that contains an array of signed 32-bit integers
		/// </summary>
		IntArray = 11
	}
}
