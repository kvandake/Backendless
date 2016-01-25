using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Backendless.Core
{
	// DateTimeConverterBase
	public class UnixDateTimeConverter: IsoDateTimeConverter
	{

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
//			base.WriteJson (writer,value,serializer);
//			//writer.WriteValue (((DateTime)value).ToString ("yyyy-MM-dd'T'hh:mm:ss.SSS'Z'"));
//			return;
//			writer.WriteValue (value);
			if (value is DateTime?) {
				var nullDate = (DateTime?)value;
				if (nullDate.HasValue) {
					writer.WriteValue (nullDate.Value.ToUnixTime ());
				} else {
					writer.WriteNull ();
				}
				return;
			}
			if (value is DateTime) {
				long val = ((DateTime)value).ToUnixTime ();
				writer.WriteValue (val);
				return;
			}
			throw new Exception ("Expected date object value.");
//			if (value is DateTime?) {
//				var nullDate = (DateTime?)value;
//				if (nullDate.HasValue) {
//					writer.WriteValue (nullDate.Value.ToString ("o"));
//				} else {
//					writer.WriteNull ();
//				}
//				return;
//			}
//			if (value is DateTime) {
//				writer.WriteValue (((DateTime)value).ToString ("o"));
//				return;
//			}
//			throw new Exception ("Expected date object value.");
		}

		/// <summary>
		///   Reads the JSON representation of the object.
		/// </summary>
		/// <param name = "reader">The <see cref = "JsonReader" /> to read from.</param>
		/// <param name = "objectType">Type of the object.</param>
		/// <param name = "existingValue">The existing value of object being read.</param>
		/// <param name = "serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
			JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null) {
				if (objectType == typeof(DateTime)) {
					return DateTime.MinValue;
				} else {
					return null;
				}
			}
			if (reader.TokenType != JsonToken.Integer)
				throw new Exception("Wrong Token Type");

			long ticks = (long) reader.Value;
			return ticks.FromUnixTime();
		}
	}
}

