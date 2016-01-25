using System;

namespace Backendless.Core
{
	public static class BackendlessConstant
	{

		public const string DefaultBaseUrl = "https://api.backendless.com";
		public const string SuffixApi = "/api/";
		public const string ApplicationIdKey = "application-id";
		public const string SecretKeyKey = "secret-key";
		public const string ApplicationTypeKey = "application-type";
		public const string DefaultApplicationTypeValue = "REST";

		public const string ContentTypeKey = "Content-Type";
		public const string JsonContentTypeValue = "application/json";
		public const string TextContentTypeValue = "text/plain;charset=UTF-8";
		public const string DataContentTypeValue = "multipart/form-data";
	}
}

