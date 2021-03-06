﻿namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for plant blocks that can grow
	/// </summary>
	public abstract class PlantBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		public override bool Physics
		{
			get { return false; }
		}
		#endregion

		/// <summary>
		/// Age of the plant block (0-15)
		/// </summary>
		/// <remarks>0 is a newly placed block, 15 is a fully grown block meaning it should have a piece added above it (if the maximum height isn't reached).</remarks>
		public byte Age
		{
			get { return _data; }
		}

		/// <summary>
		/// Whether or not the plant is fully grown
		/// </summary>
		public bool FullyGrown
		{
			get { return Age >= 15; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Age.ToString(System.Globalization.CultureInfo.InvariantCulture) + "/15"; }
		}

		/// <summary>
		/// Creates a new plant block
		/// </summary>
		protected PlantBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new plant block
		/// </summary>
		/// <param name="age">Age of the plant block (0-15)</param>
		protected PlantBlock (byte age)
			: base(age)
		{
			// ...
		}
	}
}
