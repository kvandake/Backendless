using System;
using System.Collections.Generic;

namespace Backendless.Core
{
	public abstract class BackendlessBootstrap
	{

		public static void Init(IBackendlessPlatform platform, string applicationId, string secretKey, string apiVersion,string baseUrl = null){
			var backenlessInternal = new BackendlessInternal (platform, applicationId, secretKey, apiVersion, baseUrl);
			BackendlessInternal.Locator = backenlessInternal;
		}
			
	}


	class BackendlessInternal {

		const string DefaultBaseUrl = "https://api.backendless.com";
		const string SuffixApi = "/api/";
		const string ApplicationIdKey = "application-id";
		const string SecretKeyKey = "secret-key";
		const string ContentTypeKey = "Content-Type";
		const string ApplicationTypeKey = "application-type";
		const string DefaultContentTypeValue = "application/json";
		const string DefaultApplicationTypeValue = "REST";

		readonly string _apiVersion;
		readonly string _baseUrl;
		readonly string _applicationId;
		readonly string _secretKey;
		readonly IBackendlessPlatform _platform;


		#region Services

		IUserService userService;
		IEntityService entityService;

		#endregion


		public string BaseUrl {
			get {
				return _baseUrl;
			}
		}

		public string ApiVersion {
			get {
				return _apiVersion;
			}
		}

		public string ApplicationId {
			get {
				return _applicationId;
			}
		}

		public string SecretKey {
			get {
				return _secretKey;
			}
		}

		public IBackendlessPlatform Platform {
			get {
				return _platform;
			}
		}


		public IUserService UserService {
			get {
				return userService ?? (userService = new SimpleUserService ());
			}
		}

		public IEntityService EntityService {
			get {
				return entityService ?? (entityService = new SimpleEntityService ());
			}
		}

		public static string RootUrl {
			get {
				if (Locator == null)
					throw new NullReferenceException ("Locator from the BackendlessBootsrap");
				string url = string.Empty;
				url += Locator.BaseUrl == DefaultBaseUrl ? DefaultBaseUrl : Locator.BaseUrl + SuffixApi;
				url += Locator.ApiVersion;
				return url;	
			}
		}

		#region Static

		static volatile BackendlessInternal _locator;

		internal static BackendlessInternal Locator {
			get {
				return _locator;
			} set {
				_locator = value;
			}
		}
			
		#endregion


		public static IDictionary<string,string> DefaultHeader {
			get {
				if (Locator == null)
					throw new NullReferenceException ("Locator from the BackendlessBootsrap");
				var header = new Dictionary<string,string> ();
				header.Add (ApplicationIdKey, Locator.ApplicationId);
				header.Add (SecretKeyKey, Locator.SecretKey);
				header.Add (ContentTypeKey, DefaultContentTypeValue);
				header.Add (ApplicationTypeKey, DefaultApplicationTypeValue);
				return header;
			}
		}


		internal BackendlessInternal (IBackendlessPlatform platform, string applicationId, string secretKey, string apiVersion,string baseUrl = null)
		{
			_platform = platform;
			_baseUrl = string.IsNullOrEmpty (baseUrl) ? DefaultBaseUrl : baseUrl;
			_apiVersion = apiVersion;
			_applicationId = applicationId;
			_secretKey = secretKey;
		}
	}
}

