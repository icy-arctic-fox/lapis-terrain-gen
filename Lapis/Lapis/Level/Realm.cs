using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using Lapis.IO;
using Lapis.Level.Data;
using Lapis.Level.Generation;
using Lapis.Spatial;
using Lapis.Utility;
using P = System.IO.Path;

namespace Lapis.Level
{
	// TODO: Re-implement features in this class

	/// <summary>
	/// A realm within the world.
	/// Contains and manages chunks.
	/// </summary>
	/// <remarks>Realms can have a different ID numbers, which are used for directory names (i.e.: DIM1 and realm5).
	/// However, realms must have a vanilla dimension type.
	/// The dimension type is used to be compatible with vanilla Minecraft.</remarks>
	public sealed class Realm
	{
		private const string DimensionPrefix   = "DIM";
		private const string RealmPrefix       = "realm";
		private const string LevelDataFilename = "level.dat";
		private const string RegionDirectory   = "region";

		private static readonly string _directorySeparator = P.DirectorySeparatorChar.ToString(System.Globalization.CultureInfo.InvariantCulture);

		private readonly World _world;
		private readonly Dimension _dimension;
		private readonly int _id;
		private readonly LevelData _levelData;
		private readonly string _diskName, _path, _levelFilePath, _regionPath;

		#region Properties
		/// <summary>
		/// ID number of the realm
		/// </summary>
		/// <remarks>For vanilla Minecraft compatible realms, this will just be an integer value of a dimension.</remarks>
		public int Id
		{
			get { return _id; }
		}

		/// <summary>
		/// Type of dimension
		/// </summary>
		/// <remarks>This is used for compatibility with vanilla Minecraft.</remarks>
		public Dimension Dimension
		{
			get { return _dimension; }
		}

		/// <summary>
		/// Name of the realm directory on disk
		/// </summary>
		public string DiskName
		{
			get { return _diskName; }
		}

		/// <summary>
		/// Path to the realm directory on disk
		/// </summary>
		public string Path
		{
			get { return _path; }
		}

		/// <summary>
		/// World that the realm is in
		/// </summary>
		public World World
		{
			get { return _world; }
		}

		/// <summary>
		/// Whether or not the realm is the overworld (normal world)
		/// </summary>
		public bool IsOverworld
		{
			get { return (int)Dimension.Normal == _id; }
		}

		/// <summary>
		/// Whether or not the realm is the nether
		/// </summary>
		public bool IsNether
		{
			get { return (int)Dimension.Nether == _id; }
		}

		/// <summary>
		/// Whether or not the realm is "The End"
		/// </summary>
		public bool IsTheEnd
		{
			get { return (int)Dimension.End == _id; }
		}

		/// <summary>
		/// Access chunks within the realm
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		/// <returns>Chunk at the coordinates</returns>
		/// <remarks>If the chunk doesn't exist, it will be generated and populated</remarks>
		public Chunk this[int cx, int cz]
		{
			get { return GetChunk(cx, cz); }
		}

		/// <summary>
		/// Whether or not the realm is initialized (ready to be played on)
		/// </summary>
		public bool Initialized
		{
			get { return _levelData.Initialized; }
			set { _levelData.Initialized = value; }
		}
		#endregion

		private static bool isOverworld (int realmId)
		{
			return (int)Dimension.Normal == realmId;
		}

		private static string generatePath (string worldPath, int realmId)
		{
			string path;
			if(isOverworld(realmId))
				path = worldPath; // No special directory for the overworld
			else
			{// Sub-directory for the realm
				var diskName = DimensionPrefix + realmId;
				path = String.Join(_directorySeparator, worldPath, diskName);
			}
			return path;
		}

		private Realm (World world, int realmId, Dimension dimension, LevelData level, ITerrainGenerator generator)
		{
			if(null == world)
				throw new ArgumentNullException("world", "The world that the realm belongs to can't be null.");
			if(null == level)
				throw new ArgumentNullException("level", "The level data can't be null.");
			if(null == generator)
				throw new ArgumentNullException("generator", "The terrain generator can't be null.");

			_world     = world;
			_id        = realmId;
			_dimension = dimension;

			if(isOverworld(realmId))
			{// No special directory for the overworld
				_diskName = String.Empty;
				_path     = world.Path;
			}
			else
			{// Sub-directory for the realm
				_diskName = DimensionPrefix + realmId;
				_path     = String.Join(_directorySeparator, world.Path, _diskName);
			}
			_levelFilePath = String.Join(_directorySeparator, _path, LevelDataFilename);
			_regionPath    = String.Join(_directorySeparator, _path, RegionDirectory);

			_levelData = level;
			_generator = generator;

			if(!Directory.Exists(_regionPath)) // TODO: Move directory creation to Create()
				Directory.CreateDirectory(_regionPath);
			_afm = new AnvilFileManager(_regionPath);
		}

		#region Creation and loading
		private static readonly Random _rng = new Random();

		/// <summary>
		/// Creates a new realm
		/// </summary>
		/// <param name="world">World that the realm belongs to</param>
		/// <param name="generator">Terrain generator to use for the realm</param>
		/// <param name="dimension">Dimension type for the realm</param>
		/// <returns>A new realm</returns>
		/// <remarks>This realm will be compatible with vanilla Minecraft.</remarks>
		internal static Realm Create (World world, ITerrainGenerator generator, Dimension dimension)
		{
			return Create(world, generator, (int)dimension, dimension);
		}

		/// <summary>
		/// Creates a new custom realm
		/// </summary>
		/// <param name="world">World that the realm belongs to</param>
		/// <param name="generator">Terrain generator to use for the realm</param>
		/// <param name="realmId">ID number of the realm</param>
		/// <param name="dimension">Dimension type for the realm</param>
		/// <returns>A new realm</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="world"/> is null</exception>
		/// <remarks>A custom realm will not be detected by vanilla Minecraft.</remarks>
		internal static Realm Create (World world, ITerrainGenerator generator, int realmId, Dimension dimension = Dimension.Normal)
		{
			if(null == generator)
				throw new ArgumentNullException("generator", "The terrain generator can't be null.");

			var temp = new byte[sizeof(long)];
			_rng.NextBytes(temp);
			var seed  = temp.ToLong();
			var level = new LevelData(world.Name, seed, generator.Name, generator.Version, generator.GeneratorOptions);

			var realm = new Realm(world, realmId, dimension, level, generator);
			realm.Save();
			return realm;
		}

		/// <summary>
		/// Creates a new custom realm
		/// </summary>
		/// <param name="world">World that the realm belongs to</param>
		/// <param name="seed">Generator seed</param>
		/// <param name="generator">Terrain generator to use for the realm</param>
		/// <param name="realmId">ID number of the realm</param>
		/// <param name="dimension">Dimension type for the realm</param>
		/// <returns>A new realm</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="world"/> is null</exception>
		/// <remarks>A custom realm will not be detected by vanilla Minecraft.</remarks>
		internal static Realm Create (World world, int realmId, long seed, ITerrainGenerator generator, Dimension dimension = Dimension.Normal)
		{
			if(null == generator)
				throw new ArgumentNullException("generator", "The terrain generator can't be null.");

			var level = new LevelData(world.Name, seed, generator.Name, generator.Version, generator.GeneratorOptions);

			var realm = new Realm(world, realmId, dimension, level, generator);
			realm.Save();
			return realm;
		}

		/// <summary>
		/// Forces the realm and the chunks in it to save
		/// </summary>
		public void Save ()
		{
			if(!Directory.Exists(_regionPath))
				Directory.CreateDirectory(_regionPath);

			// TODO: Save all chunks

			saveLevelData(_levelFilePath, _levelData);
		}

		/// <summary>
		/// Loads a realm from disk
		/// </summary>
		/// <param name="world">World that the realm belongs to</param>
		/// <param name="realmId">ID number of the realm</param>
		/// <returns>The loaded realm</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="world"/> is null</exception>
		internal static Realm Load (World world, int realmId)
		{
			var path          = generatePath(world.Path, realmId);
			var levelFilePath = String.Join(_directorySeparator, path, LevelDataFilename);
			var level = loadLevelData(levelFilePath);
//			return new Realm(world, realmId, dimension, level, generator);
			return null; // TODO: Implement
		}

		private static void saveLevelData (string path, LevelData data)
		{
			using(var fs = new FileStream(path, FileMode.Create))
			using(var compressor = new GZipStream(fs, CompressionMode.Compress))
			using(var bw = new EndianBinaryWriter(compressor, Endian.Big))
				data.WriteToStream(bw);
		}

		private static LevelData loadLevelData (string path)
		{
			using(var fs = new FileStream(path, FileMode.Open))
			using(var decompressor = new GZipStream(fs, CompressionMode.Decompress))
			using(var br = new EndianBinaryReader(decompressor, Endian.Big))
				return LevelData.ReadFromStream(br);
		}
		#endregion

		#region Chunk management
		private readonly AnvilFileManager _afm;
		private readonly ITerrainGenerator _generator;

		/// <summary>
		/// Contains all active chunks
		/// </summary>
		/// <remarks>Chunks will not be disposed when removed from the cache.
		/// This is because they may be in use elsewhere still and we don't want an ObjectDisposedException to be thrown.
		/// The garbage collector will take care of those chunks as soon as they are released.</remarks>
		private readonly Cache<XZCoordinate, Chunk> _chunkCache = new Cache<XZCoordinate, Chunk>(false);

		/// <summary>
		/// Maintains a list of active chunks
		/// </summary>
		/// <remarks>Active chunks are the chunks that are referenced somewhere in the program.
		/// After all references (not ChunkRefs) have been removed, garbage collection will cleanup the chunk.
		/// At that point, the weak reference to the chunk is removed from this collection.
		/// This collection is needed because the same Chunk object has to be retrieved for multiple GetChunk() calls.</remarks>
		private readonly Dictionary<XZCoordinate, WeakReference> _activeChunks = new Dictionary<XZCoordinate, WeakReference>();

		// Both the _chunkCache and _activeChunks collections are needed.
		// The _chunkCache collection prevents chunks from being unloaded when they might be referenced again soon.
		// The _activeChunks collection tracks chunks that are actively being held in memory.
		// They are used together to prevent early disposal of chunks that are still active (but not in the cache),
		// and to serve the same chunk object if it's already in memory.

		/// <summary>
		/// Retrieves a chunk from the realm
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		/// <returns>Chunk at the coordinates</returns>
		/// <remarks>If the chunk doesn't exist, it will be generated and populated</remarks>
		public Chunk GetChunk (int cx, int cz)
		{
			var coord = new XZCoordinate(cx, cz);
			lock(_activeChunks)
				return _chunkCache.GetItem(coord, getOrCreateChunk);
		}

		/// <summary>
		/// Saves chunk data
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		/// <param name="data">Chunk data to save at the position</param>
		internal void SaveChunk (int cx, int cz, ChunkData data)
		{
			_afm.PutChunk(cx, cz, data);
		}

		/// <summary>
		/// Removes a chunk from being considered active
		/// </summary>
		/// <param name="cx"></param>
		/// <param name="cz"></param>
		/// <remarks>This method should ONLY be called from within a Chunk's Dispose() method.</remarks>
		internal void FreeChunk (int cx, int cz)
		{
			var coord = new XZCoordinate(cx, cz);
			lock(_activeChunks)
				_activeChunks.Remove(coord);
		}

		private Chunk getOrCreateChunk (XZCoordinate coord)
		{
			ChunkData data = null;
			if(_afm.ChunkExists(coord.X, coord.Z))
				data = _afm.GetChunk(coord.X, coord.Z);
			if(null == data) // The chunk provider might return null for a chunk
				data = generate(coord.X, coord.Z);

			if(!data.TerrainPopulated)
				data.TerrainPopulated = true; // TODO: Populate the chunk

			return new Chunk(this, data);
		}

		private ChunkData generate (int cx, int cz)
		{
			var data = _generator.GenerateChunk(cx, cz);
			if(null == data)
				throw new ApplicationException("The chunk data can't be null.");
			_afm.PutChunk(cx, cz, data);
			return data;
		}

		/// <summary>
		/// Forces a chunk to generate
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		/// <param name="replace">Whether or not the chunk should be re-generated.
		/// When true, the chunk will be recreated from scratch - all modifications will be lost.
		/// When false, the chunk won't be touched if it is already generated (default).</param>
		/// <remarks>This method won't populate the chunk after it has been generated.</remarks>
		public void GenerateChunk (int cx, int cz, bool replace = false)
		{
			bool create;
			lock(_activeChunks)
				create = (!_afm.ChunkExists(cx, cz) || replace);

			if(create)
			{
				var data = generate(cx, cz);
				lock(_activeChunks)
				{
					// TODO: Add chunk to activeChunks
				}
			}
		}

		/// <summary>
		/// Forces a chunk to populate
		/// </summary>
		/// <param name="cx">X-position of the chunk within the realm</param>
		/// <param name="cz">Z-position of the chunk within the realm</param>
		/// <param name="repopulate">Whether or not the chunk should populate again (even if it already is populated).
		/// When true, the chunk will be populated (this may cause extra amounts of trees, ores, and etc. if the chunk is already populated).
		/// When false, the chunk will populate only if it wasn't populated before (default).</param>
		public void PopulateChunk (int cx, int cz, bool repopulate = false)
		{
			throw new NotImplementedException();
		}
		#endregion

		/// <summary>
		/// Finds all directories that appear to contain realms (dimensions)
		/// </summary>
		/// <param name="path">World path</param>
		/// <returns>A collection of realm ID numbers</returns>
		/// <remarks>It is assumed that <paramref name="path"/> is a valid directory.
		/// If it is not valid, no realms will be found.</remarks>
		internal static IEnumerable<int> DiscoverRealms (string path)
		{
			var realmIds = new HashSet<int>();

			if(Directory.Exists(path))
			{
				var levelDataPath = string.Join(_directorySeparator, path, LevelDataFilename);
				if(File.Exists(levelDataPath)) // Overworld found
					realmIds.Add((int)Dimension.Normal);

				foreach(var directoryName in Directory.EnumerateDirectories(path))
				{// Iterate through each directory
					levelDataPath = string.Join(_directorySeparator, path, directoryName, LevelDataFilename);
					if(File.Exists(levelDataPath))
					{// It contains a level.dat file
						if(directoryName.Length > DimensionPrefix.Length && directoryName.Substring(0, DimensionPrefix.Length) == DimensionPrefix)
						{// Vanilla Minecraft dimension directory
							var numberPart = directoryName.Substring(DimensionPrefix.Length);
							int realmId;
							if(int.TryParse(numberPart, out realmId))
								realmIds.Add(realmId);
						}
						else if(directoryName.Length > RealmPrefix.Length && directoryName.Substring(0, RealmPrefix.Length) == RealmPrefix)
						{// Custom realm directory
							var numberPart = directoryName.Substring(RealmPrefix.Length);
							int realmId;
							if(int.TryParse(numberPart, out realmId))
								realmIds.Add(realmId);
						}
					}
				}
			}

			return realmIds;
		}
	}
}
