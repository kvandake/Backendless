using System;
using System.Collections.Generic;

namespace Backendless.Core
{
	public abstract class BackendlessBootstrap
	{
		const string DefaultBaseUrl = "https://api.backendless.com";
		const string SuffixApi = "/api/";
		const string ApplicationIdKey = "application-id";
		const string SecretKeyKey = "secret-key";
		const string ContentTypeKey = "Content-Type";
		const string ApplicationTypeKey = "application-type";
		const string DefaultContentTypeValue = "application/json";
		const string DefaultApplicationTypeValue = "REST";



		static readonly object mutex = new object ();
		readonly string _apiVersion;
		readonly string _baseUrl;
		readonly string _applicationId;
		readonly string _secretKey;

		protected abstract IDictionary<Type,Type> Endpoints { get; }

		protected abstract IDictionary<Type, object> Services { get; }

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

		protected BackendlessBootstrap (string applicationId, string secretKey, string apiVersion,string baseUrl = null)
		{
			_baseUrl = string.IsNullOrEmpty (baseUrl) ? DefaultBaseUrl : baseUrl;
			_apiVersion = apiVersion;
			_applicationId = applicationId;
			_secretKey = secretKey;
		}


		/// <summary>
		/// Creators the end point.
		/// </summary>
		/// <returns>The end point.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		internal T CreatorEndPoint<T>() where T: class{
			var type = typeof(T);
			return Endpoints.ContainsKey (type) 
				? BackendlessExtension.CreatorObjectWithDefaultConstructor (Endpoints[type]) as T
					: null;
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






		void ApplyConfig(IConfigBackendless config){
			if (config.EndPoints != null) {
				foreach (var localEndpoint in config.EndPoints) {
					Endpoints [localEndpoint.Key] = localEndpoint.Value;
				}
			}
			if (config.Services != null) {
				foreach (var localService in config.Services) {
					Services [localService.Key] = localService.Value;
				}
			}
		}




		/// <summary>
		/// Resolve the instance.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T FromContextInternal<T>(){
			lock (mutex) {
				var type = typeof(T);
				return Services.ContainsKey (type) ? (T)Services [type] : default(T);
			}
		}


		#region Static

		static volatile BackendlessBootstrap _locator;

		internal static BackendlessBootstrap Locator{
			get{
				return _locator;
			}
		}


		protected static void Init(BackendlessBootstrap backendless, IConfigBackendless config){
			if (config != null) {
				backendless.ApplyConfig (config);
			}
			_locator = backendless;
		}


		/// <summary>
		/// Resolve the instance for global.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T FromContext<T>(){
			return Locator.FromContextInternal<T> ();
		}
			
		#endregion

	}
}

