using System;

namespace Backendless.Core
{
	public class BackendlessErrorHttpEventArgs : EventArgs
	{

		public int ErrorCode { get; private set;}

		public string ErrorDescription { get; private set;}

		public string Message { get; private set;}

		public BackendlessErrorHttpEventArgs ()
		{
		}

		public BackendlessErrorHttpEventArgs (int errorCode,string errorDescription,string message = null)
		{
			ErrorCode = errorCode;
			ErrorDescription = errorDescription;
			Message = message;
		}
	}
}

