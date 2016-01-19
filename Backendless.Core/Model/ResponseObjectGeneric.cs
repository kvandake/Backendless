using System.Net;

namespace Backendless.Core
{
	public class ResponseObjectGeneric<T>
	{
		public T Data { get; private set;}

		public HttpStatusCode StatusCode { get; private set;}

		public ResponseObjectGeneric(HttpStatusCode statusCode, T data){
			Data = data;
			StatusCode = statusCode;
		}
	}
}

