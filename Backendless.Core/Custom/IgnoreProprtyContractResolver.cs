using System;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Backendless.Core
{
	class IgnoreProprtyContractResolver : DefaultContractResolver
	{
		readonly IList<string> _ignoreProperties;

		public IgnoreProprtyContractResolver(IList<string> ignoreProperties)
		{
			_ignoreProperties = ignoreProperties;
		}

		protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
		{
			var properties = base.CreateProperties (type, memberSerialization);
			return properties.Where (p => !_ignoreProperties.Contains (p.PropertyName)).ToList ();
		}
	}
}

