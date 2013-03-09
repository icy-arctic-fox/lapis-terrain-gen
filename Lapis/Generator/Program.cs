using System;
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

			var generator = GenerationManager.GetGenerator(generatorNames[0]);

			var world = World.Create("New World");
			world.CreateRealm(generator);

			world.Save();
		}
	}
}
