using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// A tool made out of iron
	/// </summary>
	public abstract class IronTool : DamageableItem, IToolItem
	{
		/// <summary>
		/// Maximum number of times the item can be used
		/// </summary>
		public const int MaxUseCount = 251;

		/// <summary>
		/// Maximum number of times the item can be used
		/// </summary>
		public override short MaxUses
		{
			get { return MaxUseCount; }
		}

		/// <summary>
		/// Creates a new iron tool
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected IronTool (short damage)
			: base(damage)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron tool with a repair cost
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		protected IronTool (short damage, int repairCost)
			: base(damage, repairCost)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron tool with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected IronTool (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron tool with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected IronTool (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron tool
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected IronTool (short data, IEnumerable<Enchantment> enchantments)
			: base(data, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron tool
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected IronTool (short data, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(data, repairCost, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron tool
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected IronTool (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted iron tool
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected IronTool (short data, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, repairCost, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron tool from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected IronTool (Node node)
			: base(node)
		{
			// ...
		}
	}
}
