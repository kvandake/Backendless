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

		bool? isDeleted;
		[JsonProperty (IsDeletedKey)]
		public bool? IsDeleted {
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

		public virtual async Task<bool> SaveAsync(ErrorBackendlessCallback errorCallback = null){
			return await SaveAsync (this, errorCallback);	
		}

		public virtual async Task<bool> UpdateAsync(ErrorBackendlessCallback errorCallback = null){
			return await UpdateAsync (this, errorCallback);	
		}

		public virtual async Task<bool> DeleteAsync(ErrorBackendlessCallback errorCallback = null){
			return await DeleteAsync (this, errorCallback);	
		}

		#region Static


		public static async Task<bool> SaveAsync(BackendlessEntity item, ErrorBackendlessCallback errorCallback = null){
			return await EntityService.SaveItem (item, errorCallback);
		}


		public static async Task<bool> UpdateAsync(BackendlessEntity item, ErrorBackendlessCallback errorCallback = null){
			return await EntityService.UpdateItem (item, errorCallback);
		}
			
		public static async Task<bool> DeleteAsync(BackendlessEntity item, ErrorBackendlessCallback errorCallback = null){
			return await EntityService.DeleteItem (item, errorCallback);
		}

		public static async Task<IList<T>> ReadAsync<T>(string objectId, ErrorBackendlessCallback errorCallback = null) where T: BackendlessEntity {
			return await EntityService.ReadItems<T> (BackendlessQuery<T>.FromObjectIdQuery (objectId), errorCallback);
		}

		public static async Task<IList<T>> ReadAsync<T>(IBackendlessQuery<T> query, ErrorBackendlessCallback errorCallback = null) where T: BackendlessEntity {
			return await EntityService.ReadItems<T> (query, errorCallback);
		}
			


		#endregion
 	}
}

