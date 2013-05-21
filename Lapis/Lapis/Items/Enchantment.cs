using System;
using System.Text;

namespace Lapis.Items
{
	/// <summary>
	/// Information about an enchantment
	/// </summary>
	public struct Enchantment : IEquatable<Enchantment>, IComparable<Enchantment>
	{
		private readonly EnchantmentType _type;
		private readonly short _level;

		/// <summary>
		/// Enchantment type
		/// </summary>
		public EnchantmentType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Enchantment level (base 0)
		/// </summary>
		/// <remarks>The level is base 0.
		/// This means that 0 is level 1, 1 is level 2, and so on.</remarks>
		public short Level
		{
			get { return _level; }
		}

		/// <summary>
		/// Creates a new enchantment
		/// </summary>
		/// <param name="type">Type of enchantment</param>
		/// <param name="level">Level of the enchantment (base 0)</param>
		/// <remarks><paramref name="level"/> is base 0.
		/// This means that 0 is level 1, 1 is level 2, and so on.</remarks>
		public Enchantment (EnchantmentType type, short level)
		{
			_type  = type;
			_level = level;
		}

		#region Equality operators
		/// <summary>
		/// Compares two enchantments to check if they're equal
		/// </summary>
		/// <param name="enchA">First enchantment</param>
		/// <param name="enchB">Second enchantment</param>
		/// <returns>True if the enchantments are the same type and level</returns>
		public static bool operator == (Enchantment enchA, Enchantment enchB)
		{
			return (enchA._type == enchB._type && enchA._level == enchB._level);
		}

		/// <summary>
		/// Compares two enchantments to check if they're different
		/// </summary>
		/// <param name="enchA">First enchantment</param>
		/// <param name="enchB">Second enchantment</param>
		/// <returns>True if the enchantments have a different type or level than each other</returns>
		public static bool operator != (Enchantment enchA, Enchantment enchB)
		{
			return (enchA._type != enchB._type || enchA._level != enchB._level);
		}
		#endregion

		/// <summary>
		/// Compares the enchantment to another one to see if they're equal
		/// </summary>
		/// <param name="other">Enchantment to compare against</param>
		/// <returns>True if the enchantments are the same</returns>
		public bool Equals (Enchantment other)
		{
			return this == other;
		}

		/// <summary>
		/// Compares the enchantment to another one
		/// </summary>
		/// <param name="other">Enchantment to compare against</param>
		/// <returns>Less than 0 if the enchantment is less than <paramref name="other"/>,
		/// 0 if the enchantments are the same,
		/// or greater than 0 if the enchantment is greater than <paramref name="other"/></returns>
		public int CompareTo (Enchantment other)
		{
			var val = _type.CompareTo(other);
			return (0 == val) ? _level.CompareTo(other) : val;
		}

		/// <summary>
		/// Compares another object to the enchantment to check if they're equal
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is an enchantment and has the same properties (type and level)</returns>
		public override bool Equals (object obj)
		{
			if(obj is Enchantment)
			{
				var ench = (Enchantment)obj;
				return ench == this; // Call equality == operator above
			}
			return false;
		}

		/// <summary>
		/// Generates a hash code of the enchantment
		/// </summary>
		/// <returns>A hash</returns>
		public override int GetHashCode ()
		{
			return (_level | ((int)_type << 16));
		}
		
		/// <summary>
		/// Generates a string representation of the enchantment
		/// </summary>
		/// <returns>A string</returns>
		/// <remarks>The string will be in the format: TYPE Level #</remarks>
		public override string ToString ()
		{
			var sb = new StringBuilder();
			sb.Append(_type);
			sb.Append(" Level ");
			sb.Append(_level + 1);
			return sb.ToString();
		}
	}
}
