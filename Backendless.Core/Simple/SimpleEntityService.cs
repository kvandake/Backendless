using System;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Backendless.Core
{
	public class SimpleEntityService : IEntityService
	{

		public const string PrefixData = "/data/";

		IDictionary<Type,CacheEntityParameters> tableParameters;

		IDictionary<Type,CacheEntityParameters> TableParameters {
			get {
				return tableParameters ?? (tableParameters = new Dictionary<Type,CacheEntityParameters> ());
			}
		}



		static IBackendlessRestEndPoint RestPoint {
			get {
				var rest = BackendlessInternal.Locator.Platform.CreatorRestPoint;
				rest.BaseAddress = BackendlessInternal.RootUrl;
				rest.Header = BackendlessInternal.DefaultHeader;
				return rest;
			}
		}

		static IBackendlessConnectivity Connectivity{
			get{
				return BackendlessInternal.Locator.Platform.Connectivity;
			}
		}

		static IBackendlessCacheTableProvider DefaultCacheTableProvider {
			get {
				return BackendlessInternal.Locator.Platform.CreatorDefaultCacheTableProvider;
			}
		}


		#region IPersistenceService implementation

		public async Task<bool> SaveItem (BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken))
		{
			try {
				var entityParameters = GetParametersFromEntityType (item.GetType ());
				item.IsDirty = true;
				if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.CacheOnly) {
					if (Connectivity.IsConnected) {
						var ignoreProperties = new List<string> () { 
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
							var response = await rest.PostAsync (json, cancellationToken);
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
				return false;
			}
		}



		public async Task<bool> UpdateItem (BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken))
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
							var response = await rest.PutAsync (json, cancellationToken);
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
				return false;
			}
		}

		public async Task<bool> DeleteItem (BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken))
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
								var responseUpdate = await rest.PutAsync (json, cancellationToken);
								if (responseUpdate.StatusCode == HttpStatusCode.OK) {
									isSuccess = true;
								}
							} else {
								rest.Method = string.Concat (rest.Method, "/", item.ObjectId);
								var responseDelete = await rest.DeleteAsync (cancellationToken);
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
				return false;
			}
		}
			

	

		public async Task<IList<T>> ReadItems<T> (IBackendlessQuery query, CancellationToken cancellationToken = null) where T : BackendlessEntity
		{
			try {
				var entityParameters = GetParametersFromEntityType (typeof(T));
				var array = await entityParameters.CacheTableProvider.ReadObjects (entityParameters.TableName,query);
			} catch (Exception ex) {
				return null;
			}
		}
		#endregion


		CacheEntityParameters GetParametersFromEntityType(Type type){

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
				var parameters = new CacheEntityParameters (tableName, cacheTableProvider,cachePolicy, permanentRemoval);
				TableParameters.Add (type, parameters);
				return parameters;
			}
		}
	}
}

