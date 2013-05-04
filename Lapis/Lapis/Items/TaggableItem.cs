using System;

namespace Lapis.Items
{
	public abstract class TaggableItem : Item
	{
		private readonly string _name;
		private readonly string[] _lore;

		public string Name
		{
			get { return _name; }
		}

		public string[] Lore
		{
			// Make sure the contents of the array can't be modified
			get { throw new NotImplementedException(); }
		}
	}
}
