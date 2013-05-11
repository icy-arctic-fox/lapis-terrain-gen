namespace Lapis
{
	/// <summary>
	/// A component that can be loaded in at runtime
	/// </summary>
	public interface IPlugin : IPluginInfo
	{
		/// <summary>
		/// Invoked when the plug-in is being started
		/// </summary>
		/// <remarks>Activating the plug-in should take place in this method instead of a constructor.
		/// Variables can be initialized in a constructor,
		/// but listeners, threads, and any other actions should be performed in this method.</remarks>
		void StartPlugin ();

		/// <summary>
		/// Invoked when the plug-in is being stopped
		/// </summary>
		/// <remarks>The plug-in is responsible for cleaning up any resources it created when this method is called.</remarks>
		void StopPlugin ();
	}

	/// <summary>
	/// Basic information about a plug-in
	/// </summary>
	public interface IPluginInfo
	{
		/// <summary>
		/// Name of the plug-in
		/// </summary>
		string PluginName { get; }

		/// <summary>
		/// Version number of the plug-in
		/// </summary>
		/// <remarks>This value is used to track multiple versions of plug-ins.
		/// With this value, multiple versions of the same plug-in (same name) can be loaded.</remarks>
		int PluginVersion { get; }

		/// <summary>
		/// Brief description of what the plug-in does
		/// </summary>
		string PluginDescription { get; }

		/// <summary>
		/// Name and email address of the creator of the plug-in
		/// </summary>
		string PluginCreator { get; }
	}
}
