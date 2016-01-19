using System;
using Newtonsoft.Json;

namespace Backendless.Core
{
	public class BackendlessEntity : BackendlessObject
	{

		public const string TableKey = "___class";


		string table;


		[JsonProperty(TableKey)]
		public string Table {
			get {
				return table;
			}
			internal set {
				table = value;
				OnPropertyChanged ();
			}
		}

	}
}

