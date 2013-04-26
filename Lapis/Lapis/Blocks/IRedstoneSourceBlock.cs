using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lapis.Blocks
{
	public interface IRedstoneSourceBlock
	{
		/// <summary>
		/// Whether or not the block is powered and giving off a redstone current
		/// </summary>
		bool Powered { get; }

		/// <summary>
		/// Strength of the redstone current that the block is giving off (0-15)
		/// </summary>
		byte CurrentStrength { get; }
	}
}
