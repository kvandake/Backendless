using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Backendless.Core
{
	public class BackendlessEntity : BackendlessObject
	{

		public const string TableKey = "___class";
		public const string IsDeletedKey = "_isDeleted";

		string table;


		[JsonProperty(TableKey)]
		public string Table {
			get {
				return table;
			}
			internal set {
				table = value;
				OnPropertyChanged ();
			}
		}

		bool isDeleted;
		[JsonProperty (IsDeletedKey)]
		public bool IsDeleted {
			get {
				return isDeleted;
			}
			internal set {
				isDeleted = value;
				OnPropertyChanged ();
			}
		}

		static IEntityService EntityService {
			get {
				return BackendlessInternal.Locator.EntityService;
			}
		}

		public virtual async Task<bool> SaveAsync(CancellationToken cancellationToken = default(CancellationToken)){
			return await SaveAsync (this, cancellationToken);	
		}

		public virtual async Task<bool> UpdateAsync(CancellationToken cancellationToken = default(CancellationToken)){
			return await UpdateAsync (this, cancellationToken);	
		}

		public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken = default(CancellationToken)){
			return await DeleteAsync (this, cancellationToken);	
		}

		#region Static


		public static async Task<bool> SaveAsync(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken)){
			return await EntityService.SaveItem (item, cancellationToken);
		}


		public static async Task<bool> UpdateAsync(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken)){
			return await EntityService.UpdateItem (item, cancellationToken);
		}
			
		public static async Task<bool> DeleteAsync(BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken)){
			return await EntityService.DeleteItem (item, cancellationToken);
		}

		public static async Task<IList<T>> ReadAsync<T>(string objectId, CancellationToken cancellationToken = default(CancellationToken)) where T: BackendlessEntity {
			return await EntityService.ReadItems<T> (BackendlessQuery<T>.FromObjectIdQuery (objectId), cancellationToken);
		}

		public static async Task<IList<T>> ReadAsync<T>(IBackendlessQuery query, CancellationToken cancellationToken = default(CancellationToken)) where T: BackendlessEntity {
			return await EntityService.ReadItems<T> (query, cancellationToken);
		}

		public static async Task<bool> ClearAll(Type table, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> Sync(Type table, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}
			
		public static async Task<bool> Sync<T>(Expression<Func<T,bool>> localFilter, string serverFilter, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> Sync<T>(string serverFilter, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> PushLocalItems(Type table, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> PushLocalItems<T>(Expression<Func<T,bool>> filter, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}


		public static async Task<bool> PullServerItems(Type table, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> PullServerItems(Type table,string filter, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}


		//-----------------------------------------------------------------------------------------------------------------------------------------


		public static async Task<bool> SaveAsync(IList<BackendlessEntity> items, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> UpdateAsync(IList<BackendlessEntity> items, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		public static async Task<bool> DeleteAsync(IList<BackendlessEntity> items, CancellationToken cancellationToken = default(CancellationToken)){
			return false;
		}

		#endregion
 	}
}

