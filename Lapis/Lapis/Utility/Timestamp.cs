using System;

namespace Lapis.Utility
{
	/// <summary>
	/// Timestamp conversion functions
	/// </summary>
	/// <remarks>Timestamps are your typical Unix 32-bit timestamp (seconds since the epoch).
	/// The "long" variations of the timestamps are just the number of milliseconds since the epoch.</remarks>
	public static class Timestamp
	{
		private const int MillisecondsToTicks = 10000;
		private const int SecondsToTicks      = MillisecondsToTicks * 1000;

		private static readonly DateTime _epoch = new DateTime(1970, 1, 1);

		/// <summary>
		/// Date and time at the epoch
		/// </summary>
		public static DateTime Epoch
		{
			get { return _epoch; }
		}

		/// <summary>
		/// Converts a date/time object to a timestamp
		/// </summary>
		/// <param name="dt">Date and time to convert</param>
		/// <returns>A timestamp</returns>
		/// <remarks>The value of this timestamp is the number of seconds since the epoch.</remarks>
		public static int ToTimestamp (this DateTime dt)
		{
			var ticks = (dt - _epoch).Ticks;
			return (int)(ticks / SecondsToTicks);
		}

		/// <summary>
		/// Converts a date/time object to a long timestamp
		/// </summary>
		/// <param name="dt">Date and time to convert</param>
		/// <returns>A long timestamp</returns>
		/// <remarks>The value of this timestamp is the number of milliseconds since the epoch.</remarks>
		public static long ToLongTimestamp (this DateTime dt)
		{
			var ticks = (dt - _epoch).Ticks;
			return ticks / MillisecondsToTicks;
		}

		/// <summary>
		/// Converts a timestamp to a date/time object
		/// </summary>
		/// <param name="timestamp">Timestamp to convert</param>
		/// <returns>A date and time</returns>
		/// <remarks>The value of <paramref name="timestamp"/> should be the number of seconds since the epoch.</remarks>
		public static DateTime ToDateTime (this int timestamp)
		{
			var dt = Epoch.AddTicks(timestamp * SecondsToTicks);
			return dt;
		}

		/// <summary>
		/// Converts a long timestamp to a date/time object
		/// </summary>
		/// <param name="timestamp">Timestamp to convert</param>
		/// <returns>A date and time</returns>
		/// <remarks>The value of <paramref name="timestamp"/> should be the number of milliseconds since the epoch.</remarks>
		public static DateTime ToDateTime (this long timestamp)
		{
			var dt = Epoch.AddTicks(timestamp * MillisecondsToTicks);
			return dt;
		}

		/// <summary>
		/// A timestamp for the current time
		/// </summary>
		public static int Now
		{
			get { return ToTimestamp(DateTime.UtcNow); }
		}

		/// <summary>
		/// A long timestamp for the current time
		/// </summary>
		public static long LongNow
		{
			get { return ToLongTimestamp(DateTime.UtcNow); }
		}
	}
}
