using System;
using Newtonsoft.Json;
using Backendless.Core;

namespace Backendless.Core.Test
{
	[BackendlessEntitySettings("testEntity",typeof(CustomCachTableProvider),BackendlessCachePolicy.Standart,true)]
	public class PermanentTestEntity : BackendlessEntity
	{

		[JsonProperty("testName")]
		public string TestName {get;set;}

		[JsonProperty("testCreated")]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime TestCreated {get;set;}
	}
}

