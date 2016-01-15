using System;
using Backendless.Core;

namespace Backendless.iOS
{
	public class TouchBackendlessBootstrap : BackendlessBootstrapBase
	{
		#region implemented abstract members of BackendlessBootstrapBase

		protected override IHttpClient HttpClient {
			get {
				return new TouchHttpClient ();
			}
		}

		#endregion



		public TouchBackendlessBootstrap (string applicationId, string secretKey):base(applicationId,secretKey)
		{
		}
	}
}

