using Lapis.Blocks;

namespace Lapis.Level.Data
{
	public struct BlockInformation
	{
		private readonly BlockType _type;
		private readonly byte _data, _light;

		/// <summary>
		/// Creates new block information for a single block
		/// </summary>
		/// <param name="type"></param>
		/// <param name="data"></param>
		/// <param name="skyLight"></param>
		/// <param name="blockLight"></param>
		public BlockInformation (BlockType type, byte data, byte skyLight, byte blockLight)
		{
			_type  = type;
			_data  = (byte)(data & 0x0f);
			_light = (byte)((skyLight & 0x0f) | ((blockLight & 0x0f) << 4));
		}

		/// <summary>
		/// Block type
		/// </summary>
		public BlockType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Block data
		/// </summary>
		public byte Data
		{
			get { return _data; }
		}

		/// <summary>
		/// Sky light
		/// </summary>
		public byte SkyLight
		{
			get { return (byte)(_light & 0x0f); }
		}

		/// <summary>
		/// Block light
		/// </summary>
		public byte BlockLight
		{
			get { return (byte)((_light & 0xf0) >> 4); }
		}
	}
}
