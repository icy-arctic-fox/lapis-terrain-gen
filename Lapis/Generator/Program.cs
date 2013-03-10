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
			var realm = world.CreateRealm(generator);

			for(var cx = -10; cx <= 10; ++cx)
				for(var cz = -10; cz <= 10; ++cz)
					realm.GenerateChunk(cx, cz);

			world.Save();
//			World.Load("New World").LoadRealm((int)Dimension.Normal);
		}
	}
}
