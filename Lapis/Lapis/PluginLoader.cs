using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Lapis.Utility;

namespace Lapis
{
	/// <summary>
	/// Loads, activates, and manages plug-ins
	/// </summary>
	/// <remarks>This class loads .dll files in the plug-ins directory.
	/// Access to the types and plug-ins is available from methods in this class.
	/// Due to limitations in the .NET framework, it is not possible to unload plug-ins without incurring performance issues.
	/// Once a plug-in has been loaded, it cannot be unloaded or reloaded - the program will have to be restarted.</remarks>
	/// <seealso cref="IPlugin"/>
	public static class PluginLoader
	{
		private static readonly object _locker = new object();

		/// <summary>
		/// Types of known and loaded plug-ins
		/// </summary>
		/// <remarks>Key: Type name, Value: Type</remarks>
		private static readonly Dictionary<string, Type> _knownPlugins = new Dictionary<string, Type>();

		/// <summary>
		/// Types and versions of known and loaded plug-ins
		/// </summary>
		/// <remarks>Key: PluginName, Value: List of PluginVersion and Type</remarks>
		private static readonly Dictionary<string, List<Tuple<int, Type>>> _knownVersions = new Dictionary<string, List<Tuple<int, Type>>>();

		/// <summary>
		/// Plug-ins that are currently active
		/// </summary>
		/// <remarks>Key: Type name, Value: Plug-in instance</remarks>
		private static readonly Dictionary<string, IPlugin> _activePlugins = new Dictionary<string, IPlugin>();

		/// <summary>
		/// Plug-ins that are currently active identified by name and type
		/// </summary>
		/// <remarks>Key: PluginName, Value List of PluginVersion and Type</remarks>
		private static readonly Dictionary<string, List<Tuple<int, IPlugin>>> _activeVersions = new Dictionary<string, List<Tuple<int, IPlugin>>>();

		#region Plug-in directory
		private const string PluginDirectoryName = "plugins";
		private static readonly string _fullPluginPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + PluginDirectoryName;

		/// <summary>
		/// Absolute path to the plug-in directory
		/// </summary>
		public static string PluginDirectory
		{
			get { return _fullPluginPath; }
		}
		#endregion

		#region Assembly loading and management
		private const string PluginFileExtension = "*.dll";

		private static readonly Type _pluginType = typeof(IPlugin);

		/// <summary>
		/// Assemblies that have been loaded
		/// </summary>
		private static readonly Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();

		/// <summary>
		/// Triggered when there was an error loading a plug-in
		/// </summary>
		public static event EventHandler<PluginLoadErrorEventArgs> LoadError;

		/// <summary>
		/// Loads all of the plug-ins in the default plug-in directory
		/// </summary>
		public static void LoadPlugins ()
		{
			LoadPlugins(PluginDirectory);
		}

		/// <summary>
		/// Loads all of the plug-ins in a directory
		/// </summary>
		/// <param name="path">Path of the directory to load from</param>
		/// <param name="recurse">If true, loads plug-ins from sub-directories</param>
		public static void LoadPlugins (string path, bool recurse = false)
		{
			if(null == path)
				throw new ArgumentNullException("path", "The path to load from can't be null.");

			var fullPath = Path.GetFullPath(path);
			if(Directory.Exists(fullPath))
			{
				var searchOption = recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
				lock(_locker)
					foreach(var filepath in Directory.EnumerateFiles(path, PluginFileExtension, searchOption))
						loadAssembly(filepath);
			}
		}

		private static void loadAssembly (string filepath)
		{
			if(!_loadedAssemblies.ContainsKey(filepath))
			{// Don't reload
				Assembly asm;

				try
				{// Attempt to load the assembly
					asm = Assembly.LoadFrom(filepath);
					_loadedAssemblies.Add(filepath, asm);
				}
				catch(Exception e)
				{// Loading failed, report the error
					var args = new PluginLoadErrorEventArgs(filepath, null, e);
					LoadError.TriggerEvent(null, args);
					return;
				}
				discoverPlugins(asm);
			}
		}

		private static void discoverPlugins (Assembly asm)
		{
			foreach(var type in asm.GetTypes())
			{// Look for plug-in types
				if(type.IsAssignableFrom(_pluginType) && type.IsClass && !type.IsAbstract)
				{
					var typeName = type.FullName;
					try
					{// Attempt to create and start the plug-in
						if(_knownPlugins.ContainsKey(typeName))
							throw new Exception("Name conflict - the class " + typeName + " already exists.");

						string pluginName;
						int pluginVer;
						var plugin = activatePlugin(type, out pluginName, out pluginVer);
						if(null != plugin)
						{
							// Store information about the plug-in.
							// Add it after we initialize so we don't reactivate bad plug-ins later.
							List<Tuple<int, Type>> typeVersions;
							if(_knownVersions.ContainsKey(pluginName))
								typeVersions = _knownVersions[pluginName];
							else
								_knownVersions[pluginName] = typeVersions = new List<Tuple<int, Type>>();
							typeVersions.Add(new Tuple<int, Type>(pluginVer, type));

							_knownPlugins.Add(typeName, type);
						}
					}
					catch(Exception e)
					{
						var args = new PluginLoadErrorEventArgs(asm.Location, type.FullName, e);
						LoadError.TriggerEvent(null, args);
					}
				}
			}
		}
		#endregion

		#region Plug-in management
		/// <summary>
		/// Collection of all loaded plug-ins
		/// </summary>
		/// <remarks>The array contains a list of full class names of plug-ins.</remarks>
		public static string[] KnownPlugins
		{
			get
			{
				lock(_locker)
					return _knownPlugins.Keys.ToArray();
			}
		}

		/// <summary>
		/// Collection of all loaded plug-ins and their versions
		/// </summary>
		/// <remarks>The array contains a list of plug-in names (from the PluginName property) and their version (from the PluginVersion) property.</remarks>
		/// <seealso cref="IPlugin.PluginName"/>
		/// <seealso cref="IPlugin.PluginVersion"/>
		public static Tuple<string, int>[] PluginVersions
		{
			get
			{
				lock(_locker)
				{
					var tuples = new Tuple<string, int>[_knownVersions.Count];
					var i = 0;
					foreach(var entry in _knownVersions)
					{
						var versionList = entry.Value;
						foreach(var tuple in versionList)
							tuples[i++] = new Tuple<string, int>(entry.Key, tuple.Item1);
					}
					return tuples;
				}
			}
		}

		#region Activation
		/// <summary>
		/// Collection of active plug-ins
		/// </summary>
		public static IPlugin[] ActivePlugins
		{
			get
			{
				lock(_locker)
					return _activePlugins.Values.ToArray();
			}
		}

		/// <summary>
		/// Activates a plug-in
		/// </summary>
		/// <param name="pluginName">Name of the plug-in to activate (according to the PluginName property)</param>
		/// <returns>True if the plug-in was activated successfully or false if it failed or is already active</returns>
		/// <remarks>This method will activate the latest version of the plug-in.
		/// If the plug-in throws an exception while starting, it will not be caught by this method.</remarks>
		/// <seealso cref="IPlugin.PluginName"/>
		public static bool ActivatePlugin (string pluginName)
		{
			lock(_locker)
			{
				List<Tuple<int, Type>> list;
				if(_knownVersions.TryGetValue(pluginName, out list))
				{
					var latestType = findLatestVersion(list);
					return activatePlugin(latestType);
				}
			}
			return false;
		}

		/// <summary>
		/// Activates a plug-in
		/// </summary>
		/// <param name="pluginName">Name of the plug-in to activate (according to the PluginName property)</param>
		/// <param name="version">Version of the plug-in to activate (according to the PluginVersion property)</param>
		/// <returns>True if the plug-in was activated successfully or false if it failed or is already active</returns>
		/// <remarks>If the plug-in throws an exception while starting, it will not be caught by this method.</remarks>
		/// <seealso cref="IPlugin.PluginName"/>
		/// <seealso cref="IPlugin.PluginVersion"/>
		public static bool ActivatePlugin (string pluginName, int version)
		{
			lock(_locker)
			{
				List<Tuple<int, Type>> list;
				if(_knownVersions.TryGetValue(pluginName, out list))
				{
					var pluginType = findVersion(list, version);
					if(null != pluginType)
						return activatePlugin(pluginType);
				}
			}
			return false;
		}

		/// <summary>
		/// Activates a plug-in
		/// </summary>
		/// <param name="className">Full class name of the plug-in to activate</param>
		/// <returns>True if the plug-in was activated successfully or false if it failed or is already active</returns>
		/// <remarks>If the plug-in throws an exception while starting, it will not be caught by this method.</remarks>
		public static bool ActivatePluginClass (string className)
		{
			lock(_locker)
			{
				Type pluginType;
				if(_knownPlugins.TryGetValue(className, out pluginType))
					return activatePlugin(pluginType);
			}
			return false;
		}

		private static bool activatePlugin (Type pluginType)
		{
			string pluginName;
			int pluginVer;
			return null != activatePlugin(pluginType, out pluginName, out pluginVer);
		}

		private static IPlugin activatePlugin (Type pluginType, out string pluginName, out int pluginVer)
		{
			var typeName = pluginType.FullName;
			if(!_activePlugins.ContainsKey(typeName))
			{
				var plugin = (IPlugin)Activator.CreateInstance(pluginType);
				plugin.StartPlugin();

				pluginName = plugin.PluginName;
				pluginVer  = plugin.PluginVersion;

				// Store information about the plug-in.
				// Add it after we initialize so we don't reactivate bad plug-ins later.
				List<Tuple<int, IPlugin>> activePluginVersions;
				if(_activeVersions.ContainsKey(pluginName))
					activePluginVersions = _activeVersions[pluginName];
				else
					_activeVersions[pluginName] = activePluginVersions = new List<Tuple<int, IPlugin>>();
				activePluginVersions.Add(new Tuple<int, IPlugin>(pluginVer, plugin));
				_activePlugins.Add(typeName, plugin);

				return plugin;
			}

			pluginName = null;
			pluginVer  = 0;
			return null;
		}
		#endregion

		#region Deactivation
		/// <summary>
		/// Deactivates a plug-in
		/// </summary>
		/// <param name="pluginName">Name of the plug-in to deactivate (according to the PluginName property)</param>
		/// <returns>True if the plug-in was active and has been deactivated, false otherwise</returns>
		/// <remarks>This method will deactivate the latest active version of the plug-in.</remarks>
		/// <seealso cref="IPlugin.PluginName"/>
		public static bool DeactivatePlugin (string pluginName)
		{
			lock(_locker)
			{
				List<Tuple<int, IPlugin>> list;
				if(_activeVersions.TryGetValue(pluginName, out list))
				{
					var plugin = findLatestVersion(list);
					return deactivatePlugin(plugin);
				}
			}
			return false;
		}

		/// <summary>
		/// Deactivates a plug-in
		/// </summary>
		/// <param name="pluginName">Name of the plug-in to deactivate (according to the PluginName property)</param>
		/// <param name="version">Version of the plug-in to deactivate (according to the PluginVersion property)</param>
		/// <returns>True if the plug-in was active and has been deactivated, false otherwise</returns>
		/// <seealso cref="IPlugin.PluginName"/>
		/// <seealso cref="IPlugin.PluginVersion"/>
		public static bool DeactivatePlugin (string pluginName, int version)
		{
			lock(_locker)
			{
				List<Tuple<int, IPlugin>> list;
				if(_activeVersions.TryGetValue(pluginName, out list))
				{
					var plugin = findVersion(list, version);
					if(null != plugin)
						return deactivatePlugin(plugin);
				}
			}
			return false;
		}

		/// <summary>
		/// Deactivates a plug-in
		/// </summary>
		/// <param name="className">Full class name of the plug-in to deactivate</param>
		/// <returns>True if the plug-in was active and has been deactivated, false otherwise</returns>
		public static bool DeactivatePluginClass (string className)
		{
			lock(_locker)
			{
				IPlugin plugin;
				if(_activePlugins.TryGetValue(className, out plugin))
					return deactivatePlugin(plugin);
			}
			return false;
		}

		private static bool deactivatePlugin (IPlugin plugin)
		{
			var pluginType = plugin.GetType();
			var typeName   = pluginType.FullName;
			if(_activePlugins.ContainsKey(typeName))
			{
				var pluginName = plugin.PluginName;
				var pluginVer  = plugin.PluginVersion;

				// Remove from active list first even if it fails to disable later
				var activePluginVersions = _activeVersions[pluginName];
				for(var i = 0; i < activePluginVersions.Count; ++i)
				{
					var tuple = activePluginVersions[i];
					if(pluginVer == tuple.Item1 && plugin == tuple.Item2)
						activePluginVersions.RemoveAt(i--);
				}
				if(activePluginVersions.Count <= 0)
					_activeVersions.Remove(pluginName);
				_activePlugins.Remove(typeName);

				plugin.StopPlugin();
				GC.Collect();

				return true;
			}
			return false;
		}
		#endregion

		/// <summary>
		/// Gets the instance of a plug-in
		/// </summary>
		/// <param name="pluginName">Name of the plug-in (according to the PluginName property)</param>
		/// <returns>The instance of a plug-in or null if the plug-in wasn't found or is inactive</returns>
		/// <remarks>This method will get the latest version available for the plug-in.</remarks>
		/// <seealso cref="IPlugin.PluginName"/>
		public static IPlugin GetPlugin (string pluginName)
		{
			lock(_locker)
			{
				List<Tuple<int, IPlugin>> list;
				if(_activeVersions.TryGetValue(pluginName, out list))
					return findLatestVersion(list);
			}
			return null;
		}

		/// <summary>
		/// Gets the instance of a plug-in
		/// </summary>
		/// <param name="pluginName">Name of the plug-in (according to the PluginName property)</param>
		/// <param name="version">Version of the plug-in (according to the PluginVersion property)</param>
		/// <returns>The instance of a plug-in or null if the plug-in wasn't found or is inactive</returns>
		/// <seealso cref="IPlugin.PluginName"/>
		/// <seealso cref="IPlugin.PluginVersion"/>
		public static IPlugin GetPlugin (string pluginName, int version)
		{
			lock(_locker)
			{
				List<Tuple<int, IPlugin>> list;
				if(_activeVersions.TryGetValue(pluginName, out list))
					return findVersion(list, version);
			}
			return null;
		}

		/// <summary>
		/// Gets the instance of a plug-in
		/// </summary>
		/// <param name="className">Full class name of the plug-in</param>
		/// <returns>The instance of the plug-in or null if the plug-in wasn't found or is inactive</returns>
		public static IPlugin GetPluginClass (string className)
		{
			lock(_locker)
			{
				IPlugin plugin;
				if(_activePlugins.TryGetValue(className, out plugin))
					return plugin;
			}
			return null;
		}

		private static T findLatestVersion<T> (IEnumerable<Tuple<int, T>> list)
		{
			var max   = Int32.MinValue;
			var found = default(T);
			foreach(var item in list)
			{
				var version = item.Item1;
				if(version > max)
				{
					max = version;
					found = item.Item2;
				}
			}
			return found;
		}

		private static T findVersion<T> (IEnumerable<Tuple<int, T>> list, int version)
		{
			foreach(var item in list)
			{
				var pluginVersion = item.Item1;
				if(pluginVersion == version)
					return item.Item2;
			}
			return default(T);
		}
		#endregion
	}
}
