using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Manages terrain generators and chunk populators
	/// </summary>
	/// <remarks>Use this class to get available generators.</remarks>
	public static class GenerationManager
	{
		private static readonly List<Type> _terrainGeneratorTypes = new List<Type>();
		private static readonly List<string> _terrainGeneratorNames = new List<string>();
		private static readonly Type _terrainGeneratorType = typeof(ITerrainGenerator);

		/// <summary>
		/// Names of the terrain generators available
		/// </summary>
		public static string[] GeneratorNames
		{
			get
			{
				lock(_terrainGeneratorTypes)
					return _terrainGeneratorNames.ToArray<string>();
			}
		}

		#region Discovery
		private const string GeneratorDir = "generators";

		/// <summary>
		/// Runs on first reference to load available generators
		/// </summary>
		static GenerationManager ()
		{
			lock(_terrainGeneratorTypes)
			{
				discoverGenerators();
				// TODO: Add terrain populator loading
			}
		}

		private static void discoverGenerators ()
		{
			loadGenerators(GeneratorDir);
			var types = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany(asm => asm.GetTypes()
													.Where(t =>
															_terrainGeneratorType.IsAssignableFrom(t) &&
															!t.IsInterface &&
															_terrainGeneratorType != t));

			_terrainGeneratorTypes.Clear();
			_terrainGeneratorTypes.AddRange(types);
			_terrainGeneratorNames.AddRange(
				types.Select(t => ((ITerrainGenerator)Activator.CreateInstance(t)).GeneratorName));
		}

		private static void loadGenerators (string path)
		{
			var sep = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

			if(Directory.Exists(path))
			{
				foreach(var file in Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories))
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
		#endregion
	}
}
