using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Backendless.Core
{
	public interface IBackendlessCacheTableProvider
	{
		void InitProvider(string tableName, Type type);

		Task<bool> SaveObject(string tableName,Type type, BackendlessEntity item);

		Task<bool> UpdateObject(string tableName,Type type, BackendlessEntity item);

		Task<bool> DeleteObject(string tableName,Type type, string objectId);

		Task<IList<T>> ReadObjects<T> (string tableName,Type type, IBackendlessQuery<T> query) where T: BackendlessEntity;

		Task<bool> MergeObjects<T> (string tableName,Type type, IBackendlessQuery<T> query, IList<T> serverItems) where T: BackendlessEntity;

	}
}

