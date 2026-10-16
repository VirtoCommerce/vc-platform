using System;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Data.Model;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DynamicProperties;

[Trait("Category", "Unit")]
public class DynamicPropertyObjectValueEntityTests
{
    private sealed class TestDynamicPropertyObjectValueEntity : DynamicPropertyObjectValueEntity
    {
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(DynamicPropertyObjectValueEntity.ShortTextValueMaxLength)]
    public void FromModel_ShortTextWithinLimit_SetsValue(int? length)
    {
        // Arrange
        var value = length.HasValue ? new string('y', length.Value) : null;

        // Act
        var entity = FromModel(DynamicPropertyValueType.ShortText, value);

        // Assert
        entity.ShortTextValue.Should().Be(value);
    }

    [Theory]
    [InlineData(DynamicPropertyObjectValueEntity.ShortTextValueMaxLength + 1)]
    [InlineData(1025)]
    public void FromModel_ShortTextOverLimit_ThrowsClearError(int length)
    {
        // Act
        Action act = () => FromModel(DynamicPropertyValueType.ShortText, new string('y', length));

        // Assert
        act.Should().Throw<PlatformException>()
            .WithMessage($"*'Notes'*{length} characters*cannot exceed {DynamicPropertyObjectValueEntity.ShortTextValueMaxLength} characters*");
    }

    [Fact]
    public void FromModel_LongTextOverShortTextLimit_SetsValue()
    {
        // Arrange
        var value = new string('y', 10_000);

        // Act
        var entity = FromModel(DynamicPropertyValueType.LongText, value);

        // Assert
        entity.LongTextValue.Should().Be(value);
    }

    private static TestDynamicPropertyObjectValueEntity FromModel(DynamicPropertyValueType valueType, string value)
    {
        var owner = new TestEntityWithDynamicProperties { Id = "owner-id" };
        var property = new DynamicProperty { Id = "property-id", Name = "Notes", ValueType = valueType };
        var propValue = new DynamicPropertyObjectValue { ValueType = valueType, Value = value };

        var entity = new TestDynamicPropertyObjectValueEntity();
        entity.FromModel(propValue, owner, property);

        return entity;
    }
}
