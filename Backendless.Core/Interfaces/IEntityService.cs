using System;
using System.Threading.Tasks;
using System.Threading;

namespace Backendless.Core
{
	public interface IEntityService
	{
		Task<bool> SaveItem(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken));
	}
}

