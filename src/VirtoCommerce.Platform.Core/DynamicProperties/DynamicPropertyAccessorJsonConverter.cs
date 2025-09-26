using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Core.DynamicProperties;

public class DynamicPropertyAccessorJsonConverter : JsonConverter<DynamicPropertyAccessor>
{
    public override void WriteJson(JsonWriter writer, DynamicPropertyAccessor value, JsonSerializer serializer)
    {
        var jObject = new JObject();

        var properties = DynamicPropertyMetadata.GetProperties(value.GetConnectedEntity().ObjectType).GetAwaiter().GetResult();

        foreach (var prop in properties)
        {
            if (value.TryGetPropertyValue(prop.Name, out object result))
            {
                jObject[prop.Name] = result != null ? JToken.FromObject(result, serializer) : JValue.CreateNull();
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
            customProperties.Add(property.Name, property.Value.ToObject<object>(serializer));
        }

        return new DynamicPropertyAccessor(customProperties);
    }
}
