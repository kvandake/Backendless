using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System;

namespace Backendless.Core
{
	public interface IBackendlessCacheTableProvider
	{
		void InitProvider(string tableName, Type type);

		Task<bool> SaveObject(string tableName,Type type, JObject item);

		Task<bool> UpdateObject(string tableName,Type type, JObject item);

		Task<bool> DeleteObject(string tableName,Type type, string objectId);

		Task<JArray> ReadObjects<T> (string tableName,Type type, IBackendlessQuery<T> query) where T: BackendlessEntity;

		Task<bool> MergeObjects<T> (string tableName,Type type, IBackendlessQuery<T> query, JArray serverItems) where T: BackendlessEntity;

	}
}

