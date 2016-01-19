using System;
using System.Net;

namespace Backendless.Core
{
	public class ResponseObject
	{

		public HttpStatusCode StatusCode { get; private set;}


		public string Json { get; private set;}

		public ResponseObject (HttpStatusCode statusCode, string json)
		{
			StatusCode = statusCode;
			Json = json;
		}
	}
}

