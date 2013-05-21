using System;
using System.IO;
using Lapis.Blocks;
using Lapis.IO.NBT;
using Lapis.Level;

namespace Lapis.Items
{
	/// <summary>
	/// Base class for items
	/// </summary>
	/// <remarks>An item an be a block or regular item.
	/// Item IDs start after block IDs.</remarks>
	public abstract class Item : IItem
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
			var root = new CompoundNode(name) {
				new ShortNode(IdNodeName, ItemId),
				new ShortNode(DataNodeName, _data),
				new ByteNode(CountNodeName, count)
			};

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

		/// <summary>
		/// Checks if the item is equal to another object
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is considered equal to the item or false if it's not</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it's not null, is an Item or Block/BlockRef, and the item/block type and data are the same.
		/// Sub-classes that have additional properties will want to override the protected Equals() method instead of this one.</remarks>
		public override bool Equals (object obj)
		{
			if(null != obj)
			{
				var item = obj as Item;
				if(item != null)
					return Equals(item);
				if(IsBlock)
				{
					var blockRef = obj as BlockRef;
					if(blockRef != null)
					{
						Block block = blockRef;
						return Equals(block);
					}
					else
					{
						var block = obj as Block;
						if(block != null)
							return Equals(block);
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Compares the item against a block
		/// </summary>
		/// <param name="block">Block to compare against</param>
		/// <returns>True if the item is a block and has the same type and value as <paramref name="block"/></returns>
		public bool Equals (Block block)
		{
			if(IsBlock && null != block)
				return (block.Type == BlockType && block.Data == _data);
			return false;
		}

		/// <summary>
		/// Checks if an item's contents are equal to the current item
		/// </summary>
		/// <param name="item">Item to compare against</param>
		/// <returns>True if the item contents are the same or false if they aren't</returns>
		/// <remarks>Sub-classes should override this method if they have additional properties (such as a taggable item).
		/// This method only compares the types and data.</remarks>
		public virtual bool Equals (Item item)
		{
			if(null != item)
				return (item.ItemId == ItemId && item._data == _data);
			return false;
		}

		/// <summary>
		/// Compares the item to a block
		/// </summary>
		/// <param name="block">Block to compare against</param>
		/// <returns>Less than 0 if the item is less than the block,
		/// 0 if the block is the same as the item,
		/// or greater that 0 if the item is greater than the block.</returns>
		public int CompareTo (Block block)
		{
			if(!ReferenceEquals(null, block))
			{
				var val = ItemId.CompareTo((int)block.Type);
				return (0 == val) ? _data.CompareTo(block.Data) : val;
			}
			return 1;
		}

		/// <summary>
		/// Compares the item to another item
		/// </summary>
		/// <param name="item">Item to compare against</param>
		/// <returns>Less than 0 if the item is less than <paramref name="item"/>,
		/// 0 if the items are equal,
		/// or greater than 0 if the item is greater than <paramref name="item"/></returns>
		public virtual int CompareTo (Item item)
		{
			if(null != item)
			{
				var val = ItemId.CompareTo(item.ItemId);
				return (0 == val) ? _data.CompareTo(item._data) : val;
			}
			return 1;
		}

		/// <summary>
		/// Generates a hash code of the item
		/// </summary>
		/// <returns>A hash</returns>
		public override int GetHashCode ()
		{
			return ItemId | (Data << 16);
		}

		/// <summary>
		/// Gets a string representation of the item
		/// </summary>
		/// <returns>A string</returns>
		/// <remarks>The string will be formatted as: TYPE(DATA_HEX)</remarks>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append(IsBlock ? BlockType.ToString() : ItemType.ToString());
			sb.Append('(');
			sb.AppendFormat("{0:x}", _data);
			sb.Append(')');
			return sb.ToString();
		}
	}
}
