using System;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Armor made out of chainmail
	/// </summary>
	public abstract class ChainArmor : DamageableItem, IArmorItem
	{
		#region Properties
		#endregion

		/// <summary>
		/// Creates a new chainmail armor item
		/// </summary>
		/// <param name="damage">Amount of damage the armor has taken</param>
		protected ChainArmor (short damage)
			: base(damage)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new chainmail armor item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the armor has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected ChainArmor (short damage, string name, string[] lore)
			: base(damage, name, lore)
		{
			throw new NotImplementedException();
		}

		// TODO: Add constructor for enchanted items (requires enchanted values first)

		#region Serialization
		/// <summary>
		/// Creates a new chainmail armor item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected ChainArmor (Node node)
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
