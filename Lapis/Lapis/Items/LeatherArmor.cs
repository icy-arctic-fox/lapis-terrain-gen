using System;
using System.Collections.Generic;
using Lapis.IO.NBT;

namespace Lapis.Items
{
	/// <summary>
	/// Armor made out of leather
	/// </summary>
	public abstract class LeatherArmor : DamageableItem, IArmorItem, IDyeableItem
	{
		/// <summary>
		/// Default color of the leather armor
		/// </summary>
		public const int DefaultColor = 0;

		private readonly int _color;

		/// <summary>
		/// Color value for the leather armor
		/// </summary>
		/// <remarks>The color is in the format RGB using the formula:
		/// Red &lt;&lt; 16 | Green &lt;&lt; 8 | Blue</remarks>
		public int Color
		{
			get { return _color; }
		}

		/// <summary>
		/// Amount of red in the leather armor
		/// </summary>
		public byte Red
		{
			get { return (byte)((_color >> 16) & 0xff); }
		}

		/// <summary>
		/// Amount of green in the leather armor
		/// </summary>
		public byte Green
		{
			get { return (byte)((_color >> 8) & 0xff); }
		}

		/// <summary>
		/// Amount of blue in the leather armor
		/// </summary>
		public byte Blue
		{
			get { return (byte)(_color & 0xff); }
		}

		/// <summary>
		/// The item's armor type
		/// </summary>
		public abstract ArmorType ArmorType { get; }

		/// <summary>
		/// Creates a new leather armor item
		/// </summary>
		protected LeatherArmor ()
		{
			_color = DefaultColor;
		}

		/// <summary>
		/// Creates a new leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage)
			: base(damage)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new leather armor item with a repair cost
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, int repairCost)
			: base(damage, repairCost)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new leather armor item with tag data
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, string name, IEnumerable<string> lore)
			: base(name, lore)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new leather armor item with tag data
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new leather armor item with tag data
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, IEnumerable<Enchantment> enchantments)
			: base(enchantments)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(damage, repairCost, enchantments)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchantments, name, lore)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			_color = color;
		}

		/// <summary>
		/// Creates a new enchanted leather armor item
		/// </summary>
		/// <param name="color">Color of the armor</param>
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		/// <remarks>MakeColor can be used to get a color value from its components.</remarks>
		protected LeatherArmor (int color, short damage, int repairCost, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, repairCost, enchantments, name, lore)
		{
			_color = color;
		}

		#region Serialization
		/// <summary>
		/// Creates a new leather armor item from NBT data
		/// </summary>
		/// <param name="node">Node containing information about the item</param>
		protected LeatherArmor (Node node)
			: base(node)
		{
			var displayNode = ((CompoundNode)((CompoundNode)node)[TagNodeName])[DisplayNodeName] as CompoundNode;
			_color = validateColorNode(displayNode);
		}

		#region Node names
		private const string ColorNodeName = "color";
		#endregion

		#region Validation
		private static int validateColorNode (CompoundNode displayNode)
		{
			if(displayNode.Contains(ColorNodeName))
			{
				var colorNode = displayNode[ColorNodeName] as IntNode;
				if(null != colorNode)
					return colorNode.Value;
			}
			return 0;
		}
		#endregion

		#region Construction
		/// <summary>
		/// Inserts the color value into the display node
		/// </summary>
		/// <param name="displayNode">Node to insert into</param>
		protected override void InsertIntoDisplayData (CompoundNode displayNode)
		{
			base.InsertIntoDisplayData(displayNode);
			displayNode.Add(new IntNode(ColorNodeName, _color));
		}
		#endregion
		#endregion

		/// <summary>
		/// Makes a color value from its components
		/// </summary>
		/// <param name="red">Amount of red</param>
		/// <param name="green">Amount of green</param>
		/// <param name="blue">Amount of blue</param>
		/// <returns>A color value</returns>
		public static int MakeColor (byte red, byte green, byte blue)
		{
			return (red << 16) | (green << 8) | blue;
		}
	}
}
