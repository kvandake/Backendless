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

		const string LastLoginKey = "lastLogin";
		const string UserTokenKey = "user-token";
		const string EmailKey = "email";

		DateTime? lastLogin;
		string userToken;
		string email;


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
		async Task<bool> SignUpAsync(BackendlessCallback<BackendlessUser> callback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().SignUpAsync (this,callback);
		}


		/// <summary>
		/// Logout this instance.
		/// </summary>
		async Task<bool> Logout(BackendlessCallback<BackendlessUser> callback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().Logout (this,callback);
		}


		#region Static

		public static async Task<T> LoginAsync<T>(string username, string password, BackendlessCallback<T> callback = null) where T: BackendlessUser{
			return await BackendlessBootstrap.FromContext<IUserService> ().LoginAsync<T>(username, password,callback);
		}
			
		public static async Task<bool> PasswordReset(string username, BackendlessCallback<BackendlessUser> callback = null){
			return await BackendlessBootstrap.FromContext<IUserService> ().PasswordReset (username,callback);
		}



		#endregion

	}
}

