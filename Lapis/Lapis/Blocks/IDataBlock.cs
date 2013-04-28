namespace Lapis.Blocks
{
	/// <summary>
	/// A block class that makes use of extra data values
	/// </summary>
	public interface IDataBlock
	{
		/// <summary>
		/// Representation of the block's extra data as a string
		/// </summary>
		string DataString { get; }
	}
}
