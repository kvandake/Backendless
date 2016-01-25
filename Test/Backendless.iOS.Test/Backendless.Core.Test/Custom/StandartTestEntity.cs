using System;
using Newtonsoft.Json;

namespace Backendless.Core.Test
{
	[BackendlessEntitySettings("standartTestEntity",typeof(CustomCachTableProvider),BackendlessCachePolicy.Standart,false)]
	public class StandartTestEntity : BackendlessEntity
	{
		[JsonProperty("testName")]
		public string TestName {get;set;}

		[JsonProperty("testCreated")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime TestCreated {get;set;}
	}
}

