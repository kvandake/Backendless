using System;
using Backendless.Core;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backendless.iOS
{
	public class TouchHttpClient : HttpClient, IHttpClient
	{
		#region IHttpClient implementation

		public event EventHandler<BackendlessErrorHttpEventArgs> ErrorHandler;

		public Task<T> GetAsync<T> (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			throw new NotImplementedException ();
		}

		public Task<T> PutAsync<T> (object body = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			throw new NotImplementedException ();
		}

		public Task<T> PostAsync<T> (object body = null, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			throw new NotImplementedException ();
		}

		public Task<T> DeleteAsync<T> (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			throw new NotImplementedException ();
		}

		public Task<System.IO.Stream> GetStreamAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			throw new NotImplementedException ();
		}

		public Task<byte[]> GetByteArrayAsync (System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))
		{
			throw new NotImplementedException ();
		}

		public T Get<T> ()
		{
			throw new NotImplementedException ();
		}

		public T Put<T> (object body = null)
		{
			throw new NotImplementedException ();
		}

		public T Post<T> (object body = null)
		{
			throw new NotImplementedException ();
		}

		public T Delete<T> ()
		{
			throw new NotImplementedException ();
		}

		public System.IO.Stream GetStream ()
		{
			throw new NotImplementedException ();
		}

		public byte[] GetByteArray ()
		{
			throw new NotImplementedException ();
		}

		public IDictionary<string, string> Headers {
			get {
				throw new NotImplementedException ();
			}
		}

		public IDictionary<string, string> Parameters {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion



		protected virtual void OnErrorHandler (BackendlessErrorHttpEventArgs e)
		{
			var handler = ErrorHandler;
			if (handler != null)
				handler (this, e);
		}
	}
}

