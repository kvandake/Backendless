using System;

namespace Backendless.Core
{
	[AttributeUsage(AttributeTargets.Class)]
	public class BackendlessEntitySettings : Attribute
	{

		public BackendlessEntitySettings ()
		{

		}

		public BackendlessEntitySettings (string tableName, Type tableProvider,BackendlessCachePolicy cachePolicy = BackendlessCachePolicy.Standart, bool permanentRemoval = false)
		{
			TableName = tableName;
			CacheProviderType = tableProvider;
			PermanentRemoval = permanentRemoval;
			CachePolicy = cachePolicy;
		}

		public BackendlessEntitySettings (string tableName, Type tableProvider, bool permanentRemoval = false)
		{
			TableName = tableName;
			CacheProviderType = tableProvider;
			PermanentRemoval = permanentRemoval;
		}

		public BackendlessEntitySettings (string tableName, bool permanentRemoval = false)
		{
			TableName = tableName;
			PermanentRemoval = permanentRemoval;
		}

		public BackendlessEntitySettings (Type tableCacheProvider, bool permanentRemoval = false)
		{
			CacheProviderType = tableCacheProvider;
			PermanentRemoval = permanentRemoval;
		}

		/// <summary>
		/// название таблицы 
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName { get; set;}


		/// <summary>
		/// Провайдер сохранения данных
		/// </summary>
		/// <value>The table provider.</value>
		public Type CacheProviderType {get;set;}

		/// <summary>
		/// Политика кэша
		/// </summary>
		/// <value>The cache policy.</value>
		public BackendlessCachePolicy CachePolicy { get; set;}

		/// <summary>
		/// Обратимое удаление
		/// </summary>
		/// <value><c>true</c> if permanent removal; otherwise, <c>false</c>.</value>
		public bool PermanentRemoval {get;set; }

		public IBackendlessCacheTableProvider CacheTableProvider {
			get {
				if (CacheProviderType == null || CacheProviderType != typeof(IBackendlessCacheTableProvider)) {
					return null;
				}
				var obj = Activator.CreateInstance (CacheProviderType);
				return (IBackendlessCacheTableProvider)obj;
			}
		}
	}
}

