using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.IO
{
	/// <summary>
	/// An object that can be serialized and de-serialized so that it can be sent over a stream
	/// </summary>
	/// <remarks>Classes that implement this interface should also have a static method named ReadFromStream().</remarks>
	public interface ISerializable
	{
		/// <summary>
		/// Writes the contents of the object to a stream
		/// </summary>
		/// <param name="bw">Stream writer to use</param>
		void WriteToStream (System.IO.BinaryWriter bw);

		/// <summary>
		/// Creates an NBT node that contains the information for the object
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node</returns>
		NBT.Node ConstructNbtNode (string name);
	}
}
