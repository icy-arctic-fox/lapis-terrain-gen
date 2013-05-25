using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Armor made out of gold
	/// </summary>
	public abstract class GoldArmor : DamageableItem, IArmorItem
	{
		/// <summary>
		/// The item's armor type
		/// </summary>
		public abstract ArmorType ArmorType { get; }

		/// <summary>
		/// Amount of protection that the armor provides (0 - 20)
		/// </summary>
		public abstract byte Protection { get; }

		/// <summary>
		/// Creates a new gold armor item
		/// </summary>
		protected GoldArmor ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new gold armor item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected GoldArmor (short damage)
			: base(damage)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gold armor item with a repair cost
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		protected GoldArmor (short damage, int repairCost)
			: base(damage, repairCost)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gold armor item with tag data
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected GoldArmor (string name, IEnumerable<string> lore)
			: base(name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gold armor item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected GoldArmor (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gold armor item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected GoldArmor (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted gold armor item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldArmor (IEnumerable<Enchantment> enchantments)
			: base(enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted gold armor item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldArmor (short damage, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted gold armor item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldArmor (short damage, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(damage, repairCost, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted gold armor item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldArmor (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted gold armor item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldArmor (short damage, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted gold armor item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldArmor (short damage, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, repairCost, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new gold armor item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected GoldArmor (Node node)
			: base(node)
		{
			// ...
		}
	}
}
