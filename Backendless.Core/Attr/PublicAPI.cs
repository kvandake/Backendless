using System;

namespace Backendless.Core
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Method)]
	public class PublicAPI : Attribute
	{

		public string Title { get; set;}

		public PublicAPI ()
		{
		}

		public PublicAPI (string title)
		{
			Title = title;
		}
	}
}

