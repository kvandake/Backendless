using System;

namespace Backendless.Core.Test
{
	public class CustomCachTableProvider : IBackendlessCacheTableProvider
	{
		#region IBackendlessCacheTableProvider implementation

		public async System.Threading.Tasks.Task<bool> SaveObject (string tableName, Newtonsoft.Json.Linq.JObject item)
		{
			var g = item.ToString ();
			var p = 0;
			return true;
		}

		public async System.Threading.Tasks.Task<bool> UpdateObject (string tableName, Newtonsoft.Json.Linq.JObject item)
		{
			var g = item.ToString ();
			var p = 0;
			return true;
		}

		public async System.Threading.Tasks.Task<bool> DeleteObject (string tableName, string objectId)
		{

			return true;
		}


		public async System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JArray> ReadObjects<T> (string tableName, IBackendlessQuery<T> query) where T : BackendlessEntity
		{
			return null;
		}

		public async System.Threading.Tasks.Task<bool> MergeObjects<T> (string tableName, IBackendlessQuery<T> query, Newtonsoft.Json.Linq.JArray serverItems) where T : BackendlessEntity
		{
			return true;
		}
		#endregion



	}
}

