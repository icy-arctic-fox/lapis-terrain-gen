using System;

namespace Lapis
{
	/// <summary>
	/// Describes an event where an error occurred while loading a plug-in
	/// </summary>
	public class PluginLoadErrorEventArgs : EventArgs
	{
		private readonly string _filepath, _plugin;
		private readonly Exception _exception;

		/// <summary>
		/// Creates a new plug-in load error event
		/// </summary>
		/// <param name="filepath">Path to the offending DLL</param>
		/// <param name="plugin">Name of the plug-in that caused the error</param>
		/// <param name="exception">Exception that was thrown</param>
		public PluginLoadErrorEventArgs (string filepath, string plugin, Exception exception)
		{
			_filepath  = filepath;
			_plugin    = plugin;
			_exception = exception;
		}

		/// <summary>
		/// Path to the offending DLL
		/// </summary>
		public string Filepath
		{
			get { return _filepath; }
		}

		/// <summary>
		/// Class name of the plug-in that caused the error
		/// </summary>
		/// <remarks>This value may be null if the DLL file containing the plug-in cause the problem before the plug-in was even found.</remarks>
		public string PluginClassName
		{
			get { return _plugin; }
		}

		/// <summary>
		/// Exception that was thrown
		/// </summary>
		public Exception Exception
		{
			get { return _exception; }
		}
	}
}
