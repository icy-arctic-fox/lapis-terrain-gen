using Lapis.IO.NBT;

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
		public abstract string TileEntityId { get; }

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
			ConstructNode(node);
			return node;
		}

		/// <summary>
		/// Fills the NBT node with data from the tile entity
		/// </summary>
		/// <param name="node">Node to put data in</param>
		protected abstract void ConstructNode (Node node);
		#endregion
	}
}
