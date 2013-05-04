using System;
using System.IO;
using Lapis.Blocks;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Base class for items
	/// </summary>
	/// <remarks>An item an be a block or regular item.
	/// Item IDs start after block IDs.</remarks>
	public abstract class Item
	{
		/// <summary>
		/// Extra raw data associated with the item
		/// </summary>
		/// <remarks>This is the damage value for items that can take damage.</remarks>
		protected readonly short _data;

		#region Properties
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		public abstract short ItemId { get; }

		/// <summary>
		/// Whether or not the item is a block
		/// </summary>
		public bool IsBlock
		{
			get { return ItemId <= byte.MaxValue; }
		}

		/// <summary>
		/// Whether or not the item is actually an item (not a block)
		/// </summary>
		public bool IsItem
		{
			get { return ItemId > byte.MaxValue; }
		}

		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>This property can only be used if the item is a block.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if the item is not a block</exception>
		public BlockType BlockType
		{
			get
			{
				if(!IsBlock)
					throw new InvalidOperationException("The item is not a block.");
				return (BlockType)ItemId;
			}
		}

		/// <summary>
		/// Type that describes the item
		/// </summary>
		/// <remarks>This property can only be used if the item is not a block.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if the item is a block</exception>
		public ItemType ItemType
		{
			get
			{
				if(!IsItem)
					throw new InvalidOperationException("The item is a block.");
				return (ItemType)ItemId;
			}
		}

		/// <summary>
		/// Raw meta-data associated with the item
		/// </summary>
		public short Data
		{
			get { return _data; }
		}
		#endregion

		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected Item (short data)
		{
			_data = data;
		}

		#region Serialization
		/// <summary>
		/// Creates a new item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="node"/> is null</exception>
		/// <exception cref="InvalidDataException">Thrown if the structure of the node is invalid</exception>
		protected Item (Node node)
		{
			if(null == node)
				throw new ArgumentNullException("node", "The root node can't be null.");

			var root = validateRootNode(node);
			_data = validateDataNode(root);
		}

		#region Node names
		private const string IdNodeName    = "id";
		private const string DataNodeName  = "Damage";
		private const string CountNodeName = "Count";
		private const string SlotNodeName  = "Slot";
		#endregion

		#region Validation
		private static CompoundNode validateRootNode (Node node)
		{
			var root = node as CompoundNode;
			if(null == root)
				throw new InvalidDataException("The root node type is not of the correct type");
			return root;
		}

		private static short validateDataNode (CompoundNode root)
		{
			if(root.Contains(DataNodeName))
			{
				var dataNode = root[DataNodeName] as ShortNode;
				if(null != dataNode)
					return dataNode.Value;
			}
			return 0;
		}
		#endregion

		#region Construction
		/// <summary>
		/// Creates an NBT node containing information about the item
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>A node containing information about the item</returns>
		public Node GetNbtData (string name)
		{
			return constructNode(name, 1);
		}

		/// <summary>
		/// Creates an NBT node containing information about the item
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <param name="count">Number of items in the stack</param>
		/// <returns>A node containing information about the item</returns>
		public Node GetNbtData (string name, byte count)
		{
			return constructNode(name, count);
		}

		/// <summary>
		/// Creates an NBT node containing information about the item
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <param name="count">Number of items in the stack</param>
		/// <param name="slot">Slot number that the item belongs in</param>
		/// <returns>A node containing information about the item</returns>
		public Node GetNbtData (string name, byte count, byte slot)
		{
			var root = constructNode(name, count);
			root.Add(new ByteNode(SlotNodeName, slot));
			return root;
		}

		private CompoundNode constructNode (string name, byte count)
		{
			var root = new CompoundNode(name);
			root.Add(new ShortNode(IdNodeName, ItemId));
			root.Add(new ShortNode(DataNodeName, _data));
			root.Add(new ByteNode(CountNodeName, count));

			InsertIntoItemData(root);
			return root;
		}

		/// <summary>
		/// Inserts custom item information into the root of the item node
		/// </summary>
		/// <param name="node">Node to insert values into</param>
		/// <remarks>This method does nothing, but is virtual so that sub-classes can add data if needed.</remarks>
		protected virtual void InsertIntoItemData (CompoundNode node)
		{
			// ...
		}
		#endregion
		#endregion
	}
}
