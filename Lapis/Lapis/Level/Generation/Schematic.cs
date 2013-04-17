﻿using System;
using System.IO;
using Ionic.Zlib;
using Lapis.IO;
using Lapis.IO.NBT;

namespace Lapis.Level.Generation
{
	/// <summary>
	/// A schematic for an object made of blocks that can be placed in a world
	/// </summary>
	public class Schematic : ISerializable
	{
		public Schematic (int width, int height, int length)
		{

		}

		#region Save and load
		private const string DefaultNodeName = "Schematic";

		public static Schematic FromFile (string filename)
		{
			if(null == filename)
				throw new ArgumentNullException("filename", "The name of the file to load from can't be null.");

			using(var fs = new FileStream(filename, FileMode.Open))
				return FromStream(fs);
		}

		public static Schematic FromStream (Stream s)
		{
			if(null == s)
				throw new ArgumentNullException("s", "The source stream can't be null.");

			using(var gzStream = new GZipStream(s, CompressionMode.Decompress))
			using(var br = new EndianBinaryReader(gzStream, Endian.Big))
				return new Schematic(Node.ReadFromStream(br));
		}

		private Schematic (Node node)
		{
			throw new NotImplementedException();
		}

		public void Save (string filename)
		{
			if(null == filename)
				throw new ArgumentNullException("filename", "The name of the file to save to can't be null.");

			using(var fs = new FileStream(filename, FileMode.Create))
				WriteToStream(fs);
		}

		public void WriteToStream (Stream s)
		{
			using(var gzStream = new GZipStream(s, CompressionMode.Compress))
			using(var bw = new EndianBinaryWriter(gzStream, Endian.Big))
				WriteToStream(bw);
		}

		public void WriteToStream (BinaryWriter bw)
		{
			var node = ConstructNbtNode(DefaultNodeName);
			node.WriteToStream(bw);
		}

		public Node ConstructNbtNode (string name)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
