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
        foreach (var prop in value.GetConnectedEntity().DynamicProperties)
        {
            var propValue = prop.Values?.FirstOrDefault()?.Value;
            jObject[prop.Name] = propValue != null ? JToken.FromObject(propValue, serializer) : JValue.CreateNull();
        }
        jObject.WriteTo(writer);
    }

    public override DynamicPropertyAccessor ReadJson(JsonReader reader, Type objectType, DynamicPropertyAccessor existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        var customProperties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        foreach (var property in jObject.Properties())
        {
            customProperties.Add(property.Name, property.Value.ToObject<object>(serializer));
        }

        return new DynamicPropertyAccessor(customProperties);
    }
}
