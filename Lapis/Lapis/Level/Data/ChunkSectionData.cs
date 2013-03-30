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
	public class ChunkSectionData : ISerializable
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
		private readonly BlockType[] _blockTypes = new BlockType[SectionLength];
		private readonly NibbleArray
			_blockData  = new NibbleArray(SectionLength),
			_skyLight   = new NibbleArray(SectionLength),
			_blockLight = new NibbleArray(SectionLength);

		/// <summary>
		/// Creates an arbitrary chunk section
		/// </summary>
		/// <remarks>This will make the section invalid, but the section can still be modified.</remarks>
		public ChunkSectionData ()
		{
			_sy = InvalidSection;
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
		}

		/// <summary>
		/// Whether or not the section is valid
		/// </summary>
		public bool ValidSection
		{
			get { return Chunk.SectionCount <= _sy; }
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
		public BlockType this [int index]
		{
			get { return GetBlockType(index); }
			set { SetBlockType(index, value); }
		}

		public BlockType this [byte bx, byte by, byte bz]
		{
			get { return GetBlockType(bx, by, bz); }
			set { SetBlockType(bx, by, bz, value); }
		}

		public BlockType[] BlockTypes
		{
			get { return _blockTypes; }
		}

		public BlockType GetBlockType (int index)
		{
			throw new NotImplementedException();
		}

		public void SetBlockType (int index, BlockType type)
		{
			throw new NotImplementedException();
		}

		public BlockType GetBlockType (byte bx, byte by, byte bz)
		{
			throw new NotImplementedException();
		}

		public void SetBlockType (byte bx, byte by, byte bz, BlockType type)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Block data
		public NibbleArray BlockData
		{
			get { return _blockData; }
		}

		public byte GetBlockData (int index)
		{
			throw new NotImplementedException();
		}

		public void SetBlockData (int index, byte data)
		{
			throw new NotImplementedException();
		}

		public byte GetBlockData (byte bx, byte by, byte bz)
		{
			throw new NotImplementedException();
		}

		public void SetBlockData (byte bx, byte by, byte bz, byte data)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Sky light
		public NibbleArray SkyLight
		{
			get { return _skyLight; }
		}

		public byte GetSkyLight (int index)
		{
			throw new NotImplementedException();
		}

		public void SetSkyLight (int index, byte amount)
		{
			throw new NotImplementedException();
		}

		public byte GetSkyLight (byte bx, byte by, byte bz)
		{
			throw new NotImplementedException();
		}

		public void SetSkyLight (byte bx, byte by, byte bz, byte amount)
		{
			// TODO: Check bounds here
			throw new NotImplementedException();
		}
		#endregion

		#region Block light
		public NibbleArray BlockLight
		{
			get { return _blockLight; }
		}

		public byte GetBlockLight (int index)
		{
			throw new NotImplementedException();
		}

		public void SetBlockLight (int index, byte amount)
		{
			throw new NotImplementedException();
		}

		public byte GetBlockLight (byte bx, byte by, byte bz)
		{
			throw new NotImplementedException();
		}

		public void SetBlockLight (byte bx, byte by, byte bz, byte amount)
		{
			throw new NotImplementedException();
		}
		#endregion
		#endregion

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
