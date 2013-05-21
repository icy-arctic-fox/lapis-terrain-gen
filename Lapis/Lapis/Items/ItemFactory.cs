using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Manages the creation of new items
	/// </summary>
	public static class ItemFactory
	{
		#region Registry
		/// <summary>
		/// Describes a method that creates an item
		/// </summary>
		/// <param name="itemData">Node that contains the information about the item</param>
		/// <returns>A block</returns>
		public delegate Item ItemCreation (Node itemData);

		/// <summary>
		/// Contains delegates for constructing items for all known types
		/// </summary>
		private static readonly Dictionary<ItemType, ItemCreation> _knownItemTypes = new Dictionary<ItemType, ItemCreation>();

		/// <summary>
		/// Registers all default item types
		/// </summary>
		static ItemFactory ()
		{
			lock(_knownItemTypes)
			{
				// TODO: Implement
			}
		}

		/// <summary>
		/// Registers a new type of item
		/// </summary>
		/// <param name="type">item type</param>
		/// <param name="constructor">Method that statically creates the item</param>
		public static void Register (ItemType type, ItemCreation constructor)
		{
			if(null == constructor)
				throw new ArgumentNullException("constructor", "The item constructor method can't be null.");

			lock(_knownItemTypes)
				_knownItemTypes[type] = constructor;
		}
		#endregion

		#region Factory methods
		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="type">Type of item to statically create</param>
		/// <param name="node">Node containing the information about the item</param>
		/// <returns>A block</returns>
		public static Item Create (ItemType type, Node node)
		{
			var constructor = _knownItemTypes[type]; // This isn't locked because it would reduce concurrency down to none
			return (null == constructor) ? null : constructor(node);
		}
		#endregion
	}
}
