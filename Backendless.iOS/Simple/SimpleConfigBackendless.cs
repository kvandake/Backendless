using System;
using Backendless.Core;

namespace Backendless.iOS
{
	public class SimpleConfigBackendless : IConfigBackendless
	{
		#region IConfigBackendless implementation

		public System.Collections.Generic.IDictionary<Type, object> Services {
			get {
				throw new NotImplementedException ();
			}
		}

		public System.Collections.Generic.IDictionary<Type, Type> EndPoints {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion




	}
}

