using System;

namespace Backendless.Core
{
	public static class BackendlessHadlerException
	{

		/// <summary>
		/// Occurs when invoke exception. (Global Handler)
		/// </summary>
		public static event EventHandler<BackendlessError> InvokeError;



		internal static void SendException(BackendlessException exception){
			OnInvokeError((BackendlessError)exception);
		}




		internal static void SendException(Exception ex, Action<BackendlessError> errorCalback){
			var backendlessException = ex as BackendlessException;
			int errorCode = 0;
			string errorMessage = ex.Message;
			if (backendlessException != null) {
				errorCode = backendlessException.ErrorCode;
				SendException (backendlessException);
			} else {
				SendException (new BackendlessException (0, ex.Message, ex));
			}
			if (errorCalback != null) {
				errorCalback (new BackendlessError (errorCode, errorMessage));
			}
		}



		static void OnInvokeError (BackendlessError e)
		{
			var handler = InvokeError;
			if (handler != null)
				handler (null, e);
		}
	}
}

