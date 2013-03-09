using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Lapis.Level.Generation;
using Lapis.Utility;
using P = System.IO.Path;

namespace Lapis.Level
{
	/// <summary>
	/// A single world in Minecraft.
	/// May contains multiple realms.
	/// </summary>
	public sealed class World
	{
		private readonly string _name, _diskName, _path;

		/// <summary>
		/// All realms that the world contains (knows about)
		/// </summary>
		/// <remarks>The values in the dictionary will be null if the realm isn't loaded.</remarks>
		private readonly Dictionary<int, Realm> _realms = new Dictionary<int,Realm>();

		#region Properties
		/// <summary>
		/// Name of the world
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Name of the world directory on disk
		/// </summary>
		public string DiskName
		{
			get { return _diskName; }
		}

		/// <summary>
		/// Path to the world directory on disk
		/// </summary>
		public string Path
		{
			get { return _path; }
		}
		#endregion

		#region Realms
		/// <summary>
		/// Checks if the world has a realm
		/// </summary>
		/// <param name="realmId">ID number of the realm to look for</param>
		/// <returns>True if the the realm is in the world or false if it isn't</returns>
		public bool ContainsRealm (int realmId)
		{
			lock(_realms)
				return _realms.ContainsKey(realmId);
		}

		/// <summary>
		/// Checks if the world has a realm
		/// </summary>
		/// <param name="dimension">Dimension to look for</param>
		/// <returns>True if the the realm is in the world or false if it isn't</returns>
		public bool ContainsRealm (Dimension dimension)
		{
			lock(_realms)
				return _realms.ContainsKey((int)dimension);
		}

		/// <summary>
		/// Loads a realm into memory (if it isn't already)
		/// </summary>
		/// <param name="realmId">ID number of the realm</param>
		/// <returns>The loaded realm</returns>
		public Realm LoadRealm (int realmId)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new realm in the world and adds it to the world
		/// </summary>
		/// <param name="generator">Terrain generator to use for the realm</param>
		/// <param name="dimension">Dimension type for the realm</param>
		/// <returns>Newly created realm</returns>
		/// <remarks>Warning: Creating a realm for a dimension that already exists will overwrite the existing realm.</remarks>
		public Realm CreateRealm (ITerrainGenerator generator, Dimension dimension = Dimension.Normal)
		{
			var realm = Realm.Create(this, generator, dimension);
			lock(_realms)
				_realms[realm.Id] = realm;
			return realm;
		}

		/// <summary>
		/// Creates a new realm in the world and adds it to the world
		/// </summary>
		/// <param name="generator">Terrain generator to use for the realm</param>
		/// <param name="realmId">ID number of the realm</param>
		/// <param name="dimension">Dimension type for the realm</param>
		/// <returns>Newly created realm</returns>
		/// <remarks>Warning: Creating a realm with an ID that already exists will overwrite the existing realm.</remarks>
		public Realm CreateRealm (ITerrainGenerator generator, int realmId, Dimension dimension = Dimension.Normal)
		{
			var realm = Realm.Create(this, generator, realmId, dimension);
			lock(_realms)
				_realms[realm.Id] = realm;
			return realm;
		}

		/// <summary>
		/// ID numbers of realms present in the world
		/// </summary>
		public IEnumerable<int> RealmIds
		{
			get
			{
				int[] ids;
				lock(_realms)
				{
					ids   = new int[_realms.Count];
					var i = 0;
					foreach(var id in _realms.Keys)
						ids[i++] = id;
				}
				return ids;
			}
		}

		/// <summary>
		/// Collection of realms in the world
		/// </summary>
		public IEnumerable<Realm> Realms
		{
			get
			{
				lock(_realms)
					return _realms.Values;
			}
		}

		/// <summary>
		/// Access to realms contained in the world
		/// </summary>
		/// <param name="realmId">ID number of the realm</param>
		/// <returns>A realm</returns>
		/// <exception cref="KeyNotFoundException">Thrown if the realm specified by <paramref name="realmId"/> does not exist</exception>
		public Realm this[int realmId]
		{
			get
			{
				lock(_realms)
					return _realms[realmId];
			}
		}

		/// <summary>
		/// Access to realms contained in the world
		/// </summary>
		/// <param name="dimension">Dimension (realm) to access</param>
		/// <returns>A realm</returns>
		/// <exception cref="KeyNotFoundException">Thrown if the realm specified by <paramref name="dimension"/> does not exist</exception>
		public Realm this[Dimension dimension]
		{
			get
			{
				lock(_realms)
					return _realms[(int)dimension];
			}
		}
		#endregion

		#region World management
		private const string SafeReplacement = "_";
		private static readonly Regex _diskNameRegex = new Regex(@"\W+");

		private static readonly Dictionary<string, World> _loadedWorlds;

		/// <summary>
		/// Unloads the current world from memory
		/// </summary>
		public void Unload ()
		{
			throw new NotImplementedException();
		}

		private static string generateDiskName (string name, bool newWorld)
		{
			var diskName = _diskNameRegex.Replace(name, SafeReplacement);
			if(newWorld && Directory.Exists(diskName))
			{
				var i = 1;
				for(; Directory.Exists(diskName + i); ++i) { }
				diskName = diskName + i;
			}
			return diskName;
		}

		#region World creation and loading
		private World (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the world can't be null.");

			_name     = name;
			_diskName = generateDiskName(name, true);
			_path     = P.GetFullPath(_diskName);
		}

		/// <summary>
		/// Creates a new world on disk
		/// </summary>
		/// <param name="name">Name of the world</param>
		/// <returns>The created world</returns>
		/// <remarks>The world name may be changed slightly so that it is safe to use in a file system.
		/// This method will not generate any realms.</remarks>
		public static World Create (string name)
		{
			var world = new World(name);
			world.Save();
			return world;
		}

		/// <summary>
		/// Loads an existing world from disk
		/// </summary>
		/// <param name="name">Name of the world</param>
		/// <returns>The loaded world</returns>
		/// <remarks>Use the original world name for <paramref name="name"/> that was used in Create(), not the disk name.</remarks>
		public static World Load (string name)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Forces all realms and chunks to save
		/// </summary>
		public void Save ()
		{
			if(!Directory.Exists(_path))
				Directory.CreateDirectory(_path);
			foreach(var realm in _realms.Values)
				realm.Save();
		}
		#endregion
		#endregion
	}
}
