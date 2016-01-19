using System;
using Newtonsoft.Json;

namespace Backendless.Core.Test
{
	public interface ITestInteerface
	{
		[JsonProperty("tester")]
		string Test {get;set;}

	}


	public class TestInteerface : ITestInteerface{
		#region ITestInteerface implementation
		//[JsonProperty("gfgfg")]
		public string Test {get;set;}
		#endregion
	}
}

