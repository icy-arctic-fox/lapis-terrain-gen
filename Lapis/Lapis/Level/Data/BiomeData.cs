using System;
using Lapis.IO;
using Lapis.IO.NBT;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Contains data about a chunk's biomes
	/// </summary>
	/// <remarks>This class is not tied to any active world data.
	/// The purpose of this class is for creating chunks, load chunk data from disk, and save chunk data to disk.
	/// Locking for thread safety is not performed in this class. It is assumed that a higher level encases this class safely (for speed reasons).</remarks>
	public sealed class BiomeData : ISerializable
	{
		private const string DefaultNodeName = "BiomeData";

		private readonly BiomeType[] _data;

		/// <summary>
		/// The biome type at an index (0 - 255)
		/// </summary>
		/// <param name="index">Index of the cell</param>
		/// <returns>The biome type at the given cell index</returns>
		public BiomeType this[byte index]
		{
			get { return _data[index]; }
			set { _data[index] = value; }
		}

		/// <summary>
		/// The biome type at a cell coordinate (BlockX, BlockZ)
		/// </summary>
		/// <param name="bx">X-value of the coordinate</param>
		/// <param name="bz">Z-value of the coordinate</param>
		/// <returns>The biome type at the given cell coordinate</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bx"/> or <paramref name="bz"/> are outside the bounds of the chunk</exception>
		public BiomeType this[byte bx, byte bz]
		{
			get
			{
				var index = CalculateIndex(bx, bz);
				return _data[index];
			}

			set
			{
				var index = CalculateIndex(bx, bz);
				_data[index] = value;
			}
		}

		/// <summary>
		/// Creates new biome data for a chunk
		/// </summary>
		/// <remarks>The data will be set to BiomeType.Ocean</remarks>
		public BiomeData ()
		{
			_data = new BiomeType[Chunk.Size * Chunk.Size];
		}

		/// <summary>
		/// Creates new biome data for a chunk
		/// </summary>
		/// <param name="initialType">Biome type to set for the entire chunk</param>
		public BiomeData (BiomeType initialType)
		{
			_data = new BiomeType[Chunk.Size * Chunk.Size];
			_data.Fill(initialType);
		}

		/// <summary>
		/// Calculates the cell index for a chunk from and x and z-value of a coordinate
		/// </summary>
		/// <param name="bx">X-value of the coordinate</param>
		/// <param name="bz">Z-value of the coordinate</param>
		/// <returns>The index of the cell</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bx"/> or <paramref name="bz"/> are outside the bounds of the chunk</exception>
		public static byte CalculateIndex (byte bx, byte bz)
		{
			if(Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The x-value of the block coordinate can't be at or above " + Chunk.Size);
			if(Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The z-value of the block coordinate can't be at or above " + Chunk.Size);

			var index = (byte)(bz * Chunk.Size + bx);
			return index;
		}

		#region Serialization
		/// <summary>
		/// Writes the biome data to a stream
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
		/// Reads biome data from a stream
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>Biome data read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static BiomeData ReadFromStream (System.IO.BinaryReader br)
		{
			if(null == br)
				throw new ArgumentNullException("br", "The stream reader can't be null.");

			var tree = Tree.ReadFromStream(br);
			return new BiomeData(tree.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the biome data
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node containing the biome data</returns>
		public Node ConstructNbtNode (string name)
		{
			var bytes = _data.GetBytes();
			return new ByteArrayNode(name, bytes);
		}

		/// <summary>
		/// Creates biome data from an NBT node
		/// </summary>
		/// <param name="node">Node containing the biome data</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the format of the data contained in <paramref name="node"/> is incorrect for biome data</exception>
		public BiomeData (Node node)
		{
			var baNode = validateNode(node);
			var bytes  = validateByteArray(baNode);
			_data = bytes.ToBiomeTypes();
		}

		#region Validation
		private static ByteArrayNode validateNode (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The node containing the biome data can't be null.");
			if(node.Type != NodeType.ByteArray)
				throw new FormatException("The biome data node type must be a byte array.");
			return (ByteArrayNode)node;
		}

		private static byte[] validateByteArray (ByteArrayNode node)
		{
			if(node.Data.Length != Chunk.Size * Chunk.Size)
				throw new FormatException("The length of the biome data is not valid. Expected: " + (Chunk.Size * Chunk.Size) + ", got: " + node.Data.Length);
			return node.Data;
		}
		#endregion
		#endregion
	}
}
