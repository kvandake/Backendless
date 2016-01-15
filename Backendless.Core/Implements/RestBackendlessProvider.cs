using System;

namespace Backendless.Core
{
	public class RestBackendlessProvider : IBackendlessProvider
	{
		#region IBackendlessProvider implementation

		public int Priority {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		public RestBackendlessProvider ()
		{
		}
	}
}

