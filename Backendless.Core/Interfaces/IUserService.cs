using System;
using System.Threading.Tasks;

namespace Backendless.Core
{
	public interface IUserService
	{

		Task<bool> SignUpAsync(BackendlessUser user,BackendlessCallback<BackendlessUser> callback = null);

		Task<T> LoginAsync<T> (string @username, string @password, BackendlessCallback<T> callback = null) where T: BackendlessUser;

		Task<bool> PasswordReset(string @username,BackendlessCallback<BackendlessUser> callback = null);

		Task<bool> Logout (BackendlessUser user,BackendlessCallback<BackendlessUser> callback = null);

	}
}

