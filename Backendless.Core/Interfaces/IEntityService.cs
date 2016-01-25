using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Backendless.Core
{
	public interface IEntityService
	{
		Task<bool> SaveItem(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> UpdateItem(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> DeleteItem(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken));

		Task<IList<T>> ReadItems<T> (IBackendlessQuery query, CancellationToken cancellationToken = default(CancellationToken)) where T: BackendlessEntity;

	}
}

