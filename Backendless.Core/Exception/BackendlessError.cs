using System;
using Newtonsoft.Json;

namespace Backendless.Core
{


	public delegate void ErrorBackendlessCallback (BackendlessError error);



	public class BackendlessError
	{

		[JsonProperty("code")]
		public int ErrorCode { get; private set;}

		[JsonProperty("message")]
		public string Message { get; private set;}

		public BackendlessError (int errorCode, string message)
		{
			ErrorCode = errorCode;
			Message = message;
		}


		public override string ToString ()
		{
			return string.Format ("[BackendlessError: ErrorCode={0}, Message={1}]", ErrorCode, Message);
		}

		public static explicit operator BackendlessError(BackendlessException exception){
			return new BackendlessError(exception.ErrorCode,exception.Message);
		}


		public static bool TryParse(string json,ref BackendlessError error){
			try {
				error = JsonConvert.DeserializeObject<BackendlessError> (json);
				return true;
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex);
				return false;
			}
		}

	}
}

