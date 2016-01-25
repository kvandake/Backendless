using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Backendless.Core
{
	public interface IBackendlessQuery<Entity> where Entity: BackendlessEntity
	{
		IList<string> Props { get; }

		string WhereToString{ get;}

		Expression<Func<Entity,bool>> Where { get;}

		int? Limit {get;}

		IList<string> SortBy {get;}

		uint Offset { get;}

		string Query {get;}

		BackendlessQueryType QueryType { get;}
	}
}

