using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;
using Lapis.Level.Data;
using Lapis.Utility;

namespace Lapis.IO
{
	/// <summary>
	/// A single file that contains data about chunks
	/// </summary>
	sealed class AnvilFile : IDisposable
	{
		#region Constants
		/// <summary>
		/// Number of chunks along one axis in the region file
		/// </summary>
		public const int ChunkCount = 32;

		/// <summary>
		/// Total number of chunks contained within the file
		/// </summary>
		public const int TotalChunks = ChunkCount * ChunkCount;

		/// <summary>
		/// Number of bytes each sector is
		/// </summary>
		private const int SectorSize = 1024 * 4; // 4 KB per sector

		/// <summary>
		/// Number of bytes in the header
		/// </summary>
		private const int HeaderSize = TotalChunks * sizeof(int);

		/// <summary>
		/// Length of a chunk that hasn't been generated
		/// </summary>
		private const int NotGenerated = -1;

		/// <summary>
		/// Endian the the file uses
		/// </summary>
		private const Endian FileEndian = Endian.Big;

		/// <summary>
		/// An entire sector of nothing, used to quickly blank out sectors
		/// </summary>
		private static readonly byte[] _emptySector = new byte[SectorSize];
		#endregion

		private readonly int[] _headerData, _timestamps;
		private readonly SortedSet<int> _freeSectors;
		private readonly EndianBinaryReader _reader;
		private readonly EndianBinaryWriter _writer;

		#region Get and store
		/// <summary>
		/// Chunk data contained in the file
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <returns>Chunk data</returns>
		public ChunkData this[int rcx, int rcz]
		{
			get { return GetChunk(rcx, rcz); }
			set { PutChunk(rcx, rcz, value); }
		}

		/// <summary>
		/// Retrieves chunk data contained in the file
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <returns>Chunk data or null if the chunk doesn't exist</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rcx"/> or <paramref name="rcz"/> are outside the bounds of the region</exception>
		/// <exception cref="InvalidDataException">Thrown if the region file appears to be corrupt</exception>
		public ChunkData GetChunk (int rcx, int rcz)
		{
			checkBounds(rcx, rcz);
			var index = computeIndex(rcx, rcz);

			lock(_headerData)
			{
				// Does this chunk even exist?
				gotoChunk(_reader.BaseStream, index);
				var dataLength = _reader.ReadInt32();
				if(NotGenerated == dataLength || 0 >= dataLength)
					return null;
				var compression = (CompressionType)_reader.ReadByte();

				// Verify the length
				var sectorCount = getSectorCount(index);
				var length      = dataLength - sizeof(byte);
				var totalLength = dataLength + sizeof(int);
				if(totalLength / SectorSize > sectorCount)
					throw new InvalidDataException("The length reported for the compressed data (" + totalLength + ") does not correlate with the sector count (" + sectorCount + " -> " + (sectorCount * SectorSize) + " bytes) for chunk " + rcx + ", " + rcz + " (" + index + ")");

				// Read the compressed data
				var compressedData = _reader.ReadBytes(length);
				using(var memoryStream = new MemoryStream(compressedData))
				{
					// Set up a stream for the compression type we're using
					Stream sourceStream;
					switch(compression)
					{
					case CompressionType.GZip:
						sourceStream = new GZipStream(memoryStream, CompressionMode.Decompress);
						break;
					case CompressionType.ZLib:
						sourceStream = new ZlibStream(memoryStream, CompressionMode.Decompress);
						break;
					default:
						throw new InvalidDataException("Bad compression type " + compression);
					}

					// Extract the chunk data from the stream
					using(var br = new EndianBinaryReader(sourceStream, FileEndian))
					{
						ChunkData data;
						try
						{
							data = ChunkData.ReadFromStream(br);
						}
						catch(Exception e)
						{// There was a problem reading the chunk, assume it's bad and delete it
							DeleteChunk(rcx, rcz);
							throw new InvalidDataException("Failed to read chunk " + rcx + ", " + rcz + " (" + index + ") from region file", e);
						}
						return data;
					}
				}
			}
		}

		/// <summary>
		/// Stores chunk data to the file
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <param name="data">Chunk data</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rcx"/> or <paramref name="rcz"/> are outside the bounds of the region</exception>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="data"/> is null</exception>
		public void PutChunk (int rcx, int rcz, ChunkData data)
		{
			checkBounds(rcx, rcz);
			var index = computeIndex(rcx, rcz);

			// Using zlib compression for now
			var compression = CompressionType.ZLib;

			// Create the streams and write the compressed chunk data
			using(var memoryStream = new MemoryStream(SectorSize))
			{
				// Create the compression stream
				Stream outputStream;
				switch(compression)
				{
				case CompressionType.GZip:
					outputStream = new GZipStream(memoryStream, CompressionMode.Compress, true);
					break;
				case CompressionType.ZLib:
					outputStream = new ZlibStream(memoryStream, CompressionMode.Compress, true);
					break;
				default:
					throw new InvalidDataException("Bad compression type " + compression);
				}

				// Write the data to the stream
				using(var bw = new EndianBinaryWriter(outputStream, FileEndian))
				{
					data.WriteToStream(bw);

					// Force stream to flush (this is an issue with compression streams) and reset to the beginning for copying
					outputStream.Flush();
					outputStream.Close(); // This line actually writes the data in most cases
					outputStream.Dispose();
					memoryStream.Seek(0, SeekOrigin.Begin);
				}

				// Get the length and sector count
				var length      = (int)memoryStream.Length;
				var dataLength  = length + sizeof(byte);
				var totalLength = dataLength + sizeof(int);
				var count       = (int)Math.Ceiling((double)totalLength / SectorSize);
				if(byte.MaxValue < count) // How you'd ever get a chunk this big, I don't know
					throw new InternalBufferOverflowException("The size of a chunk (" + totalLength + " bytes) cannot be more than " + byte.MaxValue + " sectors (" + (byte.MaxValue * SectorSize) + " bytes)");
				var sectorCount = (byte)count;

				lock(_headerData)
				{
					// Remove the old chunk
					DeleteChunk(rcx, rcz);

					// Find where to put the chunk
					var sector = findFreeSpace(sectorCount);
					if(0 > sector) // Append to the end
						_writer.BaseStream.Seek(0, SeekOrigin.End);
					else
					{// Go to a free sector
						gotoSector(_writer.BaseStream, sector);
						var end = sector + sectorCount;
						for(var i = sector; i < end; ++i)
							_freeSectors.Remove(i);
					}

					// Write the chunk
					_writer.Write(dataLength);
					_writer.Write((byte)compression);
					memoryStream.CopyTo(_writer.BaseStream, SectorSize);

					// Write any extra zeroes if it doesn't line up with the sector size
					var remaining = SectorSize - (totalLength % SectorSize);
					if(0 < remaining)
						_writer.Write(_emptySector, 0, remaining);

					updateHeader(index, sector, sectorCount);
					_writer.Flush(); // Done!
				}
			}
		}

		/// <summary>
		/// Checks if a chunk exists (has been generated)
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <returns>True if the chunk exists or false if it doesn't</returns>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rcx"/> or <paramref name="rcz"/> are outside the bounds of the region</exception>
		public bool ChunkExists (int rcx, int rcz)
		{
			checkBounds(rcx, rcz);
			var index = computeIndex(rcx, rcz);

			lock(_headerData)
				return chunkExists(index);
		}

		/// <summary>
		/// Deletes a chunk from the file
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rcx"/> or <paramref name="rcz"/> are outside the bounds of the region</exception>
		/// <remarks>This makes the chunk appear as if it was never generated.</remarks>
		public void DeleteChunk (int rcx, int rcz)
		{
			checkBounds(rcx, rcz);
			var index = computeIndex(rcx, rcz);

			lock(_headerData)
			{
				var sector      = getSector(index);
				var sectorCount = getSectorCount(index);
				updateHeader(index, 0, 0);
				var end = sector + sectorCount;
				for(var i = sector; i < end; ++i)
					_freeSectors.Add(i);
				// We don't need to blank the sector, just remove the reference to it
			}
		}

		/// <summary>
		/// Retrieves the date and time that a chunk was last modified
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rcx"/> or <paramref name="rcz"/> are outside the bounds of the region</exception>
		/// <returns>A date and time</returns>
		/// <remarks>If the chunk doesn't exist, the date and time "modified" will be the epoch.</remarks>
		public DateTime GetLastModified (int rcx, int rcz)
		{
			checkBounds(rcx, rcz);
			var index = computeIndex(rcx, rcz);

			int timestamp;
			lock(_headerData)
				timestamp = _timestamps[index];

			return timestamp.ToDateTime();
		}
		#endregion

		#region Initialization and shutdown
		/// <summary>
		/// Creates a new Anvil file
		/// </summary>
		/// <param name="s">Stream to read and write from</param>
		private AnvilFile (Stream s)
		{
			_headerData  = new int[TotalChunks];
			_timestamps  = new int[TotalChunks];
			_freeSectors = new SortedSet<int>();
			_reader = new EndianBinaryReader(s, FileEndian);
			_writer = new EndianBinaryWriter(s, FileEndian);
		}

		/// <summary>
		/// Loads an Anvil file from disk
		/// </summary>
		/// <param name="path">Path to the Anvil file</param>
		/// <returns>The loaded file</returns>
		/// <exception cref="IOException">Thrown if there was an error loading the region file</exception>
		public static AnvilFile FromFile (string path)
		{
			AnvilFile af;

			try
			{// Attempt to open the file
				var fs = new FileStream(path, FileMode.Open);
				af = new AnvilFile(fs);
				af.readHeader();
			}
			catch(Exception e)
			{// Something went wrong, re-throw it
				throw new IOException("Failed to open region file " + path, e);
			}

			return af;
		}

		/// <summary>
		/// Creates a new Anvil file on disk
		/// </summary>
		/// <param name="path">Path to the Anvil file</param>
		/// <returns>The created file</returns>
		/// <remarks>If the file already exists, its contents will be lost.</remarks>
		public static AnvilFile Create (string path)
		{
			AnvilFile af;

			try
			{// Attempt to open the file
				var fs = new FileStream(path, FileMode.Create);
				af = new AnvilFile(fs);
				af.blankHeader();
			}
			catch(Exception e)
			{// Something went wrong, re-throw it
				throw new IOException("Failed to open region file " + path, e);
			}

			return af;
		}

		/// <summary>
		/// Closes the region file and disposes of it.
		/// </summary>
		/// <remarks>Only dispose of the region file if you forcibly want to unload it's data (like unloading an entire world).
		/// Otherwise, let the garbage collector decide when to remove it, which is better for performance.</remarks>
		public void Dispose ()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		private void Dispose (bool disposing)
		{
			if(disposing)
			{
				lock(_headerData)
				{
//					Compact();
					_writer.Flush();
					_writer.Close();
					_writer.Dispose();
					_reader.Dispose();
				}
			}
		}

		/// <summary>
		/// Finalizer - simply calls Dispose()
		/// </summary>
		~AnvilFile ()
		{
			Dispose(false);
		}
		#endregion

		#region Utility
		/// <summary>
		/// Checks the x and z-value in the coordinate to make sure it is within the bounds of the region
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="rcx"/> or <paramref name="rcz"/> are outside the bounds of the region</exception>
		private static void checkBounds (int rcx, int rcz)
		{
			if(ChunkCount <= rcx)
				throw new ArgumentOutOfRangeException("rcx", "The x-position of the chunk must be less than " + ChunkCount);
			if(ChunkCount <= rcz)
				throw new ArgumentOutOfRangeException("rcz", "The z-position of the chunk must be less than " + ChunkCount);
		}

		/// <summary>
		/// Calculates the index of a chunk given a relative chunk coordinate
		/// </summary>
		/// <param name="rcx">X-position of the chunk relative to the region</param>
		/// <param name="rcz">Z-position of the chunk relative to the region</param>
		/// <returns>Chunk index</returns>
		private static int computeIndex (int rcx, int rcz)
		{
			var index = rcx + (rcz * ChunkCount);
			return index;
		}

		/// <summary>
		/// Gets the sector that a chunk is at
		/// </summary>
		/// <param name="index">Index of the chunk</param>
		/// <returns>Sector number that the chunk starts at</returns>
		private int getSector (int index)
		{
			var value = _headerData[index];
			value = (int)((value & 0xffffff00) >> 8);
			return value;
		}

		/// <summary>
		/// Gets the number of sectors that a chunk uses
		/// </summary>
		/// <param name="index">Index of the chunk</param>
		/// <returns>Sector count</returns>
		private byte getSectorCount (int index)
		{
			var value = _headerData[index];
			var count = (byte)(value & 0xff);
			return count;
		}
		#endregion

		#region IO
		/// <summary>
		/// Compacts the region file, which removes any unused sectors and reduces the file size
		/// </summary>
		/// <remarks>This can be an expensive call, use sparingly.</remarks>
		public void Compact ()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the header data from the file and stores it into headerData
		/// </summary>
		private void readHeader ()
		{
			var maximum = 1; // This is 1 because the actual chunk data starts right after sector 1
			_reader.BaseStream.Seek(0, SeekOrigin.Begin);

			// Read location and sector counts
			for(var index = 0; index < TotalChunks; ++index)
			{
				_headerData[index] = _reader.ReadInt32();
				var sector      = getSector(index);
				var sectorCount = getSectorCount(index);
				if(sector > maximum)
				{
					for(var j = maximum + 1; j <= sector; ++j)
						_freeSectors.Add(j);
					maximum = sector;
				}
				var end = sector + sectorCount;
				for(var j = sector; j < end; ++j)
					_freeSectors.Remove(j);
			}

			// Read timestamps
			for(var index = 0; index < TotalChunks; ++index)
				_timestamps[index] = _reader.ReadInt32();
		}

		/// <summary>
		/// Writes a blank header to the file and clears headerData
		/// </summary>
		private void blankHeader ()
		{
			for(var i = 0; i < TotalChunks; ++i)
				_headerData[i] = 0;
			_writer.BaseStream.Seek(0, SeekOrigin.Begin);

			// This only works because the header is exactly the size of 2 sectors
			_writer.Write(_emptySector); // Location and sector counts
			_writer.Write(_emptySector); // Timestamps
		}

		/// <summary>
		/// Updates the header in the file and headerData with new information
		/// </summary>
		/// <param name="index">Index of the chunk to update</param>
		/// <param name="sector">Sector that the chunk is located at</param>
		/// <param name="sectorCount">Number of sectors the chunk uses</param>
		private void updateHeader (int index, int sector, byte sectorCount)
		{
			var value = ((sector & 0x00ffffff) << 8) | sectorCount;
			_headerData[index] = value;

			// Write location and sector count
			_writer.BaseStream.Seek(index * sizeof(int), SeekOrigin.Begin);
			_writer.Write(value);

			// Write timestamp
			_writer.BaseStream.Seek(SectorSize - sizeof(int), SeekOrigin.Current);
			_writer.Write(Timestamp.Now);
		}

		/// <summary>
		/// Checks if the chunk exists
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private bool chunkExists (int index)
		{
			if(0 != getSectorCount(index))
			{
				gotoChunk(_reader.BaseStream, index);
				var length = _reader.ReadInt32();
				if(NotGenerated != length && 0 < length)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Goes to the beginning of a sector
		/// </summary>
		/// <param name="s">Stream to seek with</param>
		/// <param name="sector">Sector number</param>
		private void gotoSector (Stream s, int sector)
		{
			long position = sector * SectorSize;
			s.Seek(position, SeekOrigin.Begin);
		}

		/// <summary>
		/// Goes to the beginning of the sector that a chunk is at
		/// </summary>
		/// <param name="s">Stream to seek with</param>
		/// <param name="index">Index of the chunk</param>
		private void gotoChunk (Stream s, int index)
		{
			var sector = getSector(index);
			gotoSector(s, sector);
		}

		/// <summary>
		/// Finds a section of free space in the region file
		/// </summary>
		/// <param name="sectorCount">Number of consecutive sectors</param>
		/// <returns>First sector number or -1 if none was found (in that case, append to the file)</returns>
		private int findFreeSpace (byte sectorCount)
		{
			var enumerator  = _freeSectors.GetEnumerator();
			var foundSector = -1;
			while(enumerator.MoveNext())
			{
				var sector = enumerator.Current;
				var end    = sector + sectorCount;
				var found  = true;
				for(var curSector = sector + 1; curSector < end; ++curSector)
				{
					if(_freeSectors.Contains(curSector))
						enumerator.MoveNext();
					else
					{
						found = false;
						break;
					}
				}
				if(found)
				{
					foundSector = sector;
					break;
				}
			}
			return foundSector;
		}
		#endregion

		/// <summary>
		/// Types of compression that the chunk could use
		/// </summary>
		private enum CompressionType : byte
		{
			GZip = 1,

			ZLib = 2
		}
	}
}
