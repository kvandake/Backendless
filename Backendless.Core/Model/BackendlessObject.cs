using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using Newtonsoft.Json;

namespace Backendless.Core
{

	/// <summary>
	/// Backendless object.
	/// </summary>
	public class BackendlessObject : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public const string ObjectIdKey = "objectId";
		public const string OwnerIdKey = "ownerId";
		public const string UpdatedKey = "updated";
		public const string CreatedKey = "created";
		public const string IsDirtyKey = "isDirty";
		public const string MetaKey = "__meta";

		IDictionary<string,object> items;
		string ownerId;
		string objectId;
		bool isDirty;
		DateTime? updated;
		DateTime created;
		object meta;

		[JsonExtensionData]
		IDictionary<string, object> Items {
			get {
				return items ?? (items = new Dictionary<string,object> ());
			}
			set {
				items = value;
			}
		}



		/// <summary>
		/// Containses the key.
		/// </summary>
		/// <returns><c>true</c>, if key was containsed, <c>false</c> otherwise.</returns>
		/// <param name="key">Key.</param>
		public bool ContainsProperty(string key){
			return Items.ContainsKey (key);
		}

		public void RemoveProperty(string key){
			if (ContainsProperty (key)) {
				Items.Remove (key);
			}
		}

		protected BackendlessObject(){
		}

		/// <summary>
		/// Gets or sets the <see cref="Backendless.Core.BackendlessObject"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public object this [string key] {
			get {
				return Items [key];
			}
			set {
				Items [key] = value;
			}
		}

		/// <summary>
		/// Gets or sets the object identifier.
		/// </summary>
		/// <value>The object identifier.</value>
		[JsonProperty(ObjectIdKey)]
		public string ObjectId {
			get {
				return objectId;
			}
			//TODO Set to internal
			set {
				objectId = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Gets or sets the owner identifier.
		/// </summary>
		/// <value>The owner identifier.</value>
		[JsonProperty(OwnerIdKey)]
		public string OwnerId {
			get {
				return ownerId;
			}
			internal set {
				ownerId = value;
				OnPropertyChanged ();
			}
		}


		#region Internal

		/// <summary>
		/// Gets or sets a value indicating whether this instance is dirty.
		/// </summary>
		/// <value><c>true</c> if this instance is dirty; otherwise, <c>false</c>.</value>
		[JsonProperty(IsDirtyKey)]
		public bool IsDirty {
			get {
				return isDirty;
			}
			internal set {
				isDirty = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Gets or sets the updated.
		/// </summary>
		/// <value>The updated.</value>
		[JsonProperty(UpdatedKey)]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? Updated {
			get {
				return updated;
			}
			internal set {
				updated = value;
				OnPropertyChanged ();
			}
		}

		/// <summary>
		/// Gets or sets the created.
		/// </summary>
		/// <value>The created.</value>
		[JsonProperty(CreatedKey)]
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime Created {
			get {
				return created;
			}
			internal set {
				created = value;
				OnPropertyChanged ();
			}
		}

		//TODO no implement the logic
		/// <summary>
		/// Gets or sets the meta.
		/// </summary>
		/// <value>The meta.</value>
		[JsonProperty(MetaKey)]
		public object Meta {
			get {
				return meta;
			}
			internal set {
				meta = value;
				OnPropertyChanged ();
			}
		}


		#endregion


		public IEnumerator<KeyValuePair<string,object>> GetEnumerator(){
			return Items.GetEnumerator ();
		}
			
		protected virtual void OnPropertyChanged ([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			handler?.Invoke (this, new PropertyChangedEventArgs (propertyName));
		}


	}
}

