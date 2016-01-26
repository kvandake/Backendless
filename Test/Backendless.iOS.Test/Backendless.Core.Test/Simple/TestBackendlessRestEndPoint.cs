using System;
using Backendless.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;

namespace Backendless.Core.Test
{
	public class TestBackendlessRestEndPoint :  HttpClient, IBackendlessRestEndPoint
	{

		public TestBackendlessRestEndPoint():base(){
			Init ();
		}

		public TestBackendlessRestEndPoint(HttpMessageHandler messageHandler):base(messageHandler){
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

		public async Task<ResponseObject> GetJsonAsync(System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.GetAsync (ToQueryParameters (Parameters),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObject> PutJsonAsync(string json = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.PutAsync (ToQueryParameters (Parameters), CreateBody(json),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObject> PostJsonAsync(string json = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.PostAsync (ToQueryParameters (Parameters), CreateBody(json),cancellationToken);
			return await ReadResponse(response);
		}

		public async Task<ResponseObjectGeneric<string>> PostAsync (byte[] array, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{

			ApplyHeader ();
			var content = new ByteArrayContent (array);
			if (Header.ContainsKey (BackendlessConstant.ContentTypeKey)) {
				content.Headers.TryAddWithoutValidation (BackendlessConstant.ContentTypeKey, Header [BackendlessConstant.ContentTypeKey]);
			}
			var response = await base.PostAsync (ToQueryParameters (Parameters), content).ConfigureAwait (false);
			var contentMessage = await response.Content.ReadAsStringAsync ();
			return new ResponseObjectGeneric<string> (response.StatusCode, contentMessage);
		
		}

		public async Task<ResponseObjectGeneric<string>> PutAsync (byte[] array, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var content = new ByteArrayContent (array);
			if (Header.ContainsKey (BackendlessConstant.ContentTypeKey)) {
				content.Headers.TryAddWithoutValidation (BackendlessConstant.ContentTypeKey, Header [BackendlessConstant.ContentTypeKey]);
			}
			var response = base.PutAsync (ToQueryParameters (Parameters), content, cancellationToken).Result;
			var contentMessage = await response.Content.ReadAsStringAsync ();
			return new ResponseObjectGeneric<string> (response.StatusCode, contentMessage);
		}



		public async Task<ResponseObject> DeleteJsonAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await DeleteAsync (ToQueryParameters (Parameters),cancellationToken);
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
			

		public IDictionary<string, string> Header {get;set;}

		IDictionary<string, string> parameters;
		public IDictionary<string, string> Parameters {
			get {
				return parameters ?? (parameters = new Dictionary<string,string> ());
			}
			set {
				parameters = value;
			}
		}

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
			var path = Method ?? string.Empty;
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

