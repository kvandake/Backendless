using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Reflection;

namespace Backendless.Core
{
	public class SimpleEntityService : BackendlessServiceBase, IEntityService
	{

		public const string PrefixData = "/data/";
			
		static IDictionary<Type,CacheEntityParameters> tableParameters;

		static IDictionary<Type,CacheEntityParameters> TableParameters {
			get {
				return tableParameters ?? (tableParameters = new Dictionary<Type,CacheEntityParameters> ());
			}
		}

		protected override IBackendlessRestEndPoint RestPoint {
			get {
				var rest = base.RestPoint;
				rest.Header [BackendlessConstant.ContentTypeKey] = BackendlessConstant.JsonContentTypeValue;
				return rest;
			}
		}

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
							rest.Method = string.Concat (RootPath, PrefixData, entityParameters.TableName);
							var response = await rest.PostJsonAsync (json);
							CheckResponse (response);
							JsonConvert.PopulateObject (response.Json, item);
							item.IsDirty = false;
						}
					}
				}
				return entityParameters.BackendlessCachePolicy == BackendlessCachePolicy.ServerOnly
					|| await entityParameters.CacheTableProvider.SaveObject (entityParameters.TableName,item.GetType (), item);
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
							rest.Method = string.Concat (RootPath, PrefixData, entityParameters.TableName);
							var response = await rest.PutJsonAsync (json);
							CheckResponse (response);
							JsonConvert.PopulateObject (response.Json, item);
							item.IsDirty = false;
						}
					}
				}
				return entityParameters.BackendlessCachePolicy == BackendlessCachePolicy.ServerOnly
					|| await entityParameters.CacheTableProvider.UpdateObject (entityParameters.TableName,item.GetType (), item);
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
							rest.Method = string.Concat (RootPath, PrefixData, entityParameters.TableName);
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
								CheckResponse (responseUpdate);
								isSuccess = true;
							} else {
								rest.Method = string.Concat (rest.Method, "/", item.ObjectId);
								var responseDelete = await rest.DeleteJsonAsync ();
								CheckResponse (responseDelete);
								isSuccess = true;
							}
						}
					}
				}
				if (isSuccess) {
					item.IsDirty = false;
					if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.ServerOnly) {
						return await cacheTableProvider.DeleteObject (entityParameters.TableName,item.GetType (), item.ObjectId);
					}
				} else {
					if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.ServerOnly) {
						return  await cacheTableProvider.UpdateObject (entityParameters.TableName,item.GetType (), item);
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
					if (Connectivity.IsConnected) {
						using (var rest = RestPoint) {
							bool isAdvanced = query.QueryType == BackendlessQueryType.Advanced;
							rest.Method = string.Concat (RootPath, PrefixData, entityParameters.TableName, isAdvanced ? BuildQuery (query) : query.Query);
							var response = await rest.GetJsonAsync ();
							CheckResponse (response);
							if (isAdvanced) {
								var jObject = JObject.Parse (response.Json);
								var data = jObject ["data"];
								items = data.ToObject<List<T>> ();
							} else {
								items = new List<T> ();
								items.Add (JsonConvert.DeserializeObject<T> (response.Json));
							}
							if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.ServerOnly && items != null && items.Count > 0) {
								var result = await entityParameters.CacheTableProvider.MergeObjects<T> (entityParameters.TableName, typeof(T), query, items);
								if (result) {
									return items;
								}
							}
						}
					}
				}
				return await entityParameters.CacheTableProvider.ReadObjects (entityParameters.TableName, typeof(T), query);
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



		protected CacheEntityParameters GetParametersFromEntityType(Type type){

			if (TableParameters.ContainsKey (type)) {
				return TableParameters [type];
			} else {
				string tableName = null; 
				IBackendlessCacheTableProvider cacheTableProvider = null;
				BackendlessCachePolicy cachePolicy = BackendlessCachePolicy.Standart;
				bool permanentRemoval = false;
				var settingsAttribute = type.GetTypeInfo ().GetCustomAttribute<BackendlessEntitySettings> ();
				if (settingsAttribute != null) {
					tableName = settingsAttribute.TableName;
					cacheTableProvider = settingsAttribute.CacheTableProvider;
					permanentRemoval = settingsAttribute.PermanentRemoval;
					cachePolicy = settingsAttribute.CachePolicy;
				}
				if (string.IsNullOrEmpty (tableName)) {
					tableName = type.Name;
				}
				if (cacheTableProvider == null) {
					cacheTableProvider = DefaultCacheTableProvider;
				}
				//init provider for create tables
				cacheTableProvider.InitProvider (tableName, type);
				var parameters = new CacheEntityParameters (tableName, cacheTableProvider, cachePolicy, permanentRemoval);
				TableParameters.Add (type, parameters);
				return parameters;
			}
		}


	}
}

