using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Backendless.Core
{
	public class SimpleUserService : IUserService
	{

		const string RegisterMethodPath = "/users/register";
		const string LoginMethodPath = "/users/login";
		const string LogoutMethodPath = "/users/logout";
		const string UpdateMethodPath = "/users";
		const string RestorePassword = "/users/restorepassword";

		static IBackendlessRestEndPoint RestPoint {
			get {
				var rest = BackendlessInternal.DefaultRestPoint;
				rest.Header [BackendlessConstant.ContentTypeKey] = BackendlessConstant.JsonContentTypeValue;
				return rest;
			}
		}

		#region IUserService implementation


		public async Task<bool> SignUpAsync (BackendlessUser user,string @password, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				using (var restPoint = RestPoint) {
					restPoint.Method = RegisterMethodPath;
					user ["password"] = @password;
					var ignoreProperties = new [] { 
						BackendlessObject.ObjectIdKey,
						BackendlessObject.OwnerIdKey,
						BackendlessObject.UpdatedKey,
						BackendlessObject.MetaKey,
						BackendlessObject.CreatedKey,
						BackendlessUser.LastLoginKey,
						BackendlessUser.UserTokenKey
					};
					var json = JsonConvert.SerializeObject (user, new JsonSerializerSettings {
						ContractResolver = new IgnoreProprtyContractResolver (ignoreProperties),
						NullValueHandling = NullValueHandling.Ignore
					});
					var response = await restPoint.PostJsonAsync (json);
					CheckResponse(response);
					user.RemoveProperty ("password");
					JsonConvert.PopulateObject (response.Json, user);
					return true;
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}



		public async Task<T> LoginAsync<T>(string @username, string @password,ErrorBackendlessCallback errorCallback = null) where T: BackendlessUser
		{
			try {
				using (var restPoint = RestPoint) {
					restPoint.Method = LoginMethodPath;
					var json = JsonConvert.SerializeObject (new { login = @username, password = @password});
					var response = await restPoint.PostJsonAsync (json);
					CheckResponse(response);
					var user = JsonConvert.DeserializeObject <T> (response.Json);
					return user;
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return null;
			}
		}

		public async Task<bool> UpdateAsync (BackendlessUser user, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				if (string.IsNullOrEmpty (user.UserToken))
					throw new ArgumentException ("user.UserToken is null or empty");
				using (var restPoint = RestPoint) {
					restPoint.Header[BackendlessUser.UserTokenKey]=user.UserToken;
					restPoint.Method = string.Format ("{0}/{1}",UpdateMethodPath,user.ObjectId);
					var ignoreProperties = new [] { 
						BackendlessObject.ObjectIdKey,
						BackendlessObject.OwnerIdKey,
						BackendlessObject.UpdatedKey,
						BackendlessObject.MetaKey,
						BackendlessObject.CreatedKey,
						BackendlessUser.LastLoginKey,
						BackendlessUser.UserTokenKey
					};
					var json = JsonConvert.SerializeObject (user, new JsonSerializerSettings {
						ContractResolver = new IgnoreProprtyContractResolver (ignoreProperties),
						NullValueHandling = NullValueHandling.Ignore
					});
					var response = await restPoint.PutJsonAsync (json);
					CheckResponse (response);
					JsonConvert.PopulateObject (response.Json, user);
					return true;
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}

		public async Task<bool> PasswordResetAsync (string @username,ErrorBackendlessCallback errorCallback = null)
		{
			try {
				if (string.IsNullOrEmpty (@username))
					throw new ArgumentException ("@username is null or empty");
				using (var restPoint = RestPoint) {
					restPoint.Method = string.Format ("{0}/{1}", RestorePassword, @username);
					var response = await restPoint.GetJsonAsync ();
					CheckResponse (response);
					return true;
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}

		public async Task<bool> LogoutAsync (BackendlessUser user,ErrorBackendlessCallback errorCallback = null)
		{
			try {
				if (string.IsNullOrEmpty (user.UserToken))
					throw new ArgumentException ("user.UserToken is null or empty");
				using (var restPoint = RestPoint) {
					restPoint.Header[BackendlessUser.UserTokenKey]=user.UserToken;
					restPoint.Method = LogoutMethodPath;
					var response = await restPoint.GetJsonAsync ();
					CheckResponse (response);
					return true;
				}
			} catch(Exception ex){
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}

		#endregion


		static void CheckResponse(ResponseObject response){
			if (response.StatusCode != HttpStatusCode.OK) {
				BackendlessError error = null;
				if (BackendlessError.TryParse (response.Json, ref error)) {
					throw new BackendlessException (error.ErrorCode, error.Message);
				} else {
					throw new BackendlessException (0, response.Json);
				}
			}
		}

	}
}

