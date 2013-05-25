using System;
using System.Collections.Generic;
using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	public class FlintAndSteelItem : DamageableItem
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		/// <remarks>This value is always ItemType.FlintAndSteel</remarks>
		public override short ItemId
		{
			get { return (short)ItemType.FlintAndSteel; }
		}

		/// <summary>
		/// Maximum number of times that the item can be used
		/// </summary>
		public override short MaxUses
		{
			get { return 65; }
		}

		/// <summary>
		/// Creates a new flint and steel item
		/// </summary>
		public FlintAndSteelItem ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new flint and steel item
		/// </summary>
		/// <param name="uses">Amount of times the item has been used</param>
		protected FlintAndSteelItem (short uses)
			: base(uses)
		{
			// ...
		}

		/// <summary>
		/// Creates a new flint and steel item with no enchantments
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		public FlintAndSteelItem (string name, IEnumerable<string> lore)
			: base(name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new flint and steel item with no enchantments
		/// </summary>
		/// <param name="uses">Amount of times the item has been used</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected FlintAndSteelItem (short uses, string name, IEnumerable<string> lore)
			: base(uses, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted flint and steel item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public FlintAndSteelItem (IEnumerable<Enchantment> enchantments)
			: base(enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted flint and steel item
		/// </summary>
		/// <param name="uses">Amount of times the item has been used</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected FlintAndSteelItem (short uses, IEnumerable<Enchantment> enchantments)
			: base(uses, enchantments)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted flint and steel item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		public FlintAndSteelItem (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new enchanted flint and steel item
		/// </summary>
		/// <param name="uses">Amount of times the item has been used</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected FlintAndSteelItem (short uses, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(uses, enchantments, name, lore)
		{
			// ...
		}

		/// <summary>
		/// Creates a new flint and steel item from its NBT node data
		/// </summary>
		/// <param name="node">Node that contains information about the item</param>
		/// <remarks>The node data returned by GetNbtData() is the format expected for <paramref name="node"/>.</remarks>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		public FlintAndSteelItem (Node node)
			: base(node)
		{
			// ...
		}
	}
}
