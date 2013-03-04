using System;
using Lapis.Blocks;
using Lapis.IO.NBT;
using Lapis.Utility;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Contains raw data for a chunk (16 sections stacked)
	/// </summary>
	/// <remarks>This class is not tied to any active world data.
	/// The purpose of this class is for creating chunks, loading chunk data from disk, and saving chunk data to disk.
	/// Locking for thread safety is not performed in this class. It is assumed that a higher level encases this class safely (for speed reasons).
	/// This class will verify the validity of NBT nodes it requires, but will not fix them if they are wrong.
	/// If you want to automatically fix invalid chunk data, use SafeChunkData instead.</remarks>
	public class ChunkData
	{
		private const int SectionLength     = Chunk.SectionHeight * Chunk.Size * Chunk.Size;
		private const int HalfSectionLength = SectionLength / 2;

		private bool _terrainPopulated;
		private readonly int _cx, _cz;
		private long _lastUpdate;
		private readonly BlockType[][] _blockTypes = new BlockType[Chunk.SectionCount][];
		private readonly byte[][]
			_blockData  = new byte[Chunk.SectionCount][],
			_skyLight   = new byte[Chunk.SectionCount][],
			_blockLight = new byte[Chunk.SectionCount][];
		private readonly BiomeData _biomes;
		private readonly HeightData _heightMap;

		// TODO: Add TileTicks support
		// TODO: Add TileEntities support

		#region Properties
		/// <summary>
		/// Whether or not the terrain in the chunk has been populated
		/// </summary>
		public bool TerrainPopulated
		{
			get { return _terrainPopulated; }
			set { _terrainPopulated = value; }
		}

		/// <summary>
		/// Global, world-relative, x-position of the chunk
		/// </summary>
		public int ChunkX
		{
			get { return _cx; }
		}

		/// <summary>
		/// Global, world-relative, z-position of the chunk
		/// </summary>
		public int ChunkZ
		{
			get { return _cz; }
		}

		/// <summary>
		/// Last time the world was updated
		/// TODO: Investigate the units this uses
		/// </summary>
		public long LastUpdate
		{
			get { return _lastUpdate; }
			set { _lastUpdate = value; }
		}

		/// <summary>
		/// Biomes contained within the chunk
		/// </summary>
		public BiomeData Biomes
		{
			get { return _biomes; }
		}

		/// <summary>
		/// Heights of the highest blocks within the chunk
		/// </summary>
		public HeightData HeightMap
		{
			get { return _heightMap; }
		}
		#endregion

		/// <summary>
		/// Creates new chunk data that is completely empty
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		public ChunkData (int cx, int cz)
		{
			_cx         = cx;
			_cz         = cz;
			_lastUpdate = 0;
			_biomes     = new BiomeData();
			_heightMap  = new HeightData();

			for(var i = 0; i < Chunk.SectionCount; ++i)
			{
				_blockTypes[i] = new BlockType[SectionLength];
				_blockData[i]  = new byte[HalfSectionLength];
				_blockLight[i] = new byte[HalfSectionLength];
				_skyLight[i]   = new byte[HalfSectionLength];
			}
		}

		private static void checkBounds (byte bx, byte bz)
		{
			if(Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The x-position of the block must be less than " + Chunk.Size);
			if(Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The z-position of the block must be less than " + Chunk.Size);
		}

		/// <summary>
		/// Calculates the index of a block within a chunk section
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk section</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <returns>Flattened index of the block</returns>
		private static int calculateIndex (byte bx, byte by, byte bz)
		{
			var index = by + Chunk.Size * (bx + Chunk.Size * bz);
			return index;
		}

		#region Block information
		/// <summary>
		/// Retrieves the type of block at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <returns>The block's type</returns>
		public BlockType GetBlockType (byte bx, byte by, byte bz)
		{
			return getBlock(bx, by, bz, _blockTypes);
		}

		/// <summary>
		/// Updates the type of block at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="type">New block type</param>
		public void SetBlockType (byte bx, byte by, byte bz, BlockType type)
		{
			setBlock(bx, by, bz, _blockTypes, type);
		}

		/// <summary>
		/// Retrieves the extra data about a block at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <returns>The block's data</returns>
		public byte GetBlockData (byte bx, byte by, byte bz)
		{
			return getBlockNibble(bx, by, bz, _blockData);
		}

		/// <summary>
		/// Updates the extra data about a block at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="data">New block data</param>
		public void SetBlockData (byte bx, byte by, byte bz, byte data)
		{
			setBlockNibble(bx, by, bz, _blockData, data);
		}

		/// <summary>
		/// Retrieves the amount block light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <returns>The amount of block light (0 - 15)</returns>
		public byte GetBlockLight (byte bx, byte by, byte bz)
		{
			return getBlockNibble(bx, by, bz, _blockLight);
		}

		/// <summary>
		/// Updates the amount of block light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="value">New block light amount (0 - 15)</param>
		public void SetBlockLight (byte bx, byte by, byte bz, byte value)
		{
			setBlockNibble(bx, by, bz, _blockLight, value);
		}

		/// <summary>
		/// Retrieves the amount of sky light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <returns>The amount of sky light (0 - 15)</returns>
		public byte GetSkyLight (byte bx, byte by, byte bz)
		{
			return getBlockNibble(bx, by, bz, _skyLight);
		}

		/// <summary>
		/// Updates the amount of sky light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="value">New sky light amount (0 - 15)</param>
		public void SetSkyLight (byte bx, byte by, byte bz, byte value)
		{
			setBlockNibble(bx, by, bz, _skyLight, value);
		}

		#region Block get/set utility
		private static T getBlock<T> (byte bx, byte by, byte bz, T[][] blocks)
		{
			checkBounds(bx, bz);

			var section = by / Chunk.Size;
			var secBy   = (byte)(by % Chunk.Size);
			var index   = calculateIndex(bx, secBy, bz);

			return blocks[section][index];
		}

		private static void setBlock<T> (byte bx, byte by, byte bz, T[][] blocks, T value)
		{
			checkBounds(bx, bz);

			var section = by / Chunk.Size;
			var newBy   = (byte)(by % Chunk.Size);
			var index   = calculateIndex(bx, newBy, bz);

			blocks[section][index] = value;
		}

		private static byte getBlockNibble (byte bx, byte by, byte bz, byte[][] blocks)
		{
			checkBounds(bx, bz);

			var section = by / Chunk.Size;
			var secBy   = (byte)(by % Chunk.Size);
			var index   = calculateIndex(bx, secBy, bz);

			return blocks[section].GetNibble(index);
		}

		private static void setBlockNibble (byte bx, byte by, byte bz, byte[][] blocks, byte value)
		{
			checkBounds(bx, bz);

			var section = by / Chunk.Size;
			var secBy   = (byte)(by % Chunk.Size);
			var index   = calculateIndex(bx, secBy, bz);

			blocks[section].SetNibble(index, value);
		}
		#endregion
		#endregion

		#region Serialization
		#region Node names
		protected const string RootNodeName             = "Level";
		protected const string TerrainPopulatedNodeName = "TerrainPopulated";
		protected const string ChunkXNodeName           = "xPos";
		protected const string ChunkZNodeName           = "zPos";
		protected const string LastUpdateNodeName       = "LastUpdate";
		protected const string BiomesNodeName           = "Biomes";
		protected const string HeightMapNodeName        = "HeightMap";
		protected const string EntitiesNodeName         = "Entities";
		protected const string TileEntitiesNodeName     = "TileEntities";
		protected const string SectionsNodeName         = "Sections";

		protected const string ChunkSectionNodeName     = "ChunkSection";
		protected const string YPositionNodeName        = "Y";
		protected const string BlockTypesNodeName       = "Blocks";
		protected const string BlockDataNodeName        = "Data";
		protected const string SkyLightNodeName         = "SkyLight";
		protected const string BlockLightNodeName       = "BlockLight";
		#endregion

		/// <summary>
		/// Writes the contents of the chunk to a stream.
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (System.IO.BinaryWriter bw)
		{
			var node = GetNbtNode(RootNodeName);
			var tree = new Tree(node);
			tree.WriteToStream(bw);
		}

		/// <summary>
		/// Reads the contents of the chunk from a stream.
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>Chunk data read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static ChunkData ReadFromStream (System.IO.BinaryReader br)
		{
			var tree = Tree.ReadFromStream(br);
			return new ChunkData(tree.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the chunk data.
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node that contains the chunk data</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public Node GetNbtNode (string name)
		{
			var root = new CompoundNode(name);
			ConstructNode(root);
			return root;
		}

		/// <summary>
		/// Creates new chunk data from the data contained in an NBT node.
		/// </summary>
		/// <param name="node">Node that contains the chunk data</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the format of the chunk data contained in <paramref name="node"/> is invalid</exception>
		public ChunkData (Node node)
		{
			var rootNode = validateLevelNode(node);

			// Not required "repairable" fields
			_terrainPopulated = validateTerrainPopulatedNode(rootNode);
			_cx               = validateChunkXNode(rootNode);
			_cz               = validateChunkZNode(rootNode);
			_lastUpdate       = validateLastUpdateNode(rootNode);
			_biomes           = validateBiomesNode(rootNode);
			_heightMap        = validateHeightMapNode(rootNode);
			validateSectionsNode(rootNode);
		}

		#region NBT construction
		/// <summary>
		/// Constructs the NBT node that contains the chunk data
		/// </summary>
		/// <param name="root">A compound node to insert child nodes into (that contain parts of the chunk data)</param>
		/// <remarks>If a sub-class needs to add custom data to the chunk (NBT structure), do so by overriding this function.
		/// To include nodes from parent classes, use base.ConstructNode(root).</remarks>
		protected virtual void ConstructNode (CompoundNode root)
		{
			root.Add(constructTerrainPopulatedNode());
			root.Add(constructChunkXNode());
			root.Add(constructChunkZNode());
			root.Add(constructLastUpdateNode());
			root.Add(constructBiomesNode());
			root.Add(constructEntitiesNode());
			root.Add(constructSectionsNode());
			root.Add(constructTileEntitiesNode());
			root.Add(constructHeightMapNode());
		}

		private Node constructTerrainPopulatedNode ()
		{
			return new ByteNode(TerrainPopulatedNodeName, _terrainPopulated);
		}

		private Node constructChunkXNode ()
		{
			return new IntNode(ChunkXNodeName, _cx);
		}

		private Node constructChunkZNode ()
		{
			return new IntNode(ChunkZNodeName, _cz);
		}

		private Node constructLastUpdateNode ()
		{
			return new LongNode(LastUpdateNodeName, _lastUpdate);
		}

		private Node constructBiomesNode ()
		{
			return _biomes.GetNbtNode(BiomesNodeName);
		}

		private Node constructHeightMapNode ()
		{
			return _heightMap.GetNBTNode(HeightMapNodeName);
		}

		private Node constructEntitiesNode ()
		{
			// TODO: Implement entities
			return new ListNode(EntitiesNodeName, NodeType.Compound);
		}

		private Node constructTileEntitiesNode ()
		{
			// TODO: Implement tile entities
			return new ListNode(TileEntitiesNodeName, NodeType.Compound);
		}

		private Node constructSectionsNode ()
		{
			var chunkSections = new ListNode(SectionsNodeName, NodeType.Compound);
			var sectionCount  = (_heightMap.Maximum / Chunk.SectionHeight) + 1;
			for(var i = 0; i < sectionCount; ++i)
				chunkSections.Add(constructChunkSectionNode(i));
			return chunkSections;
		}

		private Node constructChunkSectionNode (int level)
		{
			var chunkSection   = new CompoundNode(ChunkSectionNodeName);
			var yPosNode       = new ByteNode(YPositionNodeName, (byte)level);
			var blocksNode     = new ByteArrayNode(BlockTypesNodeName, _blockTypes[level].GetBytes());
			var dataNode       = new ByteArrayNode(BlockDataNodeName, _blockData[level]);
			var skyLightNode   = new ByteArrayNode(SkyLightNodeName, _skyLight[level]);
			var blockLightNode = new ByteArrayNode(BlockLightNodeName, _blockLight[level]);

			chunkSection.Add(yPosNode);
			chunkSection.Add(blocksNode);
			chunkSection.Add(dataNode);
			chunkSection.Add(skyLightNode);
			chunkSection.Add(blockLightNode);

			return chunkSection;
		}
		#endregion

		#region Validation
		private static CompoundNode validateLevelNode (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The node that contains the chunk data can't be null.");
			if(node.Type != NodeType.Compound)
				throw new FormatException("The chunk NBT node must be a compound node.");
			return (CompoundNode)node;
		}

		private static bool validateTerrainPopulatedNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(TerrainPopulatedNodeName))
			{
				var tempNode = rootNode[TerrainPopulatedNodeName];
				if(tempNode.Type == NodeType.Byte)
					return ((ByteNode)tempNode).BooleanValue;
			}
			return false;
		}

		private static int validateChunkXNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(ChunkXNodeName))
			{
				var tempNode = rootNode[ChunkXNodeName];
				if(tempNode.Type == NodeType.Int)
					return ((IntNode)tempNode).Value;
			}
			return 0;
		}

		private static int validateChunkZNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(ChunkZNodeName))
			{
				var tempNode = rootNode[ChunkZNodeName];
				if(tempNode.Type == NodeType.Int)
					return ((IntNode)tempNode).Value;
			}
			return 0;
		}

		private static long validateLastUpdateNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(LastUpdateNodeName))
			{
				var tempNode = rootNode[LastUpdateNodeName];
				if(tempNode.Type == NodeType.Long)
					return ((LongNode)tempNode).Value;
			}
			return Timestamp.Now;
		}

		private static BiomeData validateBiomesNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(BiomesNodeName))
			{
				var node = rootNode[BiomesNodeName];
				try
				{
					return new BiomeData(node);
				}
				catch(FormatException) { }
			}
			return new BiomeData();
		}

		private static HeightData validateHeightMapNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(HeightMapNodeName))
			{
				var node = rootNode[HeightMapNodeName];
				try
				{
					return new HeightData(node);
				}
				catch(FormatException) { }
			}
			return new HeightData();
		}

		private void validateSectionsNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SectionsNodeName))
			{
				var tempNode = rootNode[SectionsNodeName];
				if(tempNode.Type == NodeType.List)
				{
					var node = (ListNode)tempNode;
					if(node.ElementType == NodeType.Compound)
					{
						// Retrieve existing data from NBT
						foreach(var chunkSection in node)
							validateChunkSectionNode((CompoundNode)chunkSection);

						// Create empty data for any remaining chunks
						for(var i = 0; i < Chunk.SectionCount; ++i)
						{
							if(null == _blockTypes[i])
							{
								_blockTypes[i] = new BlockType[SectionLength];
								_blockData[i]  = new byte[HalfSectionLength];
								_blockLight[i] = new byte[HalfSectionLength];
								_skyLight[i]   = new byte[HalfSectionLength];	// TODO: Possible problem here with setting light to 0
							}
						}
					}
				}
			}
		}

		private void validateChunkSectionNode (CompoundNode chunkSection)
		{
			int? yPos = validateYPositionNode(chunkSection);
			if(yPos.HasValue)
			{
				var y = yPos.Value;
				_blockTypes[y] = validateBlockTypesNode(chunkSection);
				_blockData[y]  = validateBlockDataNode(chunkSection);
				_skyLight[y]   = validateSkyLightNode(chunkSection);
				_blockLight[y] = validateBlockLightNode(chunkSection);
			}
		}

		private static byte? validateYPositionNode (CompoundNode chunkSection)
		{
			if(chunkSection.Contains(YPositionNodeName))
			{
				Node tempNode = chunkSection[YPositionNodeName];
				if(tempNode.Type == NodeType.Byte)
					return ((ByteNode)tempNode).Value;
			}
			return null;
		}

		private static BlockType[] validateBlockTypesNode (CompoundNode chunkSection)
		{
			if(chunkSection.Contains(BlockTypesNodeName))
			{
				var tempNode = chunkSection[BlockTypesNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == SectionLength)
						return node.Data.ToBlockTypes();
				}
			}
			return new BlockType[SectionLength];
		}

		private static byte[] validateBlockDataNode (CompoundNode chunkSection)
		{
			if(chunkSection.Contains(BlockDataNodeName))
			{
				var tempNode = chunkSection[BlockDataNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == HalfSectionLength)
						return node.Data;
				}
			}
			return new byte[HalfSectionLength];
		}

		private static byte[] validateSkyLightNode (CompoundNode chunkSection)
		{
			if(chunkSection.Contains(SkyLightNodeName))
			{
				var tempNode = chunkSection[SkyLightNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == HalfSectionLength)
						return node.Data;
				}
			}
			return new byte[HalfSectionLength];
		}

		private static byte[] validateBlockLightNode (CompoundNode chunkSection)
		{
			if(chunkSection.Contains(BlockLightNodeName))
			{
				var tempNode = chunkSection[BlockLightNodeName];
				if(tempNode.Type == NodeType.ByteArray)
				{
					var node = (ByteArrayNode)tempNode;
					if(node.Data.Length == HalfSectionLength)
						return node.Data;
				}
			}
			return new byte[HalfSectionLength];
		}
		#endregion
		#endregion
	}
}
