using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;

namespace Backendless.Core
{
	public interface IBackendlessCacheTable
	{

		string Table { get;}

		Task<bool> CreateObject(string apiName, JToken token,Type type,CancellationToken cancellationToken = default(CancellationToken));

		Task<JToken> ReadObjectExpression<T>(string apiName, Expression<Func<T,bool>> filter,Type type,CancellationToken cancellationToken = default(CancellationToken));

		Task<JToken> ReadObject(string apiName, string query,Type type,CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> UpdateObject(string apiName, JToken token, Type type,CancellationToken cancellationToken = default(CancellationToken));

		Task<bool> DeleteObject(string apiName, JToken token, Type type,CancellationToken cancellationToken = default(CancellationToken));
	}
}

