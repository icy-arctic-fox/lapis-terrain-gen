using System;
using Lapis.Blocks;
using Lapis.IO;
using Lapis.IO.NBT;
using Lapis.Utility;

namespace Lapis.Level.Data
{
	/// <summary>
	/// Contains raw data for a realm in a world (level.dat)
	/// </summary>
	/// <remarks>This class is not tied to any active world data.
	/// The purpose of this class is for creating levels, loading level data from disk, and saving level data to disk.
	/// Locking for thread safety is not performed in this class. It is assumed that a higher level encases this class safely (for speed reasons).</remarks>
	public class LevelData
	{
		private bool
			initialized = false,
			hardcore    = false,
			cheats      = false,
			mapFeatures = false,
			raining     = false,
			storm       = false;
		private int
			spawnX = 0,
			spawnY = 65,
			spawnZ = 0;
		private int
			rainTime = 0,
			stormTime = 0;
		private long
			lastPlayed = Timestamp.Now,
			size       = 0;
		private long
			gameTicks = 0,
			time      = 0;
		private GameMode mode = GameMode.Survival;
		private Dimension? dimension = null;

		// Required fields
		private readonly string name, generatorName, generatorOptions;
		private readonly int generatorVersion;
		private readonly long seed;

		// TODO: Implement player compound
		// TODO: Implement game rules

		#region Properties
		/// <summary>
		/// Whether or not the level was initialized
		/// </summary>
		/// <remarks>If this is false, the world should be re-initialized on the next load.</remarks>
		public bool Initialized
		{
			get { return initialized; }
			set { initialized = value; }
		}

		/// <summary>
		/// Name of the level
		/// </summary>
		/// <remarks>This is the user-friendly visible name of the level.
		/// It can contain spaces and symbols.</remarks>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		/// Name of the generator used to create chunks for the level
		/// </summary>
		public string GeneratorName
		{
			get { return generatorName; }
		}

		/// <summary>
		/// Version number of the generator used to create chunks for the level
		/// </summary>
		/// <remarks>This should be used to keep consistent chunk generation between updates of terrain generator.
		/// For example, instead of using a new version of a terrain generator which may create sheer cliffs due to changes,
		/// an older version can be used to maintain consistency in the level.</remarks>
		public int GeneratorVersion
		{
			get { return generatorVersion; }
		}

		/// <summary>
		/// Additional options that may be passed to the generator for configuration
		/// </summary>
		public string GeneratorOptions
		{
			get { return generatorOptions; }
		}

		/// <summary>
		/// Random seed used for chunk generation
		/// </summary>
		public long Seed
		{
			get { return seed; }
		}

		/// <summary>
		/// Whether or not a dimension is defined in the level data
		/// </summary>
		public bool HasDimension
		{
			get { return dimension.HasValue; }
		}

		/// <summary>
		/// The dimension defined in the level data
		/// </summary>
		/// <remarks>If the level data does not define a dimension, this will be DimensionType.Normal.</remarks>
		public Dimension Dimension
		{
			get { return dimension.GetValueOrDefault(Dimension.Normal); }
			set { dimension = new Dimension?(value); }
		}

		/// <summary>
		/// Whether or not structures should be generated in the level
		/// </summary>
		/// <remarks>Structures include things like temples, mine shafts, strongholds, and villages.
		/// The structures that can be generated depends on the generator being used.</remarks>
		public bool MapFeatures
		{
			get { return mapFeatures; }
			set { mapFeatures = value; }
		}

		/// <summary>
		/// Unix timestamp of the last played time
		/// </summary>
		/// <remarks>Technically, this is the last time the level was saved.
		/// This value is automatically updated when an NBT structure is generated (about to save).</remarks>
		public long LastPlayed
		{
			get { return lastPlayed; }
		}

		/// <summary>
		/// Size of the level on disk (in bytes)
		/// </summary>
		public long DiskUsage
		{
			get { return size; }
			set { size = value; }
		}

		/// <summary>
		/// Whether or not cheats (access to commands) are allowed in this level
		/// </summary>
		public bool Cheats
		{
			get { return cheats; }
			set { cheats = value; }
		}

		/// <summary>
		/// Whether or not hardcore is enabled
		/// </summary>
		/// <remarks>If a player dies in hardcore, they cannot return to this level (or world/server).</remarks>
		public bool Hardcore
		{
			get { return hardcore; }
			set { hardcore = value; }
		}

		/// <summary>
		/// Default game mode that players are put in when they enter the level for the first time
		/// </summary>
		public GameMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}

		/// <summary>
		/// Number of ticks that have passed since the world was created
		/// </summary>
		public long Age
		{
			get { return gameTicks; }
			set { gameTicks = value; }
		}

		/// <summary>
		/// Current time of day in the level in ticks
		/// </summary>
		public long Time
		{
			get { return time; }
			set { time = value; }
		}

		/// <summary>
		/// X-position of the point where players spawn at
		/// </summary>
		/// <remarks>This is a global position within the level/realm of a block where the player's head will be.</remarks>
		public int SpawnX
		{
			get { return spawnX; }
			set { spawnX = value; }
		}

		/// <summary>
		/// Y-position of the point where players spawn at
		/// </summary>
		/// <remarks>This is a global position within the level/realm of a block where the player's head will be.</remarks>
		public int SpawnY
		{
			get { return spawnY; }
			set { spawnY = value; }
		}

		/// <summary>
		/// Z-position of the point where players spawn at
		/// </summary>
		/// <remarks>This is a global position within the level/realm of a block where the player's head will be.</remarks>
		public int SpawnZ
		{
			get { return spawnZ; }
			set { spawnZ = value; }
		}

		/// <summary>
		/// Whether or not it's raining (snowing) currently in the world
		/// </summary>
		public bool Downfall
		{
			get { return raining; }
			set { raining = value; }
		}

		/// <summary>
		/// Amount of ticks until it starts or stops raining (snowing)
		/// </summary>
		/// <remarks>When this value hits 0, Downfall should be toggled.</remarks>
		public int DownfallTime
		{
			get { return rainTime; }
			set { rainTime = value; }
		}

		/// <summary>
		/// Whether or not a thunderstorm is active
		/// </summary>
		/// <remarks>Thunderstorms decrease lighting enough that hostile mobs will spawn in the day.</remarks>
		public bool IsThunderstorm
		{
			get { return storm; }
			set { storm = false; }
		}

		/// <summary>
		/// Amount of ticks until a thunderstorm starts or stops
		/// </summary>
		/// <remarks>When this value hits 0, IsThunderstorm should be toggled.</remarks>
		public int ThunderstormTime
		{
			get { return stormTime; }
			set { stormTime = value; }
		}
		#endregion

		/// <summary>
		/// Creates new level data with default values
		/// </summary>
		/// <param name="name">Display name of the level</param>
		/// <param name="seed">Random seed used for chunk generation</param>
		/// <param name="generatorName">Name of the generator to use for chunk generation</param>
		/// <param name="generatorVersion">Version of the generator to use</param>
		/// <param name="generatorOptions">Additional options to provide to the generator</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> or <paramref name="generatorName"/> is null</exception>
		public LevelData (string name, long seed, string generatorName, int generatorVersion = 0, string generatorOptions = null)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the level can't be null.");
			if(null == generatorName)
				throw new ArgumentNullException("generatorName", "The name of the generator to use can't be null.");

			this.name             = name;
			this.generatorName    = generatorName;
			this.generatorVersion = generatorVersion;
			this.generatorOptions = (null == generatorOptions) ? string.Empty : generatorOptions;
		}

		#region Serialization
		private const int LevelVersion = 19133;

		#region Node names
		/// <summary>
		/// Default name of the level data's root node
		/// </summary>
		protected const string RootNodeName = "Data";

		/// <summary>
		/// Name for the version node
		/// </summary>
		protected const string VersionNodeName = "version";

		/// <summary>
		/// Name for the initialized node
		/// </summary>
		protected const string InitializedNodeName = "initialized";

		/// <summary>
		/// Name for the level name node
		/// </summary>
		protected const string LevelNameNodeName = "LevelName";

		/// <summary>
		/// Name for the generator name node
		/// </summary>
		protected const string GeneratorNameNodeName = "generatorName";

		/// <summary>
		/// Name for the generator version node
		/// </summary>
		protected const string GeneratorVersionNodeName = "generatorVersion";

		/// <summary>
		/// Name for the generator options node
		/// </summary>
		protected const string GeneratorOptionsNodeName = "generatorOptions";

		/// <summary>
		/// Name for the seed node
		/// </summary>
		protected const string SeedNodeName = "RandomSeed";

		/// <summary>
		/// Name for the map features node
		/// </summary>
		protected const string MapFeaturesNodeName = "MapFeatures";

		/// <summary>
		/// Name for the last played node
		/// </summary>
		protected const string LastPlayedNodeName = "LastPlayed";

		/// <summary>
		/// Name for the disk usage node
		/// </summary>
		protected const string DiskUsageNodeName = "SizeOnDisk";

		/// <summary>
		/// Name for the cheats node
		/// </summary>
		protected const string CheatsNodeName = "allowCommands";

		/// <summary>
		/// Name for the hardcore node
		/// </summary>
		protected const string HardcoreNodeName = "hardcore";

		/// <summary>
		/// Name for the game type node
		/// </summary>
		protected const string ModeNodeName = "GameType";

		/// <summary>
		/// Name for the time played node
		/// </summary>
		protected const string AgeNodeName = "Time";

		/// <summary>
		/// Name for the time of day node
		/// </summary>
		protected const string TimeNodeName = "DayTime";

		/// <summary>
		/// Name for the spawn x-position node
		/// </summary>
		protected const string SpawnXNodeName = "SpawnX";

		/// <summary>
		/// Name for the spawn y-position node
		/// </summary>
		protected const string SpawnYNodeName = "SpawnY";

		/// <summary>
		/// Name for the spawn z-position node
		/// </summary>
		protected const string SpawnZNodeName = "SpawnZ";

		/// <summary>
		/// Name for the downfall node
		/// </summary>
		protected const string DownfallNodeName = "raining";

		/// <summary>
		/// Name for the downfall time node
		/// </summary>
		protected const string DownfallTimeNodeName = "rainTime";

		/// <summary>
		/// Name for the thunderstorm node
		/// </summary>
		protected const string IsThunderstormNodeName = "thundering";

		/// <summary>
		/// Name for the thunderstorm time node
		/// </summary>
		protected const string ThunderstormTimeNodeName = "thunderTime";
		#endregion

		/// <summary>
		/// Writes the contents of the level to a stream.
		/// </summary>
		/// <param name="bw">Stream writer</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="bw"/> is null</exception>
		public void WriteToStream (System.IO.BinaryWriter bw)
		{
			Node node = GetNBTNode(RootNodeName);
			Tree nbt   = new Tree(node);
			nbt.WriteToStream(bw);
		}

		/// <summary>
		/// Reads the contents of the level from a stream.
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>Level data read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static LevelData ReadFromStream (System.IO.BinaryReader br)
		{
			Tree nbt = Tree.ReadFromStream(br);
			return new LevelData(nbt.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the level data.
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node that contains the level data</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public Node GetNBTNode (string name)
		{
			CompoundNode root = new CompoundNode(name);
			ConstructNode(root);
			return root;
		}

		/// <summary>
		/// Creates new level data from the data contained in an NBT node.
		/// </summary>
		/// <param name="node">Node that contains the level data</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="FormatException">Thrown if the format of the level data contained in <paramref name="node"/> is invalid</exception>
		public LevelData (Node node)
		{
			CompoundNode rootNode = validateLevelNode(node);
			validateVersionNode(rootNode);	// It is important to check the version first

			// Required nodes
			name             = validateLevelNameNode(rootNode);
			seed             = validateSeedNode(rootNode);
			generatorName    = validateGeneratorNameNode(rootNode);
			generatorVersion = validateGeneratorVersionNode(rootNode);
			generatorOptions = validateGeneratorOptionsNode(rootNode);

			// Not required "repairable" nodes
			validateInitializedNode(rootNode);
			validateMapFeaturesNode(rootNode);
			validateLastPlayedNode(rootNode);
			validateDiskUsageNode(rootNode);
			validateCheatsNode(rootNode);
			validateHardcoreNode(rootNode);
			validateModeNode(rootNode);
			validateAgeNode(rootNode);
			validateTimeNode(rootNode);
			validateSpawnXNode(rootNode);
			validateSpawnYNode(rootNode);
			validateSpawnZNode(rootNode);
			validateDownfallNode(rootNode);
			validateDownfallTimeNode(rootNode);
			validateIsThunderstormNode(rootNode);
			validateThunderstormTimeNode(rootNode);
		}

		#region NBT construction
		/// <summary>
		/// Constructs the NBT node that contains the level data
		/// </summary>
		/// <param name="root">A compound node to insert child nodes into (that contain parts of the level data)</param>
		/// <remarks>If a sub-class needs to add custom data to the level (NBT structure), do so by overriding this function.
		/// To include nodes from parent classes, use base.ConstructNode(root).</remarks>
		protected virtual void ConstructNode (CompoundNode root)
		{
			root.Add(constructVersionNode());
			root.Add(constructLevelNameNode());
			root.Add(constructSeedNode());
			root.Add(constructGeneratorNameNode());
			root.Add(constructGeneratorVersionNode());
			root.Add(constructGeneratorOptionsNode());
			root.Add(constructInitializedNode());
			root.Add(constructMapFeaturesNode());
			root.Add(constructLastPlayedNode());
			root.Add(constructDiskUsageNode());
			root.Add(constructCheatsNode());
			root.Add(constructHardcoreNode());
			root.Add(constructModeNode());
			root.Add(constructAgeNode());
			root.Add(constructTimeNode());
			root.Add(constructSpawnXNode());
			root.Add(constructSpawnYNode());
			root.Add(constructSpawnZNode());
			root.Add(constructDownfallNode());
			root.Add(constructDownfallTimeNode());
			root.Add(constructIsThunderstormNode());
			root.Add(constructThunderstormTimeNode());
		}

		private Node constructVersionNode ()
		{
			return new IntNode(VersionNodeName, LevelVersion);
		}

		private Node constructLevelNameNode ()
		{
			return new StringNode(LevelNameNodeName, name);
		}

		private Node constructSeedNode ()
		{
			return new LongNode(SeedNodeName, seed);
		}

		private Node constructGeneratorNameNode ()
		{
			return new StringNode(GeneratorNameNodeName, generatorName);
		}

		private Node constructGeneratorVersionNode ()
		{
			return new IntNode(GeneratorVersionNodeName, generatorVersion);
		}

		private Node constructGeneratorOptionsNode ()
		{
			return new StringNode(GeneratorOptionsNodeName, generatorOptions);
		}

		private Node constructInitializedNode ()
		{
			return new ByteNode(InitializedNodeName, initialized);
		}

		private Node constructMapFeaturesNode ()
		{
			return new ByteNode(MapFeaturesNodeName, mapFeatures);
		}

		private Node constructLastPlayedNode ()
		{
			return new LongNode(LastPlayedNodeName, lastPlayed);
		}

		private Node constructDiskUsageNode ()
		{
			return new LongNode(DiskUsageNodeName, size);
		}

		private Node constructCheatsNode ()
		{
			return new ByteNode(CheatsNodeName, cheats);
		}

		private Node constructHardcoreNode ()
		{
			return new ByteNode(HardcoreNodeName, hardcore);
		}

		private Node constructModeNode ()
		{
			return new IntNode(ModeNodeName, (int)mode);
		}

		private Node constructAgeNode ()
		{
			return new LongNode(AgeNodeName, gameTicks);
		}

		private Node constructTimeNode ()
		{
			return new LongNode(TimeNodeName, time);
		}

		private Node constructSpawnXNode ()
		{
			return new IntNode(SpawnXNodeName, spawnX);
		}

		private Node constructSpawnYNode ()
		{
			return new IntNode(SpawnYNodeName, spawnY);
		}

		private Node constructSpawnZNode ()
		{
			return new IntNode(SpawnZNodeName, spawnZ);
		}

		private Node constructDownfallNode ()
		{
			return new ByteNode(DownfallNodeName, raining);
		}

		private Node constructDownfallTimeNode ()
		{
			return new IntNode(DownfallTimeNodeName, rainTime);
		}

		private Node constructIsThunderstormNode ()
		{
			return new ByteNode(IsThunderstormNodeName, storm);
		}

		private Node constructThunderstormTimeNode ()
		{
			return new IntNode(ThunderstormTimeNodeName, stormTime);
		}
		#endregion

		#region Validation
		private CompoundNode validateLevelNode (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The node that contains the level data can't be null.");
			if(node.Type != NodeType.Compound)
				throw new FormatException("The level NBT node must be a compound node.");
			return (CompoundNode)node;
		}

		private void validateVersionNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(VersionNodeName))
				throw new FormatException("The level NBT does not contain a version node.");
			Node tempNode = rootNode[VersionNodeName];
			if(tempNode.Type != NodeType.Int)
				throw new FormatException("The version node must be an integer node.");
			int version = ((IntNode)tempNode).Value;
			if(LevelVersion != version)
				throw new FormatException("The version of the level data is not supported. Expected: " + LevelVersion + " got: " + version);
		}

		private string validateLevelNameNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(LevelNameNodeName))
				throw new FormatException("The level NBT does not contain a level name node.");
			Node tempNode = rootNode[LevelNameNodeName];
			if(tempNode.Type != NodeType.String)
				throw new FormatException("The level name node must be a string node.");
			return ((StringNode)tempNode).Value;
		}

		private long validateSeedNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(SeedNodeName))
				throw new FormatException("The level NBT does not contain a random seed node.");
			Node tempNode = rootNode[SeedNodeName];
			if(tempNode.Type != NodeType.Long)
				throw new FormatException("The random seed node must be a long node.");
			return ((LongNode)tempNode).Value;
		}

		private string validateGeneratorNameNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(GeneratorNameNodeName))
				throw new FormatException("The level NBT does not contain a generator name node.");
			Node tempNode = rootNode[GeneratorNameNodeName];
			if(tempNode.Type != NodeType.String)
				throw new FormatException("The generator name name node must be a string node.");
			return ((StringNode)tempNode).Value;
		}

		private int validateGeneratorVersionNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(GeneratorVersionNodeName))
				throw new FormatException("The level NBT does not contain a generator version node.");
			Node tempNode = rootNode[GeneratorVersionNodeName];
			if(tempNode.Type != NodeType.Int)
				throw new FormatException("The generator version node must be an integer node.");
			return ((IntNode)tempNode).Value;
		}

		private string validateGeneratorOptionsNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(GeneratorOptionsNodeName))
			{
				Node tempNode = rootNode[GeneratorOptionsNodeName];
				if(tempNode.Type == NodeType.String)
					return ((StringNode)tempNode).Value;
			}
			return string.Empty;
		}

		private void validateInitializedNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(InitializedNodeName))
			{
				Node tempNode = rootNode[InitializedNodeName];
				if(tempNode.Type == NodeType.Byte)
					initialized = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateMapFeaturesNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(MapFeaturesNodeName))
			{
				Node tempNode = rootNode[MapFeaturesNodeName];
				if(tempNode.Type == NodeType.Byte)
					mapFeatures = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateLastPlayedNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(LastPlayedNodeName))
			{
				Node tempNode = rootNode[LastPlayedNodeName];
				if(tempNode.Type == NodeType.Long)
					lastPlayed = ((LongNode)tempNode).Value;
			}
		}

		private void validateDiskUsageNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(DiskUsageNodeName))
			{
				Node tempNode = rootNode[DiskUsageNodeName];
				if(tempNode.Type == NodeType.Long)
					size = ((LongNode)tempNode).Value;
			}
		}

		private void validateCheatsNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(CheatsNodeName))
			{
				Node tempNode = rootNode[CheatsNodeName];
				if(tempNode.Type == NodeType.Byte)
					cheats = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateHardcoreNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(HardcoreNodeName))
			{
				Node tempNode = rootNode[HardcoreNodeName];
				if(tempNode.Type == NodeType.Byte)
					hardcore = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateModeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(ModeNodeName))
			{
				Node tempNode = rootNode[ModeNodeName];
				if(tempNode.Type == NodeType.Int)
				{
					int value = ((IntNode)tempNode).Value;
					mode = (GameMode)value;
				}
			}
		}

		private void validateAgeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(AgeNodeName))
			{
				Node tempNode = rootNode[AgeNodeName];
				if(tempNode.Type == NodeType.Long)
					gameTicks = ((LongNode)tempNode).Value;
			}
		}

		private void validateTimeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(TimeNodeName))
			{
				Node tempNode = rootNode[TimeNodeName];
				if(tempNode.Type == NodeType.Long)
					time = ((LongNode)tempNode).Value;
			}
		}

		private void validateSpawnXNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SpawnXNodeName))
			{
				Node tempNode = rootNode[SpawnXNodeName];
				if(tempNode.Type == NodeType.Int)
					spawnX = ((IntNode)tempNode).Value;
			}
		}

		private void validateSpawnYNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SpawnYNodeName))
			{
				Node tempNode = rootNode[SpawnYNodeName];
				if(tempNode.Type == NodeType.Int)
					spawnY = ((IntNode)tempNode).Value;
			}
		}

		private void validateSpawnZNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SpawnZNodeName))
			{
				Node tempNode = rootNode[SpawnZNodeName];
				if(tempNode.Type == NodeType.Int)
					spawnZ = ((IntNode)tempNode).Value;
			}
		}

		private void validateDownfallNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(DownfallNodeName))
			{
				Node tempNode = rootNode[DownfallNodeName];
				if(tempNode.Type == NodeType.Byte)
					raining = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateDownfallTimeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(DownfallTimeNodeName))
			{
				Node tempNode = rootNode[DownfallTimeNodeName];
				if(tempNode.Type == NodeType.Int)
					rainTime = ((IntNode)tempNode).Value;
			}
		}

		private void validateIsThunderstormNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(IsThunderstormNodeName))
			{
				Node tempNode = rootNode[IsThunderstormNodeName];
				if(tempNode.Type == NodeType.Byte)
					storm = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateThunderstormTimeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(ThunderstormTimeNodeName))
			{
				Node tempNode = rootNode[ThunderstormTimeNodeName];
				if(tempNode.Type == NodeType.Int)
					stormTime = ((IntNode)tempNode).Value;
			}
		}
		#endregion
		#endregion
	}
}
