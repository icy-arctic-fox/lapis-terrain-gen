using System;
using Lapis.Blocks;

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

		private readonly Realm realm;
//		private readonly ChunkData data;

		#region Properties
		/// <summary>
		/// World that the chunk is in
		/// </summary>
		public World World
		{
			get { return realm.World; }
		}

		/// <summary>
		/// Realm that the chunk is in
		/// </summary>
		public Realm Realm
		{
			get { return realm; }
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
/*		public int ChunkX
		{
			get { return data.ChunkX; }
		}*/

		/// <summary>
		/// Z-position of the chunk within the realm
		/// </summary>
/*		public int ChunkZ
		{
			get { return data.ChunkZ; }
		}*/
		#endregion

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
/*			if(!disposing)
				realm.FreeChunk(ChunkX, ChunkZ);*/
		}
		#endregion
	}
}
