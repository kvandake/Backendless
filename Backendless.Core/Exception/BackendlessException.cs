using System;

namespace Backendless.Core
{
	public class BackendlessException : Exception
	{

		public int ErrorCode { get; private set;}
	

		public BackendlessException (int errorCode, string message):base(message)
		{
			ErrorCode = errorCode;
		}

		public BackendlessException (int errorCode, string message,Exception ex):base(message,ex)
		{
			ErrorCode = errorCode;
		}

	}
}

