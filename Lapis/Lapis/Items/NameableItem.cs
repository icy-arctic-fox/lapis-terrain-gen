using System;
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
		/// <remarks>This property will be null if the item doesn't have a description.</remarks>
		public string[] Lore
		{
			// Make sure the contents of the array can't be modified
			get { throw new NotImplementedException(); }
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
		protected NameableItem (short data, string name, string[] lore)
			: base(data)
		{
			throw new NotImplementedException();
		}

		#region Serialization
		/// <summary>
		/// Creates a new item with tag data from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected NameableItem (Node node)
			: base(node)
		{
			var tagNode = ((CompoundNode)node)[TagNodeName] as CompoundNode;
			var displayNode = validateDisplayNode(tagNode);
			_name = validateNameNode(displayNode);
			_lore = validateLoreNode(displayNode);
		}		

		#region Node names
		private const string DisplayNodeName = "display";
		private const string NameNodeName    = "Name";
		private const string LoreNodeName    = "Lore";
		#endregion

		#region Validation
		private static CompoundNode validateDisplayNode (CompoundNode tagNode)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}
		#endregion

		#region Construction
		#endregion
		#endregion
	}
}
