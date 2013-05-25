using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class GlisteringMelonItem : EnchantableItem
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.GlisteringMelon</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.GlisteringMelon; }
		}

		/// <summary>
		/// Creates a new glistering melon item
		/// </summary>
		public GlisteringMelonItem ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new glistering melon item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected GlisteringMelonItem (short data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new glistering melon item with no enchantments
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public GlisteringMelonItem (string name, IEnumerable<string> lore)
			: base(0, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new glistering melon item with no enchantments
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected GlisteringMelonItem (short data, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted glistering melon item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public GlisteringMelonItem (IEnumerable<Enchantment> enchantments)
			: base(0, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted glistering melon item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GlisteringMelonItem (short data, IEnumerable<Enchantment> enchantments)
			: base(data, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted glistering melon item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public GlisteringMelonItem (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(0, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted glistering melon item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected GlisteringMelonItem (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new glistering melon item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public GlisteringMelonItem (Node node)
			: base(node)
		{
			// ...
		}
	}
}