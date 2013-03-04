using System;
using Lapis.IO;
using Lapis.IO.NBT;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Contains information about the highest blocks in a chunk
	/// </summary>
	/// <remarks>This class is not tied to any active world data.
	/// The purpose of this class is for creating chunks, load chunk data from disk, and save chunk data to disk.
	/// Locking for thread safety is not performed in this class. It is assumed that a higher level encases this class safely (for speed reasons).</remarks>
	public sealed class HeightData : ISerializable
	{
		private const string DefaultNodeName = "HeightMap";

		private readonly int[] _data;

		/// <summary>
		/// The height of the highest block an index (0 - 255)
		/// </summary>
		/// <param name="index">Index of the cell</param>
		/// <returns>The height of the block at the given cell index</returns>
		public int this[byte index]
		{
			get { return _data[index]; }
			set
			{
				_data[index] = value;
				if(value > Maximum)
					Maximum = value;
			}
		}

		/// <summary>
		/// The height of the highest block at a cell coordinate (BlockX, BlockY)
		/// </summary>
		/// <param name="cx">X-value of the coordinate</param>
		/// <param name="cz">Z-value of the coordinate</param>
		/// <returns>The height of the highest block at the given cell coordinate</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="cx"/> or <paramref name="cz"/> are outside the bounds of the chunk</exception>
		public int this[byte cx, byte cz]
		{
			get
			{
				var index = CalculateIndex(cx, cz);
				return _data[index];
			}

			set
			{
				var index = CalculateIndex(cx, cz);
				_data[index] = value;
				if(value > Maximum)
					Maximum = value;
			}
		}

		/// <summary>
		/// Maximum height in the entire chunk
		/// </summary>
		public int Maximum { get; private set; }

		/// <summary>
		/// Creates new height map for a chunk
		/// </summary>
		/// <remarks>The data will be set to 0</remarks>
		public HeightData ()
		{
			_data = new int[Chunk.Size * Chunk.Size];
			Maximum  = 0;
		}

		/// <summary>
		/// Creates new biome data for a chunk
		/// </summary>
		/// <param name="initialHeight">Height to set for the entire chunk</param>
		public HeightData (int initialHeight)
		{
			_data = new int[Chunk.Size * Chunk.Size];
			for(var i = 0; i < _data.Length; ++i)
				_data[i] = initialHeight;
			Maximum = initialHeight;
		}

		/// <summary>
		/// Calculates the cell index for a chunk from and x and z-value of a coordinate
		/// </summary>
		/// <param name="cx">X-value of the coordinate</param>
		/// <param name="cz">Z-value of the coordinate</param>
		/// <returns>The index of the cell</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="cx"/> or <paramref name="cz"/> are outside the bounds of the chunk</exception>
		public static byte CalculateIndex (byte cx, byte cz)
		{
			if(Chunk.Size <= cx)
				throw new ArgumentOutOfRangeException("cx", "The x-value of the block coordinate can't be at or above " + Chunk.Size);
			if(Chunk.Size <= cz)
				throw new ArgumentOutOfRangeException("cz", "The z-value of the block coordinate can't be at or above " + Chunk.Size);

			var index = (byte)(cz * Chunk.Size + cx);
			return index;
		}

		#region Serialization
		/// <summary>
		/// Writes the height map to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (System.IO.BinaryWriter bw)
		{
			if(null == bw)
				throw new ArgumentNullException("bw", "The stream writer can't be null.");

			var node = ConstructNbtNode(DefaultNodeName);
			new Tree(node).WriteToStream(bw);
		}

		/// <summary>
		/// Reads a height map from a stream
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>A height map read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static HeightData ReadFromStream (System.IO.BinaryReader br)
		{
			if(null == br)
				throw new ArgumentNullException("br", "The stream reader can't be null.");

			var tree = Tree.ReadFromStream(br);
			return new HeightData(tree.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the height map
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node containing the height map</returns>
		public Node ConstructNbtNode (string name)
		{
			return new IntArrayNode(name, _data);
		}

		/// <summary>
		/// Creates a height map from an NBT node
		/// </summary>
		/// <param name="node">Node containing the height map</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the format of the data contained in <paramref name="node"/> is incorrect for a height map</exception>
		public HeightData (Node node)
		{
			var iaNode = validateNode(node);
			_data = validateIntArray(iaNode);
		}

		#region Validation
		private static IntArrayNode validateNode (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The node containing the height map can't be null.");
			if(node.Type != NodeType.IntArray)
				throw new FormatException("The height map node type must be an integer array.");
			return (IntArrayNode)node;
		}

		private static int[] validateIntArray (IntArrayNode node)
		{
			if(node.Data.Length != Chunk.Size * Chunk.Size)
				throw new FormatException("The length of the height map is not valid. Expected: " + (Chunk.Size * Chunk.Size) + ", got: " + node.Data.Length);
			return node.Data;
		}
		#endregion
		#endregion
	}
}
