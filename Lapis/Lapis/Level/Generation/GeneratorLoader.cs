using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Manages terrain generators and chunk populators
	/// </summary>
	/// <remarks>Use this class to get available generators.</remarks>
	public static class GeneratorLoader
	{
		private static readonly Dictionary<string, List<Tuple<int, Type>>> _terrainGeneratorTypes = new Dictionary<string, List<Tuple<int, Type>>>();
		private static readonly Type _terrainGeneratorType = typeof(ITerrainGenerator);

		/// <summary>
		/// Names of the terrain generators available
		/// </summary>
		public static string[] GeneratorNames
		{
			get
			{
				lock(_terrainGeneratorTypes)
					return _terrainGeneratorTypes.Keys.ToArray();
			}
		}

		#region Discovery
		private const string GeneratorDir  = "generators";
		private const string FileExtension = "*.dll";

		/// <summary>
		/// Runs on first reference to load available generators
		/// </summary>
		static GeneratorLoader ()
		{
			lock(_terrainGeneratorTypes)
			{
				discoverGenerators();
				// TODO: Add chunk populator loading
			}
		}

		private static void discoverGenerators ()
		{
			loadGenerators(GeneratorDir);
			var types = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany(asm => asm.GetTypes()
									.Where(isSuitableTerrainGenerator));

			_terrainGeneratorTypes.Clear();
			try
			{
				foreach(var type in types)
				{
					var instance = (ITerrainGenerator)Activator.CreateInstance(type);
					var name = instance.PluginName;
					var version = instance.PluginVersion;
					List<Tuple<int, Type>> list;
					if(_terrainGeneratorTypes.ContainsKey(name)) // Multiple versions under the same name
						list = _terrainGeneratorTypes[name];
					else // First seen
						_terrainGeneratorTypes[name] = list = new List<Tuple<int, Type>>();
					var item = new Tuple<int, Type>(version, type);
					list.Add(item);
				}
			}
			catch(TypeLoadException)
			{
				// TODO: Do something...
			}
			catch(ReflectionTypeLoadException)
			{
				// TODO: Do something
			}
		}

		private static void loadGenerators (string path)
		{
			var sep = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

			if(Directory.Exists(path))
			{
				foreach(var file in Directory.EnumerateFiles(path, FileExtension, SearchOption.AllDirectories))
				{
					try
					{
						var asm = Assembly.LoadFrom(file);
					}
					catch(Exception e)
					{
						// TODO: It would be nice to somehow notify the user and continue
					}
				}
			}
		}

		private static bool isSuitableTerrainGenerator (Type t)
		{
			return _terrainGeneratorType.IsAssignableFrom(t) &&
					null != t.GetConstructor(Type.EmptyTypes) &&
					!t.IsInterface &&
					!t.IsAbstract &&
					_terrainGeneratorType != t;
		}
		#endregion

		/// <summary>
		/// Gets a terrain generator
		/// </summary>
		/// <param name="name">Name of the generator</param>
		/// <returns>A terrain generator or null if the generator doesn't exist</returns>
		/// <remarks>If there are multiple versions of the generator, the latest one is used.</remarks>
		public static ITerrainGenerator GetGenerator (string name)
		{
			ITerrainGenerator generator = null;
			lock(_terrainGeneratorTypes)
			{
				if(_terrainGeneratorTypes.ContainsKey(name))
				{
					var versions   = _terrainGeneratorTypes[name];
					var maxVersion = versions[0].Item1;
					var type       = versions[0].Item2;
					for(var i = 1; i < versions.Count; ++i)
					{// More than one version
						var item = versions[i];
						if(item.Item1 > maxVersion)
						{// Found a newer version
							maxVersion = item.Item1;
							type       = item.Item2;
						}
					}
					generator = (ITerrainGenerator)Activator.CreateInstance(type);
				}
			}
			return generator;
		}

		/// <summary>
		/// Gets a terrain generator
		/// </summary>
		/// <param name="name">Name of the generator</param>
		/// <param name="version">Specific version of a generator</param>
		/// <returns>A terrain generator or null if the generator (and version) doesn't exist</returns>
		public static ITerrainGenerator GetGenerator (string name, int version)
		{
			ITerrainGenerator generator = null;
			lock(_terrainGeneratorTypes)
			{
				if(_terrainGeneratorTypes.ContainsKey(name))
				{
					var versions = _terrainGeneratorTypes[name];
					foreach(var item in versions)
						if(version == item.Item1)
							generator = (ITerrainGenerator)Activator.CreateInstance(item.Item2);
				}
			}
			return generator;
		}
	}
}
