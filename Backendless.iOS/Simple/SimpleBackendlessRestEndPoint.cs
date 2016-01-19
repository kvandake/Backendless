using System;
using Backendless.Core;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace Backendless.iOS
{
	public class SimpleBackendlessRestEndPoint :  HttpClient, IBackendlessRestEndPoint
	{

		public SimpleBackendlessRestEndPoint(){
		}

		public SimpleBackendlessRestEndPoint(HttpMessageHandler messageHandler):base(messageHandler){
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

		public async Task<ResponseObject<T>> GetAsync<T> (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.GetAsync (ToQueryParameters (Parameters),cancellationToken);
			return await ReadResponse<T> (response);
		}

		public async Task<ResponseObject<T>> PutAsync<T> (object body = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.PutAsync (ToQueryParameters (Parameters), CreateBody(body),cancellationToken);
			return await ReadResponse<T> (response);
		}

		public async Task<ResponseObject<T>> PostAsync<T> (object body = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.PostAsync (ToQueryParameters (Parameters), CreateBody(body),cancellationToken);
			return await ReadResponse<T> (response);
		}

		public async Task<ResponseObject<T>> DeleteAsync<T> (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var response = await base.DeleteAsync (ToQueryParameters (Parameters),cancellationToken);
			return await ReadResponse<T> (response);
		}

		public async Task<ResponseObject<System.IO.Stream>> GetStreamAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var stream = await base.GetStreamAsync (ToQueryParameters (Parameters));
			return new ResponseObject<System.IO.Stream> (HttpStatusCode.OK, stream);
		}

		public async Task<ResponseObject<byte[]>> GetByteArrayAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			ApplyHeader ();
			var byteArray = await base.GetByteArrayAsync (ToQueryParameters (Parameters));
			return new ResponseObject<byte[]> (HttpStatusCode.OK, byteArray);
		}

		public ResponseObject<T> Get<T> ()
		{
			throw new NotImplementedException ();
		}

		public ResponseObject<T> Put<T> (object body = null)
		{
			throw new NotImplementedException ();
		}

		public ResponseObject<T> Post<T> (object body = null)
		{
			throw new NotImplementedException ();
		}

		public ResponseObject<T> Delete<T> ()
		{
			throw new NotImplementedException ();
		}

		public ResponseObject<System.IO.Stream> GetStream ()
		{
			throw new NotImplementedException ();
		}

		public ResponseObject<byte[]> GetByteArray ()
		{
			throw new NotImplementedException ();
		}

		public IDictionary<string, string> Header {get;set;}

		public IDictionary<string, string> Parameters {get;set;}

		#endregion


		static HttpContent CreateBody(object body){
			HttpContent requestContent = null;
			if (body != null) {
				string json = JsonConvert.SerializeObject (body);
				requestContent = new StringContent (json);
			}
			return requestContent;
		}

		static async Task<ResponseObject<T>> ReadResponse<T>(HttpResponseMessage response){
			var content = await response.Content.ReadAsStringAsync ();
			if (response.IsSuccessStatusCode) {
				var obj = JsonConvert.DeserializeObject<T> (content);
				return new ResponseObject<T> (response.StatusCode, obj);
			}
			return new ResponseObject<T> (response.StatusCode, default(T), content);
		}


		public void ApplyHeader(){
			if (Header == null || Header.Count == 0)
				return;
			foreach (var item in Header) {
				DefaultRequestHeaders.Add (item.Key,item.Value);
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

