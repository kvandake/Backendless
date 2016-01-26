using System;
using System.Threading.Tasks;

namespace Backendless.Core
{
	public static class BackendlessFile
	{


		public static async Task<bool> UploadFile(string filePath, byte[] array, string contentType, ErrorBackendlessCallback errorCallback = null){
			return await BackendlessInternal.Locator.FileService.UploadFile (filePath, array, contentType, errorCallback);
		}

		public static async Task<T> DownloadFile<T> (string filePath, Func<byte[], T> converter, ErrorBackendlessCallback errorCallback = null)
		{
			return await BackendlessInternal.Locator.FileService.DownloadFile<T>(filePath, converter, errorCallback);
		}

		public static async Task<byte[]> DownloadFile (string filePath, ErrorBackendlessCallback errorCallback = null)
		{
			return await BackendlessInternal.Locator.FileService.DownloadFile (filePath, errorCallback);
		}


	}
}

