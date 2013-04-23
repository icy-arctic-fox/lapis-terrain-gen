namespace Lapis.Blocks
{
	public class IronDoorBlock : DoorBlock
	{
		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.IronDoor; }
		}

		/// <summary>
		/// Whether or not the block can catch fire
		/// </summary>
		public override bool Flammable
		{
			get { return false; }
		}

		/// <summary>
		/// Amount of resistance to blasts before being destroyed
		/// </summary>
		public override float BlastResistance
		{
			get { return 25f; }
		}
		#endregion

		/// <summary>
		/// Creates a new iron door block
		/// </summary>
		public IronDoorBlock ()
			: base(0)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron door block
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		public IronDoorBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates an new iron door block for the top half of the door
		/// </summary>
		/// <param name="leftHinge">True if the hinge is on the left side of door when looking at the front</param>
		protected IronDoorBlock (bool leftHinge)
			: base(leftHinge)
		{
			// ...
		}

		/// <summary>
		/// Creates a new iron door block for the bottom half of the door
		/// </summary>
		/// <param name="orientation">Direction that the door is facing</param>
		/// <param name="open">True if the door is open</param>
		protected IronDoorBlock (DoorOrientation orientation, bool open)
			: base(orientation, open)
		{
			// ...
		}
	}
}
