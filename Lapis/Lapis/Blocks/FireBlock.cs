namespace Lapis.Blocks
{
	public class FireBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Fire; }
		}

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return false; }
		}

		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		public override bool Physics
		{
			get { return false; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		public override byte Opacity
		{
			get { return 0; }
		}

		/// <summary>
		/// Whether or not the block diffuses light
		/// </summary>
		public override bool Diffuse
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 15; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 0f; }
		}
		#endregion

		/// <summary>
		/// Age of the fire
		/// </summary>
		public byte Age
		{
			get { return _data; }
		}

		/// <summary>
		/// Whether or not the fire burns forever
		/// </summary>
		public bool Eternal
		{
			get { return 0xf == _data; }
		}

		/// <summary>
		/// Representation of the block's data as a string
		/// </summary>
		public string DataString
		{
			get { return Eternal ? "Eternal" : "Age: " + Age.ToString(System.Globalization.CultureInfo.InvariantCulture); }
		}

		/// <summary>
		/// Creates a new fire block
		/// </summary>
		public FireBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new fire block
		/// </summary>
		/// <param name="age">Age of the fire (0-14)</param>
		public FireBlock (byte age)
			: base(age)
		{
			// ...
		}

		/// <summary>
		/// Creates a new fire block
		/// </summary>
		/// <param name="eternal">Whether or not the fire burns forever</param>
		public FireBlock (bool eternal)
			: base((byte)(eternal ? 0xf : 0x0))
		{
			// ...
		}
	}
}
