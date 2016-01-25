using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Backendless.Core
{
	public interface IBackendlessCacheTableProvider
	{

		Task<bool> SaveObject(string tableName, JObject item);

		Task<bool> UpdateObject(string tableName, JObject item);

		Task<bool> DeleteObject(string tableName, string objectId);

		Task<JArray> ReadObjects<T> (string tableName, IBackendlessQuery<T> query) where T: BackendlessEntity;

		Task<bool> MergeObjects<T> (string tableName, IBackendlessQuery<T> query, JArray serverItems) where T: BackendlessEntity;

	}
}

