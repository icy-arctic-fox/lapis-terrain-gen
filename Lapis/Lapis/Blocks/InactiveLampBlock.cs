namespace Lapis.Blocks
{
	public class InactiveLampBlock : LampBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.InactiveLamp; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 0; }
		}
		#endregion

		/// <summary>
		/// Creates a new inactive lamp block
		/// </summary>
		public InactiveLampBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new inactive lamp block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		protected InactiveLampBlock (byte data)
			: base(data)
		{
			// ...
		}
	}
}
