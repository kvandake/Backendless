using System;

namespace Backendless.Core
{
	public static class BackendlessExtension
	{

		/// <summary>
		/// Creator the object with default constructor.
		/// </summary>
		/// <returns>The object with default constructor.</returns>
		/// <param name="type">Type.</param>
		public static object CreatorObjectWithDefaultConstructor(Type type){
			return Activator.CreateInstance (type);
		}


		public static DateTime GetDateTimeFromGmtTicks(long jsSeconds)
		{
			DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return start.AddMilliseconds(jsSeconds).ToLocalTime();
		}




	
		/// <summary>
		///   Convert a long into a DateTime
		/// </summary>
		public static DateTime FromUnixTime(this Int64 self)
		{
			DateTime start = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return start.AddMilliseconds (self).ToLocalTime ();
		}

		/// <summary>
		///   Convert a DateTime into a long
		/// </summary>
		public static Int64 ToUnixTime(this DateTime self)
		{
			if (self == DateTime.MinValue) {
				return 0;
			}
			var epoc = new DateTime (1970, 1, 1);
			var delta = self - epoc;
			if (delta.TotalSeconds < 0)
				throw new ArgumentOutOfRangeException ("Unix epoc starts January 1st, 1970");

			return (long)delta.TotalSeconds;
		}

	}
}

