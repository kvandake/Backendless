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

		readonly IBackendlessPlatform _platform;

		public static string AppId { get; private set; }

		public static string SecretKey { get; private set; }

		public static string VersionNum { get; private set;}

		public static string RootPath { get; private set;}

		public static string RootUrl { get; private set;}

		public static bool IsCustomBackendless {
			get{
				return RootUrl != BackendlessConstant.DefaultBaseUrl;
			}
		}

		#region Services

		IUserService userService;
		IEntityService entityService;
		IFileService fileService;

		#endregion



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


		internal IDictionary<string,string> DefaultHeader {
			get {
				var header = new Dictionary<string,string> ();
				header.Add (BackendlessConstant.ApplicationIdKey, AppId);
				header.Add (BackendlessConstant.SecretKeyKey, SecretKey);
				header.Add (BackendlessConstant.ApplicationTypeKey, BackendlessConstant.DefaultApplicationTypeValue);
				return header;
			}
		}

		internal IBackendlessRestEndPoint DefaultRestPoint {
			get {
				var rest = Platform.CreatorRestPoint;
				rest.BaseAddress = RootUrl;
				rest.Header = DefaultHeader;
				return rest;
			}
		}


		internal BackendlessInternal (IBackendlessPlatform platform, string applicationId, string secretKey, string apiVersion,string baseUrl = null)
		{
			_platform = platform;
			if (string.IsNullOrEmpty (baseUrl)) {
				RootUrl = BackendlessConstant.DefaultBaseUrl;
				RootPath = string.Concat ("/", apiVersion);
			} else {
				RootUrl = baseUrl;
				RootPath = string.Concat (BackendlessConstant.SuffixApi,apiVersion);
			}
			VersionNum = apiVersion;
			AppId = applicationId;
			SecretKey = secretKey;
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

