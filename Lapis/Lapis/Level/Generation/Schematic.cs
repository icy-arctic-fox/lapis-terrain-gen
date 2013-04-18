using System;
using System.IO;
using Ionic.Zlib;
using Lapis.Blocks;
using Lapis.IO;
using Lapis.IO.NBT;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// A schematic for an object made of blocks that can be placed in a world
	/// </summary>
	public class Schematic : ISerializable
	{
		private readonly BlockType[] _blocks;
		private readonly byte[] _data;

		private readonly int _length, _height, _width;

		// TODO: Implement tile entities

		/// <summary>
		/// Creates a new schematic
		/// </summary>
		/// <param name="length">Length (x distance) of the region</param>
		/// <param name="height">Height (y distance) of the region</param>
		/// <param name="width">Width (z distance) of the region</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="length"/>, <paramref name="height"/>, or <paramref name="width"/> is less than 1</exception>
		public Schematic (int length, int height, int width)
		{
			if(0 >= length)
				throw new ArgumentOutOfRangeException("length", "The length (x distance) can't be less than 1.");
			if(0 >= height)
				throw new ArgumentOutOfRangeException("height", "The height (y distance) can't be less than 1.");
			if(0 >= width)
				throw new ArgumentOutOfRangeException("width", "The width (z distance) can't be less than 1.");

			_length = length;
			_height = height;
			_width  = width;

			var arrSize = length * height * width;
			_blocks = new BlockType[arrSize];
			_data   = new byte[arrSize];
		}

		#region Properties
		/// <summary>
		/// Length (x distance) of the region
		/// </summary>
		public int Length
		{
			get { return _length; }
		}

		/// <summary>
		/// Height (y distance) of the region
		/// </summary>
		public int Height
		{
			get { return _height; }
		}

		/// <summary>
		/// Width (z distance) of the region
		/// </summary>
		public int Width
		{
			get { return _width; }
		}
		#endregion

		#region Save and load
		private const string DefaultNodeName = "Schematic";

		/// <summary>
		/// Loads a schematic from a file
		/// </summary>
		/// <param name="filename">Name of the schematic file</param>
		/// <returns>A schematic</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null</exception>
		public static Schematic FromFile (string filename)
		{
			if(null == filename)
				throw new ArgumentNullException("filename", "The name of the file to load from can't be null.");

			using(var fs = new FileStream(filename, FileMode.Open))
				return FromStream(fs);
		}

		/// <summary>
		/// Loads a schematic from a stream
		/// </summary>
		/// <param name="s">Stream to read from</param>
		/// <returns>A schematic</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null</exception>
		public static Schematic FromStream (Stream s)
		{
			if(null == s)
				throw new ArgumentNullException("s", "The source stream can't be null.");

			using(var gzStream = new GZipStream(s, CompressionMode.Decompress))
			using(var br = new EndianBinaryReader(gzStream, Endian.Big))
				return new Schematic(Node.ReadFromStream(br));
		}

		/// <summary>
		/// Saves the schematic to a file
		/// </summary>
		/// <param name="filename">Name of the file to save to</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="filename"/> is null</exception>
		public void Save (string filename)
		{
			if(null == filename)
				throw new ArgumentNullException("filename", "The name of the file to save to can't be null.");

			using(var fs = new FileStream(filename, FileMode.Create))
				WriteToStream(fs);
		}

		/// <summary>
		/// Writes the schematic to a stream
		/// </summary>
		/// <param name="s">Stream to write to</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="s"/> is null</exception>
		public void WriteToStream (Stream s)
		{
			using(var gzStream = new GZipStream(s, CompressionMode.Compress))
			using(var bw = new EndianBinaryWriter(gzStream, Endian.Big))
				WriteToStream(bw);
		}

		/// <summary>
		/// Writes the raw schematic data to a stream
		/// </summary>
		/// <param name="bw">Binary writer to use</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (BinaryWriter bw)
		{
			if(null == bw)
				throw new ArgumentNullException("bw", "The stream writer can't be null.");

			var node = ConstructNbtNode(DefaultNodeName);
			node.WriteToStream(bw);
		}

		#region Serialization
		/// <summary>
		/// Creates a new schematic from an NBT node
		/// </summary>
		/// <param name="node">Node to pull schematic data from</param>
		/// <exception cref="node">Thrown if <paramref name="node"/> is null</exception>
		protected Schematic (Node node)
		{
			throw new NotImplementedException();
		}

		#region Validation
		#endregion

		#region Nbt Construction
		/// <summary>
		/// Constructs an NBT node that contains the schematic data
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>A node containing schematic data</returns>
		public Node ConstructNbtNode (string name)
		{
			var node = new CompoundNode(name);
			ConstructNode(node);
			return node;
		}

		/// <summary>
		/// Adds the fields (sub-nodes) to the top-level NBT node
		/// </summary>
		/// <param name="node">Node to add to</param>
		/// <remarks>Sub-classes of this schematic class should override this method if they add extra values.
		/// base.ConstructNode(node) should also be called so that this class can add the default nodes.</remarks>
		protected virtual void ConstructNode (CompoundNode node)
		{
			throw new NotImplementedException();
		}
		#endregion
		#endregion
		#endregion
	}
}
