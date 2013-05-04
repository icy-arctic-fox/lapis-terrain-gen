using System;
using System.Collections.Generic;
using System.Linq;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that can be enchanted
	/// </summary>
	public abstract class EnchantableItem : NameableItem
	{
		private readonly Enchantment[] _enchants;

		#region Properties
		/// <summary>
		/// Whether or not the item has enchantments
		/// </summary>
		public bool HasEnchantments
		{
			get { return _enchants.Length > 0; }
		}

		/// <summary>
		/// Number of enchantments the item has
		/// </summary>
		public int EnchantmentCount
		{
			get { return _enchants.Length; }
		}

		/// <summary>
		/// Information about the enchantments on the item
		/// </summary>
		/// <remarks>Modifying the elements of this property will not change the item.</remarks>
		public Enchantment[] Enchantments
		{
			get
			{
				var enchants = new Enchantment[_enchants.Length];
				for(var i = 0; i < _enchants.Length; ++i)
					enchants[i] = _enchants[i];
				return enchants;
			}
		}
		#endregion

		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected EnchantableItem (short data)
			: base(data)
		{
			_enchants = new Enchantment[0];
		}

		/// <summary>
		/// Creates a new item with no enchantments
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected EnchantableItem (short data, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			_enchants = new Enchantment[0];
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected EnchantableItem (short data, IEnumerable<Enchantment> enchantments)
			: base(data)
		{
			if(null == enchantments)
				throw new ArgumentNullException("enchantments", "The collection of enchantments can't be null.");
			_enchants = enchantments.ToArray();
		}

		/// <summary>
		/// Creates a new enchanted item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected EnchantableItem (short data, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(data, name, lore)
		{
			if(null == enchantments)
				throw new ArgumentNullException("enchantments", "The collection of enchantments can't be null.");
			_enchants = enchantments.ToArray();
		}

		#region Serialization
		/// <summary>
		/// Creates a new item with enchanted data from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected EnchantableItem (Node node)
			: base(node)
		{
			var tagNode = ((CompoundNode)node)[TagNodeName] as CompoundNode;
			_enchants = validateEnchantsNode(tagNode);
		}

		#region Node names
		private const string EnchantNodeName      = "ench";
		private const string EnchantIdNodeName    = "id";
		private const string EnchantLevelNodeName = "lvl";
		#endregion

		#region Validation
		private static Enchantment[] validateEnchantsNode (CompoundNode tagNode)
		{
			var enchants = new List<Enchantment>();
			if(tagNode.Contains(EnchantNodeName))
			{
				var enchantsNode = tagNode[EnchantNodeName] as ListNode;
				if(null != enchantsNode && enchantsNode.ElementType == NodeType.Compound)
				{
					foreach(CompoundNode enchantNode in enchantsNode)
					{
						if(enchantNode.Contains(EnchantIdNodeName) && enchantNode.Contains(EnchantLevelNodeName))
						{
							var idNode    = enchantNode[EnchantIdNodeName] as ShortNode;
							var levelNode = enchantNode[EnchantLevelNodeName] as ShortNode;
							if(null != idNode && null != levelNode)
								enchants.Add(new Enchantment((EnchantmentType)idNode.Value, levelNode.Value));
						}
					}
				}
			}
			return enchants.ToArray();
		}
		#endregion

		#region Construction
		/// <summary>
		/// Inserts the enchantment information into the tag of the item node
		/// </summary>
		/// <param name="tagNode">Node to insert into</param>
		protected override void InsertIntoTagData (CompoundNode tagNode)
		{
			base.InsertIntoTagData(tagNode);
			tagNode.Add(constructEnchantNode());
		}

		private Node constructEnchantNode ()
		{
			var enchantsNode = new ListNode(EnchantNodeName, NodeType.Compound);
			for(var i = 0; i < _enchants.Length; ++i)
			{
				var enchant = _enchants[i];
				var enchantNode = new CompoundNode("Enchantment") {
					new ShortNode(EnchantIdNodeName, (short)enchant.Type),
					new ShortNode(EnchantLevelNodeName, enchant.Level)
				};
				enchantsNode.Add(enchantNode);
			}
			return enchantsNode;
		}
		#endregion
		#endregion
	}
}
