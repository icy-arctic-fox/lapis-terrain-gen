using System;

namespace Lapis.Blocks
{
	public class VinesBlock : Block
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Vines; }
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
			get { return true; }
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
			get { return 0; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 1f; }
		}
		#endregion

		/// <summary>
		/// Sides within the block that the vines appear
		/// </summary>
		public VinesLocation Sides
		{
			get { return (VinesLocation)_data; }
		}

		/// <summary>
		/// Creates a new vines block
		/// </summary>
		public VinesBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new vines block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public VinesBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new vines block
		/// </summary>
		/// <param name="sides">Sides of the block that vines appear on</param>
		public VinesBlock (VinesLocation sides)
			: base((byte)sides)
		{
			// ...
		}

		/// <summary>
		/// Sides of the block that vines appear on
		/// </summary>
		/// <remarks>It is implied that there are vines on the top of the block by default, or if there is a solid block above.</remarks>
		[Flags]
		public enum VinesLocation : byte
		{
			Top   = 0x0,
			South = 0x1,
			West  = 0x2,
			North = 0x4,
			East  = 0x8
		}
	}
}
