namespace Lapis.Blocks
{
	public class JackOLanternBlock : PumpkinBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.JackOLantern; }
		}

		/// <summary>
		/// Amount of block light that the block gives off
		/// </summary>
		public override byte Luminance
		{
			get { return 15; }
		}
		#endregion

		/// <summary>
		/// Creates a new jack-o-lantern block
		/// </summary>
		public JackOLanternBlock ()
		{
			// ...
		}

		/// <summary>
		/// Creates a new jack-o-lantern block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public JackOLanternBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates a new jack-o-lantern block
		/// </summary>
		/// <param name="orientation">Direction that the pumpkin is facing</param>
		public JackOLanternBlock (PumpkinOrientation orientation)
			: base(orientation)
		{
			// ...
		}
	}
}
