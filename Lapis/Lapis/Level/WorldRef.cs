using System;

namespace Lapis.Level
{
	/// <summary>
	/// References a world
	/// </summary>
	/// <remarks>World references are useful for when you want to reference a world, but don't care about what's there.
	/// A world reference doesn't contain any block information.
	/// However, a world reference won't cause the chunk data to become loaded in memory.</remarks>
	public class WorldRef
	{
		private readonly string _name;

		/// <summary>
		/// Creates a new world reference
		/// </summary>
		/// <param name="name">Name of the world</param>
		internal WorldRef (string name)
		{
			if(null == name)
				throw new ArgumentNullException("name", "The name of the world can't be null.");
			_name = name;
		}

		#region Properties
		public string WorldName
		{
			get { return _name; }
		}
		#endregion
	}
}
