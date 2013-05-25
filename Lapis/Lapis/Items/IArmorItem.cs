namespace Lapis.Items
{
	/// <summary>
	/// An item that is a piece of armor
	/// </summary>
	public interface IArmorItem : IItem
	{
		/// <summary>
		/// The item's armor type
		/// </summary>
		ArmorType ArmorType { get; }

		/// <summary>
		/// Amount of protection that the armor provides (0-20)
		/// </summary>
		byte Protection { get; }
	}
}
