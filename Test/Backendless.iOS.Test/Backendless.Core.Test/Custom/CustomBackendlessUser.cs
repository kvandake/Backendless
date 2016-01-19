using System;
using Newtonsoft.Json;

namespace Backendless.Core.Test
{
	public class CustomBackendlessUser : BackendlessUser
	{
		public CustomBackendlessUser ()
		{
		}

		[JsonProperty("avatarSource")]
		public string AvatarSource { get; set;}
	}
}

