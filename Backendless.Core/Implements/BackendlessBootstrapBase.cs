namespace Backendless.Core
{

//	public abstract class BackendlessBootstrapBase
//	{
//		const string ApplicationIdKey = "application-id";
//		const string SecretKey = "secret-key";
//		const string ContentTypeKey = "Content-Type";
//		const string ApplicationTypeKey = "application-type";
//
//		const string DefaultContentTypeValue = "application/json";
//		const string DefaultApplicationTypeValue = "REST";
//
//		readonly string _applicationId;
//		readonly string _secretKey;
//	
//
//		internal IHttpClient CreateHttpClient(bool includeBaseHeaders = true){
//			var httpClient = HttpClient;
//			if (includeBaseHeaders) {
//				httpClient.Headers.Add (ApplicationIdKey, _applicationId);
//				httpClient.Headers.Add (SecretKey, _secretKey);
//				httpClient.Headers.Add (ContentTypeKey, DefaultContentTypeValue);
//				httpClient.Headers.Add (ApplicationTypeKey, DefaultApplicationTypeValue);
//			}
//			return httpClient;
//		}
//
//		static BackendlessBootstrapBase _locator;
//
//		internal static BackendlessBootstrapBase Locator {
//			get{
//				return _locator;
//			}
//		}
//
//		protected BackendlessBootstrapBase (string applicationId, string secretKey)
//		{
//			_applicationId = applicationId;
//			_secretKey = secretKey;
//			_locator = this;
//		}
//			
//	}
}

