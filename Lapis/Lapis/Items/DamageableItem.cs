using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that can be damaged
	/// </summary>
	public abstract class DamageableItem : EnchantableItem
	{
		private const int RepairLevelMultiplier = 2;

		private readonly int _cost;

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
		public int RepairCost
		{
			get { return _cost; }
		}
		#endregion

		/// <summary>
		/// Creates a new item
		/// </summary>
		protected DamageableItem ()
			: base(0)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		protected DamageableItem (short damage)
			: base(damage)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new item with a repair cost
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		protected DamageableItem (short damage, int repairCost)
			: base(damage)
		{
			_cost = repairCost;
		}

		/// <summary>
		/// Creates a new item with tag data
		/// </summary>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected DamageableItem (string name, IEnumerable<string> lore)
			: base(0, name, lore)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected DamageableItem (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new item with tag data
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected DamageableItem (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			_cost = repairCost;
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected DamageableItem (IEnumerable<Enchantment> enchantments)
			: base(0, enchantments)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected DamageableItem (short damage, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected DamageableItem (short damage, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			_cost = repairCost;
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected DamageableItem (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(0, enchantments, name, lore)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected DamageableItem (short damage, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			_cost = 0;
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected DamageableItem (short damage, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			_cost = repairCost;
		}

		#region Serialization
		/// <summary>
		/// Creates a new item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected DamageableItem (Node node)
			: base(node)
		{
			var tagNode = ((CompoundNode)node)[TagNodeName] as CompoundNode;
			_cost = validateCostNode(tagNode);
		}

		#region Node names
		private const string CostNodeName = "RepairCost";
		#endregion

		#region Validation
		private static int validateCostNode (CompoundNode tagNode)
		{
			if(tagNode.Contains(CostNodeName))
			{
				var costNode = tagNode[CostNodeName] as IntNode;
				if(null != costNode)
					return costNode.Value;
			}
			return 0;
		}
		#endregion

		#region Construction
		/// <summary>
		/// Inserts the repair information into the tag of the item node
		/// </summary>
		/// <param name="tagNode">Node to insert into</param>
		protected override void InsertIntoTagData (CompoundNode tagNode)
		{
			base.InsertIntoTagData(tagNode);
			tagNode.Add(new IntNode(CostNodeName, _cost));
		}
		#endregion
		#endregion
	}
}
