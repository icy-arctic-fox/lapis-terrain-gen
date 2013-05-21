using System;
using System.Collections.Generic;
using System.Linq;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that can be named
	/// </summary>
	public abstract class NameableItem : TaggableItem, IEquatable<NameableItem>, IComparable<NameableItem>
	{
		private readonly string _name;
		private readonly string[] _lore;

		/// <summary>
		/// Whether or not the item has a custom name
		/// </summary>
		public bool HasName
		{
			get { return null != _name; }
		}

		/// <summary>
		/// Visible name of the item
		/// </summary>
		/// <remarks>This property will be null if the item doesn't have a name.</remarks>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Whether or not the item has a description
		/// </summary>
		public bool HasLore
		{
			get { return null == _lore; }
		}

		/// <summary>
		/// Additional description (or "lore") displayed on the item
		/// </summary>
		/// <remarks>This property will be null if the item doesn't have a description.
		/// Modifying the elements of this property will not update the item.</remarks>
		public string[] Lore
		{
			get
			{
				if(null != _lore)
				{
					var lore = new string[_lore.Length];
					for(var i = 0; i < _lore.Length; ++i)
						lore[i] = _lore[i];
					return lore;
				}
				return null;
			}
		}

		/// <summary>
		/// Creates a new item with no name and no lore
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected NameableItem (short data)
			: base(data)
		{
			_name = null;
			_lore = null;
		}

		/// <summary>
		/// Creates a new item with name and lore
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected NameableItem (short data, string name, IEnumerable<string> lore)
			: base(data)
		{
			_name = name;
			_lore = (null == lore) ? null : lore.ToArray();
		}

		#region Serialization
		/// <summary>
		/// Creates a new item with tag data from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected NameableItem (Node node)
			: base(node)
		{
			var rootNode = (CompoundNode)node;
			if(rootNode.Contains(TagNodeName))
			{
				var tagNode = rootNode[TagNodeName] as CompoundNode;
				if(null != tagNode)
				{
					var displayNode = validateDisplayNode(tagNode);
					if(null != displayNode)
					{
						_name = validateNameNode(displayNode);
						_lore = validateLoreNode(displayNode);
					}
				}
			}
		}		

		#region Node names
		/// <summary>
		/// Name of the display node under the tag node
		/// </summary>
		protected const string DisplayNodeName = "display";
		private const string NameNodeName      = "Name";
		private const string LoreNodeName      = "Lore";
		#endregion

		#region Validation
		private static CompoundNode validateDisplayNode (CompoundNode tagNode)
		{
			if(tagNode.Contains(DisplayNodeName))
			{
				var displayNode = tagNode[DisplayNodeName] as CompoundNode;
				if(null != displayNode)
					return displayNode;
			}
			return null;
		}

		private static string validateNameNode (CompoundNode displayNode)
		{
			if(displayNode.Contains(NameNodeName))
			{
				var nameNode = displayNode[NameNodeName] as StringNode;
				if(null != nameNode)
					return nameNode.Value;
			}
			return null;
		}

		private static string[] validateLoreNode (CompoundNode displayNode)
		{
			if(displayNode.Contains(LoreNodeName))
			{
				var loreNode = displayNode[LoreNodeName] as ListNode;
				if(null != loreNode && loreNode.ElementType == NodeType.String)
					return (from StringNode lore in loreNode where lore != null && lore.Value != null select lore.Value).ToArray();
			}
			return null;
		}
		#endregion

		#region Construction
		/// <summary>
		/// Inserts the display data into the tag of the item node
		/// </summary>
		/// <param name="tagNode">Node to insert into</param>
		protected override void InsertIntoTagData (CompoundNode tagNode)
		{
			base.InsertIntoTagData(tagNode);
			var displayNode = new CompoundNode(DisplayNodeName);
			if(null != _name)
				displayNode.Add(new StringNode(NameNodeName, _name));
			if(null != _lore)
			{
				var loreNode = new ListNode(LoreNodeName, NodeType.String);
				for(var i = 0; i < _lore.Length; ++i)
					loreNode.Add(new StringNode("Lore", _lore[i]));
				displayNode.Add(loreNode);
			}

			InsertIntoDisplayData(displayNode);
			if(0 < displayNode.Count)
				tagNode.Add(displayNode);
		}

		/// <summary>
		/// Inserts custom item information into the display node of the tag
		/// </summary>
		/// <param name="displayNode">Node to insert values into</param>
		/// <remarks>This method does nothing, but is virtual so that sub-classes can add data if needed.</remarks>
		protected virtual void InsertIntoDisplayData (CompoundNode displayNode)
		{
			// ...
		}
		#endregion
		#endregion

		/// <summary>
		/// Checks if an item's contents are equal to the current item
		/// </summary>
		/// <param name="item">Item to compare against</param>
		/// <returns>True if the item contents are the same or false if they aren't</returns>
		/// <remarks>Sub-classes should override this method if they have additional properties (such as a taggable item).
		/// This method only compares the types and data.</remarks>
		public override bool Equals (Item item)
		{
			if(base.Equals(item))
			{
				var nameable = item as NameableItem;
				if(null != nameable)
					return Equals((nameable));
			}
			return false;
		}

		/// <summary>
		/// Checks if an nameable item's contents are equal to the current item
		/// </summary>
		/// <param name="item">Item to compare against</param>
		/// <returns>True if the item contents are the same or false if they aren't</returns>
		/// <remarks>Sub-classes should override this method if they have additional properties (such as a taggable item).
		/// This method only compares the types and data.</remarks>
		public virtual bool Equals (NameableItem item)
		{
			if(base.Equals(item))
			{
				if(_name == item._name)
				{
					if(null != _lore && null != item._lore)
					{
						if(_lore.Length == item._lore.Length)
						{
							for(var i = 0; i < _lore.Length; ++i)
								if(_lore[i] != item._lore[i])
									return false;
							return true;
						}
						return false;
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Compares the item to another item
		/// </summary>
		/// <param name="item">Item to compare against</param>
		/// <returns>Less than 0 if the item is less than <paramref name="item"/>,
		/// 0 if the items are equal,
		/// or greater than 0 if the item is greater than <paramref name="item"/></returns>
		public override int CompareTo (Item item)
		{
			return base.CompareTo(item);
		}

		/// <summary>
		/// Compares the item to another nameable item
		/// </summary>
		/// <param name="item">Item to compare against</param>
		/// <returns>Less than 0 if the item is less than <paramref name="item"/>,
		/// 0 if the items are equal,
		/// or greater than 0 if the item is greater than <paramref name="item"/></returns>
		public virtual int CompareTo (NameableItem item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets a string representation of the item
		/// </summary>
		/// <returns>A string</returns>
		/// <remarks>The string will be formatted as: TYPE(DATA_HEX) "NAME"</remarks>
		public override string ToString ()
		{
			var baseString = base.ToString();
			return (null == _name) ? baseString : String.Join(" ", baseString, _name);
		}

		public override int GetHashCode ()
		{
			throw new NotImplementedException();
		}
	}
}
