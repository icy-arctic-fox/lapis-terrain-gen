using System;

namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for door blocks
	/// </summary>
	/// <remarks>Door blocks have different data values for the top and bottom parts.
	/// Be sure to check which part of the door it is before using properties.</remarks>
	public abstract class DoorBlock : Block, IDataBlock
	{
		#region Properties
		/// <summary>
		/// Whether or not the block is solid (non-solid blocks can be passed through)
		/// </summary>
		public override bool IsSolid
		{
			get { return true; }
		}

		/// <summary>
		/// Whether or not the block obeys physics
		/// </summary>
		public override bool Physics
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
			get { return 0; }
		}
		#endregion

		/// <summary>
		/// Whether or not the door is the top half of the door
		/// </summary>
		/// <remarks>True means it is the top half of the door, false means it is the bottom half of the door.</remarks>
		public bool TopHalf
		{
			get { return (0x8 == (_data & 0x8)); }
		}

		/// <summary>
		/// Whether or not the door's hinge is on the left
		/// </summary>
		/// <remarks>This is true if the door's hinge is on the left when looking at the front of the door.
		/// This property is only available for the top half of the door.</remarks>
		public bool LeftHinge
		{
			get
			{
				if(!TopHalf)
					throw new InvalidOperationException("The LeftHinge property is only available to the block for the top half of the door.");
				return (0x1 == (_data & 0x1));
			}
		}

		/// <summary>
		/// Whether or not the door is open
		/// </summary>
		/// <remarks>This property is only available for the bottom half of the door.</remarks>
		public bool Open
		{
			get
			{
				if(TopHalf)
					throw new InvalidOperationException("The Open property is only available to the block for the bottom half of the door.");
				return (0x4 == (_data & 0x4));
			}
		}

		/// <summary>
		/// The direction that the door is facing
		/// </summary>
		/// <remarks>This property is only available for the bottom half of the door.</remarks>
		public DoorOrientation Orientation
		{
			get
			{
				if(TopHalf)
					throw new InvalidOperationException("The Open property is only available to the block for the bottom half of the door.");
				return ((DoorOrientation)(_data & 0x3));
			}
		}

		/// <summary>
		/// Block data value as a string
		/// </summary>
		public string DataString
		{
			get
			{
				if(TopHalf)
					return LeftHinge ? "TopHalf LeftHinge" : "TopHalf RightHinge";
				return (Open ? "BottomHalf Open " : " BottomHalf Closed ") + Orientation.ToString();
			}
		}

		/// <summary>
		/// Creates a new door block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		protected DoorBlock (byte data)
			: base(data)
		{
			// ...
		}

		/// <summary>
		/// Creates an new door block for the top half of the door
		/// </summary>
		/// <param name="leftHinge">True if the hinge is on the left side of door when looking at the front</param>
		protected DoorBlock (bool leftHinge)
			: base((byte)(0x8 | (leftHinge ? 0x1 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Creates a new door block for the bottom half of the door
		/// </summary>
		/// <param name="orientation">Direction that the door is facing</param>
		/// <param name="open">True if the door is open</param>
		protected DoorBlock (DoorOrientation orientation, bool open)
			: base((byte)((byte)orientation | (open ? 0x4 : 0x0)))
		{
			// ...
		}

		/// <summary>
		/// Directions that a door can face
		/// </summary>
		public enum DoorOrientation : byte
		{
			West  = 0x0,
			North = 0x1,
			East  = 0x2,
			South = 0x3
		}
	}
}
