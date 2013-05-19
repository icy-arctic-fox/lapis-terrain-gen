using System;
using Lapis.IO.NBT;

namespace Lapis.Blocks
{
	public class NoteBlock : TileEntity
	{
		private readonly byte _pitch;

		#region Properties
		/// <summary>
		/// Type that describes the block
		/// </summary>
		/// <remarks>By using this property, you can safely cast the block object.</remarks>
		public override BlockType Type
		{
			get { return BlockType.Note; }
		}

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
			get { return 15; }
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
			get { return 4f; }
		}

		// TODO: Implement tile entity data for 'Music'
		#endregion

		/// <summary>
		/// Pitch that the block is set to
		/// </summary>
		/// <remarks>This is effectively the number of right clicks used to set the pitch.</remarks>
		public byte Pitch
		{
			get { return _pitch; }
		}

		/// <summary>
		/// Creates a new note block
		/// </summary>
		public NoteBlock ()
			: base(0)
		{
			_pitch = 0;
		}

		/// <summary>
		/// Creates a new note block
		/// </summary>
		/// <param name="pitch">Pitch of the note block (0-24)</param>
		public NoteBlock (byte pitch)
			: base(0)
		{
			if(pitch > 24)
				throw new ArgumentOutOfRangeException("pitch", "The pitch can't be higher than 24.");
			_pitch = pitch;
		}

		#region NBT data
		/// <summary>
		/// Name of the node that stores the pitch
		/// </summary>
		protected const string PitchNodeName = "note";

		/// <summary>
		/// ID of the tile entity
		/// </summary>
		/// <remarks>The tile entity ID for this block is "Music"</remarks>
		protected override string TileEntityId
		{
			get { return "Music"; }
		}

		/// <summary>
		/// Creates a new note block
		/// </summary>
		/// <param name="tileData">Node that contains the tile entity data</param>
		public NoteBlock (Node tileData)
			: base(0, tileData)
		{
			_pitch = validatePitchNode((CompoundNode)tileData);
		}

		private static byte validatePitchNode (CompoundNode node)
		{
			if(node.Contains(PitchNodeName))
			{
				var tempNode = node[PitchNodeName] as ByteNode;
				if(null != tempNode)
					return tempNode.Value;
			}
			return 0;
		}
		
		/// <summary>
		/// Adds the NBT data associated 
		/// </summary>
		/// <param name="node">Node to add data to</param>
		protected override void InsertDataIntoNode (CompoundNode node)
		{
			var pitchNode = new ByteNode(PitchNodeName, _pitch);
			node.Add(pitchNode);
		}
		#endregion

		/// <summary>
		/// Compares another block to this one
		/// </summary>
		/// <param name="block">Block to compare against</param>
		/// <returns>True if the blocks are the same</returns>
		public override bool Equals (Block block)
		{
			if(base.Equals(block))
			{
				var noteBlock = block as NoteBlock;
				if(!ReferenceEquals(null, noteBlock))
					return _pitch == noteBlock._pitch;
			}
			return false;
		}

		/// <summary>
		/// Generates a hash code from the note block
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			return base.GetHashCode() | (_pitch << 16);
		}
	}
}
