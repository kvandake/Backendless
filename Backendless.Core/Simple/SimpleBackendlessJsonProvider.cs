using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;
using System.Security.Principal;

namespace Backendless.Core
{
	public class SimpleBackendlessJsonProvider : IBackendlessJsonProvider
	{

		IDictionary<Type,IBackendlessJsonConverter> converters;

		public IDictionary<Type, IBackendlessJsonConverter> Converters {
			get {
				return converters ?? (converters = new Dictionary<Type,IBackendlessJsonConverter> ());
			}
		}

		public SimpleBackendlessJsonProvider(){
			Converters.Add (typeof(BackendlessUser),new SimpleBackendlessConverter());
		}

		#region IBackendlessJsonConvert implementation

		public void AddConverter (Type type, IBackendlessJsonConverter converter)
		{
			Converters [type] = converter;
		}

		public void RemoveConverter (Type type)
		{
			if (Converters.ContainsKey (type)) {
				Converters.Remove (type);
			}
		}
			
		public string WriteBackendlessObjectToJson<T> (T obj) where T : BackendlessObject,new()
		{
			var type = typeof(T);
			return Converters.ContainsKey (type) 
				? Converters [type].WriteBackendlessObjectToJson<T> (obj) 
					: JsonConvert.SerializeObject (obj);
		}

		public T ReadObjectFromJson<T> (string json)
		{
			var type = typeof(T);
			return Converters.ContainsKey (type) 
				? Converters [type].ReadObjectFromJson<T> (json) 
					: JsonConvert.DeserializeObject<T> (json);
		}

		public T ReadBackendlessObjectFromJson<T> (string json) where T : BackendlessObject, new()
		{
			var type = typeof(T);

			return Converters.ContainsKey (type) 
				? Converters [type].ReadBackendlessObjectFromJson<T> (json) 
					: JsonConvert.DeserializeObject<T> (json);
		}

		public string WriteObjectToJson (object obj)
		{
			var type = obj.GetType ();
			return Converters.ContainsKey (type) 
				? Converters [type].WriteObjectToJson (obj) 
					: JsonConvert.SerializeObject (obj);
		}

		#endregion



		protected class SimpleBackendlessConverter : IBackendlessJsonConverter{
			
			#region IBackendlessJsonConverter implementation

			public string WriteBackendlessObjectToJson<T> (T obj) where T : BackendlessObject, new()
			{
				return JsonConvert.SerializeObject (obj, new BackendlessObjectJsonConvert<T> ());
			}

			public T ReadObjectFromJson<T> (string json)
			{
				return JsonConvert.DeserializeObject<T>(json);
			}


			public T ReadBackendlessObjectFromJson<T> (string json) where T : BackendlessObject, new()
			{
				//return JsonConvert.DeserializeObject<T> (json, new BackendlessObjectJsonConvert<T> ());
				return JsonConvert.DeserializeObject<T> (json);
			}


			public string WriteObjectToJson (object obj)
			{
				return JsonConvert.SerializeObject (obj);
			}

			#endregion



			class BackendlessObjectJsonConvert<T> : CustomCreationConverter<T> where T : BackendlessObject, new() {



				#region implemented abstract members of CustomCreationConverter

				public override T Create (Type objectType)
				{
					return new T ();
				}

				public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
				{
					var obj = new T ();
					JToken parameters = JToken.Load (reader);
					foreach (var parameter in parameters) {
						object j = null;
						serializer.Populate (parameter.CreateReader (),j);
						int h1 = 0;
					}
					int h = 0;
					return h;
				}

				public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
				{
					base.WriteJson (writer, value, serializer);
				}

				#endregion


				#region Write

				void WriteValue(JsonWriter writer, JToken token)
				{
					switch (token.Type) {
					case JTokenType.Object:
						WriteObject (writer, token);
						break;
					case JTokenType.Array:
						WriteList (writer, token);
						break;
					default:
						if (IsPrimitiveToken (token.Type)) {
							token.WriteTo (writer);
							return;
						}
						break;
					}
				}

				void WriteObject(JsonWriter writer, JToken token)
				{
					while (token.Next != null) {
						token.WriteTo (writer);
						token = token.Next;
					}
				}

				void WriteList(JsonWriter writer, JToken token)
				{
					while (token.Next != null) {
						token.WriteTo (writer);
						token = token.Next;
					}
				}

				#endregion


				#region Read

				object ReadValue(JsonReader reader)
				{
					while (reader.TokenType == JsonToken.Comment)
					{
						if (!reader.Read())
							throw new JsonSerializationException("no serialize");
					}

					switch (reader.TokenType)
					{
					case JsonToken.StartObject:
						return ReadObject(reader);
					case JsonToken.StartArray:
						return ReadList(reader);
					default:
						if (IsPrimitiveToken(reader.TokenType))
							return reader.Value;

						throw new JsonSerializationException("no serialize");
					}
				}

				private object ReadObject(JsonReader reader)
				{
					IDictionary<string, object> dictionary = new Dictionary<string, object> ();
					while (reader.Read ()) {
						switch (reader.TokenType) {
						case JsonToken.PropertyName:
							string propertyName = reader.Value.ToString ();

							if (!reader.Read ())
								throw new JsonSerializationException ("no serialize");

							object v = ReadValue (reader);

							dictionary [propertyName] = v;
							break;
						case JsonToken.Comment:
							break;
						case JsonToken.EndObject:
							return dictionary;
						}
					}
					throw new JsonSerializationException ("no serialize");
				}

				object ReadList(JsonReader reader)
				{
					List<object> list = new List<object> ();
					while (reader.Read ()) {
						switch (reader.TokenType) {
						case JsonToken.Comment:
							break;
						default:
							object v = ReadValue (reader);

							list.Add (v);
							break;
						case JsonToken.EndArray:
							return list;
						}
					}
					throw new JsonSerializationException ("no serialize");
				}


				#endregion

				internal static bool IsPrimitiveToken(JTokenType  type){
					switch (type) {
					case JTokenType.Array:
					case JTokenType.Constructor:
						return false;
					default:
						return true;
					}
				}

				//based on internal Newtonsoft.Json.JsonReader.IsPrimitiveToken
				internal static bool IsPrimitiveToken(JsonToken token)
				{
					switch (token) {
					case JsonToken.Integer:
					case JsonToken.Float:
					case JsonToken.String:
					case JsonToken.Boolean:
					case JsonToken.Undefined:
					case JsonToken.Null:
					case JsonToken.Date:
					case JsonToken.Bytes:
						return true;
					default:
						return false;
					}
				}
			}
			
		}


	}
}

