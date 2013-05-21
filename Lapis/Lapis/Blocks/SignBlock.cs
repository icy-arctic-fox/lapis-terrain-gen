using System;
using Lapis.IO.NBT;
using Lapis.Utility;

namespace Lapis.Blocks
{
	/// <summary>
	/// Base class for sign blocks
	/// </summary>
	public abstract class SignBlock : TileEntity, IDataBlock
	{
		/// <summary>
		/// Maximum number of characters allowed on one line
		/// </summary>
		public const int MaxLineLength = 16;

		private readonly string _line1, _line2, _line3, _line4;

		#region Properties

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
			get { return 5f; }
		}

		#endregion

		/// <summary>
		/// First line of text on the sign
		/// </summary>
		public string Line1
		{
			get { return _line1; }
		}

		/// <summary>
		/// Second line of text on the sign
		/// </summary>
		public string Line2
		{
			get { return _line2; }
		}

		/// <summary>
		/// Third line of text on the sign
		/// </summary>
		public string Line3
		{
			get { return _line3; }
		}

		/// <summary>
		/// Last line of text on the sign
		/// </summary>
		public string Line4
		{
			get { return _line4; }
		}

		/// <summary>
		/// Block data value as a string
		/// </summary>
		public abstract string DataString { get; }

		/// <summary>
		/// Creates a new sign block
		/// </summary>
		/// <param name="data">Additional meta-data associated with the block</param>
		protected SignBlock (byte data)
			: base(data)
		{
			_line1 = String.Empty;
			_line2 = String.Empty;
			_line3 = String.Empty;
			_line4 = String.Empty;
		}

		/// <summary>
		/// Creates a new sign block
		/// </summary>
		/// <param name="data">Additional meta-data associated with the block</param>
		/// <param name="line1">First line of text</param>
		/// <param name="line2">Second line of text</param>
		/// <param name="line3">Third line of text</param>
		/// <param name="line4">Last line of text</param>
		/// <remarks>If null is used for a line of text, it is changed to an empty string.
		/// If any lines of text are longer than the maximum allowed characters per line, then the strings are trimmed.</remarks>
		protected SignBlock (byte data, string line1, string line2, string line3, string line4)
			: base(data)
		{
			_line1 = correctLine(line1);
			_line2 = correctLine(line2);
			_line3 = correctLine(line3);
			_line4 = correctLine(line4);
		}

		private static string correctLine (string line)
		{
			if(line != null)
			{
				if(line.Length > MaxLineLength)
					line = line.Substring(0, MaxLineLength);
				line = line.Replace('\n', ' ');
				line = line.Replace('\r', ' ');
			}
			else
				line = String.Empty;
			return line;
		}

		#region NBT data
		/// <summary>
		/// Name of the node that stores the first line of text
		/// </summary>
		protected const string Text1NodeName = "Text1";

		/// <summary>
		/// Name of the node that stores the second line of text
		/// </summary>
		protected const string Text2NodeName = "Text2";

		/// <summary>
		/// Name of the node that stores the third line of text
		/// </summary>
		protected const string Text3NodeName = "Text3";

		/// <summary>
		/// Name of the node that stores the last line of text
		/// </summary>
		protected const string Text4NodeName = "Text4";

		/// <summary>
		/// ID of the tile entity
		/// </summary>
		/// <remarks>The tile entity ID for this block is "Sign"</remarks>
		protected override string TileEntityId
		{
			get { return "Sign"; }
		}

		/// <summary>
		/// Creates a new sign block
		/// </summary>
		/// <param name="data">Additional data for the block</param>
		/// <param name="tileData">Node that contains the tile entity data</param>
		protected SignBlock (byte data, Node tileData)
			: base(data, tileData)
		{
			var rootNode = tileData as CompoundNode;
			_line1 = validateLineNode(rootNode, Text1NodeName);
			_line2 = validateLineNode(rootNode, Text2NodeName);
			_line3 = validateLineNode(rootNode, Text3NodeName);
			_line4 = validateLineNode(rootNode, Text4NodeName);
		}

		private static string validateLineNode (CompoundNode node, string nodeName)
		{
			if(node.Contains(nodeName))
			{
				var tempNode = node[nodeName] as StringNode;
				if(null != tempNode)
					return correctLine(tempNode.Value);
			}
			return String.Empty;
		}
		
		/// <summary>
		/// Adds the NBT data associated 
		/// </summary>
		/// <param name="node">Node to add data to</param>
		protected override void InsertDataIntoNode (CompoundNode node)
		{
			var line1Node = new StringNode(Text1NodeName, _line1);
			var line2Node = new StringNode(Text2NodeName, _line2);
			var line3Node = new StringNode(Text3NodeName, _line3);
			var line4Node = new StringNode(Text4NodeName, _line4);
			node.Add(line1Node);
			node.Add(line2Node);
			node.Add(line3Node);
			node.Add(line4Node);
		}
		#endregion

		/// <summary>
		/// Compares the block to another block
		/// </summary>
		/// <param name="block">Block to compare against</param>
		/// <returns>True if the block is the same</returns>
		/// <remarks>The text on the sign is compared.</remarks>
		public override bool Equals (IBlock block)
		{
			if(base.Equals(block))
			{
				var signBlock = block as SignBlock;
				if(!ReferenceEquals(null, signBlock))
					return _line1 == signBlock._line1 &&
						   _line2 == signBlock._line2 &&
						   _line3 == signBlock._line3 &&
						   _line4 == signBlock._line4;
			}
			return false;
		}

		/// <summary>
		/// Generates a hash code from the sign block
		/// </summary>
		/// <returns>A hash code</returns>
		public override int GetHashCode ()
		{
			return HashUtility.ComputeHash(base.GetHashCode(), _line1, _line2, _line3, _line4);
		}

		/// <summary>
		/// Generates a string that contains information about and the contents of the sign
		/// </summary>
		/// <returns>A string</returns>
		public override string ToString ()
		{
			var sb = new System.Text.StringBuilder(base.ToString());
			sb.Append(Environment.NewLine);
			sb.AppendLine(_line1);
			sb.AppendLine(_line2);
			sb.AppendLine(_line3);
			sb.Append(_line4);
			return sb.ToString();
		}
	}
}
