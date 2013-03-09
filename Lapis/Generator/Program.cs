using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lapis.Level;
using Lapis.Level.Generation;

namespace Generator
{
	class Program
	{
		static void Main (string[] args)
		{
			var generatorNames = GenerationManager.GeneratorNames;
			Console.WriteLine("Generators:");
			Console.WriteLine(String.Join<string>("\n", generatorNames));
			Console.ReadKey();

			var world = World.Create("New World");

			world.Save();
		}
	}
}
