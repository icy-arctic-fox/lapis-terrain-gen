using System;
using System.ComponentModel;

namespace Lapis.Utility
{
	/// <summary>
	/// Useful methods for delegates and events
	/// </summary>
	public static class DelegateUtility
	{
		/// <summary>
		/// Dispatches event listeners for an event
		/// </summary>
		/// <typeparam name="TEventArgs">Event arguments type</typeparam>
		/// <param name="ev">Event to dispatch</param>
		/// <param name="sender">Sender of the event (this)</param>
		/// <param name="args">Event arguments to pass</param>
		public static void TriggerEvent<TEventArgs> (this EventHandler<TEventArgs> ev, object sender, TEventArgs args) where TEventArgs : EventArgs
		{
			if(null == sender)
				throw new ArgumentNullException("sender", "The sender of the event can't be null.");
			if(null == args)
				throw new ArgumentNullException("args", "The event arguments can't be null.");

			if(null != ev)
			{
				foreach(var d in ev.GetInvocationList())
					CallDelegate(d, sender, args);
			}
		}

		/// <summary>
		/// Calls a delegate that might be on another thread
		/// </summary>
		/// <param name="d">Delegate to call</param>
		/// <param name="args">Arguments to pass to the delegate</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="d"/> is null</exception>
		/// <remarks>This method handles safely calling a delegate that might want to be invoked on another thread, such as a GUI.</remarks>
		public static void CallDelegate (this Delegate d, params object[] args)
		{
			if(null == d)
				throw new ArgumentNullException("d", "The delegate to invoke can't be null.");

			var sync = d.Target as ISynchronizeInvoke;
			if(sync == null) // Not a GUI thread
				d.DynamicInvoke(args);
			else // GUI thread, need to raise event on another thread
				sync.BeginInvoke(d, args);
		}
	}
}
