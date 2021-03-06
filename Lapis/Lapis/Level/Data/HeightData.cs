﻿using System;
using System.Linq;
using Lapis.IO;
using Lapis.IO.NBT;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Contains information about the highest blocks in a chunk
	/// </summary>
	/// <remarks>Height values are the y-position of the highest air block (non-air block +1).
	/// Height data can be used to drastically speed up processing of lighting and chunk processing.
	/// This class is not tied to any active world data.
	/// The purpose of this class is for creating chunks, load chunk data from disk, and save chunk data to disk.
	/// Locking for thread safety is not performed in this class. It is assumed that a higher level encases this class safely (for speed reasons).</remarks>
	public sealed class HeightData : ISerializable, IModifiable
	{
		private readonly int[] _data;
		private bool _modified;

		/// <summary>
		/// Creates new height map for a chunk
		/// </summary>
		/// <remarks>The data will be set to 0</remarks>
		public HeightData ()
		{
			_data   = new int[Chunk.Size * Chunk.Size];
			Maximum = 0;
		}

		/// <summary>
		/// Creates new biome data for a chunk
		/// </summary>
		/// <param name="initialHeight">Height to set for the entire chunk</param>
		public HeightData (int initialHeight)
		{
			if(0 > initialHeight || Chunk.Height < initialHeight)
				throw new ArgumentOutOfRangeException("initialHeight", "The values for the height must be at least 0 and no higher than " + Chunk.Height);

			_data = new int[Chunk.Size * Chunk.Size];
			for(var i = 0; i < _data.Length; ++i)
				_data[i] = initialHeight;
			Maximum = initialHeight;
		}

		/// <summary>
		/// The height of the highest block at an index (0 - 255)
		/// </summary>
		/// <param name="index">Index of the cell</param>
		/// <returns>The height of the block at the given cell index</returns>
		public int this[byte index]
		{
			get { return _data[index]; }
			set
			{
				if(0 > value || Chunk.Height < value)
					throw new ArgumentOutOfRangeException("value", "The values for the height must be at least 0 and no higher than " + Chunk.Height);

				_modified = true;
				_data[index] = value;
				if(value > Maximum)
					Maximum = value;
			}
		}

		/// <summary>
		/// The height of the highest block at a cell coordinate (BlockX, BlockY)
		/// </summary>
		/// <param name="bx">X-value of the coordinate</param>
		/// <param name="bz">Z-value of the coordinate</param>
		/// <returns>The height of the highest block at the given cell coordinate</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="bx"/> or <paramref name="bz"/> are outside the bounds of the chunk</exception>
		public int this[byte bx, byte bz]
		{
			get
			{
				var index = CalculateIndex(bx, bz);
				return _data[index];
			}

			set
			{
				if(0 > value || Chunk.Height < value)
					throw new ArgumentOutOfRangeException("value", "The values for the height must be at least 0 and no higher than " + Chunk.Height);

				_modified = true;
				var index = CalculateIndex(bx, bz);
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
		/// Whether or not the height data has been modified
		/// </summary>
		public bool Modified
		{
			get { return _modified; }
		}

		/// <summary>
		/// Resets the modified property so that the height data appears as unmodified
		/// </summary>
		public void ClearModificationFlag ()
		{
			_modified = false;
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
		private const string DefaultNodeName = "HeightMap";

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
			Maximum = _data.Max();
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

			for(var i = 0; i < node.Data.Length; ++i)
			{// Validate height values
				if(node.Data[i] < 0)
					node.Data[i] = 0;
				else if(node.Data[i] > Chunk.Height)
					node.Data[i] = Chunk.Height;
			}
			return node.Data;
		}
		#endregion
		#endregion
	}
}
