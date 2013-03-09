using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lapis.Level.Generation;

namespace Generator
{
	class Program
	{
		static void Main (string[] args)
		{
			Console.WriteLine(String.Join<string>("\n", GenerationManager.GeneratorNames));
			Console.ReadKey();
		}
	}
}
