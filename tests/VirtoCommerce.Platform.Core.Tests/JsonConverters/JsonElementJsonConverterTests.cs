using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.JsonConverters;
using Xunit;

namespace VirtoCommerce.Platform.Core.Tests.JsonConverters;

public class JsonElementJsonConverterTests
{
    private readonly JsonSerializerSettings _settings = new()
    {
        ContractResolver = new PolymorphJsonContractResolver(),
        Converters = [new JsonElementJsonConverter()],
    };

    [Theory]
    [InlineData("\"client_id_metadata_document\"")]
    [InlineData("42")]
    [InlineData("true")]
    [InlineData("null")]
    [InlineData("[\"a\",\"b\"]")]
    [InlineData("{\"nested\":{\"value\":1}}")]
    [InlineData("\"2026-11-11T10:00:00Z\"")]
    public void Write_JsonElement_WritesItsJson(string json)
    {
        // Arrange
        var properties = new Dictionary<string, JsonElement> { ["value"] = JsonDocument.Parse(json).RootElement.Clone() };

        // Act
        var result = JsonConvert.SerializeObject(properties, _settings);

        // Assert
        result.Should().Be($"{{\"value\":{json}}}");
    }

    [Theory]
    [InlineData("\"client_id_metadata_document\"")]
    [InlineData("42")]
    [InlineData("true")]
    [InlineData("null")]
    [InlineData("[\"a\",\"b\"]")]
    [InlineData("{\"nested\":{\"value\":1}}")]
    [InlineData("\"2026-11-11T10:00:00Z\"")]
    public void Read_Json_ReadsItAsJsonElement(string json)
    {
        // Arrange
        var serialized = $"{{\"value\":{json}}}";

        // Act
        var result = JsonConvert.DeserializeObject<Dictionary<string, JsonElement>>(serialized, _settings);

        // Assert
        result["value"].GetRawText().Should().Be(json);
    }

    [Fact]
    public void Write_UndefinedElement_WritesNull()
    {
        // Arrange
        var properties = new Dictionary<string, JsonElement> { ["value"] = default };

        // Act
        var serialized = JsonConvert.SerializeObject(properties, _settings);

        // Assert
        serialized.Should().Be("{\"value\":null}");
    }
}
