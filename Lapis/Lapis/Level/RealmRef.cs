using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a realm within a world
	/// </summary>
	/// <remarks>Realm references are useful for when you want to reference a realm (or dimension), but don't care about what's there.
	/// A realm reference doesn't contain any block information.
	/// However, a realm reference won't cause the chunk data to become loaded in memory.</remarks>
	public class RealmRef
	{
		private readonly int _id;
		private readonly WorldRef _world;

		/// <summary>
		/// Creates a new realm reference
		/// </summary>
		/// <param name="world">World that the realm is a part of</param>
		/// <param name="id">ID of the realm</param>
		internal RealmRef (WorldRef world, int id)
		{
			if(null == world)
				throw new ArgumentNullException("world", "The world reference can't be null.");
			_world = world;
			_id    = id;
		}

		#region Properties
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
		#endregion
	}
}
