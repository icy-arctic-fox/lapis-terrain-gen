using System;
using System.Diagnostics;
using Lapis.Level;
using Lapis.Level.Generation;
using Lapis.Threading;

namespace Generator
{
	class Program
	{
		private const int Radius = 48;
		private const string DesiredGeneratorName = "Islands Terrain Generator";

		static void Main (string[] args)
		{
			Console.Write("World Name: ");
			var name = Console.ReadLine();
			Console.Write("Generator Options: ");
			var opts = Console.ReadLine();

			var watch = new Stopwatch();
			watch.Start();
#if DEBUG
			System.Threading.ThreadPool.SetMinThreads(1, 1);
			System.Threading.ThreadPool.SetMaxThreads(1, 1);
#endif

			var generatorNames = GeneratorLoader.GeneratorNames;
			Console.WriteLine("Generators:");
			Console.WriteLine(String.Join<string>("\n", generatorNames));

			var world = World.Create(name);

			var generator = GeneratorLoader.GetGenerator(DesiredGeneratorName);
			generator.Initialize(opts);
			var realm = world.CreateRealm(generator);
			var totalChunks = realm.GenerateRectange(-Radius, -Radius, Radius * 2, Radius * 2);
			world.Save();

			watch.Stop();
			var timeTaken = watch.Elapsed;
			var rate = totalChunks / timeTaken.TotalSeconds;
			Console.WriteLine(timeTaken);
			Console.WriteLine(totalChunks + " chunks generated");
			Console.WriteLine(rate + " chunks/sec.");
			Console.Write("Generation completed, press any key to exit");
			Console.ReadKey();
		}
	}
}
