using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Backendless.Core
{
	public interface IBackendlessQuery
	{
		IList<string> Props { get; }

		string WhereToString{ get;}

		Expression Where { get;}

		int? Limit {get;}

		IList<string> SortBy {get;}

		int Offset { get;}

		string Query {get;}

		BackendlessQueryType QueryType { get;}
	}
}

