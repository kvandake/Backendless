using System.Collections.Generic;
using System;
using System.Reflection;


namespace Backendless.Core
{
	public abstract class BackendlessServiceBase
	{
		
		static IDictionary<Type,CacheEntityParameters> tableParameters;

		static IDictionary<Type,CacheEntityParameters> TableParameters {
			get {
				return tableParameters ?? (tableParameters = new Dictionary<Type,CacheEntityParameters> ());
			}
		}

		protected virtual IBackendlessRestEndPoint RestPoint {
			get {
				var rest = BackendlessInternal.DefaultRestPoint;
				rest.Header [BackendlessConstant.ContentTypeKey] = BackendlessConstant.JsonContentTypeValue;
				return rest;
			}
		}

		protected virtual IBackendlessConnectivity Connectivity{
			get{
				return BackendlessInternal.Locator.Platform.Connectivity;
			}
		}

		protected virtual IBackendlessCacheTableProvider DefaultCacheTableProvider {
			get {
				return BackendlessInternal.Locator.Platform.CreatorDefaultCacheTableProvider;
			}
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
				var parameters = new CacheEntityParameters (tableName, cacheTableProvider,cachePolicy, permanentRemoval);
				TableParameters.Add (type, parameters);
				return parameters;
			}
		}
	}
}

