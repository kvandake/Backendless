using System;

namespace Backendless.Core
{
	public class BackendlessCallback<T>
	{
		internal readonly ErrorBackendlessCallback ErrorHandler;
		internal readonly Action<T> ResponseHandler;

		public BackendlessCallback( Action<T> responseHandler, ErrorBackendlessCallback errorHandler )
		{
			ResponseHandler = responseHandler;
			ErrorHandler = errorHandler;
		}

		public static void InvokeComplete(BackendlessCallback<T> callback,T obj){
			if (callback == null || callback.ResponseHandler == null)
				return;
			callback.ResponseHandler (obj);
		}

		public static void InvokeError(BackendlessCallback<T> callback,BackendlessError error){
			if (callback == null || callback.ErrorHandler == null)
				return;
			callback.ErrorHandler (error);
		}

		public static void InvokeError(BackendlessCallback<T> callback,int errorCode, string errorMessage){
			if (callback == null || callback.ErrorHandler == null)
				return;
			callback.ErrorHandler (new BackendlessError (errorCode,errorMessage));
		}
	}
}

