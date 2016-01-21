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

		public const string IsDeletedColumnName = "__deleted";

		IDictionary<Type,EntityParameters> tableParameters;

		IDictionary<Type,EntityParameters> TableParameters {
			get {
				return tableParameters ?? (tableParameters = new Dictionary<Type,EntityParameters> ());
			}
		}



		IBackendlessRestEndPoint RestPoint {
			get{
				return null;
			}
		}

		IBackendlessConnectivity Connectivity{
			get{
				return null;
			}
		}

		IBackendlessCacheTableProvider SimpleBackendlessCacheTableProvider{
			get{
				return null;
			}
		}


		#region IPersistenceService implementation

		public async Task<bool> SaveItem (BackendlessEntity item, CancellationToken cancellationToken = default(CancellationToken))
		{
			try {
				var entityParameters = GetParametersFromEntityType (item.GetType ());
				if (entityParameters.PermanentRemoval) {
					item [IsDeletedColumnName] = false;
				}
				var ignoreProperties = new [] { 
					BackendlessObject.ObjectIdKey,
					BackendlessObject.OwnerIdKey,
					BackendlessObject.UpdatedKey,
					BackendlessObject.MetaKey,
					BackendlessObject.CreatedKey,
					BackendlessEntity.TableKey
				};
				var json = JsonConvert.SerializeObject (item, new JsonSerializerSettings () {
					ContractResolver = new IgnoreProprtyContractResolver (ignoreProperties)
				});
				JToken tokenItem = null;
				if (entityParameters.BackendlessCachePolicy != BackendlessCachePolicy.CacheOnly) {
					if (Connectivity.IsConnected) {
						using (var rest = RestPoint) {
							rest.Method = string.Concat (PrefixData, entityParameters.TableName);
							var response = await rest.PostAsync (json, cancellationToken);
							if (response.StatusCode == HttpStatusCode.OK) {
								tokenItem = JToken.Parse (response.Json);
							}
						}
					}
				}
				//check token
				if (tokenItem == null) {
					tokenItem = JToken.Parse (json);
				}
				var cacheTableProvider = entityParameters.CacheTableProvider;
				return await cacheTableProvider.SaveObject (entityParameters.TableName, tokenItem);
			} catch (Exception ex) {
				return false;
			}
		}








		EntityParameters GetParametersFromEntityType(Type type){

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
					cacheTableProvider = SimpleBackendlessCacheTableProvider;
				}
				if (permanentRemoval == null) {
					permanentRemoval = false;
				}
				var parameters = new EntityParameters (tableName, cacheTableProvider,cachePolicy, permanentRemoval);
				TableParameters.Add (type, parameters);
				return parameters;
			}
		}



		#endregion


		class EntityParameters {

			public string TableName {get;private set;}

			public IBackendlessCacheTableProvider CacheTableProvider {get; private set;}

			public bool PermanentRemoval {get; private set;}

			public BackendlessCachePolicy BackendlessCachePolicy { get; private set;}

			public EntityParameters(string tableName, IBackendlessCacheTableProvider cacheTableProvider,BackendlessCachePolicy backendlessCachePolicy, bool permanentRemoval){
				TableName = tableName;
				CacheTableProvider = cacheTableProvider;
				BackendlessCachePolicy = backendlessCachePolicy;
				PermanentRemoval = permanentRemoval;
			}
		}
	}
}

