using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties;

public class DynamicPropertyAccessorJsonConverter : JsonConverter<DynamicPropertyAccessor>
{
    public override void WriteJson(JsonWriter writer, DynamicPropertyAccessor value, JsonSerializer serializer)
    {
        var jObject = new JObject();

        var properties = DynamicPropertyMetadata.GetProperties(value.GetConnectedEntity().ObjectType).GetAwaiter().GetResult();

        foreach (var propertyName in properties.Select(p => p.Name))
        {
            if (value.TryGetPropertyValue(propertyName, out object result))
            {
                jObject[propertyName] = result != null ? JToken.FromObject(result, serializer) : JValue.CreateNull();
            }
        }
        jObject.WriteTo(writer);
    }

    public override DynamicPropertyAccessor ReadJson(JsonReader reader, Type objectType, DynamicPropertyAccessor existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var customProperties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in jObject.Properties())
        {
            object value;
            if (property.Value is JArray jArray)
            {
                value = jArray.ToObject<List<object>>(serializer);
            }
            else
            {
                value = property.Value.ToObject<object>(serializer);
            }
            customProperties.Add(property.Name, value);
        }

        return new DynamicPropertyAccessor(customProperties);
    }
}
