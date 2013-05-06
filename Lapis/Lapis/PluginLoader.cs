using System;
using System.Collections.Generic;
using System.IO;
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
	public static class PluginLoader
	{
		private static readonly object _locker = new object();

		/// <summary>
		/// Types of known and loaded plug-ins
		/// </summary>
		private static readonly Dictionary<string, Type> _knownPlugins = new Dictionary<string, Type>();

		/// <summary>
		/// Plug-ins that are currently active
		/// </summary>
		private static readonly Dictionary<string, IPlugin> _activePlugins = new Dictionary<string, IPlugin>();

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
				{
					foreach(var filepath in Directory.EnumerateFiles(path, PluginFileExtension, searchOption))
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
								continue;
							}

							foreach(var type in asm.GetTypes())
							{// Look for plug-in types
								if(type.IsAssignableFrom(_pluginType) && type.IsClass && !type.IsAbstract)
								{
									var typeName = type.FullName;
									try
									{// Attempt to create and start the plug-in
										var plugin = Activator.CreateInstance(type) as IPlugin;
										// TODO: Call StartPlugin()
										if(_knownPlugins.ContainsKey(typeName))
											throw new Exception("Name conflict - the class " + typeName + " already exists.");
										_knownPlugins.Add(typeName, type); // Add after we initialize so we don't reactivate bad plug-ins later
										_activePlugins.Add(typeName, plugin);
									}
									catch(Exception e)
									{
										var args = new PluginLoadErrorEventArgs(filepath, type.FullName, e);
										LoadError.TriggerEvent(null, args);
									}
								}
							}
						}
					}
				}
			}
		}
		#endregion

		#region Plug-in management
		/// <summary>
		/// Collection of all loaded plug-ins
		/// </summary>
		public static IPlugin[] Plugins
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Collection of active plug-ins
		/// </summary>
		public static IPlugin[] ActivePlugins
		{
			get { throw new NotImplementedException(); }
		}

		public static void ActivatePlugin (string pluginName)
		{
			throw new NotImplementedException();
		}

		public static void DeactivatePlugin (string pluginName)
		{
			throw new NotImplementedException();
		}

		public static void GetPlugin (string pluginName)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
