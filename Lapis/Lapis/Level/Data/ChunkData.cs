using System;
using System.Linq;
using Lapis.Blocks;
using Lapis.IO;
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
	public class ChunkData : ISerializable
	{
		private bool _terrainPopulated = true; // TODO: Don't mark chunks as populated here
		private readonly int _cx, _cz;
		private long _lastUpdate;
		private readonly ChunkSectionData[] _sections;
		private readonly BiomeData _biomes;
		private readonly HeightData _heightMap;
		private bool _modified;

		// TODO: Add TileTicks support
		// TODO: Add TileEntities support

		/// <summary>
		/// Creates new chunk data
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		public ChunkData (int cx, int cz)
		{
			_cx = cx;
			_cz = cz;
			_lastUpdate = Timestamp.Now * 1000; // Milliseconds

			_sections  = new ChunkSectionData[Chunk.SectionCount];
			_biomes    = new BiomeData();
			_heightMap = new HeightData();

			for(var i = (byte)0; i < _sections.Length; ++i)
				_sections[i] = new ChunkSectionData(i);
		}

		#region Properties
		/// <summary>
		/// Whether or not the terrain in the chunk has been populated
		/// </summary>
		public bool TerrainPopulated
		{
			get { return _terrainPopulated; }
			set
			{
				_modified = true;
				_terrainPopulated = value;
			}
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
		/// Last time the world was updated (Unix time in milliseconds)
		/// </summary>
		public long LastUpdate
		{
			get { return _lastUpdate; }
			set
			{
				_modified   = true;
				_lastUpdate = value;
			}
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

		/// <summary>
		/// Sections contained within the chunk
		/// </summary>
		/// <remarks>The sections are arranged 0 to 15 (bedrock to sky).
		/// Updating any of the sections will update this chunk.</remarks>
		public ChunkSectionData[] Sections
		{
			get
			{
				var sections = new ChunkSectionData[_sections.Length];
				for(var i = 0; i < sections.Length; ++i)
					sections[i] = _sections[i];
				return sections;
			}
		}

		/// <summary>
		/// Block types contained within the chunk
		/// </summary>
		/// <remarks>The first dimension is the chunk section, arranged from 0 to 15.
		/// The second dimension is the block types arranged by index.
		/// It is ideal to use CalculateSectionIndex() with this property since it can be used to get the section index and block index.
		/// However, for faster access, don't constantly compute the index, instead, use YZX ordering.
		/// Updating any of the block values will update this chunk.</remarks>
		public BlockType[][] BlockTypes
		{
			get
			{
				var sections = new BlockType[_sections.Length][];
				for(var i = 0; i < sections.Length; ++i)
					sections[i] = _sections[i].BlockTypes;
				return sections;
			}
		}

		/// <summary>
		/// Block data contained within the chunk
		/// </summary>
		/// <remarks>Each element is a section within the chunk, arranged from 0 to 15.
		/// Each nibble array contains the block data arranged by index.
		/// It is ideal to use CalculateSectionIndex() with this property since it can be used to get the section index and block index.
		/// However, for faster access, don't constantly compute the index, instead, use YZX ordering.
		/// Updating any of the block values will update this chunk.</remarks>
		public NibbleArray[] BlockData
		{
			get
			{
				var sections = new NibbleArray[_sections.Length];
				for(var i = 0; i < sections.Length; ++i)
					sections[i] = _sections[i].BlockData;
				return sections;
			}
		}

		/// <summary>
		/// Sky light values contained within the chunk
		/// </summary>
		/// <remarks>Each element is a section within the chunk, arranged from 0 to 15.
		/// Each nibble array contains the sky light values arranged by index.
		/// It is ideal to use CalculateSectionIndex() with this property since it can be used to get the section index and block index.
		/// However, for faster access, don't constantly compute the index, instead, use YZX ordering.
		/// Updating any of the block values will update this chunk.</remarks>
		public NibbleArray[] SkyLight
		{
			get
			{
				var sections = new NibbleArray[_sections.Length];
				for(var i = 0; i < sections.Length; ++i)
					sections[i] = _sections[i].SkyLight;
				return sections;
			}
		}

		/// <summary>
		/// Block light values contained within the chunk
		/// </summary>
		/// <remarks>Each element is a section within the chunk, arranged from 0 to 15.
		/// Each nibble array contains the block light values arranged by index.
		/// It is ideal to use CalculateSectionIndex() with this property since it can be used to get the section index and block index.
		/// However, for faster access, don't constantly compute the index, instead, use YZX ordering.
		/// Updating any of the block values will update this chunk.</remarks>
		public NibbleArray[] BlockLight
		{
			get
			{
				var sections = new NibbleArray[_sections.Length];
				for(var i = 0; i < sections.Length; ++i)
					sections[i] = _sections[i].BlockLight;
				return sections;
			}
		}
		#endregion

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
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			return _sections[sy].GetBlockType(bx, by, bz);
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
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			_sections[sy].SetBlockType(bx, by, bz, type);
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
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			return _sections[sy].GetBlockData(bx, by, bz);
		}

		/// <summary>
		/// Updates the extra data about a block at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="value">New block data</param>
		public void SetBlockData (byte bx, byte by, byte bz, byte value)
		{
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			_sections[sy].SetBlockData(bx, by, bz, value);
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
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			return _sections[sy].GetBlockLight(bx, by, bz);
		}

		/// <summary>
		/// Updates the amount of block light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="amount">New block light amount (0 - 15)</param>
		public void SetBlockLight (byte bx, byte by, byte bz, byte amount)
		{
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			_sections[sy].SetBlockLight(bx, by, bz, amount);
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
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			return _sections[sy].GetSkyLight(bx, by, bz);
		}

		/// <summary>
		/// Updates the amount of sky light at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block relative to the chunk</param>
		/// <param name="by">Y-position of the block relative to the chunk</param>
		/// <param name="bz">Z-position of the block relative to the chunk</param>
		/// <param name="amount">New sky light amount (0 - 15)</param>
		public void SetSkyLight (byte bx, byte by, byte bz, byte amount)
		{
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			_sections[sy].SetSkyLight(bx, by, bz, amount);
		}

		/// <summary>
		/// Updates the information for a block at a given coordinate
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <param name="block">Block information</param>
		public void SetBlock (byte bx, byte by, byte bz, BlockInformation block)
		{
			byte sy;
			by = CalculateSectionIndex(by, out sy);
			_sections[sy].SetBlock(bx, by, bz, block);
		}
		#endregion

		/// <summary>
		/// Whether or not the chunk data has been modified
		/// </summary>
		public bool Modified
		{
			get
			{
				if(_modified)
					return true;
				if(_biomes.Modified || _heightMap.Modified)
					return true;
				return _sections.Any(s => s.Modified);
			}
		}

		/// <summary>
		/// Resets the modified property so that the chunk data appears as unmodified
		/// </summary>
		public void ClearModificationFlag ()
		{
			_modified = false;
			_biomes.ClearModificationFlag();
			_heightMap.ClearModificationFlag();
			foreach(var s in _sections)
				s.ClearModificationFlag();
		}

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
		/// <returns>Chunk data read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static ChunkData ReadFromStream (System.IO.BinaryReader br)
		{
			var tree = Tree.ReadFromStream(br);
			return new ChunkData(tree.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the chunk data
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
		/// Creates new chunk data from the data contained in an NBT node
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
			_sections         = validateSectionsNode(rootNode);
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
			return _biomes.ConstructNbtNode(BiomesNodeName);
		}

		private Node constructHeightMapNode ()
		{
			return _heightMap.ConstructNbtNode(HeightMapNodeName);
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
			var chunkSections = new ListNode(SectionsNodeName, NodeType.Compound); // This is bad to assume the node type
			var sectionCount  = Math.Min(Chunk.SectionCount - 1, Math.Max(0, _heightMap.Maximum / Chunk.SectionHeight));
			for(var i = 0; i <= sectionCount; ++i) // TODO: Take advantage of the Modified property
				chunkSections.Add(_sections[i].ConstructNbtNode(ChunkSectionData.DefaultNodeName));
			return chunkSections;
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

		private static ChunkSectionData[] validateSectionsNode (CompoundNode rootNode)
		{
			var sections = new ChunkSectionData[Chunk.SectionCount];
			if(rootNode.Contains(SectionsNodeName))
			{
				var tempNode = rootNode[SectionsNodeName];
				if(tempNode.Type == NodeType.List)
				{
					var node = (ListNode)tempNode;
					foreach(var chunkSection in node)
					{// Retrieve existing data from NBT
						byte sy;
						var section = validateChunkSectionNode(chunkSection, out sy);
						if(null != section)
							sections[sy] = section;
					}
				}
			}

			// Create empty data for any remaining chunks
			for(var i = (byte)0; i < sections.Length; ++i)
				if(null == sections[i])
					sections[i] = new ChunkSectionData(i);

			return sections;
		}

		private static ChunkSectionData validateChunkSectionNode (Node node, out byte sy)
		{
			var section = new ChunkSectionData(node);
			if(section.ValidSection)
			{
				sy = section.SectionIndex;
				return section;
			}
			sy = 0;
			return null;
		}
		#endregion
		#endregion

		#region Utility methods
		/// <summary>
		/// Calculates the index of a section (sy) and the y-position within it (by)
		/// </summary>
		/// <param name="by">Y-position of the block within the chunk, not the section</param>
		/// <param name="sy">Index of the section that the block is in (0 - 15)</param>
		/// <returns>New y-position of the block within the section</returns>
		public static byte CalculateSectionIndex (byte by, out byte sy)
		{
			sy = (byte)(by / Chunk.SectionHeight);
			return (byte)(by % Chunk.SectionHeight);
		}

		/// <summary>
		/// Calculates the index of a section (sy) and the index of a block within a section
		/// </summary>
		/// <param name="bx">X-position of the block within the chunk</param>
		/// <param name="by">Y-position of the block within the chunk</param>
		/// <param name="bz">Z-position of the block within the chunk</param>
		/// <param name="sy">Index of the section that the block is in</param>
		/// <returns>Flattened index of the block within the section</returns>
		public static int CalculateSectionIndex (byte bx, byte by, byte bz, out byte sy)
		{
			by = CalculateSectionIndex(by, out sy);
			return CalculateBlockIndex(bx, by, bz);
		}

		/// <summary>
		/// Calculates the index of a block within a chunk section
		/// </summary>
		/// <param name="bx">X-position of the block within the chunk</param>
		/// <param name="by">Y-position of the block within the chunk section</param>
		/// <param name="bz">Z-position of the block within the chunk</param>
		/// <returns>Flattened index of the block</returns>
		/// <remarks>Bounds are not checked in this method.</remarks>
		public static int CalculateBlockIndex (byte bx, byte by, byte bz)
		{
			var index = (by * Chunk.SectionHeight * Chunk.Size) + (bz * Chunk.Size) + bx;
			return index;
		}

		private static void checkBounds (byte bx, byte bz)
		{
			if(Chunk.Size <= bx)
				throw new ArgumentOutOfRangeException("bx", "The x-position of the block must be less than " + Chunk.Size);
			if(Chunk.Size <= bz)
				throw new ArgumentOutOfRangeException("bz", "The z-position of the block must be less than " + Chunk.Size);
		}
		#endregion
	}
}
