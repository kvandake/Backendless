using System;
using Backendless.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Web.Script.Serialization;
using ModernHttpClient;

namespace Backendless.Core.Test
{
	public class SimpleBackendlessRestEndPoint :  HttpClient, IBackendlessRestEndPoint
	{

		public SimpleBackendlessRestEndPoint():base(){
			Init ();
		}

		public SimpleBackendlessRestEndPoint(HttpMessageHandler messageHandler):base(messageHandler){
			Init ();
		}

		void Init(){
			Timeout = TimeSpan.FromSeconds (30);
		}

		string IBackendlessRestEndPoint.BaseAddress {
			get {
				return base.BaseAddress == null ? null : base.BaseAddress.ToString ();
			}
			set {
				base.BaseAddress = new Uri(value);
			}
		}

		public string Method {get;set;}	



		#region IRestEndPoint implementation

		public async Task<ResponseObject> GetAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.GetAsync (ToQueryParameters (Parameters),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObject> PutAsync(string json = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.PutAsync (ToQueryParameters (Parameters), CreateBody(json),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObject> PostAsync(string json = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.PostAsync (BaseAddress.LocalPath + ToQueryParameters (Parameters), CreateBody(json),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObject> DeleteAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.DeleteAsync (ToQueryParameters (Parameters),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObjectGeneric<System.IO.Stream>> GetStreamAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var stream = await base.GetStreamAsync (ToQueryParameters (Parameters));
			return new ResponseObjectGeneric<System.IO.Stream> (HttpStatusCode.OK, stream);
		}

		public async Task<ResponseObjectGeneric<byte[]>> GetByteArrayAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var byteArray = await base.GetByteArrayAsync (ToQueryParameters (Parameters));
			return new ResponseObjectGeneric<byte[]> (HttpStatusCode.OK, byteArray);
		}

		public ResponseObjectGeneric<T> Get<T> ()
		{
			throw new NotImplementedException ();
		}

		public ResponseObjectGeneric<T> Put<T> (object body = null)
		{
			throw new NotImplementedException ();
		}

		public ResponseObjectGeneric<T> Post<T> (object body = null)
		{
			throw new NotImplementedException ();
		}

		public ResponseObjectGeneric<T> Delete<T> ()
		{
			throw new NotImplementedException ();
		}

		public ResponseObjectGeneric<System.IO.Stream> GetStream ()
		{
			throw new NotImplementedException ();
		}

		public ResponseObjectGeneric<byte[]> GetByteArray ()
		{
			throw new NotImplementedException ();
		}

		public IDictionary<string, string> Header {get;set;}

		public IDictionary<string, string> Parameters {get;set;}

		#endregion


		static HttpContent CreateBody(string json){
			HttpContent requestContent = null;
			if (!string.IsNullOrEmpty (json)) {
				requestContent = new StringContent (json);
			}
			return requestContent;
		}

		static async Task<ResponseObject> ReadResponse(HttpResponseMessage response){
			var content = await response.Content.ReadAsStringAsync ();
			return new ResponseObject (response.StatusCode, content);
		}


		public void ApplyHeader(){
			if (Header == null || Header.Count == 0)
				return;
			foreach (var item in Header) {
				DefaultRequestHeaders.TryAddWithoutValidation (item.Key,item.Value);
			}
		}

		public string ToQueryParameters(IDictionary<string, string> dictionary)
		{
			string path = Method ?? string.Empty;
			if (dictionary == null || dictionary.Count == 0)
				return path;
			foreach (var item in dictionary) {
				path += path == Method ? "?" : "&";
				path += string.Format ("{0}={1}", item.Key, item.Value);
			}
			return path;
		}


	}
}

