using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

namespace Backendless.Core
{

	/// <summary>
	/// Backendless user.
	/// </summary>
	public class BackendlessUser : BackendlessObject
	{

		public const string LastLoginKey = "lastLogin";
		public const string UserTokenKey = "user-token";
		public const string UsernameKey = "name";
		public const string EmailKey = "email";

		DateTime? lastLogin;
		string userToken;
		string email;
		string username;



		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[JsonProperty (UsernameKey)]
		public string Username {
			get {
				return username;
			}
			set {
				username = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		[JsonProperty(EmailKey)]
		public string Email {
			get {
				return email;
			}
			set {
				email = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Gets or sets the last login.
		/// </summary>
		/// <value>The last login.</value>
		[JsonProperty(LastLoginKey)]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? LastLogin {
			get {
				return lastLogin;
			}
			internal set {
				lastLogin = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Gets or sets the user token.
		/// </summary>
		/// <value>The user token.</value>
		[JsonProperty(UserTokenKey)]
		public string UserToken {
			get {
				return userToken;
			}
			set {
				userToken = value;
				OnPropertyChanged ();
			}
		}





		/// <summary>
		/// Signs up async.
		/// </summary>
		/// <returns>The up async.</returns>
		public async Task<bool> SignUpAsync(string password, ErrorBackendlessCallback errorCalback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().SignUpAsync (this,password, errorCalback);
		}

		/// <summary>
		/// Updates the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="errorCalback">Error calback.</param>
		public async Task<bool> UpdateAsync(ErrorBackendlessCallback errorCalback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().UpdateAsync (this, errorCalback);
		}


		/// <summary>
		/// Logouts the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="errorCallback">Error callback.</param>
		public async Task<bool> LogoutAsync(ErrorBackendlessCallback errorCallback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().LogoutAsync (this, errorCallback);
		}


		#region Static

		public static async Task<T> LoginAsync<T>(string @username, string @password, ErrorBackendlessCallback errorCalback = null) where T: BackendlessUser{
			return await BackendlessBootstrap.FromContext<IUserService> ().LoginAsync<T> (username, password, errorCalback);
		}
			
		public static async Task<bool> PasswordResetAsync(string @username, ErrorBackendlessCallback errorCallback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().PasswordResetAsync (@username, errorCallback);
		}



		#endregion

	}
}

