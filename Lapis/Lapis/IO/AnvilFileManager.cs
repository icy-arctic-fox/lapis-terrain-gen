using System;
using System.IO;
using Lapis.Level.Data;
using Lapis.Utility;
using Lapis.Spatial;

namespace Lapis.IO
{
	/// <summary>
	/// Manages retrieving and storing chunks in the Minecraft Anvil file structure
	/// </summary>
	/// <remarks>One instance of this class should be kept for each active dimension.</remarks>
	public class AnvilFileManager : IChunkProvider
	{
		/// <summary>
		/// File extension used for Anvil files
		/// </summary>
		private const string FileExtension = ".mca";

		private readonly Cache<XZCoordinate, AnvilFile> _cache = new Cache<XZCoordinate, AnvilFile>();
		private readonly string _basePath;

		/// <summary>
		/// Creates a new file manager that can write and read chunks to and from Anvil files
		/// </summary>
		/// <param name="path">Path to the directory that contains the Anvil files</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="path"/> is null</exception>
		/// <exception cref="FileNotFoundException">Thrown if the path given by <paramref name="path"/> does not exist or is not a directory</exception>
		public AnvilFileManager (string path)
		{
			if(null == path)
				throw new ArgumentNullException("path", "The file path to the chunk files can't be null.");
			if(!Directory.Exists(path))
				throw new FileNotFoundException("The path to the chunk files does not exist or is not a directory.", path);

			_basePath = path;

			// Append slash if needed
			var lastChar = _basePath[_basePath.Length - 1];
			if(lastChar != '/' && lastChar != '\\')
				_basePath += Path.DirectorySeparatorChar;
		}

		#region Get and store
		/// <summary>
		/// Chunk data contained in the Anvil file structure
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>Chunk data or null if the chunk doesn't exist</returns>
		public ChunkData this[int cx, int cz]
		{
			get { return GetChunk(cx, cz); }
			set { PutChunk(cx, cz, value); }
		}

		/// <summary>
		/// Retrieves the raw chunk data at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>Chunk data or null if the chunk doesn't exist</returns>
		public ChunkData GetChunk (int cx, int cz)
		{
			int rcx, rcz;
			relativeCoordinate(cx, cz, out rcx, out rcz);
			var coord = toRegionCoordinate(cx, cz);
			var file  = _cache.GetItem(coord, getFile);
			return file.GetChunk(rcx, rcz);
		}

		/// <summary>
		/// Stores the raw chunk data at the given coordinate
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <param name="data">Chunk data</param>
		public void PutChunk (int cx, int cz, ChunkData data)
		{
			if(null == data)
				throw new ArgumentNullException("data", "The chunk data can't be null.");

			int rcx, rcz;
			relativeCoordinate(cx, cz, out rcx, out rcz);
			var coord = toRegionCoordinate(cx, cz);
			var file  = _cache.GetItem(coord, getFile);
			file.PutChunk(rcx, rcz, data);
		}

		/// <summary>
		/// Checks if a chunk exists (has been generated)
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>True if the chunk exists or false if it doesn't</returns>
		public bool ChunkExists (int cx, int cz)
		{
			int rcx, rcz;
			relativeCoordinate(cx, cz, out rcx, out rcz);
			var coord = toRegionCoordinate(cx, cz);
			var file  = _cache.GetItem(coord, getFile);
			return file.ChunkExists(rcx, rcz);
		}
		#endregion

		/// <summary>
		/// Converts chunk coordinates to region coordinates
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <returns>A region coordinate that contains the chunk</returns>
		private static XZCoordinate toRegionCoordinate (int cx, int cz)
		{
			var rx = cx / AnvilFile.ChunkCount;
			var rz = cz / AnvilFile.ChunkCount;
			if(0 > cx)
				--rx;
			if(0 > cz)
				--rz;
			return new XZCoordinate(rx, rz);
		}

		/// <summary>
		/// Gets the relative coordinate of the chunk within a region
		/// </summary>
		/// <param name="cx">X-position of the chunk</param>
		/// <param name="cz">Z-position of the chunk</param>
		/// <param name="rcx">Relative x-position of the chunk</param>
		/// <param name="rcz">Relative z-position of the chunk</param>
		/// <returns>Relative x and z-position of the chunk</returns>
		private static void relativeCoordinate (int cx, int cz, out int rcx, out int rcz)
		{
			rcx = cx % AnvilFile.ChunkCount;
			rcz = cz % AnvilFile.ChunkCount;
			if(0 > rcx)
				rcx += AnvilFile.ChunkCount;
			if(0 > rcz)
				rcz += AnvilFile.ChunkCount;
		}

		/// <summary>
		/// Gets the filename that would be used for an Anvil file
		/// </summary>
		/// <param name="rx">X-position of the region</param>
		/// <param name="rz">Z-position of the region</param>
		/// <returns>A string containing the filename</returns>
		private static string generateFileName (int rx, int rz)
		{
			var sb = new System.Text.StringBuilder();
			sb.Append("r.");
			sb.Append(rx);
			sb.Append('.');
			sb.Append(rz);
			sb.Append(FileExtension);
			return sb.ToString();
		}

		/// <summary>
		/// Gets an Anvil file
		/// </summary>
		/// <param name="coord">Coordinate of the region contained in the Anvil file</param>
		/// <returns>An Anvil file</returns>
		/// <remarks>If the file doesn't exist, it will be created</remarks>
		private AnvilFile getFile (XZCoordinate coord)
		{
			var path = _basePath + generateFileName(coord.X, coord.Z);
			return File.Exists(path) ? AnvilFile.Load(path) : AnvilFile.Create(path);
		}
	}
}
