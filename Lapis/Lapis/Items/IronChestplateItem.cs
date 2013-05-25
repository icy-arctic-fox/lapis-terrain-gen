using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class IronChestplateItem : IronArmor
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.IronChestplate</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.IronChestplate; }
		}

		/// <summary>
		/// Type of armor
		/// </summary>
		public override ArmorType ArmorType
		{
			get { return ArmorType.Chestplate; }
		}

		/// <summary>
		/// Maximum amount of damage the armor can absorb
		/// </summary>
		public override short MaxUses
		{
			get { return 241; }
		}

		/// <summary>
		/// Amount of protection that the armor provides
		/// </summary>
		public override byte Protection
		{
			get { return 6; }
		}

		/// <summary>
		/// Creates a new iron chestplate item
		/// </summary>
		public IronChestplateItem ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron chestplate item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		public IronChestplateItem (short damage)
			: base(damage)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron chestplate item with a repair cost
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		public IronChestplateItem (short damage, int repairCost)
			: base(damage, repairCost)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron chestplate item with no enchantments
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public IronChestplateItem (string name, IEnumerable<string> lore)
			: base(name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron chestplate item with no enchantments
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public IronChestplateItem (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron chestplate item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public IronChestplateItem (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron chestplate item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public IronChestplateItem (IEnumerable<Enchantment> enchantments)
			: base(enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron chestplate item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public IronChestplateItem (short damage, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron chestplate item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public IronChestplateItem (short damage, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(damage, repairCost, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron chestplate item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public IronChestplateItem (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron chestplate item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public IronChestplateItem (short damage, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron chestplate item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public IronChestplateItem (short damage, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, repairCost, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron chestplate item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public IronChestplateItem (Node node)
			: base(node)
		{
			// ...
		}
	}
}
