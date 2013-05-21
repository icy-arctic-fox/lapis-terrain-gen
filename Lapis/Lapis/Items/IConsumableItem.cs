namespace Lapis.Items
{
	/// <summary>
	/// An item that can be eaten
	/// </summary>
	public interface IConsumableItem : IItem
	{
		/// <summary>
		/// Number of health points added by consuming the item
		/// </summary>
		/// <remarks>This value can be negative.</remarks>
		int HealthModifier { get; }

		// TODO: Add effects
	}
}
