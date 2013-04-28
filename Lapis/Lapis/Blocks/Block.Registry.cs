using System;
using Lapis.IO.NBT;

namespace Lapis.Blocks
{
	public partial class Block
	{
		// TODO: Possibly add value lookup (i.e. GetLuminance(type))

		#region Registry
		/// <summary>
		/// Describes a method that creates a block
		/// </summary>
		/// <param name="data">Meta-data associated with the block</param>
		/// <param name="tileData">Extra NBT data used by the tile block (may be null)</param>
		/// <returns>A block</returns>
		public delegate Block BlockCreation (byte data, Node tileData);

		/// <summary>
		/// Contains delegates for constructing blocks for all known types
		/// </summary>
		/// <remarks>The type may need to be changed to a dictionary if there ever gets to be the capacity for more than 256 block types</remarks>
		private static readonly BlockCreation[] _knownBlockTypes = new BlockCreation[byte.MaxValue + 1];

		/// <summary>
		/// Registers all default block types
		/// </summary>
		static Block ()
		{
			lock(_knownBlockTypes)
			{
				// TODO: Implement NBT node data constructor
				_knownBlockTypes[(byte)BlockType.Air]                   = (data, tileData) => new AirBlock();
				_knownBlockTypes[(byte)BlockType.Stone]                 = (data, tileData) => new StoneBlock();
				_knownBlockTypes[(byte)BlockType.Grass]                 = (data, tileData) => new GrassBlock();
				_knownBlockTypes[(byte)BlockType.Dirt]                  = (data, tileData) => new DirtBlock();
				_knownBlockTypes[(byte)BlockType.Cobblestone]           = (data, tileData) => new CobblestoneBlock();
				_knownBlockTypes[(byte)BlockType.WoodPlanks]            = (data, tileData) => new WoodPlanksBlock(data);
				_knownBlockTypes[(byte)BlockType.Sapling]               = (data, tileData) => new SaplingBlock(data);
				_knownBlockTypes[(byte)BlockType.Bedrock]               = (data, tileData) => new BedrockBlock();
				_knownBlockTypes[(byte)BlockType.FlowingWater]          = (data, tileData) => new FlowingWaterBlock(data);
				_knownBlockTypes[(byte)BlockType.Water]                 = (data, tileData) => new WaterBlock();
				_knownBlockTypes[(byte)BlockType.FlowingLava]           = (data, tileData) => new FlowingLavaBlock(data);
				_knownBlockTypes[(byte)BlockType.Lava]                  = (data, tileData) => new LavaBlock();
				_knownBlockTypes[(byte)BlockType.Sand]                  = (data, tileData) => new SandBlock();
				_knownBlockTypes[(byte)BlockType.Gravel]                = (data, tileData) => new GravelBlock();
				_knownBlockTypes[(byte)BlockType.GoldOre]               = (data, tileData) => new GoldOreBlock();
				_knownBlockTypes[(byte)BlockType.IronOre]               = (data, tileData) => new IronOreBlock();
				_knownBlockTypes[(byte)BlockType.CoalOre]               = (data, tileData) => new CoalOreBlock();
				_knownBlockTypes[(byte)BlockType.Wood]                  = (data, tileData) => new WoodBlock(data);
				_knownBlockTypes[(byte)BlockType.Leaves]                = (data, tileData) => new LeavesBlock(data);
				_knownBlockTypes[(byte)BlockType.Sponge]                = (data, tileData) => new SpongeBlock();
				_knownBlockTypes[(byte)BlockType.Glass]                 = (data, tileData) => new GlassBlock();
				_knownBlockTypes[(byte)BlockType.LapisLazuliOre]        = (data, tileData) => new LapisLazuliOreBlock();
				_knownBlockTypes[(byte)BlockType.LapisLazuli]           = (data, tileData) => new LapisLazuliBlock();
				_knownBlockTypes[(byte)BlockType.Dispenser]             = (data, tileData) => new DispenserBlock(data);
				_knownBlockTypes[(byte)BlockType.Sandstone]             = (data, tileData) => new SandstoneBlock(data);
				_knownBlockTypes[(byte)BlockType.Note]                  = (data, tileData) => new NoteBlock(tileData);
				_knownBlockTypes[(byte)BlockType.Bed]                   = (data, tileData) => new BedBlock(data);
				_knownBlockTypes[(byte)BlockType.PoweredRail]           = (data, tileData) => new PoweredRailBlock(data);
				_knownBlockTypes[(byte)BlockType.DetectorRail]          = (data, tileData) => new DetectorRailBlock(data);
				_knownBlockTypes[(byte)BlockType.StickyPiston]          = (data, tileData) => new StickyPistonBlock(data);
				_knownBlockTypes[(byte)BlockType.Cobweb]                = (data, tileData) => new CobwebBlock();
				_knownBlockTypes[(byte)BlockType.TallGrass]             = (data, tileData) => new TallGrassBlock(data);
				_knownBlockTypes[(byte)BlockType.DeadBush]              = (data, tileData) => new DeadBushBlock();
				_knownBlockTypes[(byte)BlockType.Piston]                = (data, tileData) => new PistonBlock(data);
				_knownBlockTypes[(byte)BlockType.PistonExtension]       = (data, tileData) => new PistonExtensionBlock(data);
				_knownBlockTypes[(byte)BlockType.Wool]                  = (data, tileData) => new WoolBlock(data);
				_knownBlockTypes[(byte)BlockType.Technical]             = (data, tileData) => new TechnicalBlock();
				_knownBlockTypes[(byte)BlockType.Dandelion]             = (data, tileData) => new DandelionBlock();
				_knownBlockTypes[(byte)BlockType.Rose]                  = (data, tileData) => new RoseBlock();
				_knownBlockTypes[(byte)BlockType.BrownMushroom]         = (data, tileData) => new BrownMushroomBlock();
				_knownBlockTypes[(byte)BlockType.RedMushroom]           = (data, tileData) => new RedMushroomBlock();
				_knownBlockTypes[(byte)BlockType.Gold]                  = (data, tileData) => new GoldBlock();
				_knownBlockTypes[(byte)BlockType.Iron]                  = (data, tileData) => new IronBlock();
				_knownBlockTypes[(byte)BlockType.DoubleStoneSlab]       = (data, tileData) => new DoubleStoneSlabBlock(data);
				_knownBlockTypes[(byte)BlockType.StoneSlab]             = (data, tileData) => new StoneSlabBlock(data);
				_knownBlockTypes[(byte)BlockType.Brick]                 = (data, tileData) => new BrickBlock();
				_knownBlockTypes[(byte)BlockType.TNT]                   = (data, tileData) => new TntBlock();
				_knownBlockTypes[(byte)BlockType.Bookshelf]             = (data, tileData) => new BookshelfBlock();
				_knownBlockTypes[(byte)BlockType.MossStone]             = (data, tileData) => new MossStoneBlock();
				_knownBlockTypes[(byte)BlockType.Obsidian]              = (data, tileData) => new ObsidianBlock();
				_knownBlockTypes[(byte)BlockType.Torch]                 = (data, tileData) => new TorchBlock(data);
				_knownBlockTypes[(byte)BlockType.Fire]                  = (data, tileData) => new FireBlock(data);
				_knownBlockTypes[(byte)BlockType.MonsterSpawner]        = (data, tileData) => new MonsterSpawnerBlock();
				_knownBlockTypes[(byte)BlockType.WoodStairs]            = (data, tileData) => new WoodStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.Chest]                 = (data, tileData) => new ChestBlock(data);
				_knownBlockTypes[(byte)BlockType.RedstoneWire]          = (data, tileData) => new RedstoneWireBlock(data);
				_knownBlockTypes[(byte)BlockType.DiamondOre]            = (data, tileData) => new DiamondOreBlock();
				_knownBlockTypes[(byte)BlockType.Diamond]               = (data, tileData) => new DiamondBlock();
				_knownBlockTypes[(byte)BlockType.CraftingTable]         = (data, tileData) => new CraftingTableBlock();
				_knownBlockTypes[(byte)BlockType.Wheat]                 = (data, tileData) => new WheatBlock(data);
				_knownBlockTypes[(byte)BlockType.Farmland]              = (data, tileData) => new FarmlandBlock(data);
				_knownBlockTypes[(byte)BlockType.Furnace]               = (data, tileData) => new FurnaceBlock(data);
				_knownBlockTypes[(byte)BlockType.LitFurnace]            = (data, tileData) => new LitFurnaceBlock(data);
				_knownBlockTypes[(byte)BlockType.SignPost]              = (data, tileData) => new SignPostBlock(data, tileData);
				_knownBlockTypes[(byte)BlockType.WoodDoor]              = (data, tileData) => new WoodDoorBlock(data);
				_knownBlockTypes[(byte)BlockType.Ladder]                = (data, tileData) => new LadderBlock(data);
				_knownBlockTypes[(byte)BlockType.Rail]                  = (data, tileData) => new RailBlock(data);
				_knownBlockTypes[(byte)BlockType.CobblestoneStairs]     = (data, tileData) => new CobblestoneStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.WallSign]              = (data, tileData) => new WallSignBlock(data, tileData);
				_knownBlockTypes[(byte)BlockType.Lever]                 = (data, tileData) => new LeverBlock(data);
				_knownBlockTypes[(byte)BlockType.StonePlate]            = (data, tileData) => new StonePlateBlock(data);
				_knownBlockTypes[(byte)BlockType.IronDoor]              = (data, tileData) => new IronDoorBlock(data);
				_knownBlockTypes[(byte)BlockType.WoodPlate]             = (data, tileData) => new WoodPlateBlock(data);
				_knownBlockTypes[(byte)BlockType.RedstoneOre]           = (data, tileData) => new RedstoneOreBlock();
				_knownBlockTypes[(byte)BlockType.GlowingRedstoneOre]    = (data, tileData) => new GlowingRedstoneOreBlock();
				_knownBlockTypes[(byte)BlockType.DisabledRedstoneTorch] = (data, tileData) => new DisabledRedstoneTorchBlock(data);
				_knownBlockTypes[(byte)BlockType.RedstoneTorch]         = (data, tileData) => new RedstoneTorchBlock(data);
				_knownBlockTypes[(byte)BlockType.StoneButton]           = (data, tileData) => new StoneButtonBlock(data);
				_knownBlockTypes[(byte)BlockType.SnowCover]             = (data, tileData) => new SnowCoverBlock(data);
				_knownBlockTypes[(byte)BlockType.Ice]                   = (data, tileData) => new IceBlock();
				_knownBlockTypes[(byte)BlockType.Snow]                  = (data, tileData) => new SnowBlock();
				_knownBlockTypes[(byte)BlockType.Cactus]                = (data, tileData) => new CactusBlock(data);
				_knownBlockTypes[(byte)BlockType.Clay]                  = (data, tileData) => new ClayBlock();
				_knownBlockTypes[(byte)BlockType.SugarCane]             = (data, tileData) => new SugarCaneBlock(data);
				_knownBlockTypes[(byte)BlockType.Jukebox]               = (data, tileData) => new JukeboxBlock(data);
				_knownBlockTypes[(byte)BlockType.Fence]                 = (data, tileData) => new FenceBlock();
				_knownBlockTypes[(byte)BlockType.Pumpkin]               = (data, tileData) => new PumpkinBlock(data);
				_knownBlockTypes[(byte)BlockType.Netherrack]            = (data, tileData) => new NetherrackBlock();
				_knownBlockTypes[(byte)BlockType.SoulSand]              = (data, tileData) => new SoulSandBlock();
				_knownBlockTypes[(byte)BlockType.Glowstone]             = (data, tileData) => new GlowstoneBlock();
				_knownBlockTypes[(byte)BlockType.NetherPortal]          = (data, tileData) => new NetherPortalBlock();
				_knownBlockTypes[(byte)BlockType.JackOLantern]          = (data, tileData) => new JackOLanternBlock(data);
				_knownBlockTypes[(byte)BlockType.Cake]                  = (data, tileData) => new CakeBlock(data);
				_knownBlockTypes[(byte)BlockType.DisabledDiode]         = (data, tileData) => new DisabledDiodeBlock(data);
				_knownBlockTypes[(byte)BlockType.Diode]                 = (data, tileData) => new DiodeBlock(data);
				_knownBlockTypes[(byte)BlockType.LockedChest]           = (data, tileData) => new LockedChestBlock();
				_knownBlockTypes[(byte)BlockType.Trapdoor]              = (data, tileData) => new TrapdoorBlock(data);
				_knownBlockTypes[(byte)BlockType.Silverfish]            = (data, tileData) => new SilverfishBlock(data);
				_knownBlockTypes[(byte)BlockType.StoneBrick]            = (data, tileData) => new StoneBrickBlock(data);
				_knownBlockTypes[(byte)BlockType.HugeBrownMushroom]     = (data, tileData) => new HugeBrownMushroomBlock(data);
				_knownBlockTypes[(byte)BlockType.HugeRedMushroom]       = (data, tileData) => new HugeRedMushroomBlock(data);
				_knownBlockTypes[(byte)BlockType.IronBars]              = (data, tileData) => new IronBarsBlock();
				_knownBlockTypes[(byte)BlockType.GlassPane]             = (data, tileData) => new GlassPaneBlock();
				_knownBlockTypes[(byte)BlockType.Melon]                 = (data, tileData) => new MelonBlock();
				_knownBlockTypes[(byte)BlockType.PumpkinStem]           = (data, tileData) => new PumpkinStemBlock(data);
				_knownBlockTypes[(byte)BlockType.MelonStem]             = (data, tileData) => new MelonStemBlock(data);
				_knownBlockTypes[(byte)BlockType.Vines]                 = (data, tileData) => new VinesBlock(data);
				_knownBlockTypes[(byte)BlockType.Gate]                  = (data, tileData) => new GateBlock(data);
				_knownBlockTypes[(byte)BlockType.BrickStairs]           = (data, tileData) => new BrickStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.StoneBrickStairs]      = (data, tileData) => new StoneBrickStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.Mycelium]              = (data, tileData) => new MyceliumBlock();
				_knownBlockTypes[(byte)BlockType.LilyPad]               = (data, tileData) => new LilyPadBlock();
				_knownBlockTypes[(byte)BlockType.NetherBrick]           = (data, tileData) => new NetherBrickBlock();
				_knownBlockTypes[(byte)BlockType.NetherBrickFence]      = (data, tileData) => new NetherBrickFenceBlock();
				_knownBlockTypes[(byte)BlockType.NetherBrickStairs]     = (data, tileData) => new NetherBrickStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.NetherWart]            = (data, tileData) => new NetherWartBlock(data);
				_knownBlockTypes[(byte)BlockType.EnchantmentTable]      = (data, tileData) => new EnchantmentTableBlock();
				_knownBlockTypes[(byte)BlockType.BrewingStand]          = (data, tileData) => new BrewingStandBlock(data);
				_knownBlockTypes[(byte)BlockType.Cauldron]              = (data, tileData) => new CauldronBlock(data);
				_knownBlockTypes[(byte)BlockType.EndPortal]             = (data, tileData) => new EndPortalBlock();
				_knownBlockTypes[(byte)BlockType.EndPortalFrame]        = (data, tileData) => new EndPortalFrameBlock(data);
				_knownBlockTypes[(byte)BlockType.EndStone]              = (data, tileData) => new EndStoneBlock();
				_knownBlockTypes[(byte)BlockType.DragonEgg]             = (data, tileData) => new DragonEggBlock();
				_knownBlockTypes[(byte)BlockType.InactiveLamp]          = (data, tileData) => new InactiveLampBlock();
				_knownBlockTypes[(byte)BlockType.Lamp]                  = (data, tileData) => new LampBlock();
				_knownBlockTypes[(byte)BlockType.DoubleWoodSlab]        = (data, tileData) => new DoubleWoodSlabBlock(data);
				_knownBlockTypes[(byte)BlockType.WoodSlab]              = (data, tileData) => new WoodSlabBlock(data);
				_knownBlockTypes[(byte)BlockType.CocoaPlant]            = (data, tileData) => new CocoaPlantBlock(data);
				_knownBlockTypes[(byte)BlockType.SandstoneStairs]       = (data, tileData) => new SandstoneStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.EmeraldOre]            = (data, tileData) => new EmeraldOreBlock();
				_knownBlockTypes[(byte)BlockType.EnderChest]            = (data, tileData) => new EnderChestBlock(data);
				_knownBlockTypes[(byte)BlockType.TripwireHook]          = (data, tileData) => new TripwireHookBlock(data);
				_knownBlockTypes[(byte)BlockType.Tripwire]              = (data, tileData) => new TripwireBlock(data);
				_knownBlockTypes[(byte)BlockType.Emerald]               = (data, tileData) => new EmeraldBlock();
				_knownBlockTypes[(byte)BlockType.SpruceWoodStairs]      = (data, tileData) => new SpruceWoodStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.BirchWoodStairs]       = (data, tileData) => new BirchWoodStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.JungleWoodStairs]      = (data, tileData) => new JungleWoodStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.Command]               = (data, tileData) => new CommandBlock();
				_knownBlockTypes[(byte)BlockType.Beacon]                = (data, tileData) => new BeaconBlock();
				_knownBlockTypes[(byte)BlockType.CobblestoneWall]       = (data, tileData) => new CobblestoneWallBlock(data);
				_knownBlockTypes[(byte)BlockType.FlowerPot]             = (data, tileData) => new FlowerPotBlock(data);
				_knownBlockTypes[(byte)BlockType.Carrot]                = (data, tileData) => new CarrotBlock(data);
				_knownBlockTypes[(byte)BlockType.Potato]                = (data, tileData) => new PotatoBlock(data);
				_knownBlockTypes[(byte)BlockType.WoodButton]            = (data, tileData) => new WoodButtonBlock(data);
				_knownBlockTypes[(byte)BlockType.MobHead]               = (data, tileData) => new MobHeadBlock(data);
				_knownBlockTypes[(byte)BlockType.Anvil]                 = (data, tileData) => new AnvilBlock(data);
				_knownBlockTypes[(byte)BlockType.TrapChest]             = (data, tileData) => new TrapChestBlock(data);
				_knownBlockTypes[(byte)BlockType.LightPlate]            = (data, tileData) => new LightPlateBlock(data);
				_knownBlockTypes[(byte)BlockType.HeavyPlate]            = (data, tileData) => new HeavyPlateBlock(data);
				_knownBlockTypes[(byte)BlockType.InactiveComparator]    = (data, tileData) => new InactiveComparatorBlock(data);
				_knownBlockTypes[(byte)BlockType.Comparator]            = (data, tileData) => new ComparatorBlock(data);
				_knownBlockTypes[(byte)BlockType.DaylightSensor]        = (data, tileData) => new DaylightSensorBlock();
				_knownBlockTypes[(byte)BlockType.Redstone]              = (data, tileData) => new RedstoneBlock();
				_knownBlockTypes[(byte)BlockType.NetherQuartzOre]       = (data, tileData) => new NetherQuartzOreBlock();
				_knownBlockTypes[(byte)BlockType.Hopper]                = (data, tileData) => new HopperBlock(data);
				_knownBlockTypes[(byte)BlockType.Quartz]                = (data, tileData) => new QuartzBlock(data);
				_knownBlockTypes[(byte)BlockType.QuartzStairs]          = (data, tileData) => new QuartzStairsBlock(data);
				_knownBlockTypes[(byte)BlockType.ActivatorRail]         = (data, tileData) => new ActivatorRailBlock(data);
				_knownBlockTypes[(byte)BlockType.Dropper]               = (data, tileData) => new DropperBlock(data);
			}
		}

		/// <summary>
		/// Registers a block type
		/// </summary>
		/// <param name="type">Block type</param>
		/// <param name="constructor">Method that statically creates the block</param>
		public static void Register (BlockType type, BlockCreation constructor)
		{
			if(null == constructor)
				throw new ArgumentNullException("constructor", "The block constructor method can't be null.");

			lock(_knownBlockTypes)
				_knownBlockTypes[(byte)type] = constructor;
		}
		#endregion

		#region Creation
		/// <summary>
		/// Creates a new block with meta-data
		/// </summary>
		/// <param name="type">Type of block to statically create</param>
		/// <param name="data">Meta-data associated with the block</param>
		/// <returns>A block</returns>
		public static Block Create (BlockType type, byte data = 0)
		{
			var constructor = _knownBlockTypes[(byte)type]; // This isn't locked because it would reduce concurrency down to none
			return (null == constructor) ? null : constructor(data, null);
		}

		/// <summary>
		/// Creates a new tile block
		/// </summary>
		/// <param name="type">Type of block to statically create</param>
		/// <param name="data">Meta-data associated with the block</param>
		/// <param name="tileData">Extra NBT data used by the tile block</param>
		/// <returns>A tile block or null if the block type is unknown</returns>
		public static TileEntity Create (BlockType type, byte data, Node tileData)
		{
			var constructor = _knownBlockTypes[(byte)type]; // This isn't locked because it would reduce concurrency down to none
			return (null == constructor) ? null : constructor(data, tileData) as TileEntity;
		}
		#endregion
	}
}
