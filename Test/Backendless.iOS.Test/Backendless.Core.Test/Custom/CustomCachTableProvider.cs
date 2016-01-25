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

		#endregion



	}
}

