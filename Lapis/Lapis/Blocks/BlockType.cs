namespace Lapis.Blocks
{
	/// <summary>
	/// Numerical values for all possible blocks
	/// </summary>
	public enum BlockType : byte
	{
		/// <summary>
		/// An invisible block. Necessary for breathing.
		/// </summary>
		Air = 0,

		/// <summary>
		/// This block might or might not exist. It doesn't though, so don't think too hard about it.
		/// </summary>
		Nothing = 0,

		/// <summary>
		/// Very common, naturally occurring rock-type block.
		/// </summary>
		Stone = 1,

		/// <summary>
		/// A dirt block with grass on top. 
		/// </summary>
		Grass = 2,

		/// <summary>
		/// It's just dirt. What more is there to it?
		/// </summary>
		Dirt = 3,

		/// <summary>
		/// A very plain block. Not really useful for anything except building.
		/// </summary>
		Cobblestone = 4,

		/// <summary>
		/// Wood-like blocks obtained from logs. Comes in four different types.
		/// </summary>
		Planks = 5,

		/// <summary>
		/// Plant these to grow trees. There's a different sapling for each type of tree.
		/// </summary>
		Saplings = 6,

		/// <summary>
		/// Bedrock, sometimes called Adminium. Unbreakable, and indestructible. Acts as defense from falling into the void.
		/// </summary>
		Bedrock = 7,

		/// <summary>
		/// Moving water that pushes entities in the direction that it's flowing in.
		/// </summary>
		FlowingWater = 8,

		/// <summary>
		/// Fertilizes farmland. You can also drown in it, so be careful.
		/// </summary>
		Water = 9,

		/// <summary>
		/// Hot, burning death. Commonly found in the Nether.
		/// </summary>
		FlowingLava = 10,

		/// <summary>
		/// Liquid block that harms the player and incinerates anything thrown into it.
		/// </summary>
		Lava = 11,

		/// <summary>
		/// Makes up deserts. Can be smelted to make glass.
		/// </summary>
		Sand = 12,

		/// <summary>
		/// This block is pretty useless. Mine it for flint.
		/// </summary>
		Gravel = 13,

		/// <summary>
		/// Found deep underground. Can be smelted to obtain one gold ingot.
		/// </summary>
		GoldOre = 14,

		/// <summary>
		/// Found near the surface. Can be smelted to obtain one iron ingot.
		/// </summary>
		IronOre = 15,

		/// <summary>
		/// Found anywhere throughout the world. Drops one piece of coal that has many uses.
		/// </summary>
		CoalOre = 16,

		/// <summary>
		/// This is what trees are made of. Can be converted into planks. Four different types.
		/// </summary>
		Wood = 17,

		/// <summary>
		/// This is what trees are made of. Can be converted into planks. Four different types.
		/// </summary>
		Log = 17,

		/// <summary>
		/// They hang(?) from trees, and decay when the rest of the tree is gone. Sometimes drop saplings.
		/// </summary>
		Leaves = 18,

		/// <summary>
		/// Cannot be obtained legitimately in Survival. Used to "erase" water in a 5x5x5 radius. 
		/// </summary>
		Sponge = 19,

		/// <summary>
		/// Transparent block made from sand. Cannot be harvested after being placed by normal means. 
		/// </summary>
		Glass = 20,

		/// <summary>
		/// A block that rarely occurs deep underground. When mined, it drops 4-8 pieces of Lapis Lazuli.
		/// </summary>
		LapisOre = 21,

		/// <summary>
		/// A block crafted out of nine pieces of Lapis Lazuli. Good for compact storage and decoration.
		/// </summary>
		LapisBlock = 22,

		/// <summary>
		/// Powered by redstone, dispenses items that are placed into it. Can place boats on water and minecarts on rails.
		/// </summary>
		Dispenser = 23,

		/// <summary>
		/// Found naturally a few layers beneath sand. Can also be crafted. Not much use outside of decoration.
		/// </summary>
		Sandstone = 24,

		/// <summary>
		/// Notes can be selected by right clicking. Plays a tune when left-clicked once or powered by redstone. Different sounds play depending on the block under it.
		/// </summary>
		NoteBlock = 25,

		/// <summary>
		/// Used to sleep through the night and change your spawn point.
		/// </summary>
		Bed = 26,

		/// <summary>
		/// A rail used to boost or stop the momentum of minecarts.
		/// </summary>
		PoweredRail = 27,

		/// <summary>
		/// A rail that acts like a pressure plate for minecarts.
		/// </summary>
		DetectorRail = 28,

		/// <summary>
		/// A special type of block that when powered by redstone, pushes and pulls certain blocks.
		/// </summary>
		StickyPiston = 29,

		/// <summary>
		/// A block that spawns naturally in Abandoned Mineshafts. It slows down everything that enters it, except for Cave Spiders.
		/// </summary>
		Cobweb = 30,

		/// <summary>
		/// Naturally occurring on grass. Occasionally drops seeds when broken.
		/// </summary>
		TallGrass = 31,

		/// <summary>
		/// Decoration block. Only spawns in desert biomes, and rarely in swamps.
		/// </summary>
		DeadBush = 32,

		/// <summary>
		/// A block that has the ability to push other blocks.
		/// </summary>
		Piston = 33,

		/// <summary>
		/// The extension of a piston.
		/// </summary>
		PistonExtension = 34,

		/// <summary>
		/// Obtained from sheep. Can be used for bed-making. 
		/// </summary>
		Wool = 35,

		/// <summary>
		/// A block moved by a piston.
		/// </summary>
		PistonTechBlock = 36,

		/// <summary>
		/// Retains block information.
		/// </summary>
		Transparent = 36,

		/// <summary>
		/// Pretty yellow flower. Found naturally on grass.
		/// </summary>
		Dandelion = 37,

		/// <summary>
		/// Pretty red flower. Found naturally on grass.
		/// </summary>
		Rose = 38,

		/// <summary>
		/// Naturally grown in dark, shady areas. Commonly found in the Nether. Ingredient for Mushroom Stew.
		/// </summary>
		BrownMushroom = 39,

		/// <summary>
		/// Naturally grown in dark, shady areas. Less common than its brown counterpart. Ingredient for Mushroom Stew.
		/// </summary>
		RedMushroom = 40,

		/// <summary>
		/// Crafted from nine gold ingots. Used for compact storage.
		/// </summary>
		GoldBlock = 41,

		/// <summary>
		/// Crafted from nine iron ingots. Used for compact storage.
		/// </summary>
		IronBlock = 42,

		/// <summary>
		/// The block created from two slabs being placed on one another.
		/// </summary>
		DoubleSlabs = 43,

		/// <summary>
		/// Pretty much half of a block. Useful for making stairs. 
		/// </summary>
		Slab = 44,

		/// <summary>
		/// Made from clay.
		/// </summary>
		Bricks = 45,

		/// <summary>
		/// An explosive block crafted from sand and gunpowder. A great mining tool if used carefully.
		/// </summary>
		TNT = 46,

		/// <summary>
		/// Decorative block. Also used to power up enchantments. 
		/// </summary>
		Bookshelf = 47,

		/// <summary>
		/// Naturally occurring inside dungeons. No real use outside of decoration. 
		/// </summary>
		MossyCobblestone = 48,

		/// <summary>
		/// Primarily used to make Nether portals. Cannot be destroyed by anything other than a diamond pickaxe.
		/// </summary>
		Obsidian = 49,

		/// <summary>
		/// Used to light up an area. 
		/// </summary>
		Torch = 50,

		/// <summary>
		/// Harmful to the player if touched. Burns certain blocks to a crisp.
		/// </summary>
		Fire = 51,

		/// <summary>
		/// Mean things come out of this. Spawns naturally inside of dungeons. Comes in zombie, skeleton, and spider.
		/// </summary>
		MobSpawner = 52,

		/// <summary>
		/// Stairs made out of Oak planks.
		/// </summary>
		OakWoodStairs = 53,

		/// <summary>
		/// Useful item used for item storage. Place two together for a double-chest.
		/// </summary>
		Chest = 54,

		/// <summary>
		/// Placed  redstone ore. Used to carry circuits.
		/// </summary>
		RedstoneWire = 55,

		/// <summary>
		/// Found deep underground. Drops one diamond when mined with an iron or diamond pickaxe.
		/// </summary>
		DiamondOre = 56,

		/// <summary>
		/// Crafted from nine diamonds. Used for compact storage. Or if you just want to brag.
		/// </summary>
		DiamondBlock = 57,

		/// <summary>
		/// Basic need for survival. Used to make almost everything in the game.
		/// </summary>
		CraftingTable = 58,

		/// <summary>
		/// Grown from seeds. Used to make bread.
		/// </summary>
		Wheat = 59,

		/// <summary>
		/// Formed when you use a hoe on a dirt block adjacent to a water block. Can grow crops.
		/// </summary>
		Farmland = 60,

		/// <summary>
		/// Used for smelting ores, baking clay, and cooking food. 
		/// </summary>
		Furnace = 61,

		/// <summary>
		/// Things are being cooked, yo. Check the progress bar.
		/// </summary>
		BurningFurnace = 62,

		/// <summary>
		/// Can record notes written by the player.
		/// </summary>
		Sign = 63,

		/// <summary>
		/// Basic door. It keeps you separated from the mean mobs... ssssometimes.
		/// </summary>
		WoodenDoor = 64,

		/// <summary>
		/// Used to vertically climb walls. 
		/// </summary>
		Ladder = 65,

		/// <summary>
		/// Used for minecart travel.
		/// </summary>
		Rails = 66,

		/// <summary>
		/// Stairs made out of cobblestone.
		/// </summary>
		CobblestoneStairs = 67,

		/// <summary>
		/// Signs hung on a wall.
		/// </summary>
		WallSign = 68,

		/// <summary>
		/// Used to activate or deactivate redstone currents.
		/// </summary>
		Level = 69,

		/// <summary>
		/// Can only be activated by a player or mob standing on it.
		/// </summary>
		StonePressurePlate = 70,

		/// <summary>
		/// Can only be opened by one of the five switch types.
		/// </summary>
		IronDoor = 71,

		/// <summary>
		/// Can be activated by any item, mobs, or the player.
		/// </summary>
		WoodenPressurePlate = 72,

		/// <summary>
		/// Found deep underground. Drops 4-5 redstone dust when mined with an iron pickaxe or higher.
		/// </summary>
		RedstoneOre = 73,

		/// <summary>
		/// Appears when a player or entity interacts with redstone ore.
		/// </summary>
		GlowingRedstoneOre = 74,

		/// <summary>
		/// A redstone torch in an "off" state.
		/// </summary>
		RedstoneTorchOff = 75,

		/// <summary>
		/// A redstone torch in an "on" state.
		/// </summary>
		RestoneTorch = 76,

		/// <summary>
		/// Can be placed on walls. Sends a short burst of power to redstone currents.
		/// </summary>
		Button = 77,

		/// <summary>
		/// A thin layer of snow caused from snowfall.
		/// </summary>
		Snow = 78,

		/// <summary>
		/// Solid, but transparent. Found in snowy biomes. Occurs when water is in contact with the sky above. 
		/// </summary>
		Ice = 79,

		/// <summary>
		/// Naturally spawns in desert biomes. Harms player when touched.
		/// </summary>
		Cactus = 80,

		/// <summary>
		/// Found naturally in certain parts of the world. Can be made from pieces of clay.
		/// </summary>
		ClayBlock = 82,

		/// <summary>
		/// Source of sugar and paper. Found naturally in the world. Can also be planted and harvested.
		/// </summary>
		SugarCane = 83,

		/// <summary>
		/// Used to listen to records that you have found in dungeons or obtained from skillfully tricking skeletons.
		/// </summary>
		Jukebox = 84,

		/// <summary>
		/// Used to keep mobs (mainly passive) in a contained area. Cannot be jumped over without being above it. 
		/// </summary>
		Fence = 85,

		/// <summary>
		/// Found naturally in the world. Can be grown and harvested. Useful for making Jack O' Lanterns and Snow Golems.
		/// </summary>
		Pumpkin = 86,

		/// <summary>
		/// Abundant in the Nether. Burns forever, easily destructible.
		/// </summary>
		Netherrack = 97,

		/// <summary>
		/// Common in the Nether. Slows down any mobs that step on it, as well as the player.)
		/// </summary>
		SoulSand = 88,

		/// <summary>
		/// Normally found in the Nether up on the ceiling. Yields 2-4 glowstone dust when broken. 
		/// </summary>
		GlowstoneBlock = 89,

		/// <summary>
		/// The block which makes up the "portal" part of the Nether portal.
		/// </summary>
		NetherPortal = 90,

		/// <summary>
		/// Made by placing a torch into a pumpkin. Provides 15 levels of light and stays lit underwater.
		/// </summary>
		JackOLantern = 91,

		/// <summary>
		/// The only edible block in the game. Has to be placed in order to eat it.
		/// </summary>
		CakeBlock = 92,

		/// <summary>
		/// A repeater in an "off" state.
		/// </summary>
		RedstoneRepeaterOff = 93,

		/// <summary>
		/// A repeater in an "on" state.
		/// </summary>
		RedstoneRepeater = 94,

		/// <summary>
		/// A joke implemented for April Fool's.
		/// </summary>
		/// <remarks>When opened, a window would pop up in game showing the chest with the name, "Steve Co. Supply Crate", which is a joke made towards Team Fortress 2. The texture has been removed from the game and is now a placeholder block. Indestructible by hand or tools. Cannot be obtained legitimately. </remarks>
		LockedChest = 95,

		/// <summary>
		/// A block used as a horizontal door. It takes up one block space.
		/// </summary>
		Trapdoor = 96,

		/// <summary>
		/// Sometimes known as "Silverfish Block", or "Block 97". Can look like stone, stone brick, or cobblestone. When broken, Silverfish swarm the player. Found naturally in strongholds.
		/// </summary>
		MonsterEgg = 97,

		/// <summary>
		/// Comes in four types; Regular, Cracked, Mossy, and Chiseled. All types except for regular stone brick cannot be crafted, but can be used in crafting recipes. Found in strongholds and jungle temples.
		/// </summary>
		StoneBricks = 98,

		/// <summary>
		/// Occurs when bonemeal is used on a brown mushroom. Also found naturally in Mushroom Island biomes. 
		/// </summary>
		HugeBrownMushroom = 99,

		/// <summary>
		/// Occurs when bonemeal is used on a red mushroom. Also found naturally in Mushroom Island biomes.
		/// </summary>
		HugeRedMushroom = 100,

		/// <summary>
		/// Spawns naturally in strongholds and NPC villages. Can also be crafted from iron ingots. Can be jumped onto and over, unlike fences.
		/// </summary>
		IronBars = 101,

		/// <summary>
		/// Spawns naturally in NPC villages. Alternative to glass. Can be crafted just like iron bars. Can be jumped onto and over, unlike fences.
		/// </summary>
		GlassPane = 102,

		/// <summary>
		/// A block that grows from planted melon seeds. When harvested, it drops 3-7 melon slices. If using a fortune tool, the maximum that can drop is 9.
		/// </summary>
		Melon = 103,

		/// <summary>
		/// Has eight stages of growth. Happens when the player places pumpkin seeds on farmland. Grows a pumpkin during its final stage. Bonemeal can speed up this process.
		/// </summary>
		PumpkinStem = 104,

		/// <summary>
		/// Has eight stages of growth. Happens when the player places melon seeds on farmland. Grows a melon during its final stage. Bonemeal can speed up this process.
		/// </summary>
		MelonStem = 105,

		/// <summary>
		/// Non-solid block that can be placed on other solid blocks. Act as ladders, but without the collision box. Will slow the player down if they try to walk through.
		/// </summary>
		Vines = 106,

		/// <summary>
		/// Used as a door for fences. Always opens away from the player. Can be opened with redstone. 
		/// </summary>
		FenceGate = 107,

		/// <summary>
		/// Stairs made out of brick.
		/// </summary>
		BrickStairs = 108,

		/// <summary>
		/// Stairs made out of stone brick.
		/// </summary>
		StoneBrickStairs = 109,

		/// <summary>
		/// Only found in the Mushroom Island biome. Allows mushrooms to be planted at any light level. Releases a "spore" particle effect.
		/// </summary>
		Mycelium = 110,

		/// <summary>
		/// A collectible block found in swamp biomes that can be walked on. Used to be over-powered until nerfed in the 1.3 update.
		/// </summary>
		LilyPad = 111,

		/// <summary>
		/// Only found in the Nether. Nether Fortresses are made of this.
		/// </summary>
		NetherBrick = 112,

		/// <summary>
		/// Found inside Nether Fortresses. Acts just like a regular fence.
		/// </summary>
		NetherBrickFence = 113,

		/// <summary>
		/// Found inside Nether Fortresses. Stairs made out of Nether Brick.
		/// </summary>
		NetherBrickStairs = 114,

		/// <summary>
		/// Found naturally inside Nether Fortresses. Can be harvested and planted on Soul Sand in any dimension. Key ingredient for potion brewing.
		/// </summary>
		NetherWart = 115,

		/// <summary>
		/// Used to enchant some tools and weapons. Takes experience levels to use. Surrounding it with bookshelves will yield higher level enchantments.
		/// </summary>
		EnchantmentTable = 116,

		/// <summary>
		/// A block used for brewing normal potions and splash potions. 
		/// </summary>
		BrewingStand = 117,

		/// <summary>
		/// A block used to hold water. One full cauldron can fill three bottles of water. It is the only way to bring water into the nether without hacks or cheats.
		/// </summary>
		Cauldron = 118,

		/// <summary>
		/// A block that will teleport the player into or out of the End. Transparent when viewed from the bottom.
		/// </summary>
		EndPortal = 119,

		/// <summary>
		/// Found inside Strongholds in a 12-block ring shape. When all have been activated with Eye of Ender, the End Portal activates.
		/// </summary>
		EndPortalFrame = 120,

		/// <summary>
		/// Naturally found in the End. High blast resistance. Sometimes called "White Stone".
		/// </summary>
		EndStone = 121,

		/// <summary>
		/// Dropped after defeating the Ender Dragon. Tricky to collect. Some consider it to be the rarest block in the game. 
		/// </summary>
		DragonEgg = 122,

		/// <summary>
		/// A redstone lamp in an "off" state.
		/// </summary>
		RedstoneLampOff = 123,

		/// <summary>
		/// A redstone lamp in an "on" state.
		/// </summary>
		RedstoneLamp = 124,

		/// <summary>
		/// A double slab made of wood.
		/// </summary>
		WoodenDoubleSlab = 125,

		/// <summary>
		/// A half-slab made of wood.
		/// </summary>
		WoodenSlab = 126,

		/// <summary>
		/// Found naturally on jungle trees. Can be harvested and replanted. Drops three cocoa beans when harvested.
		/// </summary>
		CocoaPlant = 127,

		/// <summary>
		/// Stairs made of sandstone.
		/// </summary>
		SandstoneStairs = 128,

		/// <summary>
		/// Found deep underground in certain biomes. Drops one emerald when mined with an iron or diamond pickaxe, unless a fortune pick is used.
		/// </summary>
		EmeraldOre = 129,

		/// <summary>
		/// Used for universal storage. Works across multiple dimensions. Explosions do no damage to it whatsoever. Even if the chest is broken, the items stay locked inside.
		/// </summary>
		EnderChest = 130,

		/// <summary>
		/// A type of switch connected by tripwire. Activated by entities, and can only be disabled while using a pair of shears. 
		/// </summary>
		TripwireHook = 131,

		/// <summary>
		/// String placed between two tripwire hooks.
		/// </summary>
		Tripwire = 132,

		/// <summary>
		/// A block crafted out of nine pieces of Emerald. Good for compact storage and decoration.
		/// </summary>
		EmeraldBlock = 133,

		/// <summary>
		/// Stairs made out of spruce wood.
		/// </summary>
		SpruceWoodStairs = 134,

		/// <summary>
		/// Stairs made out of birch wood.
		/// </summary>
		BirchWoodStairs = 135,

		/// <summary>
		/// Stairs made out of jungle wood.
		/// </summary>
		JungleWoodStairs = 136,

		/// <summary>
		/// Used to support Adventure mode. Can only be placed by admins. It functions as a direct line to the server when powered by redstone.
		/// </summary>
		CommandBlock = 137,

		/// <summary>
		/// Gives temporary buffs to the player. May also provide buffs for multiple players if chosen in the GUI.
		/// </summary>
		BeaconBlock = 138,

		/// <summary>
		/// A decorative block, used for an alternative to fences.
		/// </summary>
		CobblestoneWall = 139,

		/// <summary>
		/// Decorative block that allows certain plants to be placed inside.
		/// </summary>
		FlowerPot = 140,

		/// <summary>
		/// A food item that can be planted and consumed. Restores four hunger points.
		/// </summary>
		Carrot = 141,

		/// <summary>
		/// A food item that can be planted and consumed. Restores one hunger point.
		/// </summary>
		Potato = 142,

		/// <summary>
		/// One variation of the button switch. Can be activated with arrows. 
		/// </summary>
		WoodenButton = 143,

		/// <summary>
		/// Decorative block that comes in five variations. Wither heads are needed to craft the Wither boss mob.
		/// </summary>
		Head = 144
	}
}
