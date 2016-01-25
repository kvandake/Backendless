using System;
using System.Threading.Tasks;

namespace Backendless.Core
{
	public interface IFileService
	{

		Task<bool> UploadFile (string filePath, byte[] array, ErrorBackendlessCallback errorCallback = null);

		Task<T> DownloadFile<T>(string filePath, Func<byte[],T> converter, ErrorBackendlessCallback errorCallback = null);

		Task<byte[]> DownloadFile(string filePath, ErrorBackendlessCallback errorCallback = null);

	}
}

