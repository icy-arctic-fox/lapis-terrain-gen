using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Level
{
	public class RealmRef
	{
		private readonly int _id;
		private readonly WorldRef _world;

		internal RealmRef (WorldRef world, int id)
		{
			if(null == world)
				throw new ArgumentNullException("world", "The world reference can't be null.");
		}

		public int RealmId
		{
			get { return _id; }
		}

		public WorldRef World
		{
			get { return _world; }
		}

		public string WorldName
		{
			get { return _world.WorldName; }
		}
	}
}
