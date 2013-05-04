namespace Lapis.Items
{
	/// <summary>
	/// An item that can be dyed
	/// </summary>
	public interface IDyeableItem
	{
		/// <summary>
		/// Color value for the item
		/// </summary>
		/// <remarks>The color is in the format RGB using the formula:
		/// Red &lt;&lt; 16 | Green &lt;&lt; 8 | Blue</remarks>
		int Color { get; }

		/// <summary>
		/// Amount of red in the item's color
		/// </summary>
		byte Red { get; }

		/// <summary>
		/// Amount of green in the item's color
		/// </summary>
		byte Green { get; }

		/// <summary>
		/// Amount of blue in the item's color
		/// </summary>
		byte Blue { get; }
	}
}
