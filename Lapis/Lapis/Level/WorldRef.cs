using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a world
	/// </summary>
	/// <remarks>World references are useful for when you want to reference a world, but don't care about what's there.
	/// A world reference doesn't contain any block information.
	/// However, a world reference won't cause the chunk data to become loaded in memory.</remarks>
	public class WorldRef : IEquatable<WorldRef>, IEquatable<World>
	{
		private readonly string _name;

		/// <summary>
		/// Creates a new world reference
		/// </summary>
		/// <param name="name">Name of the world</param>
		internal WorldRef (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the world can't be null.");
			_name = name;
		}

		#region Properties
		/// <summary>
		/// Name of the world
		/// </summary>
		public string WorldName
		{
			get { return _name; }
		}
		#endregion

		#region Conversion
		/// <summary>
		/// Forces a world to load (if it isn't loaded already) and returns the world object
		/// </summary>
		/// <param name="world">World reference to de-reference</param>
		/// <returns>A loaded world or null if the world being referenced doesn't exist</returns>
		/// <remarks>Null will be returned if <paramref name="world"/> is null</remarks>
		public static implicit operator World (WorldRef world)
		{
			return (ReferenceEquals(null, world)) ? null : World.Load(world._name);
		}

		/// <summary>
		/// Creates a reference to a world from a loaded world
		/// </summary>
		/// <param name="world">World to get a reference for</param>
		/// <returns>A reference to the world</returns>
		/// <remarks>Null will be returned if <paramref name="world"/> is null</remarks>
		public static implicit operator WorldRef (World world)
		{
			return (ReferenceEquals(null, world)) ? null : new WorldRef(world.Name);
		}
		#endregion

		#region Comparison
		/// <summary>
		/// Checks if two world references are equal to each other
		/// </summary>
		/// <param name="refA">First world reference</param>
		/// <param name="refB">Second world reference</param>
		/// <returns>True if the world references appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or their world names are the same.</remarks>
		public static bool operator == (WorldRef refA, WorldRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return true;
			if(ReferenceEquals(null, refA))
				return false;
			if(ReferenceEquals(null, refB))
				return false;
			return refA._name == refB._name;
		}

		/// <summary>
		/// Checks if two world references are not equal to each other
		/// </summary>
		/// <param name="refA">First world reference</param>
		/// <param name="refB">Second world reference</param>
		/// <returns>True if the world references don't appear to be equal</returns>
		/// <remarks><paramref name="refA"/> and <paramref name="refB"/> are considered equal if they're both null or their world names are the same.</remarks>
		public static bool operator != (WorldRef refA, WorldRef refB)
		{
			if(ReferenceEquals(refA, refB))
				return false;
			if(ReferenceEquals(null, refA))
				return true;
			if(ReferenceEquals(null, refB))
				return true;
			return refA._name != refB._name;
		}

		/// <summary>
		/// Checks if a world reference and world are equal
		/// </summary>
		/// <param name="worldRef">World reference</param>
		/// <param name="world">World</param>
		/// <returns>True if the world reference and world appear to be equal</returns>
		/// <remarks><paramref name="worldRef"/> and <paramref name="world"/> are considered equal if they're both null or their world names are the same.</remarks>
		public static bool operator == (WorldRef worldRef, World world)
		{
			if(ReferenceEquals(null, worldRef))
				return ReferenceEquals(null, world);
			if(ReferenceEquals(null, world))
				return false;
			return worldRef._name == world.Name;
		}

		/// <summary>
		/// Checks if a world reference and world are not equal
		/// </summary>
		/// <param name="worldRef">World reference</param>
		/// <param name="world">World</param>
		/// <returns>True if the world reference and world don't appear to be equal</returns>
		/// <remarks><paramref name="worldRef"/> and <paramref name="world"/> are considered equal if they're both null or their world names are the same.</remarks>
		public static bool operator != (WorldRef worldRef, World world)
		{
			if(ReferenceEquals(null, worldRef))
				return !ReferenceEquals(null, world);
			if(ReferenceEquals(null, world))
				return true;
			return worldRef._name != world.Name;
		}

		/// <summary>
		/// Checks if a world reference and world are equal
		/// </summary>
		/// <param name="world">World</param>
		/// <param name="worldRef">World reference</param>
		/// <returns>True if the world reference and world appear to be equal</returns>
		/// <remarks><paramref name="worldRef"/> and <paramref name="world"/> are considered equal if they're both null or their world names are the same.</remarks>
		public static bool operator == (World world, WorldRef worldRef)
		{
			return worldRef == world;
		}

		/// <summary>
		/// Checks if a world reference and world are not equal
		/// </summary>
		/// <param name="world">World</param>
		/// <param name="worldRef">World reference</param>
		/// <returns>True if the world reference and world don't appear to be equal</returns>
		/// <remarks><paramref name="worldRef"/> and <paramref name="world"/> are considered equal if they're both null or their world names are the same.</remarks>
		public static bool operator != (World world, WorldRef worldRef)
		{
			return worldRef != world;
		}

		/// <summary>
		/// Checks if the world reference is equal to another object
		/// </summary>
		/// <param name="obj">Object to check against</param>
		/// <returns>True if <paramref name="obj"/> is considered equal to the world reference</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it is not null, is a WorldRef with the same name, or a World with the same name.</remarks>
		public override bool Equals (object obj)
		{
			var worldRef = obj as WorldRef;
			if(!ReferenceEquals(null, worldRef))
				return this == worldRef;
			var world = obj as World;
			if (!ReferenceEquals(null, world))
				return this == world;
			return false;
		}

		/// <summary>
		/// Compares the world reference against another world reference to check if they're equal
		/// </summary>
		/// <param name="other">Reference to compare against</param>
		/// <returns>True if the world references point to the same world</returns>
		public bool Equals (WorldRef other)
		{
			if(!ReferenceEquals(null, other))
				return this == other;
			return false;
		}

		/// <summary>
		/// Checks if the world reference points to a world
		/// </summary>
		/// <param name="other">World to compare against</param>
		/// <returns>True if the world reference refers to <paramref name="other"/></returns>
		public bool Equals (World other)
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
			return _name.GetHashCode();
		}

		/// <summary>
		/// Gets the string representation of the world reference
		/// </summary>
		/// <returns>A string representing the world</returns>
		public override string ToString ()
		{
			return "World: " + _name;
		}
	}
}
