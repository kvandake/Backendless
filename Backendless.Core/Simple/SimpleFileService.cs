using System;
using System.Threading.Tasks;
using System.Net;

namespace Backendless.Core
{
	public class SimpleFileService : IFileService
	{
		public const string PrefixData = "/files/binary/";

		static IBackendlessRestEndPoint RestPoint {
			get {
				var rest = BackendlessInternal.DefaultRestPoint;
				rest.Header [BackendlessConstant.ContentTypeKey] = BackendlessConstant.DataContentTypeValue;
				return rest;
			}
		}

		#region IFileService implementation

		public async Task<bool> UploadFile (string filePath, byte[] array, ErrorBackendlessCallback errorCallback = null)
		{
			try {
				using (var rest = RestPoint) {
					rest.Method = string.Concat (PrefixData,filePath);
					var response = await rest.PostAsync (array);
					return response.StatusCode == HttpStatusCode.OK;
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
					rest.Method = filePath;
					var response = await rest.GetByteArrayAsync ();
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

