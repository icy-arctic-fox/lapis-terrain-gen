namespace Lapis.Blocks
{
	public class WoodDoorBlock : DoorBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.WoodDoor; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return true; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 15f; }
		}
		#endregion

		/// <summary>
		/// Creates a new wood door block
		/// </summary>
		public WoodDoorBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood door block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		public WoodDoorBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates an new wood door block for the top half of the door
		/// </summary>
		/// <param name="leftHinge">True if the hinge is on the left side of door when looking at the front</param>
		protected WoodDoorBlock (bool leftHinge)
			: base(leftHinge)
		{
			// ...
		}

		/// <summary>
		/// Creates a new wood door block for the bottom half of the door
		/// </summary>
		/// <param name="orientation">Direction that the door is facing</param>
		/// <param name="open">True if the door is open</param>
		protected WoodDoorBlock (DoorOrientation orientation, bool open)
			: base(orientation, open)
		{
			// ...
		}
	}
}
