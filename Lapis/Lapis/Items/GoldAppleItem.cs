using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class GoldAppleItem : EnchantableItem, IConsumableItem
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.GoldApple</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.GoldApple; }
		}

		/// <summary>
		/// Number of health points added by consuming the item
		/// </summary>
		/// <remarks>This value can be negative.</remarks>
		public int HealthModifier
		{
			get { return 4; }
		}

		/// <summary>
		/// Whether or not the apple is enchanted
		/// </summary>
		public bool EnchantedApple
		{
			get { return 0 != _data; }
		}

		/// <summary>
		/// Creates a new golden apple item
		/// </summary>
		/// <param name="enchanted">Whether or not the apple is enchanted</param>
		public GoldAppleItem (bool enchanted)
			: base(enchanted ? (short)1 : (short)0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new golden apple item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected GoldAppleItem (short data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new golden apple item with no enchantments
		/// </summary>
		/// <param name="enchanted">Whether or not the apple is enchanted</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public GoldAppleItem (bool enchanted, string name, IEnumerable<string> lore)
			: base(enchanted ? (short)1 : (short)0, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new golden apple item with no enchantments
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected GoldAppleItem (short data, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted golden apple item
		/// </summary>
		/// <param name="enchanted">Whether or not the apple is enchanted</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public GoldAppleItem (bool enchanted, IEnumerable<Enchantment> enchantments)
			: base(enchanted ? (short)0 : (short)1, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted golden apple item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldAppleItem (short data, IEnumerable<Enchantment> enchantments)
			: base(data, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted golden apple item
		/// </summary>
		/// <param name="enchanted">Whether or not the apple is enchanted</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public GoldAppleItem (bool enchanted, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchanted ? (short)1 : (short)0, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted golden apple item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GoldAppleItem (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new golden apple item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public GoldAppleItem (Node node)
			: base(node)
		{
			// ...
		}
	}
}
