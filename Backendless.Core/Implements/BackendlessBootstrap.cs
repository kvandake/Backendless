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


		readonly string _apiVersion;
		readonly string _baseUrl;
		readonly string _applicationId;
		readonly string _secretKey;
		readonly IBackendlessPlatform _platform;


		#region Services

		IUserService userService;
		IEntityService entityService;
		IFileService fileService;

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

		public IFileService FileService {
			get {
				return fileService ?? (fileService = new SimpleFileService ());
			}
		}

		public static string RootUrl {
			get {
				if (Locator == null)
					throw new NullReferenceException ("Locator from the BackendlessBootsrap");
				string url = string.Empty;
				url += Locator.BaseUrl == BackendlessConstant.DefaultBaseUrl ? BackendlessConstant.DefaultBaseUrl : Locator.BaseUrl + BackendlessConstant.SuffixApi;
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
				header.Add (BackendlessConstant.ApplicationIdKey, Locator.ApplicationId);
				header.Add (BackendlessConstant.SecretKeyKey, Locator.SecretKey);
				header.Add (BackendlessConstant.ApplicationTypeKey, BackendlessConstant.DefaultApplicationTypeValue);
				return header;
			}
		}

		internal static IBackendlessRestEndPoint DefaultRestPoint {
			get {
				var rest = Locator.Platform.CreatorRestPoint;
				rest.BaseAddress = BackendlessInternal.RootUrl;
				rest.Header = BackendlessInternal.DefaultHeader;
				return rest;
			}
		}


		internal BackendlessInternal (IBackendlessPlatform platform, string applicationId, string secretKey, string apiVersion,string baseUrl = null)
		{
			_platform = platform;
			_baseUrl = string.IsNullOrEmpty (baseUrl) ? BackendlessConstant.DefaultBaseUrl : baseUrl;
			_apiVersion = apiVersion;
			_applicationId = applicationId;
			_secretKey = secretKey;
		}



		internal void SendException(Exception ex, ErrorBackendlessCallback errorCalback = null){
			var backendlessException = ex as BackendlessException;
			int errorCode = 0;
			string errorMessage = ex.Message;
			if (backendlessException != null) {
				errorCode = backendlessException.ErrorCode;
				SendExceptionToPublic (backendlessException);
			} else {
				SendExceptionToPublic (new BackendlessException (0, ex.Message, ex));
			}
			if (errorCalback != null) {
				errorCalback (new BackendlessError (errorCode, errorMessage));
			}
		}

		internal void SendExceptionToPublic(BackendlessException exception){
			var handler = _platform.GlobalHandler;
			if (handler != null)
				handler (this, (BackendlessError)exception);
		}
	}
}

