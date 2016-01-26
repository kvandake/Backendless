using System.Net;


namespace Backendless.Core
{
	public abstract class BackendlessServiceBase
	{

		protected string AppId{
			get{
				return BackendlessInternal.AppId;
			}
		}


		protected string SecretKey{
			get{
				return BackendlessInternal.SecretKey;
			}
		}

		protected string RootUrl{
			get{
				return BackendlessInternal.RootUrl;
			}
		}

		protected string RootPath{
			get{
				return BackendlessInternal.RootPath;
			}
		}

		protected string VersionNum {
			get{
				return BackendlessInternal.VersionNum;
			}
		}

		protected bool IsCustomBackendless {
			get{
				return BackendlessInternal.IsCustomBackendless;
			}
		}
		
		protected virtual IBackendlessRestEndPoint RestPoint {
			get {
				return BackendlessInternal.Locator.DefaultRestPoint;
			}
		}

		protected virtual IBackendlessConnectivity Connectivity{
			get{
				return BackendlessInternal.Locator.Platform.Connectivity;
			}
		}

		protected virtual IBackendlessCacheTableProvider DefaultCacheTableProvider {
			get {
				return BackendlessInternal.Locator.Platform.CreatorDefaultCacheTableProvider;
			}
		}



		protected static void CheckResponse(ResponseObject response){
			CheckResponse (response.StatusCode, response.Json);
		}

		protected static void CheckResponse(HttpStatusCode statusCode, string message){
			if (statusCode == HttpStatusCode.OK) 
				return;
			BackendlessError error = null;
			if (BackendlessError.TryParse (message, ref error)) {
				throw new BackendlessException (error.ErrorCode, error.Message);
			} else {
				throw new BackendlessException (0, message);
			}
		}
	}
}

