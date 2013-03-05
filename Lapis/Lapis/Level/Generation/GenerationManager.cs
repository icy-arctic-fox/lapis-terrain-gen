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
		#region Discovery
		private const string GeneratorDir = "generators";

		/// <summary>
		/// Runs on first reference to load available generators
		/// </summary>
		static GenerationManager ()
		{
			var types = new List<Type>();
			loadGenerators(GeneratorDir, types);
			foreach(var type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(asm => asm.GetTypes().Where(typeof(ITerrainGenerator).IsAssignableFrom)))
				Console.WriteLine(type.FullName);
		}

		public static void foo ()
		{
			Console.WriteLine("Done");
		}

		private static void loadGenerators (string path, List<Type> types)
		{
			var type = typeof(ITerrainGenerator);
			var sep = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

			if(Directory.Exists(path))
			{
				foreach(var file in Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories))
				{
					try
					{
						var asm = Assembly.LoadFrom(file);
						types.AddRange(asm.GetTypes().Where(type.IsAssignableFrom));
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
