using System;
using SQLite.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections;
using SQLite.Net.Async;

namespace Backendless.Core.Test
{
	public class CustomCachTableProvider : IBackendlessCacheTableProvider
	{

		SQLiteConnection _connection;
		SQLiteAsyncConnection _asyncconnection;

		SQLiteConnection Connection {
			get{
				if (_connection == null) {
					var path = "/Users/home/Desktop/temp.db";
					_connection = new SQLiteConnection (new SQLite.Net.Platform.Generic.SQLitePlatformGeneric (), path);
				}
				return _connection;
			}
		}

		SQLiteAsyncConnection AsyncConnection {
			get {
				if (_asyncconnection == null) {
					var path = "/Users/home/Desktop/temp.db";
					var connString = new SQLiteConnectionString (path, true);
					var withLock = new SQLiteConnectionWithLock (new SQLite.Net.Platform.Generic.SQLitePlatformGeneric (), connString);
					_asyncconnection = new SQLiteAsyncConnection (() => withLock);
				}
				return _asyncconnection;
			}
		}


		#region IBackendlessCacheTableProvider implementation

		public void InitProvider (string tableName, Type type)
		{
			Connection.CreateTable (type);
		}


		public async System.Threading.Tasks.Task<bool> SaveObject (string tableName, Type type, BackendlessEntity item)
		{
			return await AsyncConnection.InsertOrIgnoreAsync (item) != 0;
		}

		public async System.Threading.Tasks.Task<bool> UpdateObject (string tableName,Type type, BackendlessEntity item)
		{
			return await AsyncConnection.InsertOrReplaceAsync (item) != 0;
		}

		public async System.Threading.Tasks.Task<bool> DeleteObject (string tableName,Type type, string objectId)
		{
			return await AsyncConnection.DeleteAsync (objectId) != 0;
		}


		public async System.Threading.Tasks.Task<IList<T>> ReadObjects<T> (string tableName,Type type, IBackendlessQuery<T> query) where T : BackendlessEntity
		{
			var list = await AsyncConnection.Table<T> ().ToListAsync ();
			return await AsyncConnection.Table<T> ().Where (query.Where).ToListAsync ();
		}

		public async System.Threading.Tasks.Task<bool> MergeObjects<T> (string tableName,Type type, IBackendlessQuery<T> query, IList<T> serverItems) where T : BackendlessEntity
		{
			return true;
		}
		#endregion





	}
}

