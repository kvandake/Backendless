using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Backendless.Core
{
	public class SimpleEntityService : BackendlessServiceBase, IEntityService
	{

		public const string PrefixData = "/data/";
			
		#region IPersistenceService implementation

		public async Task<bool> SaveItem (BackendlessEntity item, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				var entityParameters = GetParametersFromEntityType (item.GetType ());
				item.IsDirty = true;
				if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.CacheOnly) {
					if (Connectivity.IsConnected) {
						var ignoreProperties = new List<string> { 
							BackendlessObject.ObjectIdKey,
							BackendlessObject.IsDirtyKey,
							BackendlessObject.OwnerIdKey,
							BackendlessObject.UpdatedKey,
							BackendlessObject.MetaKey,
							BackendlessObject.CreatedKey,
							BackendlessEntity.TableKey
						};
						if (!entityParameters.PermanentRemoval) {
							ignoreProperties.Add (BackendlessEntity.IsDeletedKey);
						}
						var json = JsonConvert.SerializeObject (item, new JsonSerializerSettings {
							ContractResolver = new IgnoreProprtyContractResolver (ignoreProperties)
						});
						using (var rest = RestPoint) {
							rest.Method = string.Concat (PrefixData, entityParameters.TableName);
							var response = await rest.PostJsonAsync (json);
							if (response.StatusCode == HttpStatusCode.OK) {
								JsonConvert.PopulateObject (response.Json, item);
								item.IsDirty = false;
							}
						}
					}
				}
				return entityParameters.BackendlessCachePolicy == BackendlessCachePolicy.ServerOnly
				|| await entityParameters.CacheTableProvider.SaveObject (entityParameters.TableName, JObject.FromObject (item));
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}



		public async Task<bool> UpdateItem (BackendlessEntity item, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				var entityParameters = GetParametersFromEntityType (item.GetType ());
				item.IsDirty = true;
				if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.CacheOnly) {
					if (Connectivity.IsConnected) {
						var ignoreProperties = new  List<string> { 
							BackendlessObject.OwnerIdKey,
							BackendlessObject.IsDirtyKey,
							BackendlessObject.UpdatedKey,
							BackendlessObject.MetaKey,
							BackendlessObject.CreatedKey,
							BackendlessEntity.TableKey
						};
						if (!entityParameters.PermanentRemoval) {
							ignoreProperties.Add (BackendlessEntity.IsDeletedKey);
						}
						var json = JsonConvert.SerializeObject (item, new JsonSerializerSettings {
							ContractResolver = new IgnoreProprtyContractResolver (ignoreProperties)
						});
						using (var rest = RestPoint) {
							rest.Method = string.Concat (PrefixData, entityParameters.TableName);
							var response = await rest.PutJsonAsync (json);
							if (response.StatusCode == HttpStatusCode.OK) {
								JsonConvert.PopulateObject (response.Json, item);
								item.IsDirty = false;
							} 
						}
					}
				}
				return entityParameters.BackendlessCachePolicy == BackendlessCachePolicy.ServerOnly
				|| await entityParameters.CacheTableProvider.UpdateObject (entityParameters.TableName, JObject.FromObject (item));
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}

		public async Task<bool> DeleteItem (BackendlessEntity item, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				var entityParameters = GetParametersFromEntityType (item.GetType ());
				var cacheTableProvider = entityParameters.CacheTableProvider;
				item.IsDirty = true;
				item.IsDeleted = true;
				bool isSuccess = false;
				if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.CacheOnly) {
					if (Connectivity.IsConnected) {
						using (var rest = RestPoint) {
							rest.Method = string.Concat (PrefixData, entityParameters.TableName);
							if (entityParameters.PermanentRemoval) {
								var ignoreProperties = new List<string> { 
									BackendlessObject.OwnerIdKey,
									BackendlessObject.IsDirtyKey,
									BackendlessObject.UpdatedKey,
									BackendlessObject.MetaKey,
									BackendlessObject.CreatedKey,
									BackendlessEntity.TableKey
								};
								var json = JsonConvert.SerializeObject (item, new JsonSerializerSettings {
									ContractResolver = new IgnoreProprtyContractResolver (ignoreProperties)
								});
								var responseUpdate = await rest.PutJsonAsync (json);
								if (responseUpdate.StatusCode == HttpStatusCode.OK) {
									isSuccess = true;
								}
							} else {
								rest.Method = string.Concat (rest.Method, "/", item.ObjectId);
								var responseDelete = await rest.DeleteJsonAsync ();
								if (responseDelete.StatusCode == HttpStatusCode.OK) {
									isSuccess = true;
								}
							}
						}
					}
				}
				if (isSuccess) {
					item.IsDirty = false;
					if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.ServerOnly) {
						return await cacheTableProvider.DeleteObject (entityParameters.TableName, item.ObjectId);
					}
				} else {
					if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.ServerOnly) {
						return  await cacheTableProvider.UpdateObject (entityParameters.TableName, JObject.FromObject (item));
					}
				}
				return true;
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}
			

	

		public async Task<IList<T>> ReadItems<T> (IBackendlessQuery<T> query, ErrorBackendlessCallback errorCallback = null) where T : BackendlessEntity
		{
			try {
				var entityParameters = GetParametersFromEntityType (typeof(T));
				IList<T> items = null;
				if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.CacheOnly) {
					using (var rest = RestPoint) {
						bool isAdvanced = query.QueryType == BackendlessQueryType.Advanced;
						rest.Method = string.Concat (PrefixData, entityParameters.TableName, isAdvanced ? BuildQuery (query) : query.Query);
						var response = await rest.GetJsonAsync ();
						if (response.StatusCode == HttpStatusCode.OK) {
							if (isAdvanced) {
								var jObject = JObject.Parse (response.Json);
								var data = jObject ["data"];
								items = data.ToObject<List<T>> ();
							} else {
								items = new List<T> ();
								items.Add (JsonConvert.DeserializeObject<T> (response.Json));
							}
						}
						if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.ServerOnly && items != null && items.Count > 0) {
							var result = await entityParameters.CacheTableProvider.MergeObjects<T> (entityParameters.TableName, query, JArray.FromObject (items));
							if (result) {
								return items;
							}
						}
					}
				}
				var array = await entityParameters.CacheTableProvider.ReadObjects (entityParameters.TableName, query);
				return array.ToObject<List<T>> ();
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return null;
			}
		}
		#endregion

		static string BuildQuery<T>(IBackendlessQuery<T> query) where T: BackendlessEntity {
			var sb = new StringBuilder ("?");
			if (query.Props != null && query.Props.Count > 0) {
				sb.Append (string.Format ("props={0}", string.Join (",", query.Props)));
				sb.Append ("&");
			}
			var whereString = query.WhereToString;
			if (!string.IsNullOrEmpty (whereString)) {
				whereString = Uri.EscapeDataString (whereString);
				sb.Append (string.Format ("where={0}", whereString));
				sb.Append ("&");
			}
			if (query.Limit.HasValue) {
				sb.Append (string.Format ("pageSize={0}", query.Limit));
				sb.Append ("&");
			}
			if (query.SortBy != null && query.SortBy.Count > 0) {
				sb.Append (string.Format ("sortBy={0}", string.Join (",", query.SortBy)));
				sb.Append ("&");
			}
			sb.Append (string.Format ("offset={0}", query.Offset));
			return sb.ToString ();
		}


	}
}

