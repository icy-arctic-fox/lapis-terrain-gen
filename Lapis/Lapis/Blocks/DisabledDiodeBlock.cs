namespace Lapis.Blocks
{
	public class DisabledDiodeBlock : DiodeBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.DisabledDiode; }
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
		/// Creates a new disabled diode block
		/// </summary>
		public DisabledDiodeBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new disabled diode block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public DisabledDiodeBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new disabled diode block
		/// </summary>
		/// <param name="orientation">Direction that the diode block is pointing</param>
		/// <param name="delay">Delay that the diode is set to (1-4)</param>
		public DisabledDiodeBlock (DiodeOrientation orientation, byte delay)
			: base(orientation, delay)
		{
			// ...
		}
	}
}
