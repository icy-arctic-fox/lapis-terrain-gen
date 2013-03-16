using System;
using System.Threading;
using Lapis.Level;
using Lapis.Level.Generation;

namespace Generator
{
	class Program
	{
		private const int Radius = 50;
		private const string DesiredGeneratorName = "Flatland Terrain Generator";

		private static Realm _realm;

		static void Main (string[] args)
		{
#if DEBUG
			ThreadPool.SetMinThreads(1, 1);
			ThreadPool.SetMaxThreads(1, 1);
#endif

			var generatorNames = GeneratorLoader.GeneratorNames;
			Console.WriteLine("Generators:");
			Console.WriteLine(String.Join<string>("\n", generatorNames));

			var generator = GeneratorLoader.GetGenerator(DesiredGeneratorName);
			generator.Initialize("7,59x1,3x3,2,78");

			var world = World.Create("New World");
			var realm = world.CreateRealm(generator);
			_realm    = realm;

			var events = new ManualResetEvent[4];
			for(var i = 0; i < events.Length; ++i)
				events[i] = new ManualResetEvent(false);

			ThreadPool.QueueUserWorkItem(topLeft, events[0]);
			ThreadPool.QueueUserWorkItem(topRight, events[1]);
			ThreadPool.QueueUserWorkItem(bottomLeft, events[2]);
			ThreadPool.QueueUserWorkItem(bottomRight, events[3]);
			WaitHandle.WaitAll(events);

			world.Save();
//			World.Load("New World").LoadRealm((int)Dimension.Normal);
		}

		private static void topLeft (object state)
		{
			for(var cx = -Radius; cx < 0; ++cx)
				for(var cz = -Radius; cz < 0; ++cz)
					_realm.GenerateChunk(cx, cz);
			Console.WriteLine("Top-left done");
			((ManualResetEvent)state).Set();
		}

		private static void topRight (object state)
		{
			for(var cx = 0; cx < Radius; ++cx)
				for(var cz = -Radius; cz < 0; ++cz)
					_realm.GenerateChunk(cx, cz);
			Console.WriteLine("Top-right done");
			((ManualResetEvent)state).Set();
		}

		private static void bottomLeft (object state)
		{
			for(var cx = -Radius; cx < 0; ++cx)
				for(var cz = 0; cz < Radius; ++cz)
					_realm.GenerateChunk(cx, cz);
			Console.WriteLine("Bottom-left done");
			((ManualResetEvent)state).Set();
		}

		private static void bottomRight (object state)
		{
			for(var cx = 0; cx < Radius; ++cx)
				for(var cz = 0; cz < Radius; ++cz)
					_realm.GenerateChunk(cx, cz);
			Console.WriteLine("Bottom-right done");
			((ManualResetEvent)state).Set();
		}
	}
}
