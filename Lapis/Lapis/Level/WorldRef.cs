using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Level
{
	public class WorldRef
	{
		private readonly string _name;

		internal WorldRef (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the world can't be null.");
			_name = name;
		}

		public string WorldName
		{
			get { return _name; }
		}
	}
}
