using System;
using Lapis.Blocks;
using Lapis.Level.Data;

namespace Lapis.Level
{
	/// <summary>
	/// A manageable collection of blocks within a realm
	/// </summary>
	public sealed class Chunk : IDisposable
	{
		#region Constants
		/// <summary>
		/// Number of blocks in size of a chunk section
		/// </summary>
		public const int SectionHeight = 16;

		/// <summary>
		/// Number of sections that are stacked to make one chunk column
		/// </summary>
		public const int SectionCount = 16;

		/// <summary>
		/// Number of blocks high a chunk is
		/// </summary>
		public const int Height = SectionHeight * SectionCount;

		/// <summary>
		/// Length and width of a chunk (in blocks)
		/// </summary>
		public const int Size = 16;
		#endregion

		private readonly Realm _realm;
		private readonly ChunkData _data;

		/// <summary>
		/// Creates a chunk from chunk data
		/// </summary>
		/// <param name="realm">Realm that the chunk belongs to</param>
		/// <param name="data">Chunk data</param>
		internal Chunk (Realm realm, ChunkData data)
		{
			_realm = realm;
			_data  = data;
		}

		#region Properties
		/// <summary>
		/// World that the chunk is in
		/// </summary>
		public World World
		{
			get { return _realm.World; }
		}

		/// <summary>
		/// Realm that the chunk is in
		/// </summary>
		public Realm Realm
		{
			get { return _realm; }
		}

		/// <summary>
		/// Block information from the chunk
		/// </summary>
		/// <param name="bx">X-position of the block</param>
		/// <param name="by">Y-position of the block</param>
		/// <param name="bz">Z-position of the block</param>
		/// <returns>Block information</returns>
		public Block this[byte bx, byte by, byte bz]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// X-position of the chunk within the realm
		/// </summary>
		public int ChunkX
		{
			get { return _data.ChunkX; }
		}

		/// <summary>
		/// Z-position of the chunk within the realm
		/// </summary>
		public int ChunkZ
		{
			get { return _data.ChunkZ; }
		}
		#endregion

		#region Neighbors
		/// <summary>
		/// Gets the chunk neighboring to the north
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, z - 1)</remarks>
		public Chunk North
		{
			get { return _realm.GetChunk(ChunkX, ChunkZ - 1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south
		/// </summary>
		/// <remarks>Relative coordinates: (x + 0, z + 1)</remarks>
		public Chunk South
		{
			get { return _realm.GetChunk(ChunkX, ChunkZ + 1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z + 0)</remarks>
		public Chunk East
		{
			get { return _realm.GetChunk(ChunkX + 1, ChunkZ); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z + 0)</remarks>
		public Chunk West
		{
			get { return _realm.GetChunk(ChunkX - 1, ChunkZ); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the north-east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z - 1)</remarks>
		public Chunk NorthEast
		{
			get { return _realm.GetChunk(ChunkX + 1, ChunkZ - 1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the north-west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z - 1)</remarks>
		public Chunk NorthWest
		{
			get { return _realm.GetChunk(ChunkX - 1, ChunkZ - 1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south-east
		/// </summary>
		/// <remarks>Relative coordinates: (x + 1, z + 1)</remarks>
		public Chunk SouthEast
		{
			get { return _realm.GetChunk(ChunkX + 1, ChunkZ + 1); }
		}

		/// <summary>
		/// Gets the chunk neighboring to the south-west
		/// </summary>
		/// <remarks>Relative coordinates: (x - 1, z + 1)</remarks>
		public Chunk SouthWest
		{
			get { return _realm.GetChunk(ChunkX - 1, ChunkZ + 1); }
		}
		#endregion

		/// <summary>
		/// Forces the chunk to save its contents to disk
		/// </summary>
		public void Save ()
		{
			_realm.SaveChunk(ChunkX, ChunkZ, _data);
		}

		#region Cleanup
		/// <summary>
		/// Disposes the chunk and removes its contents from memory
		/// </summary>
		public void Dispose ()
		{
			Dispose(true);
		}

		/// <summary>
		/// Finalizer - simply calls Dispose()
		/// </summary>
		~Chunk ()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes the chunk and removes its contents from memory
		/// </summary>
		/// <param name="disposing">True if we should clean up all of our own resources (code called Dispose) or false if we should only cleanup ourselves (GC called Dispose)</param>
		private void Dispose (bool disposing)
		{
			if(!disposing)
				_realm.FreeChunk(ChunkX, ChunkZ);
		}
		#endregion
	}
}
