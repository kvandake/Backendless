using System;

namespace Backendless.Core
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class BackendlessField : Attribute
	{

		public string Value { get; set;}

		public BackendlessField ()
		{
		}

		public BackendlessField (string value)
		{
			Value = value;
		}
	}
}

