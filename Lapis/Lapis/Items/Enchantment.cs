namespace Lapis.Items
{
	/// <summary>
	/// Information about an enchantment
	/// </summary>
	public struct Enchantment
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
	}
}
