using System.Threading.Tasks;

namespace Backendless.Core
{
	[PublicAPI("base user service")]
	public interface IUserService
	{

		/// <summary>
		/// Signs up async.
		/// </summary>
		/// <returns>The up async.</returns>
		/// <param name="user">User.</param>
		/// <param name="password">Password.</param>
		/// <param name="errorCallback">Error callback.</param>
		Task<bool> SignUpAsync(BackendlessUser user,string @password, ErrorBackendlessCallback errorCallback = null);

		/// <summary>
		/// Logins the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="username">Username.</param>
		/// <param name="password">Password.</param>
		/// <param name="errorCallback">Error callback.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		Task<T> LoginAsync<T> (string @username, string @password, ErrorBackendlessCallback errorCallback = null) where T: BackendlessUser;

		/// <summary>
		/// Updates the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="user">User.</param>
		/// <param name="errorCallback">Error callback.</param>
		Task<bool> UpdateAsync (BackendlessUser user, ErrorBackendlessCallback errorCallback = null);

		/// <summary>
		/// Passwords the reset async.
		/// </summary>
		/// <returns>The reset async.</returns>
		/// <param name="username">Username.</param>
		/// <param name="errorCallback">Error callback.</param>
		Task<bool> PasswordResetAsync (string @username,ErrorBackendlessCallback errorCallback = null);

		/// <summary>
		/// Logouts the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="user">User.</param>
		/// <param name="errorCallback">Error callback.</param>
		Task<bool> LogoutAsync (BackendlessUser user,ErrorBackendlessCallback errorCallback = null);

	}
}

