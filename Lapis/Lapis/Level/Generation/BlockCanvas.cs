using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lapis.Spatial;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// Provides an interface for creating structures made of blocks and then inserting them into chunks
	/// </summary>
	public class BlockCanvas : IDisposable
	{

		#region Disposable
		/// <summary>
		/// Disposes the contents of the object
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Cleans up resources held by the object
		/// </summary>
		/// <param name="disposing">True if child variables should be cleaned up</param>
		protected virtual void Dispose (bool disposing)
		{
			// ...
		}

		~BlockCanvas ()
		{
			Dispose(false);
		}
		#endregion
	}
}
