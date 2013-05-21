using System;

namespace Lapis.Blocks
{
	/// <summary>
	/// Base interface for block interfaces and classes
	/// </summary>
	public interface IBlock : IEquatable<IBlock>, IComparable<IBlock>
	{
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		BlockType Type { get; }

		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		bool IsSolid { get; }

		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		bool Physics { get; }

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		bool Flammable { get; }

		/// <summary>
		/// Amount of light the block absorbs (0 is fully transparent and 15 is fully opaque)
		/// </summary>
		byte Opacity { get; }

		/// <summary>
		/// Whether or not the block diffuses light
		/// </summary>
		bool Diffuse { get; }

		/// <summary>
		/// Amount of block light that the block gives off (0 for none and 15 for full brightness)
		/// </summary>
		byte Luminance { get; }

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		float BlastResistance { get; }

		/// <summary>
		/// Raw meta-data associated with the block
		/// </summary>
		byte Data { get; }
	}
}
