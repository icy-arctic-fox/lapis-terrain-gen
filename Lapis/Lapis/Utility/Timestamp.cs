using System;

namespace Lapis.Utility
{
	/// <summary>
	/// Timestamp conversion functions
	/// </summary>
	/// <remarks>Timestamps are your typical Unix 32-bit timestamp.</remarks>
	public static class Timestamp
	{
		/// <summary>
		/// Date and time at the epoch
		/// </summary>
		private static readonly DateTime _epoch = new DateTime(1970, 1, 1);

		/// <summary>
		/// Converts a date/time object to a timestamp
		/// </summary>
		/// <param name="dt">Date and time to convert</param>
		/// <returns>A timestamp</returns>
		public static int ToTimestamp (this DateTime dt)
		{
			var seconds = (dt - _epoch).TotalSeconds;
			return (int)seconds;
		}

		/// <summary>
		/// Converts a timestamp to a date/time object
		/// </summary>
		/// <param name="timestamp">Timestamp to convert</param>
		/// <returns>A date and time</returns>
		public static DateTime ToDateTime (this int timestamp)
		{
			var dt = _epoch.AddSeconds(timestamp);
			return dt;
		}

		/// <summary>
		/// A timestamp for the current time
		/// </summary>
		/// <returns>A timestamp</returns>
		public static int Now
		{
			get { return ToTimestamp(DateTime.Now); }
		}
	}
}
