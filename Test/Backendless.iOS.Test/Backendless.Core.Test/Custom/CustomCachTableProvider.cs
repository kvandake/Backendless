using System;
using SQLite.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Backendless.Core.Test
{
	public class CustomCachTableProvider : IBackendlessCacheTableProvider
	{


		bool _isInit;

		SQLiteConnection _connection;

		SQLiteConnection Connection {
			get{
				if (_connection == null) {
					_connection = new SQLiteConnection (new SQLite.Net.Platform.Generic.SQLitePlatformGeneric (), Path.GetTempFileName ());
				}
				return _connection;
			}
		}




		#region IBackendlessCacheTableProvider implementation

		public void InitProvider (string tableName, Type type)
		{
			//not find signature for type
		}

		void CheckToCreateTable(string tableName,Type type, JObject item){
			if (!_isInit) {
				var table = Connection.TableMappings.FirstOrDefault (x => x.TableName == tableName);
				if (table == null) {
					var query = string.Concat("CREATE TABLE IF NOT EXISTS '",tableName,"' (\n");
					var properties = new List<string> ();
					foreach (var token in item) {
						var prop = ContainsSystemProperty(token.Key) ?? ParseJTokenForCreate (token);
						if (!string.IsNullOrEmpty (prop)) {
							properties.Add (prop);
						}
					}
					var decl = string.Join (",\n", properties);
					query += decl;
					query += ")";
					var result = Connection.CreateCommand(query).ExecuteNonQuery ();
					int j = 0;
				}
				_isInit = true;
			}
		}

		static string ContainsSystemProperty(string name){
			switch (name) {
			case BackendlessObject.UpdatedKey:
			case BackendlessObject.CreatedKey:
				return string.Concat (name, " bigint");
			case BackendlessObject.ObjectIdKey:
			case BackendlessObject.MetaKey:
			case BackendlessObject.OwnerIdKey:
			case BackendlessEntity.TableKey:
				return string.Concat (name, " text");
			case BackendlessObject.IsDirtyKey:
			case BackendlessEntity.IsDeletedKey:
				return string.Concat (name, " integer");
			default:
				return null;
			}
		}

		static string ParseJTokenForCreate(KeyValuePair<string, JToken> token){
			var dec = string.Concat (token.Key, " ");
			switch (token.Value.Type) {
			case JTokenType.Boolean:
				dec += "integer";
				break;
			case JTokenType.Float:
				dec += "float";
				break;
			case JTokenType.Integer:
			case JTokenType.Date:
			case JTokenType.TimeSpan:	
				dec += "bigint";
				break;
			case JTokenType.Null:
			case JTokenType.String:
				dec += "text";
				break;
			}
			return dec;
		}

		#region Save

		void Save(string tableName, JObject item){
			var properties = item.Properties ().Where (x=> x.Value.Type!= JTokenType.Null);
			var columns = string.Join(",",properties.Select (x => x.Name));
			var values = string.Join(",", properties.Select (x => x.Value.Value<string> ()).Select (x=>string.Format ("{0}",x)));
			var query = string.Format ("INSERT OR REPLACE INTO '{0}' ({1}) values ({2})",tableName, columns, values);
			var result = Connection.CreateCommand (query).ExecuteNonQuery ();
			int g = 0;
		}

		#endregion


		public async System.Threading.Tasks.Task<bool> SaveObject (string tableName, Type type, Newtonsoft.Json.Linq.JObject item)
		{
			CheckToCreateTable (tableName, type, item);
			Save (tableName,item);
			return true;
		}

		public async System.Threading.Tasks.Task<bool> UpdateObject (string tableName,Type type, Newtonsoft.Json.Linq.JObject item)
		{
			var g = item.ToString ();
			var p = 0;
			return true;
		}

		public async System.Threading.Tasks.Task<bool> DeleteObject (string tableName,Type type, string objectId)
		{

			return true;
		}


		public async System.Threading.Tasks.Task<Newtonsoft.Json.Linq.JArray> ReadObjects<T> (string tableName,Type type, IBackendlessQuery<T> query) where T : BackendlessEntity
		{
			return null;
		}

		public async System.Threading.Tasks.Task<bool> MergeObjects<T> (string tableName,Type type, IBackendlessQuery<T> query, Newtonsoft.Json.Linq.JArray serverItems) where T : BackendlessEntity
		{
			return true;
		}
		#endregion





	}
}

