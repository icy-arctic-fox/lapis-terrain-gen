namespace Lapis.Items
{
	/// <summary>
	/// An item that can be used for harvesting or mining resources
	/// </summary>
	public interface IToolItem : IItem
	{
		/// <summary>
		/// Item's tool type
		/// </summary>
		ToolType ToolType { get; }
	}
}
