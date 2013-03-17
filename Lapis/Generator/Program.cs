using System;
using System.Diagnostics;
using System.Threading;
using Lapis.Level;
using Lapis.Level.Generation;
using Lapis.Threading;

namespace Generator
{
	class Program
	{
		private const int Radius = 128;
		private const string DesiredGeneratorName = "Flatland Terrain Generator";

		static void Main (string[] args)
		{
			var watch = new Stopwatch();
			watch.Start();
#if DEBUG
			ThreadPool.SetMinThreads(1, 1);
			ThreadPool.SetMaxThreads(1, 1);
#endif

			var generatorNames = GeneratorLoader.GeneratorNames;
			Console.WriteLine("Generators:");
			Console.WriteLine(String.Join<string>("\n", generatorNames));

			var manager   = new GenerationManager("New World");
			var generator = GeneratorLoader.GetGenerator(DesiredGeneratorName);
			generator.Initialize("7,5x1,5x3,5x12,90x9");
			var realmId = manager.AddRealm(generator);
			manager.GenerateRectange(realmId, -Radius, -Radius, Radius * 2, Radius * 2);

			watch.Stop();
			Console.WriteLine(watch.Elapsed);
		}
	}
}
