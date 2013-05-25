using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Manages the creation of new items
	/// </summary>
	public static class ItemFactory
	{
		#region Registry
		/// <summary>
		/// Describes a method that creates an item
		/// </summary>
		/// <param name="itemData">Node that contains the information about the item</param>
		/// <returns>A block</returns>
		public delegate Item ItemCreation (Node itemData);

		/// <summary>
		/// Contains delegates for constructing items for all known types
		/// </summary>
		private static readonly Dictionary<ItemType, ItemCreation> _knownItemTypes = new Dictionary<ItemType, ItemCreation>();

		/// <summary>
		/// Registers all default item types
		/// </summary>
		static ItemFactory ()
		{
			lock(_knownItemTypes)
			{
				_knownItemTypes[ItemType.IronShovel]         = itemData => new IronShovelItem(itemData);
				_knownItemTypes[ItemType.IronPickaxe]        = itemData => new IronPickaxeItem(itemData);
				_knownItemTypes[ItemType.IronAxe]            = itemData => new IronAxeItem(itemData);
				_knownItemTypes[ItemType.FlintAndSteel]      = itemData => new FlintAndSteelItem(itemData);
				_knownItemTypes[ItemType.Apple]              = itemData => new AppleItem(itemData);
				_knownItemTypes[ItemType.Bow]                = itemData => new BowItem(itemData);
				_knownItemTypes[ItemType.Arrow]              = itemData => new ArrowItem(itemData);
				_knownItemTypes[ItemType.Coal]               = itemData => new CoalItem(itemData);
				_knownItemTypes[ItemType.Diamond]            = itemData => new DiamondItem(itemData);
				_knownItemTypes[ItemType.IronIngot]          = itemData => new IronIngotItem(itemData);
				_knownItemTypes[ItemType.GoldIngot]          = itemData => new GoldIngotItem(itemData);
				_knownItemTypes[ItemType.IronSword]          = itemData => new IronSwordItem(itemData);
				_knownItemTypes[ItemType.WoodSword]          = itemData => new WoodSwordItem(itemData);
				_knownItemTypes[ItemType.WoodShovel]         = itemData => new WoodShovelItem(itemData);
				_knownItemTypes[ItemType.WoodPickaxe]        = itemData => new WoodPickaxeItem(itemData);
				_knownItemTypes[ItemType.WoodAxe]            = itemData => new WoodBaseAxeItem(itemData);
				_knownItemTypes[ItemType.StoneSword]         = itemData => new StoneSwordItem(itemData);
				_knownItemTypes[ItemType.StoneShovel]        = itemData => new StoneShovelItem(itemData);
				_knownItemTypes[ItemType.StonePickaxe]       = itemData => new StonePickaxeItem(itemData);
				_knownItemTypes[ItemType.StoneAxe]           = itemData => new StoneAxeItem(itemData);
				_knownItemTypes[ItemType.DiamondSword]       = itemData => new DiamondSwordItem(itemData);
				_knownItemTypes[ItemType.DiamondShovel]      = itemData => new DiamondShovelItem(itemData);
				_knownItemTypes[ItemType.DiamondPickaxe]     = itemData => new DiamondPickaxeItem(itemData);
				_knownItemTypes[ItemType.DiamondAxe]         = itemData => new DiamondAxeItem(itemData);
				_knownItemTypes[ItemType.Stick]              = itemData => new StickItem(itemData);
				_knownItemTypes[ItemType.Bowl]               = itemData => new BowlItem(itemData);
				_knownItemTypes[ItemType.MushroomStew]       = itemData => new MushroomStewItem(itemData);
				_knownItemTypes[ItemType.GoldSword]          = itemData => new GoldSwordItem(itemData);
				_knownItemTypes[ItemType.GoldShovel]         = itemData => new GoldShovelItem(itemData);
				_knownItemTypes[ItemType.GoldPickaxe]        = itemData => new GoldPickaxeItem(itemData);
				_knownItemTypes[ItemType.GoldAxe]            = itemData => new GoldAxeItem(itemData);
				_knownItemTypes[ItemType.Feather]            = itemData => new FeatherItem(itemData);
				_knownItemTypes[ItemType.Gunpowder]          = itemData => new GunpowderItem(itemData);
				_knownItemTypes[ItemType.WoodHoe]            = itemData => new WoodHoeItem(itemData);
				_knownItemTypes[ItemType.StoneHoe]           = itemData => new StoneHoeItem(itemData);
				_knownItemTypes[ItemType.IronHoe]            = itemData => new IronHoeItem(itemData);
				_knownItemTypes[ItemType.DiamondHoe]         = itemData => new DiamondHoeItem(itemData);
				_knownItemTypes[ItemType.GoldHoe]            = itemData => new GoldHoeItem(itemData);
				_knownItemTypes[ItemType.Seeds]              = itemData => new SeedsItem(itemData);
				_knownItemTypes[ItemType.Wheat]              = itemData => new WheatItem(itemData);
				_knownItemTypes[ItemType.Bread]              = itemData => new BreadItem(itemData);
				_knownItemTypes[ItemType.LeatherCap]         = itemData => new LeatherCapItem(itemData);
				_knownItemTypes[ItemType.LeatherTunic]       = itemData => new LeatherTunicItem(itemData);
				_knownItemTypes[ItemType.LeatherPants]       = itemData => new LeatherPantsItem(itemData);
				_knownItemTypes[ItemType.LeatherBoots]       = itemData => new LeatherBootsItem(itemData);
				_knownItemTypes[ItemType.ChainHelmet]        = itemData => new ChainHelmetItem(itemData);
				_knownItemTypes[ItemType.ChainChestplate]    = itemData => new ChainChestplateItem(itemData);
				_knownItemTypes[ItemType.ChainLeggings]      = itemData => new ChainLeggingsItem(itemData);
				_knownItemTypes[ItemType.ChainBoots]         = itemData => new ChainBootsItem(itemData);
				_knownItemTypes[ItemType.IronHelmet]         = itemData => new IronHelmetItem(itemData);
				_knownItemTypes[ItemType.IronChestplate]     = itemData => new IronChestplateItem(itemData);
				_knownItemTypes[ItemType.IronLeggings]       = itemData => new IronLeggingsItem(itemData);
				_knownItemTypes[ItemType.IronBoots]          = itemData => new IronBootsItem(itemData);
				_knownItemTypes[ItemType.DiamondHelmet]      = itemData => new DiamondHelmetItem(itemData);
				_knownItemTypes[ItemType.DiamondChestplate]  = itemData => new DiamondChestplateItem(itemData);
				_knownItemTypes[ItemType.DiamondLeggings]    = itemData => new DiamondLeggingsItem(itemData);
				_knownItemTypes[ItemType.DiamondBoots]       = itemData => new DiamondBootsItem(itemData);
				_knownItemTypes[ItemType.GoldHelmet]         = itemData => new GoldHelmetItem(itemData);
				_knownItemTypes[ItemType.GoldChestplate]     = itemData => new GoldChestplateItem(itemData);
				_knownItemTypes[ItemType.GoldLeggings]       = itemData => new GoldLeggingsItem(itemData);
				_knownItemTypes[ItemType.GoldBoots]          = itemData => new GoldBootsItem(itemData);
				_knownItemTypes[ItemType.Flint]              = itemData => new FlintItem(itemData);
				_knownItemTypes[ItemType.RawPorkchop]        = itemData => new RawPorkchopItem(itemData);
				_knownItemTypes[ItemType.CookedPorkchop]     = itemData => new CookedPorkchopItem(itemData);
				_knownItemTypes[ItemType.Painting]           = itemData => new PaintingItem(itemData);
				_knownItemTypes[ItemType.GoldApple]          = itemData => new GoldAppleItem(itemData);
				_knownItemTypes[ItemType.Sign]               = itemData => new SignItem(itemData);
				_knownItemTypes[ItemType.WoodDoor]           = itemData => new WoodDoorItem(itemData);
				_knownItemTypes[ItemType.Bucket]             = itemData => new BucketItem(itemData);
				_knownItemTypes[ItemType.WaterBucket]        = itemData => new WaterBucketItem(itemData);
				_knownItemTypes[ItemType.LavaBucket]         = itemData => new LavaBucketItem(itemData);
				_knownItemTypes[ItemType.Minecart]           = itemData => new MinecartItem(itemData);
				_knownItemTypes[ItemType.Saddle]             = itemData => new SaddleItem(itemData);
				_knownItemTypes[ItemType.IronDoor]           = itemData => new IronDoorItem(itemData);
				_knownItemTypes[ItemType.Redstone]           = itemData => new RedstoneItem(itemData);
				_knownItemTypes[ItemType.Snowball]           = itemData => new SnowballItem(itemData);
				_knownItemTypes[ItemType.Boat]               = itemData => new BoatItem(itemData);
				_knownItemTypes[ItemType.Leather]            = itemData => new LeatherItem(itemData);
				_knownItemTypes[ItemType.Milk]               = itemData => new MilkItem(itemData);
				_knownItemTypes[ItemType.Brick]              = itemData => new BrickItem(itemData);
				_knownItemTypes[ItemType.Clay]               = itemData => new ClayItem(itemData);
				_knownItemTypes[ItemType.SugarCane]          = itemData => new SugarCaneItem(itemData);
				_knownItemTypes[ItemType.Paper]              = itemData => new PaperItem(itemData);
				_knownItemTypes[ItemType.Book]               = itemData => new BookItem(itemData);
				_knownItemTypes[ItemType.Slimeball]          = itemData => new SlimeballItem(itemData);
				_knownItemTypes[ItemType.ChestCart]          = itemData => new ChestCartItem(itemData);
				_knownItemTypes[ItemType.FurnaceCart]        = itemData => new FurnaceCartItem(itemData);
				_knownItemTypes[ItemType.Egg]                = itemData => new EggItem(itemData);
				_knownItemTypes[ItemType.Compass]            = itemData => new CompassItem(itemData);
				_knownItemTypes[ItemType.FishingRod]         = itemData => new FishingRodItem(itemData);
				_knownItemTypes[ItemType.Clock]              = itemData => new ClockItem(itemData);
				_knownItemTypes[ItemType.GlowstoneDust]      = itemData => new GlowstoneDustItem(itemData);
				_knownItemTypes[ItemType.RawFish]            = itemData => new RawFishItem(itemData);
				_knownItemTypes[ItemType.CookedFish]         = itemData => new CookedFishItem(itemData);
				_knownItemTypes[ItemType.Dye]                = itemData => new DyeItem(itemData);
				_knownItemTypes[ItemType.Bone]               = itemData => new BoneItem(itemData);
				_knownItemTypes[ItemType.Sugar]              = itemData => new SugarItem(itemData);
				_knownItemTypes[ItemType.Cake]               = itemData => new CakeItem(itemData);
				_knownItemTypes[ItemType.Bed]                = itemData => new BedItem(itemData);
				_knownItemTypes[ItemType.Diode]              = itemData => new DiodeItem(itemData);
				_knownItemTypes[ItemType.Cookie]             = itemData => new CookieItem(itemData);
				_knownItemTypes[ItemType.Map]                = itemData => new MapItem(itemData);
				_knownItemTypes[ItemType.Shears]             = itemData => new ShearsItem(itemData);
				_knownItemTypes[ItemType.Melon]              = itemData => new MelonItem(itemData);
				_knownItemTypes[ItemType.PumpkinSeeds]       = itemData => new PumpkinSeedsItem(itemData);
				_knownItemTypes[ItemType.MelonSeeds]         = itemData => new MelonSeedsItem(itemData);
				_knownItemTypes[ItemType.RawBeef]            = itemData => new RawBeefItem(itemData);
				_knownItemTypes[ItemType.Steak]              = itemData => new SteakItem(itemData);
				_knownItemTypes[ItemType.RawChicken]         = itemData => new RawChickenItem(itemData);
				_knownItemTypes[ItemType.CookedChicked]      = itemData => new CookedChickedItem(itemData);
				_knownItemTypes[ItemType.RottenFlesh]        = itemData => new RottenFleshItem(itemData);
				_knownItemTypes[ItemType.EnderPearl]         = itemData => new EnderPearlItem(itemData);
				_knownItemTypes[ItemType.BlazeRod]           = itemData => new BlazeRodItem(itemData);
				_knownItemTypes[ItemType.GhastTear]          = itemData => new GhastTearItem(itemData);
				_knownItemTypes[ItemType.GoldNugget]         = itemData => new GoldNuggetItem(itemData);
				_knownItemTypes[ItemType.NetherWart]         = itemData => new NetherWartItem(itemData);
				_knownItemTypes[ItemType.Potion]             = itemData => new PotionItem(itemData);
				_knownItemTypes[ItemType.Bottle]             = itemData => new BottleItem(itemData);
				_knownItemTypes[ItemType.SpiderEye]          = itemData => new SpiderEyeItem(itemData);
				_knownItemTypes[ItemType.FermentedSpiderEye] = itemData => new FermentedSpiderEyeItem(itemData);
				_knownItemTypes[ItemType.BlazePowder]        = itemData => new BlazePowderItem(itemData);
				_knownItemTypes[ItemType.MagmaCream]         = itemData => new MagmaCreamItem(itemData);
				_knownItemTypes[ItemType.BrewingStand]       = itemData => new BrewingStandItem(itemData);
				_knownItemTypes[ItemType.Cauldron]           = itemData => new CauldronItem(itemData);
				_knownItemTypes[ItemType.EyeOfEnder]         = itemData => new EyeOfEnderItem(itemData);
				_knownItemTypes[ItemType.GlisteringMelon]    = itemData => new GlisteringMelonItem(itemData);
				_knownItemTypes[ItemType.SpawnEgg]           = itemData => new SpawnEggItem(itemData);
				_knownItemTypes[ItemType.EnchantingBottle]   = itemData => new EnchantingBottleItem(itemData);
				_knownItemTypes[ItemType.FireCharge]         = itemData => new FireChargeItem(itemData);
				_knownItemTypes[ItemType.BookAndQuill]       = itemData => new BookAndQuillItem(itemData);
				_knownItemTypes[ItemType.WrittenBook]        = itemData => new WrittenBookItem(itemData);
				_knownItemTypes[ItemType.Emerald]            = itemData => new EmeraldItem(itemData);
				_knownItemTypes[ItemType.ItemFrame]          = itemData => new ItemFrameItem(itemData);
				_knownItemTypes[ItemType.FlowerPot]          = itemData => new FlowerPotItem(itemData);
				_knownItemTypes[ItemType.Carrot]             = itemData => new CarrotItem(itemData);
				_knownItemTypes[ItemType.Potato]             = itemData => new PotatoItem(itemData);
				_knownItemTypes[ItemType.BakedPotato]        = itemData => new BakedPotatoItem(itemData);
				_knownItemTypes[ItemType.PoisonousPotato]    = itemData => new PoisonousPotatoItem(itemData);
				_knownItemTypes[ItemType.EmptyMap]           = itemData => new EmptyMapItem(itemData);
				_knownItemTypes[ItemType.GoldCarrot]         = itemData => new GoldCarrotItem(itemData);
				_knownItemTypes[ItemType.MobHead]            = itemData => new MobHeadItem(itemData);
				_knownItemTypes[ItemType.CarrotOnAStick]     = itemData => new CarrotOnAStickItem(itemData);
				_knownItemTypes[ItemType.NetherStar]         = itemData => new NetherStarItem(itemData);
				_knownItemTypes[ItemType.PumpkinPie]         = itemData => new PumpkinPieItem(itemData);
				_knownItemTypes[ItemType.FireworkRocket]     = itemData => new FireworkRocketItem(itemData);
				_knownItemTypes[ItemType.FireworkStar]       = itemData => new FireworkStarItem(itemData);
				_knownItemTypes[ItemType.EnchantedBook]      = itemData => new EnchantedBookItem(itemData);
				_knownItemTypes[ItemType.Comparator]         = itemData => new ComparatorItem(itemData);
				_knownItemTypes[ItemType.NetherBrick]        = itemData => new NetherBrickItem(itemData);
				_knownItemTypes[ItemType.NetherQuartz]       = itemData => new NetherQuartzItem(itemData);
				_knownItemTypes[ItemType.TntCart]            = itemData => new TntCartItem(itemData);
				_knownItemTypes[ItemType.HopperCart]         = itemData => new HopperCartItem(itemData);
				_knownItemTypes[ItemType.Disc13]             = itemData => new Disc13Item(itemData);
				_knownItemTypes[ItemType.CatDisc]            = itemData => new CatDiscItem(itemData);
				_knownItemTypes[ItemType.BlocksDisc]         = itemData => new BlocksDiscItem(itemData);
				_knownItemTypes[ItemType.ChirpDisc]          = itemData => new ChirpDiscItem(itemData);
				_knownItemTypes[ItemType.FarDisc]            = itemData => new FarDiscItem(itemData);
				_knownItemTypes[ItemType.MallDisc]           = itemData => new MallDiscItem(itemData);
				_knownItemTypes[ItemType.MellohiDisc]        = itemData => new MellohiDiscItem(itemData);
				_knownItemTypes[ItemType.StalDisc]           = itemData => new StalDiscItem(itemData);
				_knownItemTypes[ItemType.StradDisc]          = itemData => new StradDiscItem(itemData);
				_knownItemTypes[ItemType.WardDisc]           = itemData => new WardDiscItem(itemData);
				_knownItemTypes[ItemType.Disc11]             = itemData => new Disc11Item(itemData);
				_knownItemTypes[ItemType.WaitDisc]           = itemData => new WaitDiscItem(itemData);
			}
		}

		/// <summary>
		/// Registers a new type of item
		/// </summary>
		/// <param name="type">item type</param>
		/// <param name="constructor">Method that statically creates the item</param>
		public static void Register (ItemType type, ItemCreation constructor)
		{
			if(null == constructor)
				throw new ArgumentNullException("constructor", "The item constructor method can't be null.");

			lock(_knownItemTypes)
				_knownItemTypes[type] = constructor;
		}
		#endregion

		#region Factory methods
		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="type">Type of item to statically create</param>
		/// <param name="node">Node containing the information about the item</param>
		/// <returns>A block</returns>
		public static Item Create (ItemType type, Node node)
		{
			var constructor = _knownItemTypes[type]; // This isn't locked because it would reduce concurrency down to none
			return (null == constructor) ? null : constructor(node);
		}
		#endregion
	}
}
