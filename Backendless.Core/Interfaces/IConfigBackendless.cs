using System;
using System.Collections.Generic;

namespace Backendless.Core
{
	public interface IConfigBackendless
	{

		IDictionary<Type,object> Services { get;}

		IDictionary<Type,Type> EndPoints { get;}


	}
}

