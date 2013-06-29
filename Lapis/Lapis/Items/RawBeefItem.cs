using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class RawBeefItem : EnchantableItem, IConsumableItem
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.RawBeef</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.RawBeef; }
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
		/// Creates a new raw beef item
		/// </summary>
		public RawBeefItem ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new raw beef item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected RawBeefItem (short data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new raw beef item with no enchantments
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public RawBeefItem (string name, IEnumerable<string> lore)
			: base(0, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new raw beef item with no enchantments
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected RawBeefItem (short data, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted raw beef item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public RawBeefItem (IEnumerable<Enchantment> enchantments)
			: base(0, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted raw beef item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected RawBeefItem (short data, IEnumerable<Enchantment> enchantments)
			: base(data, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted raw beef item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public RawBeefItem (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(0, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted raw beef item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected RawBeefItem (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new raw beef item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public RawBeefItem (Node node)
			: base(node)
		{
			// ...
		}
	}
}
