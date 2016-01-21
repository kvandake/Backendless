using System;
using System.Collections.Generic;
using Plugin.Connectivity.Abstractions;

namespace Backendless.Core
{
	public interface IConfigBackendless
	{

		IBackendlessRestEndPoint CreateRestPoint { get;}

		IBackendlessCacheTableProvider DefaultCacheTableProvider{ get;}

		IConnectivity Connectivity { get;}
	}
}

