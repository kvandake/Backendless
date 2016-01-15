using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

namespace Backendless.Core
{
	public class BackendlessObject : IEnumerable<KeyValuePair<string, object>>, INotifyPropertyChanged
	{

		IDictionary<string,object> items;
		string objectId;
		bool isDirty;
		DateTime updated;
		DateTime created;

		IDictionary<string, object> Items {
			get {
				return items ?? (items = new Dictionary<string,object> ());
			}
		}




		public bool ContainsKey(string key){
			return Items.ContainsKey (key);
		}


		public object this [string key] {
			get {
				return Items [key];
			}
			set {
				Items [key] = value;
			}
		}


		public string ObjectId {
			get {
				return objectId;
			}
			internal set {
				objectId = value;
				OnPropertyChanged ();
			}
		}


		public bool IsDirty {
			get {
				return isDirty;
			}
			internal set {
				isDirty = value;
				OnPropertyChanged ();
			}
		}


		public DateTime Updated {
			get {
				return updated;
			}
			internal set {
				updated = value;
				OnPropertyChanged ();
			}
		}


		public DateTime Created {
			get {
				return created;
			}
			internal set {
				created = value;
				OnPropertyChanged ();
			}
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator ()
		{
			return Items.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return Items.GetEnumerator ();
		}

		#endregion









		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			handler?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}

	}
}

