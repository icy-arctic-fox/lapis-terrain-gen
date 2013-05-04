using System.IO;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that contains extra information in a tag format
	/// </summary>
	public abstract class TaggableItem : Item
	{
		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected TaggableItem (short data)
			: base(data)
		{
			// ...
		}

		#region Serialization
		/// <summary>
		/// Creates a new item with tag data from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected TaggableItem (Node node)
			: base(node)
		{
			validateTagNode((CompoundNode)node);
		}		

		#region Node names
		/// <summary>
		/// Name of the tag node
		/// </summary>
		protected const string TagNodeName = "tag";
		#endregion

		#region Validation
		private static void validateTagNode (CompoundNode root)
		{
			if(!root.Contains(TagNodeName))
				throw new InvalidDataException("The root node does not contain tag data");
			var tagNode = root[TagNodeName] as CompoundNode;
			if(null == tagNode)
				throw new InvalidDataException("The tag node is not of the correct type");
		}
		#endregion

		#region Construction
		/// <summary>
		/// Inserts the tag data into the root of the item node
		/// </summary>
		/// <param name="node">Node to insert into</param>
		protected override void InsertIntoItemData (CompoundNode node)
		{
			base.InsertIntoItemData(node);
			var tagNode = new CompoundNode(TagNodeName);
			InsertIntoTagData(tagNode);
			node.Add(tagNode);
		}

		/// <summary>
		/// Inserts custom item information into the tag of the item node
		/// </summary>
		/// <param name="tagNode">Node to insert values into</param>
		/// <remarks>This method does nothing, but is virtual so that sub-classes can add data if needed.</remarks>
		protected virtual void InsertIntoTagData (CompoundNode tagNode)
		{
			// ...
		}
		#endregion
		#endregion
	}
}
