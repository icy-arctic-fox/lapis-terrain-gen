using System;
using System.Diagnostics;
using Lapis.Level.Generation;
using Lapis.Threading;

namespace Generator
{
	class Program
	{
		private const int Radius = 64;
		private const string DesiredGeneratorName = "Flatland Terrain Generator";

		static void Main (string[] args)
		{
			Console.Write("World Name: ");
			var name = Console.ReadLine();
			Console.Write("Generator Options: ");
			var opts = Console.ReadLine();

			var watch = new Stopwatch();
			watch.Start();
#if DEBUG
			ThreadPool.SetMinThreads(1, 1);
			ThreadPool.SetMaxThreads(1, 1);
#endif

			var generatorNames = GeneratorLoader.GeneratorNames;
			Console.WriteLine("Generators:");
			Console.WriteLine(String.Join<string>("\n", generatorNames));

			var manager   = new GenerationManager(name);
			var generator = GeneratorLoader.GetGenerator(DesiredGeneratorName);
			generator.Initialize(opts);
			var realmId = manager.AddRealm(generator);
			manager.GenerateRectange(realmId, -Radius, -Radius, Radius * 2, Radius * 2);

			watch.Stop();
			Console.WriteLine(watch.Elapsed);
			Console.Write("Generation completed, press any key to exit");
			Console.ReadKey();
		}
	}
}
