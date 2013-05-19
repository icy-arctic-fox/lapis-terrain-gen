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
		private readonly int? _color;

		/// <summary>
		/// Whether or not the leather armor has been dyed
		/// </summary>
		public bool Dyed
		{
			get { return _color.HasValue; }
		}

		/// <summary>
		/// Color value for the leather armor
		/// </summary>
		/// <remarks>The color is in the format RGB using the formula:
		/// Red &lt;&lt; 16 | Green &lt;&lt; 8 | Blue</remarks>
		public int Color
		{
			get { return _color.HasValue ? _color.Value : 0; }
		}

		/// <summary>
		/// Amount of red in the leather armor
		/// </summary>
		public byte Red
		{
			get { return (byte)((Color >> 16) & 0xff); }
		}

		/// <summary>
		/// Amount of green in the leather armor
		/// </summary>
		public byte Green
		{
			get { return (byte)((Color >> 8) & 0xff); }
		}

		/// <summary>
		/// Amount of blue in the leather armor
		/// </summary>
		public byte Blue
		{
			get { return (byte)(Color & 0xff); }
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
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		protected LeatherArmor (short damage)
			: base(damage)
		{
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		protected LeatherArmor (short damage, int repairCost)
			: base(damage, repairCost)
		{
			_color = null;
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
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected LeatherArmor (string name, IEnumerable<string> lore)
			: base(name, lore)
		{
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected LeatherArmor (short damage, string name, IEnumerable<string> lore)
			: base(damage, name, lore)
		{
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		protected LeatherArmor (short damage, int repairCost, string name, IEnumerable<string> lore)
			: base(damage, repairCost, name, lore)
		{
			_color = null;
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
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (IEnumerable<Enchantment> enchantments)
			: base(enchantments)
		{
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short damage, IEnumerable<Enchantment> enchantments)
			: base(damage, enchantments)
		{
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="repairCost">Additional levels required to repair the item</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short damage, int repairCost, IEnumerable<Enchantment> enchantments)
			: base(damage, repairCost, enchantments)
		{
			_color = null;
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
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(enchantments, name, lore)
		{
			_color = null;
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
		/// <param name="damage">Amount of damage the item has taken</param>
		/// <param name="enchantments">Collection of enchantments the item has</param>
		/// <param name="name">Visible name of the item</param>
		/// <param name="lore">Additional description (or "lore") displayed on the item</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="enchantments"/> is null</exception>
		protected LeatherArmor (short damage, IEnumerable<Enchantment> enchantments, string name, IEnumerable<string> lore)
			: base(damage, enchantments, name, lore)
		{
			_color = null;
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
			_color = null;

			var rootNode = (CompoundNode)node;
			if(rootNode.Contains(TagNodeName))
			{
				var tagNode = rootNode[TagNodeName] as CompoundNode;
				if(null != tagNode)
				{
					if(tagNode.Contains(DisplayNodeName))
					{
						var displayNode = tagNode[DisplayNodeName] as CompoundNode;
						if(null != displayNode)
							_color = validateColorNode(displayNode);
					}
				}
			}
		}

		#region Node names
		private const string ColorNodeName = "color";
		#endregion

		#region Validation
		private static int? validateColorNode (CompoundNode displayNode)
		{
			if(displayNode.Contains(ColorNodeName))
			{
				var colorNode = displayNode[ColorNodeName] as IntNode;
				if(null != colorNode)
					return colorNode.Value;
			}
			return null;
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
			if(_color.HasValue)
				displayNode.Add(new IntNode(ColorNodeName, _color.Value));
		}
		#endregion
		#endregion

		/// <summary>
		/// Gets a string representation of the item
		/// </summary>
		/// <returns>A string</returns>
		/// <remarks>The string will be formatted as: TYPE(DATA_HEX) "NAME" # Enchants USES_REMAINING/MAX_USES COLOR_HEX</remarks>
		public override string ToString ()
		{
			var baseString = base.ToString();
			if(_color.HasValue)
			{
				var colorString = _color.Value.ToString("x6");
				return String.Join(" ", baseString, colorString);
			}
			return baseString;
		}

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
