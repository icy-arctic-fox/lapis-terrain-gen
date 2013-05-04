namespace Lapis.Items
{
	/// <summary>
	/// An item that is a piece of armor
	/// </summary>
	public interface IArmorItem
	{
		/// <summary>
		/// The item's armor type
		/// </summary>
		ArmorType ArmorType { get; }
	}
}
