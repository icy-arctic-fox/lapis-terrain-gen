using System;
using Lapis.Blocks;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Base interface type for items
	/// </summary>
	public interface IItem : IEquatable<Item>, IEquatable<Block>, IComparable<Item>, IComparable<Block>
	{
		/// <summary>
		/// Numerical ID of the item
		/// </summary>
		short ItemId { get; }

		/// <summary>
		/// Whether or not the item is a block
		/// </summary>
		bool IsBlock { get; }

		/// <summary>
		/// Whether or not the item is actually an item (not a block)
		/// </summary>
		bool IsItem { get; }

		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>This property can only be used if the item is a block.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if the item is not a block</exception>
		BlockType BlockType { get; }

		/// <summary>
		/// Type that describes the item
		/// </summary>
		/// <remarks>This property can only be used if the item is not a block.</remarks>
		/// <exception cref="InvalidOperationException">Thrown if the item is a block</exception>
		ItemType ItemType { get; }

		/// <summary>
		/// Raw meta-data associated with the item
		/// </summary>
		short Data { get; }

		/// <summary>
		/// Creates an NBT node containing information about the item
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <returns>A node containing information about the item</returns>
		Node GetNbtData (string name);

		/// <summary>
		/// Creates an NBT node containing information about the item
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <param name="count">Number of items in the stack</param>
		/// <returns>A node containing information about the item</returns>
		Node GetNbtData (string name, byte count);

		/// <summary>
		/// Creates an NBT node containing information about the item
		/// </summary>
		/// <param name="name">Name to give the node</param>
		/// <param name="count">Number of items in the stack</param>
		/// <param name="slot">Slot number that the item belongs in</param>
		/// <returns>A node containing information about the item</returns>
		Node GetNbtData (string name, byte count, byte slot);
	}
}
