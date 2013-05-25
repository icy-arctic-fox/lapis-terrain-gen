using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class DiamondBootsItem : DiamondArmor
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.DiamondBoots</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.DiamondBoots; }
		}

		/// <summary>
		/// Type of armor
		/// </summary>
		public override ArmorType ArmorType
		{
			get { return ArmorType.Boots; }
		}

		/// <summary>
		/// Amount of protection that the armor provides
		/// </summary>
		public override byte Protection
		{
			get { return 3; }
		}

		/// <summary>
		/// Maximum number of times that the armor can be hit
		/// </summary>
		public override short MaxUses
		{
			get { return 430; }
		}

		/// <summary>
		/// Creates a new diamond boots item
		/// </summary>
		public DiamondBootsItem ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new diamond boots item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		public DiamondBootsItem (short damage)
			: base(damage)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diamond boots item with a repair cost
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		public DiamondBootsItem (short damage, int repairCost)
			: base(damage, repairCost)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diamond boots item with no enchantments
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public DiamondBootsItem (string name, IEnumerable<string> lore)
			: base(name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diamond boots item with no enchantments
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public DiamondBootsItem (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diamond boots item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public DiamondBootsItem (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted diamond boots item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public DiamondBootsItem (IEnumerable<Enchantment> enchantments)
			: base(enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted diamond boots item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public DiamondBootsItem (short damage, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted diamond boots item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public DiamondBootsItem (short damage, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(damage, repairCost, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted diamond boots item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public DiamondBootsItem (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted diamond boots item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public DiamondBootsItem (short damage, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted diamond boots item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public DiamondBootsItem (short damage, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, repairCost, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new diamond boots item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public DiamondBootsItem (Node node)
			: base(node)
		{
			// ...
		}
	}
}