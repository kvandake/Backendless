using System;

namespace Backendless.Core.Test
{
	public class TestBackendlessPlatform : IBackendlessPlatform
	{
		#region IBackendlessPlatform implementation

		public IBackendlessRestEndPoint CreatorRestPoint {
			get {
				return new TestBackendlessRestEndPoint ();
			}
		}

		public IBackendlessCacheTableProvider CreatorDefaultCacheTableProvider {
			get {
				return null;
			}
		}

		public IBackendlessConnectivity Connectivity {
			get {
				return new TestConnectivity ();
			}
		}


		public EventHandler<BackendlessError> GlobalHandler {get;set;}
		#endregion



	}


	public class TestConnectivity : IBackendlessConnectivity{
		#region IBackendlessConnectivity implementation

		public static bool EnableNetwork = true;



		public bool IsConnected {
			get {
				return EnableNetwork;
			} 
		}
		#endregion
	}
}

