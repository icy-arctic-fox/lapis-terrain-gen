using System;
using System.Collections.Generic;
using System.IO;
using Lapis.Utility;

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
		private readonly Dictionary<int, Realm> _realms;

		#region Properties
		/// <summary>
		/// Name of the world
		/// </summary>
		public string Name
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Name of the world directory on disk
		/// </summary>
		public string DiskName
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Path to the world directory on disk
		/// </summary>
		public string Path
		{
			get { throw new NotImplementedException(); }
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
			throw new NotImplementedException();
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
		/// ID numbers of realms present in the world
		/// </summary>
		public IEnumerable<int> RealmIds
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Collection of realms in the world
		/// </summary>
		public IEnumerable<Realm> Realms
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Access to realms contained in the world
		/// </summary>
		/// <param name="realmId">ID number of the realm</param>
		/// <returns>A realm</returns>
		public Realm this[int realmId]
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Access to realms contained in the world
		/// </summary>
		/// <param name="dimension">Dimension (realm) to access</param>
		/// <returns>A realm</returns>
		public Realm this[Dimension dimension]
		{
			get { return this[(int)dimension]; }
		}
		#endregion

		#region World management
		private static readonly Dictionary<string, World> _loadedWorlds;

		/// <summary>
		/// Unloads the current world from memory
		/// </summary>
		public void Unload ()
		{
			throw new NotImplementedException();
		}

		#region World creation and loading
		/// <summary>
		/// Creates a new world on disk
		/// </summary>
		/// <param name="name">Name of the world</param>
		/// <returns>The created world</returns>
		/// <remarks>The world name may be changed slightly so that it is safe to use in a file system.</remarks>
		public static World Create (string name)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		#endregion
		#endregion
	}
}
