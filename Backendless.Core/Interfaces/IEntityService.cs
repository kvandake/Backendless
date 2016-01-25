using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backendless.Core
{
	public interface IEntityService
	{
		Task<bool> SaveItem(BackendlessEntity item, ErrorBackendlessCallback errorCallback = null);

		Task<bool> UpdateItem(BackendlessEntity item, ErrorBackendlessCallback errorCallback = null);

		Task<bool> DeleteItem(BackendlessEntity item, ErrorBackendlessCallback errorCallback = null);

		Task<IList<T>> ReadItems<T> (IBackendlessQuery<T> query, ErrorBackendlessCallback errorCallback = null) where T: BackendlessEntity;

	}
}

