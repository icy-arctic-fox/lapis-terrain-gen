namespace Lapis.Items
{
	/// <summary>
	/// An item that can be used for harvesting or mining resources
	/// </summary>
	public interface IToolItem
	{
		/// <summary>
		/// Item's tool type
		/// </summary>
		ToolType ToolType { get; }
	}
}
