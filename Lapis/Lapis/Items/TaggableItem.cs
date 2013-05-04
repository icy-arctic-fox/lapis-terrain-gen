using System;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// An item that contains extra information in a tag format
	/// </summary>
	public abstract class TaggableItem : Item
	{
		private readonly string _name;
		private readonly string[] _lore;

		/// <summary>
		/// Visible name of the item
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Additional description (or "lore") displayed on the item
		/// </summary>
		public string[] Lore
		{
			// Make sure the contents of the array can't be modified
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Creates a new item
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		protected TaggableItem (short data)
			: base(data)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Creates a new item with tag data
		/// </summary>
		/// <param name="data">Data value (damage or other information)</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected TaggableItem (short data, string name, string[] lore)
			: base(data)
		{
			throw new NotImplementedException();
		}

		#region Serialization
		/// <summary>
		/// Creates a new item with tag data from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected TaggableItem (Node node)
			: base(node)
		{
			throw new NotImplementedException();
		}

		#region Validation
		#endregion

		#region Construction
		#endregion
		#endregion
	}
}
