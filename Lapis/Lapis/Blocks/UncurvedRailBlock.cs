﻿namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for rail blocks that can't curve
	/// </summary>
	/// <remarks>Technical rail types that can't curve derive from this class.</remarks>
	public abstract class UncurvedRailBlock : RailBaseBlock
	{
		/// <summary>
		/// Direction that the rail is facing
		/// </summary>
		public RailOrientation Orientation
		{
			get { return (RailOrientation)_data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public override string DataString
		{
			get { return Orientation.ToString(); }
		}

		/// <summary>
		/// Creates a new un-curved rail block
		/// </summary>
		protected UncurvedRailBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new un-curved rail block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		protected UncurvedRailBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new un-curved rail block
		/// </summary>
		/// <param name="orientation">Direction that the rail block is facing</param>
		protected UncurvedRailBlock (RailOrientation orientation)
			: base((byte)orientation)
		{
			// ...
		}
	}
}
