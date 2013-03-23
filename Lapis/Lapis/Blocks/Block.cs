namespace Lapis.Blocks
{
	// TODO: Re-implement features in this class

	/// <summary>
	/// Base block data
	/// </summary>
	/// <remarks>Instances of this class are not tied to any active chunk data.
	/// Updating the values in this block object will not update them in the chunk.
	/// This class should be used to retrieve block information from a chunk and construct block information before storing it in a chunk.</remarks>
	public abstract class Block
	{
		private readonly byte _data;

		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public abstract BlockType Type { get; }

		/// <summary>
		/// Whether or not the block is solid (non solid blocks can be passed through)
		/// </summary>
		public abstract bool IsSolid { get; }

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent)
		/// </summary>
		public abstract int Opacity { get; }

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public abstract int Luminance { get; }

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public abstract int BlastResistance { get; }

		/// <summary>
		/// Raw meta-data associated with the block
		/// </summary>
		public byte Data
		{
			get { return _data; }
		}
		#endregion

		/// <summary>
		/// Creates the base for a block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		protected Block (byte data)
		{
			_data = data;
		}

		/// <summary>
		/// Checks if another object is equal to the block
		/// </summary>
		/// <param name="obj">Object to compare against</param>
		/// <returns>True if <paramref name="obj"/> is considered equal to the block or false if it's not</returns>
		/// <remarks><paramref name="obj"/> is considered equal if it's not null, is a Block or BlockRef, and the block type and data are the same.
		/// Sub-classes that have additional properties will want to override the protected Equals() method instead of this one.</remarks>
		public override bool Equals (object obj)
		{
			if(null != obj)
			{
/*				if(obj is BlockRef)
				{
					Block block = (BlockRef)obj;
					return Equals(block);
				}
				else
				{*/
					var block = obj as Block;
					if(block != null)
						return Equals(block);
//				}
			}
			return false;
		}

		/// <summary>
		/// Checks if a block's contents are equal to the current block
		/// </summary>
		/// <param name="block">Block to compare against</param>
		/// <returns>True if the block contents are the same or false if they aren't</returns>
		/// <remarks>Sub-classes should override this method if they have additional properties (such as a tile-entity).</remarks>
		protected virtual bool Equals (Block block)
		{
			return (block.Type == Type && block._data == _data);
		}

		/// <summary>
		/// Creates a hash code from the block information
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			var hash = (byte)Type | (_data << 8);
			return hash;
		}

		/// <summary>
		/// Generates a string from the block information
		/// </summary>
		/// <returns>A string containing the block information</returns>
		/// <remarks>The string will be formatted as: TYPE(DATA_HEX)</remarks>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append(Type);
			sb.Append('(');
			sb.AppendFormat("{0:x}", _data);
			sb.Append(')');
			return sb.ToString();
		}
	}
}
