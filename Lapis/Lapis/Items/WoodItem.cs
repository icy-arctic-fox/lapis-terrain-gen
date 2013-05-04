using System;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item made out of wood
	/// </summary>
	public abstract class WoodItem : DamageableItem
	{
		#region Properties
		#endregion

		/// <summary>
		/// Creates a new wood item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected WoodItem (short damage)
			: base(damage)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new wood item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected WoodItem (short damage, string name, string[] lore)
			: base(damage, name, lore)
		{
			throw new NotImplementedException();
		}

		// TODO: Add constructor for enchanted items (requires enchanted values first)

		#region Serialization
		/// <summary>
		/// Creates a new wood item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected WoodItem (Node node)
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
