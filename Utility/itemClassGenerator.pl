#!/usr/bin/perl -w
use strict;

use constant OUTPUT_DIR => 'output/';

my @items = qw(
IronShovel
IronPickaxe
IronAxe
FlintAndSteel
Apple
Bow
Arrow
Coal
Diamond
IronIngot
GoldIngot
IronSword
WoodSword
WoodShovel
WoodPickaxe
WoodAxe
StoneSword
StoneShovel
StonePickaxe
StoneAxe
DiamondSword
DiamondShovel
DiamondPickaxe
DiamondAxe
Stick
Bowl
MushroomStew
GoldSword
GoldShovel
GoldPickaxe
GoldAxe
Feather
Gunpowder
WoodHoe
StoneHoe
IronHoe
DiamondHoe
GoldHoe
Seeds
Wheat
Bread
LeatherCap
LeatherTunic
LeatherPants
LeatherBoots
ChainHelmet
ChainChestplate
ChainLeggings
ChainBoots
IronHelmet
IronChestplate
IronLeggings
IronBoots
DiamondHelmet
DiamondChestplate
DiamondLeggings
DiamondBoots
GoldHelmet
GoldChestplate
GoldLeggings
GoldBoots
Flint
RawPorkchop
CookedPorkchop
Painting
GoldApple
Sign
WoodDoor
Bucket
WaterBucket
LavaBucket
Minecart
Saddle
IronDoor
Redstone
Snowball
Boat
Leather
Milk
Brick
Clay
SugarCane
Paper
Book
Slimeball
ChestCart
FurnaceCart
Egg
Compass
FishingRod
Clock
GlowstoneDust
RawFish
CookedFish
Dye
Bone
Sugar
Cake
Bed
Diode
Cookie
Map
Shears
Melon
PumpkinSeeds
MelonSeeds
RawBeef
Steak
RawChicken
CookedChicked
RottenFlesh
EnderPearl
BlazeRod
GhastTear
GoldNugget
NetherWart
Potion
Bottle
SpiderEye
FermentedSpiderEye
BlazePowder
MagmaCream
BrewingStand
Cauldron
EyeOfEnder
GlisteringMelon
SpawnEgg
EnchantingBottle
FireCharge
BookAndQuill
WrittenBook
Emerald
ItemFrame
FlowerPot
Carrot
Potato
BakedPotato
PoisonousPotato
EmptyMap
GoldCarrot
MobHead
CarrotOnAStick
NetherStar
PumpkinPie
FireworkRocket
FireworkStar
EnchantedBook
Comparator
NetherBrick
NetherQuartz
TntCart
HopperCart
Disc13
CatDisc
BlocksDisc
ChirpDisc
FarDisc
MallDisc
MellohiDisc
StalDisc
StradDisc
WardDisc
Disc11
WaitDisc
);

mkdir(OUTPUT_DIR) or die "Failed to create output directory - $!\n" unless(-d OUTPUT_DIR);
open(SNIPPET, '>', OUTPUT_DIR . 'ItemFactory.cs.snippet') or die "Failed to create snippet file - $!\n";

my $maxLength = 0;
foreach my $item (@items)	{
	my $length = length($item);
	$maxLength = $length if($length > $maxLength);
}

foreach my $item (@items)	{
	my $friendly  = idToFriendly($item);
	my $className = $item . 'Item';
	my $fileName  = OUTPUT_DIR . $className . '.cs';
	open(ITEM, '>', $fileName) or die "Failed to create $fileName - $!\n";
	print ITEM<<END_ITEM_CLASS;
using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class $className : EnchantableItem
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.$item</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.$item; }
		}

		/// <summary>
		/// Creates a new $friendly item
		/// </summary>
		public $className ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new $friendly item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected $className (short data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new $friendly item with no enchantments
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public $className (string name, IEnumerable<string> lore)
			: base(0, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new $friendly item with no enchantments
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected $className (short data, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted $friendly item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public $className (IEnumerable<Enchantment> enchantments)
			: base(0, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted $friendly item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected $className (short data, IEnumerable<Enchantment> enchantments)
			: base(data, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted $friendly item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public $className (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(0, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted $friendly item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected $className (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new $friendly item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public $className (Node node)
			: base(node)
		{
			// ...
		}
	}
}
END_ITEM_CLASS
	close(ITEM);

	my $spaces = ' ' x ($maxLength - length($item));
	print SNIPPET<<END_SNIPPET;
				_knownItemTypes[ItemType.$item]$spaces = itemData => new $className(itemData);
END_SNIPPET
}
close(SNIPPET);

sub idToFriendly	{
	my($id) = @_;

	my @parts = $id =~ /[A-Z](?:[A-Z]+|[a-z]*)(?=$|[A-Z])/g;
	return lc("@parts");
}
