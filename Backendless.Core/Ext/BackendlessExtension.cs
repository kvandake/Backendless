using System;

namespace Backendless.Core
{
	public static class BackendlessExtension
	{

		static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		static readonly double MaxUnixSeconds = (DateTime.MaxValue - UnixEpoch).TotalSeconds;

		/// <summary>
		/// Creator the object with default constructor.
		/// </summary>
		/// <returns>The object with default constructor.</returns>
		/// <param name="type">Type.</param>
		public static object CreatorObjectWithDefaultConstructor(Type type){
			return Activator.CreateInstance (type);
		}
			
		/// <summary>
		///   Convert a long into a DateTime
		/// </summary>
		public static DateTime FromUnixTime(this Int64 self)
		{
			return self > MaxUnixSeconds
				? UnixEpoch.AddMilliseconds (self).ToLocalTime ()
					: UnixEpoch.AddSeconds (self).ToLocalTime ();
		}

		/// <summary>
		///   Convert a DateTime into a long
		/// </summary>
		public static Int64 ToUnixTime(this DateTime self)
		{
			if (self == DateTime.MinValue) {
				return 0;
			}
			var delta = self.ToUniversalTime () - UnixEpoch;
			if (delta.TotalMilliseconds < 0)
				throw new ArgumentOutOfRangeException ("Unix epoc starts January 1st, 1970");
			return (long)delta.TotalMilliseconds;
		}

	}
}

