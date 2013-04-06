using System;
using Lapis.Blocks;
using Lapis.IO;
using Lapis.IO.NBT;
using Lapis.Utility;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Contains block information for just a section of a chunk
	/// </summary>
	public class ChunkSectionData : ISerializable, IModifiable // TODO: Implement IDisposable
	{
		/// <summary>
		/// Number of bytes/items in a chunk section
		/// </summary>
		internal const int SectionLength = Chunk.SectionHeight * Chunk.Size * Chunk.Size;

		/// <summary>
		/// Half the number of bytes/items in a chunk section (used for nibbles)
		/// </summary>
		internal const int HalfSectionLength = SectionLength / 2;

		private const byte InvalidSection = byte.MaxValue;

		private readonly byte _sy;
		private readonly BlockType[] _blockTypes;
		private readonly NibbleArray _blockData, _skyLight, _blockLight;
		private bool _modified;

		/// <summary>
		/// Creates an arbitrary chunk section
		/// </summary>
		/// <remarks>This will make the section invalid, but the section can still be modified.</remarks>
		public ChunkSectionData ()
		{
			_sy = InvalidSection;

			_blockTypes = new BlockType[SectionLength];
			_blockData  = new NibbleArray(SectionLength);
			_skyLight   = new NibbleArray(SectionLength);
			_blockLight = new NibbleArray(SectionLength);
		}

		/// <summary>
		/// Creates a chunk section
		/// </summary>
		/// <param name="sy">Index of the section (0 - 15, 0 being the lowest at bedrock)</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="sy"/> is out of bounds</exception>
		public ChunkSectionData (byte sy)
		{
			if(Chunk.SectionCount <= sy)
				throw new ArgumentOutOfRangeException("sy", "The chunk section index must be less than " + Chunk.SectionCount);

			_sy = sy;

			_blockTypes = new BlockType[SectionLength];
			_blockData  = new NibbleArray(SectionLength);
			_skyLight   = new NibbleArray(SectionLength);
			_blockLight = new NibbleArray(SectionLength);
		}

		/// <summary>
		/// Whether or not the section is valid
		/// </summary>
		public bool ValidSection
		{
			get { return Chunk.SectionCount > _sy; }
		}

		/// <summary>
		/// Index of the section within the chunk
		/// </summary>
		public byte SectionIndex
		{
			get { return _sy; }
		}

		#region Block Information
		#region Block types
		/// <summary>
		/// Get and set block type
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <returns>Type of block at the index</returns>
		public BlockType this [int index]
		{
			get { return GetBlockType(index); }
			set { SetBlockType(index, value); }
		}

		/// <summary>
		/// Get and set block type
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Type of block at the coordinate</returns>
		public BlockType this [byte bx, byte by, byte bz]
		{
			get { return GetBlockType(bx, by, bz); }
			set { SetBlockType(bx, by, bz, value); }
		}

		/// <summary>
		/// Collection of block types within the section
		/// </summary>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public BlockType[] BlockTypes
		{
			get { return _blockTypes; }
		}

		/// <summary>
		/// Gets a block type
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <returns>Type of block at the index</returns>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other GetBlockType() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public BlockType GetBlockType (int index)
		{
			checkIndex(index);
			return _blockTypes[index];
		}

		/// <summary>
		/// Sets a block type
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <param name="type">Type to set the block to</param>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other SetBlockType() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public void SetBlockType (int index, BlockType type)
		{
			checkIndex(index);
			_modified = true;
			_blockTypes[index] = type;
		}

		/// <summary>
		/// Gets a block type
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Type of block at the coordinate</returns>
		public BlockType GetBlockType (byte bx, byte by, byte bz)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			return _blockTypes[index];
		}

		/// <summary>
		/// Sets a block type
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="type">Type to set the block to</param>
		public void SetBlockType (byte bx, byte by, byte bz, BlockType type)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			_modified = true;
			_blockTypes[index] = type;
		}
		#endregion

		#region Block data
		/// <summary>
		/// Collection of block data within the section
		/// </summary>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public NibbleArray BlockData
		{
			get { return _blockData; }
		}

		/// <summary>
		/// Gets block data
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <returns>Data for the block as the given index</returns>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other GetBlockData() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public byte GetBlockData (int index)
		{
			checkIndex(index);
			return _blockData[index];
		}

		/// <summary>
		/// Sets a block type
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <param name="data">Data to set for the block</param>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other SetBlockData() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public void SetBlockData (int index, byte data)
		{
			checkIndex(index);
			_modified = true;
			_blockData[index] = data;
		}

		/// <summary>
		/// Gets block data
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Data at the coordinate</returns>
		public byte GetBlockData (byte bx, byte by, byte bz)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			return _blockData[index];
		}

		/// <summary>
		/// Sets block data
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="data">Data to set for the block</param>
		public void SetBlockData (byte bx, byte by, byte bz, byte data)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			_modified = true;
			_blockData[index] = data;
		}
		#endregion

		#region Sky light
		/// <summary>
		/// Collection of sky light within the section
		/// </summary>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public NibbleArray SkyLight
		{
			get { return _skyLight; }
		}

		/// <summary>
		/// Gets the amount of sky light for a block
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <returns>Amount of sky light</returns>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other GetSkyLight() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public byte GetSkyLight (int index)
		{
			checkIndex(index);
			return _skyLight[index];
		}

		/// <summary>
		/// Sets the amount of sky light for a block
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <param name="amount">Amount of sky light</param>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other SetSkyLight() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public void SetSkyLight (int index, byte amount)
		{
			checkIndex(index);
			_modified = true;
			_skyLight[index] = amount;
		}

		/// <summary>
		/// Gets the amount of sky light for a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Amount of sky light</returns>
		public byte GetSkyLight (byte bx, byte by, byte bz)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			return _skyLight[index];
		}

		/// <summary>
		/// Sets the amount of sky light for a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="amount">Amount of sky light</param>
		public void SetSkyLight (byte bx, byte by, byte bz, byte amount)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			_modified = true;
			_skyLight[index] = amount;
		}
		#endregion

		#region Block light
		/// <summary>
		/// Collection of block light within the section
		/// </summary>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public NibbleArray BlockLight
		{
			get { return _blockLight; }
		}

		/// <summary>
		/// Gets the amount of block light for a block
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <returns>Amount of block light</returns>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other GetBlockLight() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public byte GetBlockLight (int index)
		{
			checkIndex(index);
			return _blockLight[index];
		}

		/// <summary>
		/// Sets the amount of block light for a block
		/// </summary>
		/// <param name="index">Index of the block</param>
		/// <param name="amount">Amount of block light</param>
		/// <remarks>Block indexes must be used to access the array.
		/// Use ChunkData.CalculateBlockIndex() to get an index.
		/// Alternatively, use the other SetBlockLight() method.</remarks>
		/// <seealso cref="ChunkData.CalculateBlockIndex"/>
		public void SetBlockLight (int index, byte amount)
		{
			checkIndex(index);
			_modified = true;
			_blockLight[index] = amount;
		}

		/// <summary>
		/// Gets the amount of block light for a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Amount of block light</returns>
		public byte GetBlockLight (byte bx, byte by, byte bz)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			return _blockLight[index];
		}

		/// <summary>
		/// Sets the amount of block light for a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="amount">Amount of block light</param>
		public void SetBlockLight (byte bx, byte by, byte bz, byte amount)
		{
			checkBounds(bx, by, bz);
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			_modified = true;
			_blockLight[index] = amount;
		}
		#endregion

		/// <summary>
		/// Sets the information for a block
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="block">Block information</param>
		public void SetBlock (byte bx, byte by, byte bz, BlockInformation block)
		{
			var index = ChunkData.CalculateBlockIndex(bx, by, bz);
			_modified = true;
			_blockTypes[index] = block.Type;
			_blockData[index]  = block.Data;
			_skyLight[index]   = block.SkyLight;
			_blockLight[index] = block.BlockLight;
		}
		#endregion

		/// <summary>
		/// Whether or not the chunk section data has been modified
		/// </summary>
		public bool Modified
		{
			get { return _modified; }
		}

		/// <summary>
		/// Resets the modified property so that the chunk section data appears as unmodified
		/// </summary>
		public void ClearModificationFlag ()
		{
			_modified = false;
		}

		#region Serialization
		#region Node names
		protected const string RootNodeName       = "ChunkSection";
		protected const string YPositionNodeName  = "Y";
		protected const string BlockTypesNodeName = "Blocks";
		protected const string BlockDataNodeName  = "Data";
		protected const string SkyLightNodeName   = "SkyLight";
		protected const string BlockLightNodeName = "BlockLight";

		public const string DefaultNodeName = RootNodeName;
		#endregion

		/// <summary>
		/// Writes the contents of the chunk to a stream
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (System.IO.BinaryWriter bw)
		{
			var node = ConstructNbtNode(RootNodeName);
			var tree = new Tree(node);
			tree.WriteToStream(bw);
		}

		/// <summary>
		/// Reads the contents of the chunk from a stream
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>Chunk section data read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static ChunkSectionData ReadFromStream (System.IO.BinaryReader br)
		{
			var tree = Tree.ReadFromStream(br);
			return new ChunkSectionData(tree.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the chunk section data
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node that contains the chunk data</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public Node ConstructNbtNode (string name)
		{
			var root = new CompoundNode(name);
			ConstructNode(root);
			return root;
		}

		/// <summary>
		/// Creates new chunk section data from the data contained in an NBT node
		/// </summary>
		/// <param name="node">Node that contains the chunk section data</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the format of the chunk section data contained in <paramref name="node"/> is invalid</exception>
		public ChunkSectionData (Node node)
		{
			var rootNode = validateSectionNode(node);

			// Not required "repairable" fields
			_sy         = validateYPositionNode(rootNode);
			_blockTypes = validateBlockTypesNode(rootNode);
			_blockData  = validateBlockDataNode(rootNode);
			_skyLight   = validateSkyLightNode(rootNode);
			_blockLight = validateBlockLightNode(rootNode);
		}

		#region NBT construction
		/// <summary>
		/// Constructs the NBT node that contains the chunk section data
		/// </summary>
		/// <param name="root">A compound node to insert child nodes into (that contain parts of the chunk data)</param>
		/// <remarks>If a sub-class needs to add custom data to the chunk (NBT structure), do so by overriding this function.
		/// To include nodes from parent classes, use base.ConstructNode(root).</remarks>
		protected virtual void ConstructNode (CompoundNode root)
		{
			root.Add(constructYPosNode());
			root.Add(constructBlockTypesNode());
			root.Add(constructBlockDataNode());
			root.Add(constructSkyLightNode());
			root.Add(constructBlockLightNode());
		}

		private Node constructYPosNode ()
		{
			return new ByteNode(YPositionNodeName, _sy);
		}

		private Node constructBlockTypesNode ()
		{
			return new ByteArrayNode(BlockTypesNodeName, _blockTypes.GetBytes());
		}

		private Node constructBlockDataNode ()
		{
			return new ByteArrayNode(BlockDataNodeName, _blockData.ToByteArray());
		}

		private Node constructSkyLightNode ()
		{
			return new ByteArrayNode(SkyLightNodeName, _skyLight.ToByteArray());
		}

		private Node constructBlockLightNode ()
		{
			return new ByteArrayNode(BlockLightNodeName, _blockLight.ToByteArray());
		}
		#endregion

		#region Validation
		private static CompoundNode validateSectionNode (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The node that contains the chunk section data can't be null.");
			if(node.Type != NodeType.Compound)
				throw new FormatException("The chunk section NBT node must be a compound node.");
			return (CompoundNode)node;
		}

		private static byte validateYPositionNode (CompoundNode chunkSection)
		{
			if(chunkSection.Contains(YPositionNodeName))
			{
				var tempNode = chunkSection[YPositionNodeName];
				if(tempNode.Type == NodeType.Byte)
					return ((ByteNode)tempNode).Value;
			}
			return InvalidSection;
		}

		private static BlockType[] validateBlockTypesNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(BlockTypesNodeName))
			{
				var tempNode = rootNode[BlockTypesNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == SectionLength)
						return node.Data.ToBlockTypes();
				}
			}
			return new BlockType[SectionLength];
		}

		private static NibbleArray validateBlockDataNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(BlockDataNodeName))
			{
				var tempNode = rootNode[BlockDataNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == HalfSectionLength)
						return new NibbleArray(node.Data);
				}
			}
			return new NibbleArray(SectionLength);
		}

		private static NibbleArray validateSkyLightNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SkyLightNodeName))
			{
				var tempNode = rootNode[SkyLightNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == HalfSectionLength)
						return new NibbleArray(node.Data);
				}
			}
			return new NibbleArray(SectionLength);
		}

		private static NibbleArray validateBlockLightNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(BlockLightNodeName))
			{
				var tempNode = rootNode[BlockLightNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == HalfSectionLength)
						return new NibbleArray(node.Data);
				}
			}
			return new NibbleArray(SectionLength);
		}
		#endregion
		#endregion

		#region Utility methods
		private static void checkIndex (int index)
		{
			if(0 > index || SectionLength <= index)
				throw new ArgumentOutOfRangeException("index", "The index of the block must be at least 0 and less than " + SectionLength);
		}

		private static void checkBounds (byte bx, byte by, byte bz)
		{
			if(Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The x-position of the block must be less than " + Chunk.Size);
			if(Chunk.SectionHeight <= by)
				throw new ArgumentOutOfRangeException("by", "The y-position of the block must be less than " + Chunk.SectionHeight);
			if(Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The z-position of the block must be less than " + Chunk.Size);
		}
		#endregion
	}
}
