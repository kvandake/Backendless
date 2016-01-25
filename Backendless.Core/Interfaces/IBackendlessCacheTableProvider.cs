using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Backendless.Core
{
	public interface IBackendlessCacheTableProvider
	{

		Task<bool> SaveObject(string tableName, JObject item);

		Task<bool> UpdateObject(string tableName, JObject item);

		Task<bool> DeleteObject(string tableName, string objectId);

		Task<JArray> ReadObjects (string tableName, IBackendlessQuery query);

	}
}

