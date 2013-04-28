using System;
using System.IO;
using Lapis.IO.NBT;
using Lapis.Spatial;

namespace Lapis.Blocks
{
	/// <summary>
	/// A block that has extra data associated with it
	/// </summary>
	public abstract class TileEntity : Block
	{
		/// <summary>
		/// Creates the base for a tile entity
		/// </summary>
		/// <param name="data">Additional meta-data for the block</param>
		protected TileEntity (byte data)
			: base(data)
		{
			// ...
		}

		#region NBT data
		#region Node names
		private const string NodeName   = "Entity";
		private const string IdNodeName = "id";
		private const string XNodeName  = "x";
		private const string YNodeName  = "y";
		private const string ZNodeName  = "z";
		#endregion

		/// <summary>
		/// Internal name of the tile entity
		/// </summary>
		protected abstract string TileEntityId { get; }

		/// <summary>
		/// Creates a new tile entity
		/// </summary>
		/// <param name="data">Additional meta-data associated with the block</param>
		/// <param name="tileData">Node that contains data for the tile entity</param>
		protected TileEntity (byte data, Node tileData)
			: base(data)
		{
			if(null == tileData)
				throw new ArgumentNullException("tileData", "The tile data node can't be null.");
			var rootNode = tileData as CompoundNode;
			if(null == rootNode)
				throw new InvalidDataException("The node type for the tile data is invalid (expected: " + NodeType.Compound + ", got: " + tileData.Type + ").");
		}

		/// <summary>
		/// Fills the NBT node with data from the tile entity
		/// </summary>
		/// <param name="node">Node to put data in</param>
		protected abstract void InsertDataIntoNode (CompoundNode node);

		/// <summary>
		/// Validates and NBT node for a tile entity
		/// </summary>
		/// <param name="node">Node to check</param>
		/// <param name="coord">The coordinate of the tile entity</param>
		/// <returns>True if the node is valid</returns>
		public static bool ValidateTileEntity (Node node, out XYZCoordinate coord)
		{
			var tileEntityNode = node as CompoundNode;
			if(null != tileEntityNode)
			{
				if(tileEntityNode.Contains(XNodeName) && tileEntityNode.Contains(YNodeName) && tileEntityNode.Contains(ZNodeName))
				{
					var xNode = tileEntityNode[XNodeName];
					var yNode = tileEntityNode[YNodeName];
					var zNode = tileEntityNode[ZNodeName];
					if(xNode.Type == NodeType.Int && yNode.Type == NodeType.Int && zNode.Type == NodeType.Int)
					{
						var x = ((IntNode)xNode).Value;
						var y = ((IntNode)yNode).Value;
						var z = ((IntNode)zNode).Value;
						coord = new XYZCoordinate(x, y, z);
						return true;
					}
				}
			}
			coord = new XYZCoordinate();
			return false;
		}

		/// <summary>
		/// Constructs an NBT node that contains the extra data for the tile entity
		/// </summary>
		/// <param name="x">Position of the block within the chunk along the x-axis</param>
		/// <param name="y">Position of the block within the chunk along the y-axis</param>
		/// <param name="z">Position of the block within the chunk along the z-axis</param>
		/// <returns>An NBT node</returns>
		/// <remarks>Blocks implementing this interface need to create the ID, X, Y, and Z nodes.</remarks>
		public Node GetNbtData (int x, int y, int z)
		{
			var node = new CompoundNode(NodeName) {
				new StringNode(IdNodeName, TileEntityId),
				new IntNode(XNodeName, x),
				new IntNode(YNodeName, y),
				new IntNode(ZNodeName, z)
			};
			InsertDataIntoNode(node);
			return node;
		}
		#endregion
	}
}
