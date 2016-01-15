using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backendless.Core
{
	public class BackendlessUser : BackendlessObject
	{

		const string DefaultEmailKey = "email";
		const string DefaultPasswordKey = "password";

		public string Email {
			get {
				return ContainsKey (DefaultEmailKey) ? this [DefaultEmailKey].ToString () : string.Empty;
			}
			set {
				this [DefaultEmailKey] = value;
			}
		}


		public string Password {
			get {
				return ContainsKey (DefaultPasswordKey) ?  this [DefaultPasswordKey].ToString () : string.Empty;
			}
			set {
				this [DefaultPasswordKey] = value;
			}
		}




		async Task<bool> SignUpAsync(EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){
			
		}



		async Task<bool> Logout(EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){
			
		}

		#region Static

		/// <summary>
		/// Logs the in async.
		/// </summary>
		/// <returns>The in async.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="errorHandler">Error handler.</param>
		public static async Task<BackendlessUser> LogInAsync(string username, string password, EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){

		}

		/// <summary>
		/// Passwords the reset.
		/// </summary>
		/// <returns>The reset.</returns>
		/// <param name="username">Username.</param>
		/// <param name="errorHandler">Error handler.</param>
		public static async Task<bool> PasswordReset(string username, EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){
			
		}


		/// <summary>
		/// Retrievings the user roles.
		/// </summary>
		/// <returns>The user roles.</returns>
		/// <param name="user">User.</param>
		/// <param name="errorHandler">Error handler.</param>
		public static async Task<IList<string>> RetrievingUserRoles(BackendlessUser user, EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){
		}

		/// <summary>
		/// Assignings the role.
		/// </summary>
		/// <returns>The role.</returns>
		/// <param name="user">User.</param>
		/// <param name="role">Role.</param>
		/// <param name="errorHandler">Error handler.</param>
		public static async Task<bool> AssigningRole(BackendlessUser user,string role, EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){
		}

		/// <summary>
		/// Unassignings the role.
		/// </summary>
		/// <returns>The role.</returns>
		/// <param name="user">User.</param>
		/// <param name="role">Role.</param>
		/// <param name="errorHandler">Error handler.</param>
		public static async Task<bool> UnassigningRole(BackendlessUser user,string role, EventHandler<BackendlessErrorHttpEventArgs> errorHandler = null){
		}

		#endregion

	}
}

