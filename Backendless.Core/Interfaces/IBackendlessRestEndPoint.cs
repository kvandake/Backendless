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

		Task<ResponseObject> GetJsonAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObject> PutJsonAsync(string json = null,CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObject> PostJsonAsync(string json = null, CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObject> DeleteJsonAsync(CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObjectGeneric<string>> PostAsync(byte[] array, CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObjectGeneric<string>> PutAsync(byte[] array, CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObjectGeneric<Stream>> GetStreamAsync (CancellationToken cancellationToken = default(CancellationToken));

		Task<ResponseObjectGeneric<byte[]>> GetByteArrayAsync (CancellationToken cancellationToken = default(CancellationToken));

	}
}

