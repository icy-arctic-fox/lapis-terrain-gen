namespace Lapis.Items
{
	/// <summary>
	/// Types of enchantments that items can have
	/// </summary>
	public enum EnchantmentType : byte
	{
		/// <summary>
		/// Reduces damage done
		/// </summary>
		/// <remarks>Applicable only to armor.</remarks>
		Protection = 0,

		/// <summary>
		/// Reduces damage done by fire
		/// </summary>
		/// <remarks>Applicable only to armor.</remarks>
		FireProtection = 1,

		/// <summary>
		/// Reduces damage done by falling
		/// </summary>
		/// <remarks>Applicable only to boots.</remarks>
		FeatherFalling = 2,

		/// <summary>
		/// Reduces damage done by explosions
		/// </summary>
		/// <remarks>Applicable only to armor.</remarks>
		BlastProtection = 3,

		/// <summary>
		/// Reduces damage done by projectiles
		/// </summary>
		/// <remarks>Applicable only to armor.</remarks>
		ProjectileProtection = 4,

		/// <summary>
		/// Decreases rate of air loss under water
		/// </summary>
		/// <remarks>Applicable only to helmets.</remarks>
		Respiration = 5,

		/// <summary>
		/// Increases underwater mining speed
		/// </summary>
		/// <remarks>Applicable only to helmets.</remarks>
		AquaAffinity = 6,

		/// <summary>
		/// Chance of damaging mobs that attack the player
		/// </summary>
		/// <remarks>Applicable only to armor.</remarks>
		Thorns = 7,

		/// <summary>
		/// Extra damage
		/// </summary>
		/// <remarks>Applicable only to weapons.</remarks>
		Sharpness = 16,

		/// <summary>
		/// Extra damage to undead (zombies, zombie pigmen, skeletons, and withers)
		/// </summary>
		/// <remarks>Applicable only to weapons.</remarks>
		Smite = 17,

		/// <summary>
		/// Extra damage to spiders, cave spiders, and silverfish
		/// </summary>
		/// <remarks>Applicable only to weapons.</remarks>
		BaneOfArthropods = 18,

		/// <summary>
		/// Mobs are knocked back further when hit
		/// </summary>
		/// <remarks>Applicable only to weapons.</remarks>
		Knockback = 19,

		/// <summary>
		/// Mobs are lit on fire when hit
		/// </summary>
		/// <remarks>Applicable only to weapons.</remarks>
		FireAspect = 20,

		/// <summary>
		/// Increases mob drop rate
		/// </summary>
		/// <remarks>Applicable only to weapons.</remarks>
		Looting = 21,

		/// <summary>
		/// Faster mining speed
		/// </summary>
		/// <remarks>Applicable only to tools.</remarks>
		Efficiency = 32,

		/// <summary>
		/// Mined blocks will drop instead of their normal drop
		/// </summary>
		/// <remarks>Applicable only to tools.</remarks>
		SilkTouch = 33,

		/// <summary>
		/// Increased durability
		/// </summary>
		/// <remarks>Applicable only to items that have durability.</remarks>
		Unbreaking = 34,

		/// <summary>
		/// Increased drop rate
		/// </summary>
		/// <remarks>Applicable only to tools.</remarks>
		Fortune = 35,

		/// <summary>
		/// Increased arrow damage
		/// </summary>
		/// <remarks>Applicable only to bows.</remarks>
		Power = 48,

		/// <summary>
		/// Increased arrow knockback
		/// </summary>
		/// <remarks>Applicable only to bows.</remarks>
		Punch = 49,
		
		/// <summary>
		/// Arrows shot from the bow will light mobs on fire
		/// </summary>
		/// <remarks>Applicable only to bows.</remarks>
		Flame = 50,

		/// <summary>
		/// Firing the bow does not use any arrows
		/// </summary>
		/// <remarks>Applicable only to bows.</remarks>
		Infinity = 51
	}
}
