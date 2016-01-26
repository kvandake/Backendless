using System;
using System.Threading.Tasks;
using System.Net;

namespace Backendless.Core
{
	public class SimpleFileService : BackendlessServiceBase, IFileService
	{
		public const string PrefixUploadData = "/files/binary";
		public const string PrefixDownloadData = "/files";

		#region IFileService implementation

		public async Task<bool> UploadFile (string filePath, byte[] array, string contentType, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				using (var rest = RestPoint) {
					rest.Method = string.Concat (RootPath, PrefixUploadData, filePath);
					rest.Header [BackendlessConstant.ContentTypeKey] = contentType;
					var response = await rest.PutAsync (array);
					CheckResponse (response.StatusCode, response.Data);
					return true;
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return false;
			}
		}

		public async Task<T> DownloadFile<T> (string filePath, Func<byte[], T> converter, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				using (var rest = RestPoint) {
					if (converter == null) {
						throw new NullReferenceException ("converter");
					}
					var data = await DownloadFile (filePath, errorCallback);
					return data != null ? converter (data) : default(T);
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return default(T);
			}
		}

		public async Task<byte[]> DownloadFile (string filePath, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				using (var rest = RestPoint) {
					rest.Method = string.Concat (IsCustomBackendless ? BackendlessConstant.SuffixApi : string.Empty, BackendlessInternal.AppId, "/", VersionNum, PrefixDownloadData, filePath);
					var response = await rest.GetByteArrayAsync ();
					CheckResponse (response.StatusCode, string.Empty);
					return response.StatusCode == HttpStatusCode.OK ? response.Data : null;
				}
			} catch (Exception ex) {
				BackendlessInternal.Locator.SendException (ex, errorCallback);
				return null;
			}
		}

		#endregion
	}
}

