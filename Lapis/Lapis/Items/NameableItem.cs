using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that can be named
	/// </summary>
	public abstract class NameableItem : TaggableItem
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
	}
}
