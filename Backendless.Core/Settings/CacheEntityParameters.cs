namespace Backendless.Core
{
	public class CacheEntityParameters {

		public string TableName {get;private set;}

		public IBackendlessCacheTableProvider CacheTableProvider {get; private set;}

		public bool PermanentRemoval {get; private set;}

		public BackendlessCachePolicy BackendlessCachePolicy { get; private set;}

		public CacheEntityParameters(string tableName, IBackendlessCacheTableProvider cacheTableProvider,BackendlessCachePolicy backendlessCachePolicy, bool permanentRemoval){
			TableName = tableName;
			CacheTableProvider = cacheTableProvider;
			BackendlessCachePolicy = backendlessCachePolicy;
			PermanentRemoval = permanentRemoval;
		}
	}
}

