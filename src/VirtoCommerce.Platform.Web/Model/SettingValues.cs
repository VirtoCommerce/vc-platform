using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Web.Model
{
    /// <summary>
    /// The { name: value } body of the settings v2 save endpoints, bound like a Dictionary&lt;string, object&gt;
    /// except that date-like strings stay strings: MVC would turn them into a DateTime before the setting's type is
    /// known, dropping the offset and digits of a text setting that merely looks like a date.
    /// </summary>
    [JsonConverter(typeof(SettingValuesJsonConverter))]
    public class SettingValues : Dictionary<string, object>
    {
    }

    internal sealed class SettingValuesJsonConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(SettingValues);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var dateParseHandling = reader.DateParseHandling;
            reader.DateParseHandling = DateParseHandling.None;
            try
            {
                var result = new SettingValues();
                foreach (var property in JObject.Load(reader).Properties())
                {
                    // primitives as MVC binds them (string, long, double, bool, null); objects and arrays stay tokens
                    result[property.Name] = property.Value is JValue value ? value.Value : property.Value;
                }
                return result;
            }
            finally
            {
                reader.DateParseHandling = dateParseHandling;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
