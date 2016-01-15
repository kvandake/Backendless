using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;

namespace Backendless.Core
{
	public interface IHttpClient : IDisposable
	{

		event EventHandler<BackendlessErrorHttpEventArgs> ErrorHandler;

		IDictionary<string,string> Headers {get;}

		IDictionary<string,string> Parameters {get;}

		Task<T> GetAsync<T> (CancellationToken cancellationToken = default(CancellationToken));

		Task<T> PutAsync<T>(object body = null,CancellationToken cancellationToken = default(CancellationToken));

		Task<T> PostAsync<T>(object body = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<T> DeleteAsync<T>(CancellationToken cancellationToken = default(CancellationToken));

		Task<Stream> GetStreamAsync (CancellationToken cancellationToken = default(CancellationToken));

		Task<byte[]> GetByteArrayAsync (CancellationToken cancellationToken = default(CancellationToken));

		T Get<T> ();

		T Put<T>(object body = null);

		T Post<T>(object body = null);

		T Delete<T>();

		Stream GetStream ();

		byte[] GetByteArray ();

	}
}

