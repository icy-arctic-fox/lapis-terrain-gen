using System;
using System.Diagnostics;
using Lapis.Level;
using Lapis.Level.Generation;
using Lapis.Threading;

namespace Generator
{
	class Program
	{
		#region Constants
		#region Defaults
		/// <summary>
		/// Default terrain generator to run
		/// </summary>
		private const string DefaultGenerator = "Test";

		/// <summary>
		/// Default length (and width) of the region to generate
		/// </summary>
		private const int DefaultLength = 64;
		#endregion
		
		// TODO: Add dimension option

		#region Option tags
		/// <summary>
		/// Displays the usage/help information and exits
		/// </summary>
		private const string HelpTag = "-h";

		/// <summary>
		/// Run in interactive mode
		/// </summary>
		private const string InteractiveTag = "-i";

		/// <summary>
		/// Load an existing world and operate on it
		/// </summary>
		private const string LoadTag = "-e";

		/// <summary>
		/// Name of the generator to use
		/// </summary>
		private const string GeneratorTag = "-g";

		/// <summary>
		/// Specify a version of a generator
		/// </summary>
		private const string VersionTag = "-v";

		/// <summary>
		/// Generator options string to provide to the generator
		/// </summary>
		private const string OptionStringTag = "-o";

		/// <summary>
		/// Seed to use for generation
		/// </summary>
		private const string SeedTag = "-s";

		/// <summary>
		/// Directory to store the world output in
		/// </summary>
		private const string DirectoryTag = "-d";

		/// <summary>
		/// Speed at which to generate chunks
		/// </summary>
		private const string SpeedTag = "-p";

		/// <summary>
		/// Overwrite any existing chunks
		/// </summary>
		private const string OverwriteTag = "-t";

		/// <summary>
		/// Don't populate chunks after generation (implies NoLightingTag -y)
		/// </summary>
		private const string NoPopulationTag = "-n";

		/// <summary>
		/// Don't populate chunks, but mark them as populated (lighting will be performed unless NoLightingTag -y is provided)
		/// </summary>
		private const string EmptyPopulationTag = "-m";

		/// <summary>
		/// Don't light chunks after generation
		/// </summary>
		private const string NoLightingTag = "-y";

		/// <summary>
		/// Shape of the generation output
		/// </summary>
		private const string ShapeTag = "-a";

		/// <summary>
		/// Radius of the region to generate
		/// </summary>
		private const string RadiusTag = "-r";

		/// <summary>
		/// Length of the region to generate
		/// </summary>
		private const string LengthTag = "-l";

		/// <summary>
		/// Width of the region to generate
		/// </summary>
		private const string WidthTag = "-w";

		/// <summary>
		/// Units to use for specifying the size of the generation region
		/// </summary>
		private const string UnitsTag = "-u";

		/// <summary>
		/// Starting point (center for radius) of the region to generate along the x-axis
		/// </summary>
		private const string XTag = "-x";

		/// <summary>
		/// Starting point (center for radius) of the region to generate along the z-axis
		/// </summary>
		private const string ZTag = "-z";
		#endregion

		#region Extended option tags
		/// <summary>
		/// Displays the usage/help information and exits
		/// </summary>
		private const string ExtendedHelpTag = "--help";

		/// <summary>
		/// Run in interactive mode
		/// </summary>
		private const string ExtendedInteractiveTag = "--interactive";

		/// <summary>
		/// Load an existing world and operate on it
		/// </summary>
		private const string ExtendedLoadTag = "--existing";

		/// <summary>
		/// Name of the generator to use
		/// </summary>
		private const string ExtendedGeneratorTag = "--generator";

		/// <summary>
		/// Specify a version of a generator
		/// </summary>
		private const string ExtendedVersionTag = "--version";

		/// <summary>
		/// Generator options string to provide to the generator
		/// </summary>
		private const string ExtendedOptionStringTag = "--options";

		/// <summary>
		/// Seed to use for generation
		/// </summary>
		private const string ExtendedSeedTag = "--seed";

		/// <summary>
		/// Directory to store the world output in
		/// </summary>
		private const string ExtendedDirectoryTag = "--directory";

		/// <summary>
		/// Speed at which to generate chunks
		/// </summary>
		private const string ExtendedSpeedTag = "--speed";

		/// <summary>
		/// Overwrite any existing chunks
		/// </summary>
		private const string ExtendedOverwriteTag = "--overwrite";

		/// <summary>
		/// Don't populate chunks after generation (implies NoLighting -i)
		/// </summary>
		private const string ExtendedNoPopulationTag = "--no-population";

		/// <summary>
		/// Don't populate chunks, but mark them as populated (lighting will be performed unless NoLightingTag -i is provided)
		/// </summary>
		private const string ExtendedEmptyPopulationTag = "--empty";

		/// <summary>
		/// Don't light chunks after generation
		/// </summary>
		private const string ExtendedNoLightingTag = "--no-lighting";

		/// <summary>
		/// Shape of the generation output
		/// </summary>
		private const string ExtendedShapeTag = "--shape";

		/// <summary>
		/// Radius of the region to generate
		/// </summary>
		private const string ExtendedRadiusTag = "--radius";

		/// <summary>
		/// Length of the region to generate
		/// </summary>
		private const string ExtendedLengthTag = "--length";

		/// <summary>
		/// Width of the region to generate
		/// </summary>
		private const string ExtendedWidthTag = "--width";

		/// <summary>
		/// Units to use for specifying the size of the generation region
		/// </summary>
		private const string ExtendedUnitsTag = "--units";

		/// <summary>
		/// Starting point (center for radius) of the region to generate along the x-axis
		/// </summary>
		private const string ExtendedXTag = "--anchorX";

		/// <summary>
		/// Starting point (center for radius) of the region to generate along the z-axis
		/// </summary>
		private const string ExtendedZTag = "--anchorZ";
		#endregion
		#endregion

		private static readonly string _minecraftDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
															System.IO.Path.DirectorySeparatorChar + ".minecraft";
		private static readonly string _minecraftSaveDirectory = _minecraftDirectory + System.IO.Path.DirectorySeparatorChar + "saves";

		static void Main (string[] args)
		{
			if(null == args || 0 == args.Length || (args.Length == 1 && (HelpTag == args[0] || ExtendedHelpTag == args[0])))
				displayHelp();
			else
			{
				initThreadPool();
				if(args.Length == 1 && (InteractiveTag == args[0] || ExtendedInteractiveTag == args[0]))
					interactiveMode();
				else
				{
					var parameters = getParameters(args);
					if(null != parameters)
						doGeneration(parameters);
				}
			}
		}

		private static void initThreadPool ()
		{
#if DEBUG
			System.Threading.ThreadPool.SetMinThreads(1, 1);
			System.Threading.ThreadPool.SetMaxThreads(1, 1);
#else
			// We have to set this, otherwise .NET goes nuts and spins up too many threads
			var proccessorCount = Environment.ProcessorCount;
			var cpuBoundCount   = proccessorCount + proccessorCount / 2;
			var ioBoundCount    = proccessorCount * 4;
			System.Threading.ThreadPool.SetMaxThreads(cpuBoundCount, ioBoundCount);
#endif
		}

		private static GenerationParameters getParameters (string[] args)
		{
			GenerationParameters parameters = null;

			if(args.Length >= 1)
			{// Valid parameter count
				var worldName = args[0];
				if(!String.IsNullOrWhiteSpace(worldName))
				{// Valid world name
					parameters = new GenerationParameters {
						WorldName       = args[0].Trim(),
						GeneratorName   = DefaultGenerator,
						PopulateChunks  = true,
						LightChunks     = true,
						MarkAsPopulated = true,
						Radius          = DefaultLength,
						Length          = DefaultLength
					};

					for(var i = 1; i < args.Length; ++i)
					{// Parse options
						var tag = args[i];
						if(!String.IsNullOrWhiteSpace(tag))
						{
							tag = tag.Trim();
							string stringValue;
							int intValue;

							switch(tag)
							{
							case HelpTag: // Ignore these tags here
							case ExtendedHelpTag:
							case InteractiveTag:
							case ExtendedInteractiveTag:
								break;

							case LoadTag:
							case ExtendedLoadTag:
								parameters.LoadExisting = true;
								break;

							case GeneratorTag:
							case ExtendedGeneratorTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
									parameters.GeneratorName = stringValue;
								break;

							case VersionTag:
							case ExtendedVersionTag:
								if(tryGetIntParameter(args, ref i, out intValue))
								{
									parameters.GeneratorVersion   = intValue;
									parameters.UseSpecificVersion = true;
								}
								break;

							case OptionStringTag:
							case ExtendedOptionStringTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
									parameters.GeneratorOptions = stringValue;
								break;

							case SeedTag:
							case ExtendedSeedTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
								{
									long seed;
									if(!Int64.TryParse(stringValue, out seed))
										seed = Realm.GenerateSeedFromString(stringValue);
									parameters.Seed = seed;
								}
								break;

							case DirectoryTag:
							case ExtendedDirectoryTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
									parameters.WorkingDirectory = System.IO.Path.GetFullPath(stringValue);
								break;

							case SpeedTag:
							case ExtendedSpeedTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
								{
									stringValue = stringValue.ToLower();
									GenerationSpeed speed;
									switch(stringValue)
									{
									case "full":
									case "normal":
									case "default":
										speed = GenerationSpeed.Full;
										break;
									case "fast":
										speed = GenerationSpeed.Fast;
										break;
									case "medium":
										speed = GenerationSpeed.Medium;
										break;
									case "slow":
										speed = GenerationSpeed.Slow;
										break;
									case "veryslow":
									case "very slow":
									case "very-slow":
									case "very_slow":
									case "slower":
									case "slowest":
										speed = GenerationSpeed.VerySlow;
										break;
									default:
										Console.Error.WriteLine(String.Join(" ", "Invalid generation speed option", args[i - 1], args[i], "- using default"));
										speed = GenerationSpeed.Full;
										break;
									}
									parameters.GenerationSpeed = speed;
								}
								break;

							case OverwriteTag:
							case ExtendedOverwriteTag:
								parameters.OverwriteChunks = true;
								break;

							case NoPopulationTag:
							case ExtendedNoPopulationTag:
								parameters.PopulateChunks = false;
								break;

							case EmptyPopulationTag:
							case ExtendedEmptyPopulationTag:
								parameters.PopulateChunks  = false;
								parameters.MarkAsPopulated = true;
								break;

							case NoLightingTag:
							case ExtendedNoLightingTag:
								parameters.LightChunks = false;
								break;

							case ShapeTag:
							case ExtendedShapeTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
								{
									stringValue = stringValue.ToLower();
									parameters.CircularRegion = (stringValue == "circle" || stringValue == "circular" || stringValue == "round");
								}
								break;

							case RadiusTag:
							case ExtendedRadiusTag:
								if(tryGetIntParameter(args, ref i, out intValue))
								{
									if(0 >= intValue)
									{
										Console.Error.WriteLine(String.Join(" ", "Invalid radius value", args[i - 1], args[i], "- must be a positive number; using default"));
										intValue = DefaultLength;
									}
									parameters.Radius    = intValue;
									parameters.UseRadius = true;
								}
								break;

							case LengthTag:
							case ExtendedLengthTag:
								if(tryGetIntParameter(args, ref i, out intValue))
								{
									if(0 >= intValue)
									{
										Console.Error.WriteLine(String.Join(" ", "Invalid length value", args[i - 1], args[i], "- must be a positive number; using default"));
										intValue = DefaultLength;
									}
									parameters.Length    = intValue;
									parameters.UseRadius = false;
								}
								break;

							case WidthTag:
							case ExtendedWidthTag:
								if(tryGetIntParameter(args, ref i, out intValue))
								{
									if(0 >= intValue)
									{
										Console.Error.WriteLine(String.Join(" ", "Invalid width value", args[i - 1], args[i], "- must be a positive number; using default"));
										intValue = parameters.Length;
									}
									parameters.Width     = intValue;
									parameters.UseRadius = false;
								}
								break;

							case UnitsTag:
							case ExtendedUnitsTag:
								if(tryGetStringParameter(args, ref i, out stringValue))
								{
									stringValue = stringValue.ToLower();
									UnitType units;
									switch(stringValue)
									{
									case "block":
									case "blocks":
										units = UnitType.Blocks;
										break;
									case "chunk":
									case "chunks":
									case "default":
										units = UnitType.Chunks;
										break;
									case "region":
									case "regions":
										units = UnitType.Regions;
										break;
									default:
										Console.Error.WriteLine(String.Join(" ", "Invalid units option", args[i - 1], args[i], "- using default"));
										units = UnitType.Chunks;
										break;
									}
									parameters.UnitType = units;
								}
								break;

							case XTag:
							case ExtendedXTag:
								if(tryGetIntParameter(args, ref i, out intValue))
									parameters.AnchorX = intValue;
								break;

							case ZTag:
							case ExtendedZTag:
								if(tryGetIntParameter(args, ref i, out intValue))
									parameters.AnchorZ = intValue;
								break;

							default:
								Console.Error.WriteLine("Unrecognized option " + tag);
								break;
							}
						}
					}

					if(null == parameters.WorkingDirectory)
						parameters.WorkingDirectory = parameters.LoadExisting
														? _minecraftSaveDirectory
														: Environment.CurrentDirectory;
					if(0 >= parameters.Width)
						parameters.Width = parameters.Length;
				}
			}

			return parameters;
		}

		private static bool tryGetStringParameter (string[] args, ref int i, out string value)
		{
			if(args.Length > ++i)
			{// Enough arguments left
				if(!String.IsNullOrWhiteSpace(args[i]))
				{
					value = args[i].Trim();
					return true;
				}
			}

			// Not enough arguments
			Console.Error.WriteLine("Missing value for option: " + args[i - 1]);
			value = null;
			return false;
		}

		private static bool tryGetIntParameter (string[] args, ref int i, out int value)
		{
			string stringValue;
			if(tryGetStringParameter(args, ref i, out stringValue))
			{// Got the string parameter ok
				if(Int32.TryParse(stringValue, out value))
					return true; // Parsed integer ok

				Console.Error.WriteLine("Number expected for option: " + args[i - 1]);
			}

			// Invalid arguments
			value = 0;
			return false;
		}

		private static void interactiveMode ()
		{
			throw new NotImplementedException();
		}

		private static void doGeneration (GenerationParameters parameters)
		{
			var generator = parameters.UseSpecificVersion
								? GeneratorLoader.GetGenerator(parameters.GeneratorName, parameters.GeneratorVersion)
								: GeneratorLoader.GetGenerator(parameters.GeneratorName);

			if(null != generator)
			{
				var watch = new Stopwatch();
				watch.Start();

				// TODO: Implement loading
				var world = World.Create(parameters.WorldName); // TODO: Add destination directory
				var realm = parameters.Seed.HasValue
								? world.CreateRealm(generator, parameters.GeneratorOptions, parameters.Seed.Value, (int)Dimension.Normal)
								: world.CreateRealm(generator, parameters.GeneratorOptions);

				var startX = 0; // TODO: Use anchorX
				var startZ = 0; // TODO: Use anchorZ
				int countX, countZ;

				// TODO: Implement units
				if(parameters.UseRadius)
				{
					countX = parameters.Radius * 2;
					countZ = parameters.Radius * 2;
				}
				else
				{
					countX = parameters.Length;
					countZ = parameters.Width;
				}

				var bulkGenerator = new BulkGenerator(realm, parameters.GenerationSpeed);

				ulong totalChunks;
				if(parameters.CircularRegion)
					throw new NotImplementedException();
				else
					totalChunks = bulkGenerator.GenerateRectange(startX, startZ, countX, countZ, parameters.PopulateChunks, parameters.OverwriteChunks);

				// TODO: Implement population flags (no lighting, empty population, no population)

				world.Save();

				watch.Stop();
				var timeTaken = watch.Elapsed;
				var rate = totalChunks / timeTaken.TotalSeconds;

				Console.WriteLine("Total time:   " + timeTaken);
				Console.WriteLine("Total chunks: " + totalChunks);
				Console.WriteLine("Average rate: " + rate + " chunks/sec.");
			}

			else
				Console.Error.WriteLine("No terrain generator named '" + parameters.GeneratorName + "' was found.");
		}

		private static void displayHelp ()
		{
			const string progName   = "Generator.exe";
			const string baseSyntax = "  " + progName + " <World Name> ";

			// TODO: Print program info like creator, email, and website
			Console.WriteLine("Usage: " + progName + " <World Name> [options]");
			Console.WriteLine("<World Name> is the name of the world to create or edit." + Environment.NewLine);
			Console.WriteLine("Available Options:");

			Console.WriteLine(String.Join(", ", HelpTag, ExtendedHelpTag));
			Console.WriteLine("  Displays usage/help information (this text) and exits");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", InteractiveTag, ExtendedInteractiveTag));
			Console.WriteLine("  Runs the program in interactive mode. This lets you generate multiple worlds and see generation progress.");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", LoadTag, ExtendedLoadTag));
			Console.WriteLine("  Load an existing world and generate chunks in it");
			Console.WriteLine(baseSyntax + LoadTag);
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", GeneratorTag, ExtendedGeneratorTag));
			Console.WriteLine("  Name of the generator to use");
			Console.WriteLine("  If omitted, the default generator used is " + DefaultGenerator);
			Console.WriteLine(baseSyntax + GeneratorTag + " <Generator Name>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", VersionTag, ExtendedVersionTag));
			Console.WriteLine("  Use a specific version of a generator - usually used in conjunction with " + GeneratorTag);
			Console.WriteLine("  If omitted, the latest version of the generator is used");
			Console.WriteLine(baseSyntax + GeneratorTag + " <Generator Name> " + VersionTag + " <Version Number>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", OptionStringTag, ExtendedOptionStringTag));
			Console.WriteLine("  Provide extra options to the generator - usually used in conjunction with " + GeneratorTag);
			Console.WriteLine("  If omitted, a blank string is used");
			Console.WriteLine(baseSyntax + GeneratorTag + " <Generator Name> " + OptionStringTag + " <Generator Options>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", SeedTag, ExtendedSeedTag));
			Console.WriteLine("  Seed to use for generating chunks");
			Console.WriteLine("  If omitted, a random seed is used.");
			Console.WriteLine("  The seed can be a number or an arbitrary string.");
			Console.WriteLine(baseSyntax + SeedTag + " <Seed>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", DirectoryTag, ExtendedDirectoryTag));
			Console.WriteLine("  Work in a different directory");
			Console.WriteLine("  When generating a new world, this specifies where the world is saved.");
			Console.WriteLine("  When editing an existing world, this specifies where it can be found.");
			Console.WriteLine("  The current directory is used by default when generating new worlds.");
			Console.WriteLine("  Your .minecraft directory is used by default when editing an existing world.");
			Console.WriteLine(baseSyntax + DirectoryTag + " <Output Directory>");
			Console.WriteLine(baseSyntax + LoadTag + " " + DirectoryTag + " <Search Directory>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", SpeedTag, ExtendedSpeedTag));
			Console.WriteLine("  Set the generation speed");
			Console.WriteLine("  By default, full speed is used.");
			Console.WriteLine("  This option lets you change the speed if you want the generation to run in the background or consume less resources.");
			Console.WriteLine("  Speed options are: full, fast, medium, slow, and veryslow");
			Console.WriteLine(baseSyntax + SpeedTag + " <Generation Speed>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", OverwriteTag, ExtendedOverwriteTag));
			Console.WriteLine("  Overwrite existing chunks");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", NoPopulationTag, ExtendedNoPopulationTag));
			Console.WriteLine("  Disables chunk population");
			Console.WriteLine("  This will prevent objects like trees, ores, and structures from being generated.");
			Console.WriteLine("  Importing a world generated with this option into vanilla Minecraft will cause it to populate with default objects.");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", EmptyPopulationTag, ExtendedEmptyPopulationTag));
			Console.WriteLine("  Generate empty (unpopulated) chunks");
			Console.WriteLine("  This will prevent objects like trees, ores, and structures from being generated, but still light the chunks.");
			Console.WriteLine("  Importing a world generated with this option into vanilla Minecraft will NOT cause it to populate with default objects.");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", NoLightingTag, ExtendedNoLightingTag));
			Console.WriteLine("  Skips lighting up chunks");
			Console.WriteLine("  Using this with " + EmptyPopulationTag + " will create dark chunks");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", ShapeTag, ExtendedShapeTag));
			Console.WriteLine("  Specifies the shape used to generate chunks");
			Console.WriteLine("  Available shapes are: rectangle and circle");
			Console.WriteLine("  The default shape used is rectangle if this option is omitted.");
			Console.WriteLine(baseSyntax + ShapeTag + " <Shape>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", RadiusTag, ExtendedRadiusTag));
			Console.WriteLine("  Radius of the region to generate");
			Console.WriteLine("  When the generation region is a rectangle (see " + ShapeTag + "), the size will be a square with the sides a length of Radius x 2.");
			Console.WriteLine(baseSyntax + RadiusTag + " <Radius>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", LengthTag, ExtendedLengthTag));
			Console.WriteLine("  Length of the region to generate (x-distance)");
			Console.WriteLine("  This option is sometimes used with " + WidthTag);
			Console.WriteLine("  When used without " + WidthTag + ", the length and width of the region generated will be <Length> by <Length>.");
			Console.WriteLine("  For circular regions (see " + ShapeTag + "), this is the diameter of the circle.");
			Console.WriteLine(baseSyntax + LengthTag + " <Length>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", WidthTag, ExtendedWidthTag));
			Console.WriteLine("  Width of the region to generate (z-distance)");
			Console.WriteLine("  This option must be used in conjunction with " + LengthTag + ".");
			Console.WriteLine("  If the region is a circle (see " + ShapeTag + "), this option is ignored.");
			Console.WriteLine(baseSyntax + LengthTag + " <Length> " + WidthTag + " <Width>");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", XTag, ExtendedXTag));
			Console.WriteLine("  Anchor point along the x-axis");
			Console.WriteLine("  By default, this option is 0.");
			Console.WriteLine("  When using " + LengthTag + " and " + WidthTag + ", this will be the position along the x-axis of the top left corner.");
			Console.WriteLine("  When using " + RadiusTag + ", this will be the position along the x-axis of the center of the generated region.");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", ZTag, ExtendedZTag));
			Console.WriteLine("  Anchor point along the z-axis");
			Console.WriteLine("  By default, this option is 0.");
			Console.WriteLine("  When using " + LengthTag + " and " + WidthTag + ", this will be the position along the z-axis of the top left corner.");
			Console.WriteLine("  When using " + RadiusTag + ", this will be the position along the z-axis of the center of the generated region.");
			Console.WriteLine();

			Console.WriteLine(String.Join(", ", UnitsTag, ExtendedUnitsTag));
			Console.WriteLine("  Units of the numbers provided as options");
			Console.WriteLine("  Available units are: blocks, chunks, and regions");
			Console.WriteLine("  By default, this option is chunks.");
			Console.WriteLine("  Chunks are 16x16 blocks. Regions are 32x32 chunks (one .mca file).");
		}

		private class GenerationParameters
		{
			public string WorldName { get; set; }

			public bool LoadExisting { get; set; }

			public string GeneratorName { get; set; }

			public int GeneratorVersion { get; set; }

			public string GeneratorOptions { get; set; }

			public bool UseSpecificVersion { get; set; }

			public long? Seed { get; set; }

			public string WorkingDirectory { get; set; }

			public GenerationSpeed GenerationSpeed { get; set; }

			public bool OverwriteChunks { get; set; }

			public bool PopulateChunks { get; set; }

			public bool LightChunks { get; set; }

			public bool MarkAsPopulated { get; set; }

			public bool CircularRegion { get; set; }

			public bool UseRadius { get; set; }

			public int Radius { get; set; }

			public int Length { get; set; }

			public int Width { get; set; }

			public int AnchorX { get; set; }

			public int AnchorZ { get; set; }

			public UnitType UnitType { get; set; }
		}

		private enum UnitType
		{
			Blocks,

			Chunks,

			Regions
		}
	}
}
