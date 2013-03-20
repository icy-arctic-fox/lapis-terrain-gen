namespace Lapis
{
	/// <summary>
	/// A component that can be loaded in at runtime
	/// </summary>
	public interface IPlugin
	{
		#region Meta-data
		/// <summary>
		/// Name of the plug-in
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Version number of the plug-in
		/// </summary>
		/// <remarks>This value is used to track multiple versions of plug-ins.
		/// With this value, multiple versions of the same plug-in (same name) can be loaded.</remarks>
		int Version { get; }

		/// <summary>
		/// Brief description of what the plug-in does
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Name and email address of the creator of the plug-in
		/// </summary>
		string Author { get; }
		#endregion
	}
}
