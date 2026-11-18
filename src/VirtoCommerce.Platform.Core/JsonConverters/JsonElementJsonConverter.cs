using System;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtoCommerce.Platform.Core.JsonConverters;

public class JsonElementJsonConverter : JsonConverter<JsonElement>
{
    public override void WriteJson(JsonWriter writer, JsonElement value, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (value.ValueKind == JsonValueKind.Undefined)
        {
            writer.WriteNull();
            return;
        }

        writer.WriteRawValue(value.GetRawText());
    }

    public override JsonElement ReadJson(JsonReader reader, Type objectType, JsonElement existingValue, bool hasExistingValue,
        Newtonsoft.Json.JsonSerializer serializer)
    {
        var dateParseHandling = reader.DateParseHandling;
        reader.DateParseHandling = DateParseHandling.None;

        JToken token;
        try
        {
            token = JToken.Load(reader);
        }
        finally
        {
            reader.DateParseHandling = dateParseHandling;
        }

        var json = token switch
        {
            JValue { Value: DateTime date } => System.Text.Json.JsonSerializer.Serialize(date),
            JValue { Value: DateTimeOffset date } => System.Text.Json.JsonSerializer.Serialize(date),
            _ => token.ToString(Formatting.None),
        };

        using var document = JsonDocument.Parse(json);

        return document.RootElement.Clone();
    }
}
