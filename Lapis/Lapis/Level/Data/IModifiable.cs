namespace Lapis.Level.Data
{
	/// <summary>
	/// An object that can be modified and tracks when it has been modified
	/// </summary>
	public interface IModifiable
	{
		/// <summary>
		/// Whether or not the object has been modified
		/// </summary>
		bool Modified { get; }

		/// <summary>
		/// Marks the object an un-modified
		/// </summary>
		void ClearModificationFlag ();
	}
}
