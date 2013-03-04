using System;
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
	public class LevelData : ISerializable
	{
		private bool _initialized, _hardcore, _cheats, _mapFeatures, _raining, _storm;
		private int _spawnX, _spawnY = 65, _spawnZ;
		private int _rainTime, _stormTime;
		private long _lastPlayed = Timestamp.Now, _size;
		private long _gameTicks, _time;
		private GameMode _mode = GameMode.Survival;
		private Dimension? _dimension;

		// Required fields
		private readonly string _name, _generatorName, _generatorOptions;
		private readonly int _generatorVersion;
		private readonly long _seed;

		// TODO: Implement player compound
		// TODO: Implement game rules

		#region Properties
		/// <summary>
		/// Whether or not the level was initialized
		/// </summary>
		/// <remarks>If this is false, the world should be re-initialized on the next load.</remarks>
		public bool Initialized
		{
			get { return _initialized; }
			set { _initialized = value; }
		}

		/// <summary>
		/// Name of the level
		/// </summary>
		/// <remarks>This is the user-friendly visible name of the level.
		/// It can contain spaces and symbols.</remarks>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Name of the generator used to create chunks for the level
		/// </summary>
		public string GeneratorName
		{
			get { return _generatorName; }
		}

		/// <summary>
		/// Version number of the generator used to create chunks for the level
		/// </summary>
		/// <remarks>This should be used to keep consistent chunk generation between updates of terrain generator.
		/// For example, instead of using a new version of a terrain generator which may create sheer cliffs due to changes,
		/// an older version can be used to maintain consistency in the level.</remarks>
		public int GeneratorVersion
		{
			get { return _generatorVersion; }
		}

		/// <summary>
		/// Additional options that may be passed to the generator for configuration
		/// </summary>
		public string GeneratorOptions
		{
			get { return _generatorOptions; }
		}

		/// <summary>
		/// Random seed used for chunk generation
		/// </summary>
		public long Seed
		{
			get { return _seed; }
		}

		/// <summary>
		/// Whether or not a dimension is defined in the level data
		/// </summary>
		public bool HasDimension
		{
			get { return _dimension.HasValue; }
		}

		/// <summary>
		/// The dimension defined in the level data
		/// </summary>
		/// <remarks>If the level data does not define a dimension, this will be DimensionType.Normal.</remarks>
		public Dimension Dimension
		{
			get { return _dimension.GetValueOrDefault(Dimension.Normal); }
			set { _dimension = value; }
		}

		/// <summary>
		/// Whether or not structures should be generated in the level
		/// </summary>
		/// <remarks>Structures include things like temples, mine shafts, strongholds, and villages.
		/// The structures that can be generated depends on the generator being used.</remarks>
		public bool MapFeatures
		{
			get { return _mapFeatures; }
			set { _mapFeatures = value; }
		}

		/// <summary>
		/// Unix timestamp of the last played time
		/// </summary>
		/// <remarks>Technically, this is the last time the level was saved.
		/// This value is automatically updated when an NBT structure is generated (about to save).</remarks>
		public long LastPlayed
		{
			get { return _lastPlayed; }
		}

		/// <summary>
		/// Size of the level on disk (in bytes)
		/// </summary>
		public long DiskUsage
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Whether or not cheats (access to commands) are allowed in this level
		/// </summary>
		public bool Cheats
		{
			get { return _cheats; }
			set { _cheats = value; }
		}

		/// <summary>
		/// Whether or not hardcore is enabled
		/// </summary>
		/// <remarks>If a player dies in hardcore, they cannot return to this level (or world/server).</remarks>
		public bool Hardcore
		{
			get { return _hardcore; }
			set { _hardcore = value; }
		}

		/// <summary>
		/// Default game mode that players are put in when they enter the level for the first time
		/// </summary>
		public GameMode Mode
		{
			get { return _mode; }
			set { _mode = value; }
		}

		/// <summary>
		/// Number of ticks that have passed since the world was created
		/// </summary>
		public long Age
		{
			get { return _gameTicks; }
			set { _gameTicks = value; }
		}

		/// <summary>
		/// Current time of day in the level in ticks
		/// </summary>
		public long Time
		{
			get { return _time; }
			set { _time = value; }
		}

		/// <summary>
		/// X-position of the point where players spawn at
		/// </summary>
		/// <remarks>This is a global position within the level/realm of a block where the player's head will be.</remarks>
		public int SpawnX
		{
			get { return _spawnX; }
			set { _spawnX = value; }
		}

		/// <summary>
		/// Y-position of the point where players spawn at
		/// </summary>
		/// <remarks>This is a global position within the level/realm of a block where the player's head will be.</remarks>
		public int SpawnY
		{
			get { return _spawnY; }
			set { _spawnY = value; }
		}

		/// <summary>
		/// Z-position of the point where players spawn at
		/// </summary>
		/// <remarks>This is a global position within the level/realm of a block where the player's head will be.</remarks>
		public int SpawnZ
		{
			get { return _spawnZ; }
			set { _spawnZ = value; }
		}

		/// <summary>
		/// Whether or not it's raining (snowing) currently in the world
		/// </summary>
		public bool Downfall
		{
			get { return _raining; }
			set { _raining = value; }
		}

		/// <summary>
		/// Amount of ticks until it starts or stops raining (snowing)
		/// </summary>
		/// <remarks>When this value hits 0, Downfall should be toggled.</remarks>
		public int DownfallTime
		{
			get { return _rainTime; }
			set { _rainTime = value; }
		}

		/// <summary>
		/// Whether or not a thunderstorm is active
		/// </summary>
		/// <remarks>Thunderstorms decrease lighting enough that hostile mobs will spawn in the day.</remarks>
		public bool IsThunderstorm
		{
			get { return _storm; }
			set { _storm = value; }
		}

		/// <summary>
		/// Amount of ticks until a thunderstorm starts or stops
		/// </summary>
		/// <remarks>When this value hits 0, IsThunderstorm should be toggled.</remarks>
		public int ThunderstormTime
		{
			get { return _stormTime; }
			set { _stormTime = value; }
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

			_name             = name;
			_generatorName    = generatorName;
			_generatorVersion = generatorVersion;
			_generatorOptions = generatorOptions ?? string.Empty;
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
			var node = ConstructNbtNode(RootNodeName);
			var tree = new Tree(node);
			tree.WriteToStream(bw);
		}

		/// <summary>
		/// Reads the contents of the level from a stream.
		/// </summary>
		/// <param name="br">Stream reader</param>
		/// <returns>Level data read from a stream</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="br"/> is null</exception>
		public static LevelData ReadFromStream (System.IO.BinaryReader br)
		{
			var tree = Tree.ReadFromStream(br);
			return new LevelData(tree.Root);
		}

		/// <summary>
		/// Constructs an NBT node from the level data.
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>An NBT node that contains the level data</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="name"/> is null</exception>
		public Node ConstructNbtNode (string name)
		{
			var root = new CompoundNode(name);
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
			var rootNode = validateLevelNode(node);
			validateVersionNode(rootNode); // It is important to check the version first

			// Required nodes
			_name             = validateLevelNameNode(rootNode);
			_seed             = validateSeedNode(rootNode);
			_generatorName    = validateGeneratorNameNode(rootNode);
			_generatorVersion = validateGeneratorVersionNode(rootNode);
			_generatorOptions = validateGeneratorOptionsNode(rootNode);

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
			return new StringNode(LevelNameNodeName, _name);
		}

		private Node constructSeedNode ()
		{
			return new LongNode(SeedNodeName, _seed);
		}

		private Node constructGeneratorNameNode ()
		{
			return new StringNode(GeneratorNameNodeName, _generatorName);
		}

		private Node constructGeneratorVersionNode ()
		{
			return new IntNode(GeneratorVersionNodeName, _generatorVersion);
		}

		private Node constructGeneratorOptionsNode ()
		{
			return new StringNode(GeneratorOptionsNodeName, _generatorOptions);
		}

		private Node constructInitializedNode ()
		{
			return new ByteNode(InitializedNodeName, _initialized);
		}

		private Node constructMapFeaturesNode ()
		{
			return new ByteNode(MapFeaturesNodeName, _mapFeatures);
		}

		private Node constructLastPlayedNode ()
		{
			return new LongNode(LastPlayedNodeName, _lastPlayed);
		}

		private Node constructDiskUsageNode ()
		{
			return new LongNode(DiskUsageNodeName, _size);
		}

		private Node constructCheatsNode ()
		{
			return new ByteNode(CheatsNodeName, _cheats);
		}

		private Node constructHardcoreNode ()
		{
			return new ByteNode(HardcoreNodeName, _hardcore);
		}

		private Node constructModeNode ()
		{
			return new IntNode(ModeNodeName, (int)_mode);
		}

		private Node constructAgeNode ()
		{
			return new LongNode(AgeNodeName, _gameTicks);
		}

		private Node constructTimeNode ()
		{
			return new LongNode(TimeNodeName, _time);
		}

		private Node constructSpawnXNode ()
		{
			return new IntNode(SpawnXNodeName, _spawnX);
		}

		private Node constructSpawnYNode ()
		{
			return new IntNode(SpawnYNodeName, _spawnY);
		}

		private Node constructSpawnZNode ()
		{
			return new IntNode(SpawnZNodeName, _spawnZ);
		}

		private Node constructDownfallNode ()
		{
			return new ByteNode(DownfallNodeName, _raining);
		}

		private Node constructDownfallTimeNode ()
		{
			return new IntNode(DownfallTimeNodeName, _rainTime);
		}

		private Node constructIsThunderstormNode ()
		{
			return new ByteNode(IsThunderstormNodeName, _storm);
		}

		private Node constructThunderstormTimeNode ()
		{
			return new IntNode(ThunderstormTimeNodeName, _stormTime);
		}
		#endregion

		#region Validation
		private static CompoundNode validateLevelNode (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The node that contains the level data can't be null.");
			if(node.Type != NodeType.Compound)
				throw new FormatException("The level NBT node must be a compound node.");
			return (CompoundNode)node;
		}

		private static void validateVersionNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(VersionNodeName))
				throw new FormatException("The level NBT does not contain a version node.");
			var tempNode = rootNode[VersionNodeName];
			if(tempNode.Type != NodeType.Int)
				throw new FormatException("The version node must be an integer node.");
			var version = ((IntNode)tempNode).Value;
			if(LevelVersion != version)
				throw new FormatException("The version of the level data is not supported. Expected: " + LevelVersion + " got: " + version);
		}

		private static string validateLevelNameNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(LevelNameNodeName))
				throw new FormatException("The level NBT does not contain a level name node.");
			var tempNode = rootNode[LevelNameNodeName];
			if(tempNode.Type != NodeType.String)
				throw new FormatException("The level name node must be a string node.");
			return ((StringNode)tempNode).Value;
		}

		private static long validateSeedNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(SeedNodeName))
				throw new FormatException("The level NBT does not contain a random seed node.");
			var tempNode = rootNode[SeedNodeName];
			if(tempNode.Type != NodeType.Long)
				throw new FormatException("The random seed node must be a long node.");
			return ((LongNode)tempNode).Value;
		}

		private static string validateGeneratorNameNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(GeneratorNameNodeName))
				throw new FormatException("The level NBT does not contain a generator name node.");
			var tempNode = rootNode[GeneratorNameNodeName];
			if(tempNode.Type != NodeType.String)
				throw new FormatException("The generator name name node must be a string node.");
			return ((StringNode)tempNode).Value;
		}

		private static int validateGeneratorVersionNode (CompoundNode rootNode)
		{
			if(!rootNode.Contains(GeneratorVersionNodeName))
				throw new FormatException("The level NBT does not contain a generator version node.");
			var tempNode = rootNode[GeneratorVersionNodeName];
			if(tempNode.Type != NodeType.Int)
				throw new FormatException("The generator version node must be an integer node.");
			return ((IntNode)tempNode).Value;
		}

		private static string validateGeneratorOptionsNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(GeneratorOptionsNodeName))
			{
				var tempNode = rootNode[GeneratorOptionsNodeName];
				if(tempNode.Type == NodeType.String)
					return ((StringNode)tempNode).Value;
			}
			return string.Empty;
		}

		private void validateInitializedNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(InitializedNodeName))
			{
				var tempNode = rootNode[InitializedNodeName];
				if(tempNode.Type == NodeType.Byte)
					_initialized = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateMapFeaturesNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(MapFeaturesNodeName))
			{
				Node tempNode = rootNode[MapFeaturesNodeName];
				if(tempNode.Type == NodeType.Byte)
					_mapFeatures = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateLastPlayedNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(LastPlayedNodeName))
			{
				var tempNode = rootNode[LastPlayedNodeName];
				if(tempNode.Type == NodeType.Long)
					_lastPlayed = ((LongNode)tempNode).Value;
			}
		}

		private void validateDiskUsageNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(DiskUsageNodeName))
			{
				var tempNode = rootNode[DiskUsageNodeName];
				if(tempNode.Type == NodeType.Long)
					_size = ((LongNode)tempNode).Value;
			}
		}

		private void validateCheatsNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(CheatsNodeName))
			{
				var tempNode = rootNode[CheatsNodeName];
				if(tempNode.Type == NodeType.Byte)
					_cheats = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateHardcoreNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(HardcoreNodeName))
			{
				var tempNode = rootNode[HardcoreNodeName];
				if(tempNode.Type == NodeType.Byte)
					_hardcore = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateModeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(ModeNodeName))
			{
				var tempNode = rootNode[ModeNodeName];
				if(tempNode.Type == NodeType.Int)
				{
					var value = ((IntNode)tempNode).Value;
					_mode = (GameMode)value;
				}
			}
		}

		private void validateAgeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(AgeNodeName))
			{
				var tempNode = rootNode[AgeNodeName];
				if(tempNode.Type == NodeType.Long)
					_gameTicks = ((LongNode)tempNode).Value;
			}
		}

		private void validateTimeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(TimeNodeName))
			{
				var tempNode = rootNode[TimeNodeName];
				if(tempNode.Type == NodeType.Long)
					_time = ((LongNode)tempNode).Value;
			}
		}

		private void validateSpawnXNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SpawnXNodeName))
			{
				var tempNode = rootNode[SpawnXNodeName];
				if(tempNode.Type == NodeType.Int)
					_spawnX = ((IntNode)tempNode).Value;
			}
		}

		private void validateSpawnYNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SpawnYNodeName))
			{
				var tempNode = rootNode[SpawnYNodeName];
				if(tempNode.Type == NodeType.Int)
					_spawnY = ((IntNode)tempNode).Value;
			}
		}

		private void validateSpawnZNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(SpawnZNodeName))
			{
				var tempNode = rootNode[SpawnZNodeName];
				if(tempNode.Type == NodeType.Int)
					_spawnZ = ((IntNode)tempNode).Value;
			}
		}

		private void validateDownfallNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(DownfallNodeName))
			{
				var tempNode = rootNode[DownfallNodeName];
				if(tempNode.Type == NodeType.Byte)
					_raining = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateDownfallTimeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(DownfallTimeNodeName))
			{
				var tempNode = rootNode[DownfallTimeNodeName];
				if(tempNode.Type == NodeType.Int)
					_rainTime = ((IntNode)tempNode).Value;
			}
		}

		private void validateIsThunderstormNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(IsThunderstormNodeName))
			{
				var tempNode = rootNode[IsThunderstormNodeName];
				if(tempNode.Type == NodeType.Byte)
					_storm = ((ByteNode)tempNode).BooleanValue;
			}
		}

		private void validateThunderstormTimeNode (CompoundNode rootNode)
		{
			if(rootNode.Contains(ThunderstormTimeNodeName))
			{
				var tempNode = rootNode[ThunderstormTimeNodeName];
				if(tempNode.Type == NodeType.Int)
					_stormTime = ((IntNode)tempNode).Value;
			}
		}
		#endregion
		#endregion
	}
}
