using System;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item made out of stone
	/// </summary>
	public abstract class StoneItem : DamageableItem
	{
		#region Properties
		#endregion

		/// <summary>
		/// Creates a new stone item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected StoneItem (short damage)
			: base(damage)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new stone item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected StoneItem (short damage, string name, string[] lore)
			: base(damage, name, lore)
		{
			throw new NotImplementedException();
		}

		// TODO: Add constructor for enchanted items (requires enchanted values first)

		#region Serialization
		/// <summary>
		/// Creates a new stone item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected StoneItem (Node node)
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
