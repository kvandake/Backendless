using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Backendless.Core
{
	public class SimpleUserService : IUserService
	{

		const string RegisterMethodPath = "/users/register";
		const string LoginMethodPath = "/users/login";


		static IBackendlessRestEndPoint RestPoint{
			get{
				return BackendlessBootstrap.Locator.CreatorEndPoint<IBackendlessRestEndPoint> ();
			}
		}

		static IBackendlessJsonProvider JsonProvider {
			get{
				return BackendlessBootstrap.Locator.FromContextInternal<IBackendlessJsonProvider> ();
			}
		}

		#region IUserService implementation

		public async Task<bool> SignUpAsync (BackendlessUser user,BackendlessCallback<BackendlessUser> callback = null)
		{
//			try {
//				using (var restPoint = RestPoint) {
//					restPoint.BaseAddress = BackendlessBootstrap.RootUrl;
//					restPoint.Header = BackendlessBootstrap.DefaultHeader;
//					restPoint.Method = RegisterMethodPath;
//					var json = 
//					var response = await restPoint.PostAsync(user);
//					if (response.StatusCode != HttpStatusCode.OK) {
//						throw new BackendlessException (response.ErrorCode, response.ErrorMessage);
//					}
//					user = BackendlessObject.ParseParameters <BackendlessUser> (response.Data, BackendlessUser.ParseProvider);
//					BackendlessCallback<BackendlessUser>.InvokeComplete (callback, user);
//					return true;
//				}
//			} catch (Exception ex) {
//				BackendlessHadlerException.SendException (ex, callback.ErrorHandler);
//				return false;
//			}
			return false;
		}


		public async Task<T> LoginAsync<T>(string @username, string @password,BackendlessCallback<T> callback = null) where T: BackendlessUser
		{
			try {
				using (var restPoint = RestPoint) {
					restPoint.BaseAddress = BackendlessBootstrap.RootUrl;
					restPoint.Header = BackendlessBootstrap.DefaultHeader;
					restPoint.Method = LoginMethodPath;
					var dyn = new { login = @username, password = @password};
					var json = JsonProvider.WriteObjectToJson(dyn);
					var response = await restPoint.PostAsync(json);
					if (response.StatusCode != HttpStatusCode.OK) {
						BackendlessError error = null;
						if(BackendlessError.TryParse (response.Json, ref error)){
							throw new BackendlessException(error.ErrorCode,error.Message);
						}
					}
					var user = JsonConvert.DeserializeObject <T>(response.Json);
					//var user = JsonProvider.ReadBackendlessObjectFromJson<BackendlessUser>(response.Json);
					BackendlessCallback<T>.InvokeComplete (callback, user);
					return user;
				}
			} catch (Exception ex) {
				BackendlessHadlerException.SendException (ex, callback.ErrorHandler);
				return null;
			}
		}

		public System.Threading.Tasks.Task<bool> PasswordReset (string _username,BackendlessCallback<BackendlessUser> callback = null)
		{
			throw new NotImplementedException ();
		}

		public System.Threading.Tasks.Task<bool> Logout (BackendlessUser user,BackendlessCallback<BackendlessUser> callback = null)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

