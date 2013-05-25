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

foreach my $item (@items)	{
	my $friendly  = idToFriendly($item);
	my $className = $item . 'Item';
	my $fileName  = OUTPUT_DIR . $className . '.cs';
	open(ITEM, '>', $fileName) or die "Failed to create $fileName - $!\n";
	print<<END_ITEM_CLASS;
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
	}
}
END_ITEM_CLASS
	close(ITEM);
}

sub idToFriendly	{
	my($id) = @_;

	my @parts = $id =~ /[A-Z](?:[A-Z]+|[a-z]*)(?=$|[A-Z])/g;
	return "@parts";
}
