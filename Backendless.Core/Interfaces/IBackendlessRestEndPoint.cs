using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Backendless.Core
{
	public interface IBackendlessRestEndPoint : IDisposable
	{

		string BaseAddress {get;set;}

		string Method {get;set;}

		IDictionary<string,string> Header { get; set;}

		IDictionary<string,string> Parameters { get; set;}

		Task<ResponseObject> GetAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObject> PutAsync(string json = null,CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObject> PostAsync(string json = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObject> DeleteAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObjectGeneric<Stream>> GetStreamAsync (CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObjectGeneric<byte[]>> GetByteArrayAsync (CancellationToken cancellationToken = default(CancellationToken));

		ResponseObjectGeneric<T> Get<T> ();

		ResponseObjectGeneric<T> Put<T>(object body = null);

		ResponseObjectGeneric<T> Post<T>(object body = null);

		ResponseObjectGeneric<T> Delete<T>();

		ResponseObjectGeneric<Stream> GetStream ();

		ResponseObjectGeneric<byte[]> GetByteArray ();

	}
}

