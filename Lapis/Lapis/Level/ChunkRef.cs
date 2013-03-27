using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Level
{
	public class ChunkRef
	{
		private readonly int _cx, _cz;
		private readonly RealmRef _realm;

		internal ChunkRef (RealmRef realm, int cx, int cz)
		{
			if(null == realm)
				throw new ArgumentNullException("realm", "The realm reference can't be null.");
			_realm = realm;
			_cx    = cx;
			_cz    = cz;
		}

		public int ChunkX
		{
			get { return _cx; }
		}

		public int ChunkZ
		{
			get { return _cz; }
		}

		public RealmRef Realm
		{
			get { return _realm; }
		}

		public int RealmId
		{
			get { return _realm.RealmId; }
		}

		public WorldRef World
		{
			get { return _realm.World; }
		}

		public string WorldName
		{
			get { return _realm.WorldName; }
		}
	}
}
