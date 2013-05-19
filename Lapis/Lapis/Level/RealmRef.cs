using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a realm within a world
	/// </summary>
	/// <remarks>Realm references are useful for when you want to reference a realm (or dimension), but don't care about what's there.
	/// A realm reference doesn't contain any block information.
	/// However, a realm reference won't cause the chunk data to become loaded in memory.</remarks>
	public class RealmRef : IEquatable<RealmRef>, IEquatable<Realm>
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
			if(ReferenceEquals(null, world))
				throw new ArgumentNullException("world", "The world reference can't be null.");
			_world = world;
			_id    = id;
		}

		#region Properties
		/// <summary>
		/// ID of the realm
		/// </summary>
		public int RealmId
		{
			get { return _id; }
		}

		/// <summary>
		/// World that the realm belongs to
		/// </summary>
		public WorldRef World
		{
			get { return _world; }
		}

		/// <summary>
		/// Name of the world that the realm is a part of
		/// </summary>
		public string WorldName
		{
			get { return _world.WorldName; }
		}
		#endregion

		#region Conversion
		/// <summary>
		/// Forces a realm to load (if it isn't loaded already) and returns the realm object
		/// </summary>
		/// <param name="realm">Realm reference to de-reference</param>
		/// <returns>A loaded realm or null if the realm being referenced doesn't exist</returns>
		/// <remarks>Null will be returned if <paramref name="realm"/> is null.
		/// The world that the realm belongs to will also be loaded (if it hasn't been already).</remarks>
		public static implicit operator Realm (RealmRef realm)
		{
			if(!ReferenceEquals(null, realm))
			{
				var world = (World)realm._world; // This cast forces the world to load
				return (ReferenceEquals(null, world)) ? null : world.LoadRealm(realm._id);
			}
			return null;
		}

		/// <summary>
		/// Creates a reference to a realm from a loaded realm
		/// </summary>
		/// <param name="realm">Realm to get a reference for</param>
		/// <returns>A reference to the realm</returns>
		/// <remarks>Null will be returned if <paramref name="realm"/> is null</remarks>
		public static implicit operator RealmRef (Realm realm)
		{
			return (ReferenceEquals(null, realm)) ? null : new RealmRef(realm.World, realm.Id);
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Checks if two realm references are equal to each other
		/// </summary>
		/// <param name="refA">First realm reference</param>
		/// <param name="refB">Second realm reference</param>
		/// <returns>True if the realm references appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or they're from the same world and have the same ID.</remarks>
		public static bool operator == (RealmRef refA, RealmRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return true;
			if(ReferenceEquals(null, refA))
				return false;
			if(ReferenceEquals(null, refB))
				return false;
			return refA._id == refB._id;
		}

		/// <summary>
		/// Checks if two realm references are not equal to each other
		/// </summary>
		/// <param name="refA">First realm reference</param>
		/// <param name="refB">Second realm reference</param>
		/// <returns>True if the realm references don't appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or they're from the same world and have the same ID.</remarks>
		public static bool operator != (RealmRef refA, RealmRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return false;
			if(ReferenceEquals(null, refA))
				return true;
			if(ReferenceEquals(null, refB))
				return true;
			return refA._id != refB._id;
		}

		/// <summary>
		/// Checks if a realm reference and realm are equal
		/// </summary>
		/// <param name="realmRef">Realm reference</param>
		/// <param name="realm">Realm</param>
		/// <returns>True if the realm reference and realm appear to be equal</returns>
		/// <remarks><paramref name="realmRef"/> and <paramref name="realm"/> are considered equal if they're both null or they're from the same world and have the same ID.</remarks>
		public static bool operator == (RealmRef realmRef, Realm realm)
		{
			if(ReferenceEquals(null, realmRef))
				return ReferenceEquals(null, realm);
			if(ReferenceEquals(null, realm))
				return false;
			return realmRef._id == realm.Id;
		}

		/// <summary>
		/// Checks if a realm reference and realm are not equal
		/// </summary>
		/// <param name="realmRef">Realm reference</param>
		/// <param name="realm">Realm</param>
		/// <returns>True if the realm reference and realm don't appear to be equal</returns>
		/// <remarks><paramref name="realmRef"/> and <paramref name="realm"/> are considered equal if they're both null or they're from the same world and have the same ID.</remarks>
		public static bool operator != (RealmRef realmRef, Realm realm)
		{
			if(ReferenceEquals(null, realmRef))
				return !ReferenceEquals(null, realm);
			if(ReferenceEquals(null, realm))
				return true;
			return realmRef._id != realm.Id;
		}

		/// <summary>
		/// Checks if a realm reference and realm are equal
		/// </summary>
		/// <param name="realm">Realm</param>
		/// <param name="realmRef">Realm reference</param>
		/// <returns>True if the realm reference and realm appear to be equal</returns>
		/// <remarks><paramref name="realmRef"/> and <paramref name="realm"/> are considered equal if they're both null or they're from the same world and have the same ID.</remarks>
		public static bool operator == (Realm realm, RealmRef realmRef)
		{
			return realmRef == realm;
		}

		/// <summary>
		/// Checks if a realm reference and realm are not equal
		/// </summary>
		/// <param name="realm">Realm</param>
		/// <param name="realmRef">Realm reference</param>
		/// <returns>True if the realm reference and realm don't appear to be equal</returns>
		/// <remarks><paramref name="realmRef"/> and <paramref name="realm"/> are considered equal if they're both null or they're from the same world and have the same ID.</remarks>
		public static bool operator != (Realm realm, RealmRef realmRef)
		{
			return realmRef != realm;
		}

		/// <summary>
		/// Checks if the realm reference is equal to another object
		/// </summary>
		/// <param name="obj">Object to check against</param>
		/// <returns>True if <paramref name="obj"/> is considered equal to the realm reference</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a RealmRef with the same world and ID, or a Realm with the same world and ID.</remarks>
		public override bool Equals (object obj)
		{
			var realmRef = obj as RealmRef;
			if(!ReferenceEquals(null, realmRef))
				return this == realmRef;
			var realm = obj as Realm;
			if(!ReferenceEquals(null, realm))
				return this == realm;
			return false;
		}

		/// <summary>
		/// Compares the realm reference against another realm reference to check if they're equal
		/// </summary>
		/// <param name="other">Reference to compare against</param>
		/// <returns>True if the realm references point to the same realm</returns>
		public bool Equals (RealmRef other)
		{
			if(!ReferenceEquals(null, other))
				return this == other;
			return false;
		}

		/// <summary>
		/// Checks if the realm reference points to a realm
		/// </summary>
		/// <param name="other">Realm to compare against</param>
		/// <returns>True if the realm reference refers to <paramref name="other"/></returns>
		public bool Equals (Realm other)
		{
			if(!ReferenceEquals(null, other))
				return this == other;
			return false;
		}
		#endregion

		/// <summary>
		/// Generates a hash code from the object
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = _world.GetHashCode();
			hash *= 37;
			hash ^= _id.GetHashCode();
			return hash;
		}

		/// <summary>
		/// Gets the string representation of the world reference
		/// </summary>
		/// <returns>A string representing the world</returns>
		public override string ToString ()
		{
			return _world + " Realm: " + _id;
		}
	}
}
