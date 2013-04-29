using System;
using Lapis.Blocks;

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
	}
}
