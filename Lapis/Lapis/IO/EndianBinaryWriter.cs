using System.IO;
using Lapis.Utility;

namespace Lapis.IO
{
	/// <summary>
	/// A stream writer that can read in little and big endian
	/// </summary>
	public class EndianBinaryWriter : BinaryWriter
	{
		private readonly Endian _endian;
		private readonly byte[] _buffer = new byte[sizeof(double)];

		// A buffer is kept internally so a new byte array doesn't need to be made for every write call.
		// This doubles as an internal lock, since two writes going on at the same time would be bad.

		/// <summary>
		/// Endian being used for writing
		/// </summary>
		public Endian Endian
		{
			get { return _endian; }
		}

		/// <summary>
		/// Creates a new endian-aware binary writer
		/// </summary>
		/// <param name="output">Output stream to write to</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryWriter (Stream output, Endian e)
			: base(output)
		{
			_endian = e;
		}

		/// <summary>
		/// Creates a new endian-aware binary writer
		/// </summary>
		/// <param name="output">Output stream to write to</param>
		/// <param name="encoding">Text encoding</param>
		/// <param name="e">Endian to use</param>
		public EndianBinaryWriter (Stream output, System.Text.Encoding encoding, Endian e = Endian.Big)
			: base(output, encoding)
		{
			_endian = e;
		}

		/// <summary>
		/// Writes a double-precision floating point number and advances the stream by 8 bytes
		/// </summary>
		public override void Write (double value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(double));
			}
		}

		/// <summary>
		/// Writes a signed short integer and advances the stream by 2 bytes
		/// </summary>
		public override void Write (short value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(short));
			}
		}

		/// <summary>
		/// Writes a signed integer and advances the stream by 4 bytes
		/// </summary>
		public override void Write (int value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(int));
			}
		}

		/// <summary>
		/// Writes a signed long integer and advances the stream by 8 bytes
		/// </summary>
		public override void Write (long value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(long));
			}
		}

		/// <summary>
		/// Writes a single-precision floating point number and advances the stream by 4 bytes
		/// </summary>
		public override void Write (float value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(float));
			}
		}

		/// <summary>
		/// Writes an unsigned short integer and advances the stream by 2 bytes
		/// </summary>
		public override void Write (ushort value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(ushort));
			}
		}

		/// <summary>
		/// Writes an unsigned integer and advances the stream by 4 bytes
		/// </summary>
		public override void Write (uint value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(uint));
			}
		}

		/// <summary>
		/// Writes an unsigned long integer and advances the stream by 8 bytes
		/// </summary>
		public override void Write (ulong value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				base.Write(_buffer, 0, sizeof(ulong));
			}
		}

		/// <summary>
		/// Writes a signed integer and advances the stream by 3 bytes
		/// </summary>
		public void WriteInt24 (int value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				var start = (Endian.Big == _endian) ? 1 : 0;
				base.Write(_buffer, start, 3);
			}
		}

		/// <summary>
		/// Writes an unsigned integer and advances the stream by 3 bytes
		/// </summary>
		public void WriteUInt24 (uint value)
		{
			lock(_buffer)
			{
				_buffer.Insert(value, 0, _endian);
				var start = (Endian.Big == _endian) ? 1 : 0;
				base.Write(_buffer, start, 3);
			}
		}
	}
}
