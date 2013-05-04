using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that can be enchanted
	/// </summary>
	public abstract class EnchantableItem : NameableItem
	{
		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected EnchantableItem (short data)
			: base(data)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new item with tag data
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected EnchantableItem (short data, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			throw new NotImplementedException();
		}

		// TODO: Add constructor for enchanted items (requires enchanted values first)

		#region Serialization
		/// <summary>
		/// Creates a new item with enchanted data from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected EnchantableItem (Node node)
			: base(node)
		{
			throw new NotImplementedException();
		}

		#region Validation
		#endregion

		#region Construction
		#endregion
		#endregion
	}
}
