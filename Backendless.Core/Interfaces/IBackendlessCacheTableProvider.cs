using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Backendless.Core
{
	public interface IBackendlessCacheTableProvider
	{

		Task<bool> SaveObject(string tableName, JToken item);

	}
}

