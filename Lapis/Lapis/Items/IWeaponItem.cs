namespace Lapis.Items
{
	/// <summary>
	/// An item that does more damage to mobs
	/// </summary>
	public interface IWeaponItem : IItem
	{
		/// <summary>
		/// Amount of damage done by the weapon
		/// </summary>
		int WeaponDamage { get; }
	}
}
