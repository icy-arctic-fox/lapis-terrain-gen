using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Lapis.Level;
using Lapis.Level.Generation;

namespace Lapis.Threading
{
	/// <summary>
	/// Manages generating chunks in bulk
	/// </summary>
	public class GenerationManager
	{
		private readonly World _world;

		/// <summary>
		/// Speed at which to generate chunks
		/// </summary>
		public GenerationSpeed Speed
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public void AddRealm (ITerrainGenerator generator, Dimension dimmension = Dimension.Normal)
		{
			throw new NotImplementedException();
		}

		public void AddRealm (ITerrainGenerator generator, int realmId, Dimension dimension = Dimension.Normal)
		{
			World.Create("foo").CreateRealm(generator, realmId, dimension);
			throw new NotImplementedException();
		}

		public void GenerateRectange (int realmId, int startX, int startZ, int countX, int countZ)
		{
			throw new NotImplementedException();
		}

		private void doWork (object state)
		{
			var work = state as GenerationUnit;
			if(null != work)
			{
				var realm = work.Realm;
				var lastX = work.StartX + work.CountX;
				var lastZ = work.StartZ + work.CountZ;
				for(var x = work.StartX; x <= lastX; ++x)
					for(var z = work.StartZ; z <= lastZ; ++z)
						realm.GenerateChunk(x, z);
				work.Done();
			}
		}

		/// <summary>
		/// A unit of work
		/// </summary>
		private class GenerationUnit
		{
			private readonly Realm _realm;
			private readonly int _startX, _startZ, _countX, _countZ;
			private readonly ManualResetEvent _handle;

			public Realm Realm
			{
				get { return _realm; }
			}

			public int StartX
			{
				get { return _startX; }
			}

			public int StartZ
			{
				get { return _startZ; }
			}

			public int CountX
			{
				get { return _countX; }
			}

			public int CountZ
			{
				get { return _countZ; }
			}

			public GenerationUnit (Realm realm, int startX, int startZ, int countX, int countZ, ManualResetEvent handle)
			{
				_realm  = realm;
				_startX = startX;
				_startZ = startZ;
				_countX = countX;
				_countZ = countZ;
				_handle = handle;
			}

			public void Done ()
			{
				_handle.Set();
			}
		}
	}

	/// <summary>
	/// Speed at which to generate chunks
	/// </summary>
	public enum GenerationSpeed
	{
		VerySlow = 10,

		Slow = 5,

		Medium = 3,

		High = 1,

		Full = 0
	}
}
