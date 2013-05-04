using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Armor made out of leather
	/// </summary>
	public abstract class LeatherArmor : DamageableItem, IArmorItem, IDyeableItem
	{
		private readonly int _color;

		/// <summary>
		/// Color value for the leather armor
		/// </summary>
		/// <remarks>The color is in the format RGB using the formula:
		/// Red &lt;&lt; 16 | Green &lt;&lt; 8 | Blue</remarks>
		public int Color
		{
			get { return _color; }
		}

		/// <summary>
		/// Amount of red in the leather armor
		/// </summary>
		public byte Red
		{
			get { return (byte)((_color >> 16) & 0xff); }
		}

		/// <summary>
		/// Amount of green in the leather armor
		/// </summary>
		public byte Green
		{
			get { return (byte)((_color >> 8) & 0xff); }
		}

		/// <summary>
		/// Amount of blue in the leather armor
		/// </summary>
		public byte Blue
		{
			get { return (byte)(_color & 0xff); }
		}

		/// <summary>
		/// The item's armor type
		/// </summary>
		public abstract ArmorType ArmorType { get; }

		/// <summary>
		/// Creates a new leather armor item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected LeatherArmor (short damage)
			: base(damage)
		{
			// ...
		}

		/// <summary>
		/// Creates a new leather armor item with a repair cost
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		protected LeatherArmor (short damage, int repairCost)
			: base(damage, repairCost)
		{
			// ...
		}

		/// <summary>
		/// Creates a new leather armor item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected LeatherArmor (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new leather armor item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected LeatherArmor (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short data, IEnumerable<Enchantment> enchantments)
			: base(data, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short data, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(data, repairCost, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short data, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, repairCost, enchantments, name, lore)
		{
			// ...
		}

		#region Serialization
		/// <summary>
		/// Creates a new leather armor item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected LeatherArmor (Node node)
			: base(node)
		{
			// TODO: Implement color
		}

		#region Node names
		private const string ColorNodeName = "color";
		#endregion

		#region Validation
		#endregion

		#region Construction
		#endregion
		#endregion
	}
}
