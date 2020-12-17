using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Core.ObjectValue
{
    public class ObjectValueJsonConverter : JsonConverter
    {
        public override bool CanWrite => true;
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType.GetCustomAttributes(true).Any(a => a.GetType() == typeof(ObjectValueSerializedAsStringForIndexAttribute));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var obj = JObject.FromObject(value, new JsonSerializer() { ContractResolver = serializer.ContractResolver });

            if (value != null
                && obj.TryGetValue("value", out var objValue)
                && obj.TryGetValue("valueType", out var objValueType))
            {
                obj["value"] = ConvertValue(objValue, objValueType.ToObject<ObjectValueType>());
            }

            obj.WriteTo(writer);
        }

        private string ConvertValue(object value, ObjectValueType valueType)
        {
            string result;

            switch (valueType)
            {
                case ObjectValueType.LongText:
                case ObjectValueType.ShortText:
                case ObjectValueType.GeoPoint:
                case ObjectValueType.Integer:
                    result = Convert.ToString(value);
                    break;
                case ObjectValueType.Number:
                    result = Convert.ToString(value, CultureInfo.InstalledUICulture);
                    break;
                case ObjectValueType.DateTime:
                    result = Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToString("O");
                    break;
                case ObjectValueType.Boolean:
                    result = value.ToString().ToLower();
                    break;
                default:
                    throw new NotSupportedException();
            }

            return result;
        }
    }


    

}
