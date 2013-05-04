using System;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that can be damaged
	/// </summary>
	public abstract class DamageableItem : EnchantableItem
	{
		#region Properties
		/// <summary>
		/// Amount of damage done to the item
		/// </summary>
		public short Damage
		{
			get { return _data; }
		}

		/// <summary>
		/// Maximum number of times the item can be used
		/// </summary>
		public abstract short MaxUses { get; }

		/// <summary>
		/// Number of times left that the item can be used
		/// </summary>
		public short UsesRemaining
		{
			get { return (short)(MaxUses - Damage); }
		}

		/// <summary>
		/// Number of levels (in addition to the base cost) required to repair the item
		/// </summary>
		public abstract int RepairCost { get; }
		#endregion

		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected DamageableItem (short damage)
			: base(damage)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected DamageableItem (short damage, string name, string[] lore)
			: base(damage, name, lore)
		{
			throw new NotImplementedException();
		}

		// TODO: Add constructor for enchanted items (requires enchanted values first)

		#region Serialization
		/// <summary>
		/// Creates a new item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected DamageableItem (Node node)
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
