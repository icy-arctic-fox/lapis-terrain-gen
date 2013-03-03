using System.IO;
using Lapis.Utility;

namespace Lapis.IO
{
	/// <summary>
	/// A stream reader that can read in little and big endian
	/// </summary>
	public class EndianBinaryReader : BinaryReader
	{
		private readonly Endian _endian;
		private readonly byte[] _buffer = new byte[sizeof(double)];

		// A buffer is kept internally so a new byte array doesn't need to be made for every read call.
		// This doubles as an internal lock, since two reads going on at the same time would be bad.

		/// <summary>
		/// Endian being used for reading
		/// </summary>
		public Endian Endian
		{
			get { return _endian; }
		}

		/// <summary>
		/// Creates a new endian-aware binary reader
		/// </summary>
		/// <param name="input">Input stream to read from</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryReader (Stream input, Endian e)
			: base(input)
		{
			_endian = e;
		}

		/// <summary>
		/// Creates a new endian-aware binary reader
		/// </summary>
		/// <param name="input">Input stream to read from</param>
		/// <param name="encoding">Text encoding</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryReader (Stream input, System.Text.Encoding encoding, Endian e = Endian.Big)
			: base(input, encoding)
		{
			_endian = e;
		}

		/// <summary>
		/// Reads a double-precision floating point number and advances the stream by 8 bytes
		/// </summary>
		/// <returns>A double</returns>
		public override double ReadDouble ()
		{
			double value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(double));
				value = _buffer.ToDouble(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads a signed short integer and advances the stream by 2 bytes
		/// </summary>
		/// <returns>A signed short</returns>
		public override short ReadInt16 ()
		{
			short value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(short));
				value = _buffer.ToShort(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads a signed integer and advances the stream by 4 bytes
		/// </summary>
		/// <returns>A signed integer</returns>
		public override int ReadInt32 ()
		{
			int value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(int));
				value = _buffer.ToInt(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads a signed long integer and advances the stream by 8 bytes
		/// </summary>
		/// <returns>A signed long</returns>
		public override long ReadInt64 ()
		{
			long value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(long));
				value = _buffer.ToLong(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads a single-precision floating point number and advances the stream by 4 bytes
		/// </summary>
		/// <returns>A float</returns>
		public override float ReadSingle ()
		{
			float value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(float));
				value = _buffer.ToFloat(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads an unsigned short integer and advances the stream by 2 bytes
		/// </summary>
		/// <returns>An unsigned short</returns>
		public override ushort ReadUInt16 ()
		{
			ushort value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(ushort));
				value = _buffer.ToUShort(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads an unsigned integer and advances the stream by 4 bytes
		/// </summary>
		/// <returns>An unsigned integer</returns>
		public override uint ReadUInt32 ()
		{
			uint value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(uint));
				value = _buffer.ToUInt(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads an unsigned long integer and advances the stream by 8 bytes
		/// </summary>
		/// <returns>An unsigned long</returns>
		public override ulong ReadUInt64 ()
		{
			ulong value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, sizeof(ulong));
				value = _buffer.ToULong(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads a signed integer and advances the stream by 3 bytes
		/// </summary>
		/// <returns>A signed integer</returns>
		public int ReadInt24 ()
		{
			int value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, 3);
				if(Endian.Big == _endian)
					_buffer[0] = 0;
				else
					_buffer[3] = 0;
				value = _buffer.ToInt(0, _endian);
			}
			return value;
		}

		/// <summary>
		/// Reads an unsigned integer and advances the stream by 3 bytes
		/// </summary>
		/// <returns>An unsigned integer</returns>
		public uint ReadUInt24 ()
		{
			uint value;
			lock(_buffer)
			{
				base.Read(_buffer, 0, 3);
				if(Endian.Big == _endian)
					_buffer[0] = 0;
				else
					_buffer[3] = 0;
				value = _buffer.ToUInt(0, _endian);
			}
			return value;
		}
	}
}
