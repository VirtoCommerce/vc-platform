using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.DynamicProperties;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DynamicProperties;

[Trait("Category", "Unit")]
public class DynamicPropertyAccessorTests
{
    public DynamicPropertyAccessorTests()
    {
        var searchService = new MockDynamicPropertySearchService();

        var memoryCache = new MemoryCache(new MemoryCacheOptions()
        {
            Clock = new SystemClock(),
            ExpirationScanFrequency = TimeSpan.FromSeconds(1)
        });
        var cacheOptions = new OptionsWrapper<CachingOptions>(new CachingOptions { CacheEnabled = true });
        var logMock = new Mock<ILogger<PlatformMemoryCache>>();
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, cacheOptions, logMock.Object);

        var dynamicPropertyResolver = new DynamicPropertyMetaDataResolver(searchService, platformMemoryCache);

        DynamicPropertyMetadata.Initialize(dynamicPropertyResolver);
    }

    [Fact]
    public async Task ShouldReturn_NonEmptyProperties_ForTestEntityWithDynamicProperties()
    {
        // Act
        var properties = await DynamicPropertyMetadata.GetProperties(typeof(TestEntityWithDynamicProperties).FullName);

        // Assert
        Assert.NotNull(properties);
        Assert.NotEmpty(properties);
    }


    [Fact]
    public void Access_To_Property()
    {
        var entity = new TestEntityWithDynamicProperties();

        dynamic properties = entity.DynamicPropertyAccessor;
        properties.ShortTextFieldSingleValue = "Test Value Old";
        Assert.Equal("Test Value Old", properties.ShortTextFieldSingleValue);

        properties.ShortTextFieldSingleValue = "Test Value";
        Assert.Equal("Test Value", properties.ShortTextFieldSingleValue);


        // Check that DynamicProperties contains the property with correct value and type
        var dynamicProperty = entity.DynamicProperties
            .FirstOrDefault(p => p.Name == "ShortTextFieldSingleValue");

        Assert.NotNull(dynamicProperty);
        Assert.Equal("Test Value", dynamicProperty?.Values?.FirstOrDefault()?.Value);

        // Check type
        Assert.Equal(DynamicPropertyValueType.ShortText, dynamicProperty?.ValueType);
    }

    [Fact]
    public void External_Access_To_Property()
    {
        var entity = new TestEntityWithDynamicProperties();

        dynamic properties = new DynamicPropertyAccessor(entity);

        properties.ShortTextFieldSingleValue = "Test Value Old";
        Assert.Equal("Test Value Old", properties.ShortTextFieldSingleValue);

        properties.ShortTextFieldSingleValue = "Test Value";
        Assert.Equal("Test Value", properties.ShortTextFieldSingleValue);


        // Check that DynamicProperties contains the property with correct value and type
        var dynamicProperty = entity.DynamicProperties
            .FirstOrDefault(p => p.Name == "ShortTextFieldSingleValue");

        Assert.NotNull(dynamicProperty);
        Assert.Equal("Test Value", dynamicProperty?.Values?.FirstOrDefault()?.Value);

        // Check type
        Assert.Equal(DynamicPropertyValueType.ShortText, dynamicProperty?.ValueType);
    }

    [Fact]
    public void Direct_Access_To_ShortTextField_MultiValue()
    {
        var entity = new TestEntityWithDynamicProperties();

        var readPropertyValue1 = entity.ShortTextField_MultiValue;
        Assert.Empty(readPropertyValue1);

        entity.ShortTextField_MultiValue = [];
        var readPropertyValue2 = entity.ShortTextField_MultiValue;
        Assert.Empty(readPropertyValue2);

        entity.ShortTextField_MultiValue = ["Test1", "Test2"];
        var readPropertyValue3 = entity.ShortTextField_MultiValue;
        Assert.Equal(2, readPropertyValue3.Length);
        Assert.Equal("Test1", readPropertyValue3[0]);
        Assert.Equal("Test2", readPropertyValue3[1]);
    }

    [Fact]
    public void Direct_Access_To_IntegerFieldSingleValue()
    {
        var entity = new TestEntityWithDynamicProperties();

        var readPropertyValue1 = entity.IntegerFieldSingleValue;
        Assert.Null(readPropertyValue1);

        entity.IntegerFieldSingleValue = 123;
        var readPropertyValue2 = entity.IntegerFieldSingleValue;
        Assert.Equal(123, readPropertyValue2);

        entity.IntegerFieldSingleValue = null;
        var readPropertyValue3 = entity.IntegerFieldSingleValue;
        Assert.Null(readPropertyValue3);
    }


    [Theory]
    [InlineData("ShortTextFieldSingleValue", DynamicPropertyValueType.ShortText, "Test Short Text Value")]
    [InlineData("LongTextFieldSingleValue", DynamicPropertyValueType.LongText, "Test Long Long Long Text Value")]
    [InlineData("HtmlFieldSingleValue", DynamicPropertyValueType.Html, "<p>Html Text</p>")]
    [InlineData("IntegerFieldSingleValue", DynamicPropertyValueType.Integer, 123)]
    [InlineData("IntegerFieldSingleValue", DynamicPropertyValueType.Integer, 123456L)]
    [InlineData("BooleanFieldSingleValue", DynamicPropertyValueType.Boolean, true)]
    [InlineData("BooleanFieldSingleValue", DynamicPropertyValueType.Boolean, false)]
    [InlineData("DecimalFieldSingleValue", DynamicPropertyValueType.Decimal, 3.14)]
    [InlineData("DecimalFieldSingleValue", DynamicPropertyValueType.Decimal, 123)]
    [InlineData("DecimalFieldSingleValue", DynamicPropertyValueType.Decimal, 12345L)]
    [InlineData("DateTimeFieldSingleValue", DynamicPropertyValueType.DateTime, "2025-06-13T18:40:10.6332198Z")]
    [InlineData("ImageFieldSingleValue", DynamicPropertyValueType.Image, "https://localhost:5001/assets/images/Suitespot1.png")]
    [InlineData("ShortTextFieldMultiValue", DynamicPropertyValueType.ShortText, new string[] { "Test Short Text Value 1", "Test Short Text Value 2" })]
    [InlineData("LongTextFieldMultiValue", DynamicPropertyValueType.LongText, new string[] { "Test Long Long Long Text Value" })]
    //[InlineData("HtmlFieldMultiValue", DynamicPropertyValueType.Html, new string[] { "<p>Html Text</p>", "<p>Another Html Text</p>" })]
    [InlineData("IntegerFieldMultiValue", DynamicPropertyValueType.Integer, new int[] { 123, 345 })]
    [InlineData("IntegerFieldMultiValue", DynamicPropertyValueType.Integer, new long[] { 123456L, 654321L })]
    //[InlineData("BooleantFieldMultiValue", DynamicPropertyValueType.Boolean, new bool[] { true, false })]
    [InlineData("DecimalFieldMultiValue", DynamicPropertyValueType.Decimal, new string[] { "3.14", "2.18" })]
    //[InlineData("ImageFieldMultiValue", DynamicPropertyValueType.Image, new string[] { "https://localhost:5001/assets/images/Suitespot1.png" })]
    public void Test_Set_Get_DynamicProperty_With_Simple_Value(string propertyName, DynamicPropertyValueType propertyType, object value)
    {
        if (propertyType == DynamicPropertyValueType.DateTime &&
            value is string dateTimeString)
        {
            // Convert string to DateTime for comparison
            value = DateTime.Parse(dateTimeString);
        }
        else if (propertyType == DynamicPropertyValueType.Decimal &&
            value is string[])
        {
            // Convert string to DateTime for comparison
            value = (value as string[]).Select(x => decimal.Parse(x)).ToArray();
        }

        var entity = new TestEntityWithDynamicProperties();

        var setResult = entity.DynamicPropertyAccessor.TrySetPropertyValue(propertyName, value);
        Assert.True(setResult);

        var getResult = entity.DynamicPropertyAccessor.TryGetPropertyValue(propertyName, out var resultValue);
        Assert.True(getResult);

        if (resultValue is int[] intArray && value is long[] longArray)
        {
            Assert.Equal(intArray.Length, longArray.Length);
            for (int i = 0; i < intArray.Length; i++)
            {
                Assert.Equal(intArray[i], longArray[i]);
            }
        }
        else
        {
            Assert.Equal(value, resultValue);
        }

        // Check that DynamicProperties contains the property with correct value and type
        var dynamicProperty = entity.DynamicProperties
            .FirstOrDefault(p => p.Name == propertyName);

        Assert.NotNull(dynamicProperty);
        Assert.Equal(propertyType, dynamicProperty?.ValueType);

        // Additional validation for array values
        if (value is Array valueArray)
        {
            Assert.Equal(value, dynamicProperty.Values.Select(x => x.Value).ToArray());
        }
        else
        {
            Assert.Equal(value, dynamicProperty.Values.FirstOrDefault().Value);
        }
    }

    [Theory]
    [InlineData("ShortTextFieldSingleValue_Localized", DynamicPropertyValueType.ShortText, "{\"Values\":{\"en-US\":\"Hello\",\"fr-FR\":\"Bonjour\"}}")]
    [InlineData("LongTextFieldSingleValue_Localized", DynamicPropertyValueType.LongText, "{\"Values\":{\"en-US\":\"Hello\", \"fr-FR\":\"Bonjour\", \"de-DE\":\"Hallo\"}}")]
    [InlineData("HtmlTextFieldSingleValue_Localized", DynamicPropertyValueType.Html, "{\"Values\":{\"en-US\":\"<p>Hello</p>\", \"fr-FR\":\"<p>Bonjour</p>\", \"de-DE\":\"<p>Hallo</p>\"}}")]

    public void Test_Set_Get_DynamicProperty_With_Localized_Value(string propertyName, DynamicPropertyValueType propertyType, string jsonValue)
    {
        var entity = new TestEntityWithDynamicProperties();

        var value = Newtonsoft.Json.JsonConvert.DeserializeObject<LocalizedString>(jsonValue);

        var setResult = entity.DynamicPropertyAccessor.TrySetPropertyValue(propertyName, value);
        Assert.True(setResult);

        var getResult = entity.DynamicPropertyAccessor.TryGetPropertyValue(propertyName, out var resultValue);
        Assert.True(getResult);

        Assert.Equal(value.Values, ((LocalizedString)resultValue).Values);

        // Check that DynamicProperties contains the property with correct value and type
        var dynamicProperty = entity.DynamicProperties
            .FirstOrDefault(p => p.Name == propertyName);

        Assert.NotNull(dynamicProperty);
        Assert.Equal(propertyType, dynamicProperty?.ValueType);

        // Additional validation for array values
        Assert.Equal(value.Values.Count, dynamicProperty.Values.Count);
    }

    [Theory]
    [InlineData("ShortTextField_MultiValue", DynamicPropertyValueType.ShortText, new object[] { "test1", "test2" })]
    [InlineData("IntegerField_MultiValue", DynamicPropertyValueType.Integer, new object[] { 123, 345 })]
    [InlineData("DecimalField_MultiValue", DynamicPropertyValueType.Decimal, new object[] { "2.18", "3.14" })]
    public void Test_Set_Get_DynamicProperty_With_MultiValue_Value(string propertyName, DynamicPropertyValueType propertyType, object[] values)
    {
        object value = values;
        if (propertyType == DynamicPropertyValueType.Decimal)
        {
            // Convert string to Decimal
            value = values.Select(x => decimal.Parse(x.ToString())).ToArray();
        }

        var entity = new TestEntityWithDynamicProperties();

        var setResult = entity.DynamicPropertyAccessor.TrySetPropertyValue(propertyName, value);
        Assert.True(setResult);

        var getResult = entity.DynamicPropertyAccessor.TryGetPropertyValue(propertyName, out var resultValue);
        Assert.True(getResult);

        Assert.Equal(value, resultValue);

        // Check that DynamicProperties contains the property with correct value and type
        var dynamicProperty = entity.DynamicProperties
            .FirstOrDefault(p => p.Name == propertyName);

        Assert.NotNull(dynamicProperty);
        Assert.Equal(propertyType, dynamicProperty?.ValueType);

        // Additional validation for array values
        Assert.Equal(values.Length, dynamicProperty.Values.Count);
    }

    // Not Implemented Yet
    //[Theory]
    //[InlineData("ShortTextField_MultiValue_Localized", DynamicPropertyValueType.ShortText,
    //    new object[]
    //    {
    //        "{\"Values\":{\"en-US\":\"Hello\",\"fr-FR\":\"Bonjour\"}}",
    //        "{\"Values\":{\"en-US\":\"Hello\",\"fr-FR\":\"Bonjour\"}}" })
    //    ]
    //public void Test_Set_Get_ShortTextField_MultiValue_Localized(string propertyName, DynamicPropertyValueType propertyType, object[] jsonValues)
    //{
    //    var entity = new TestEntityWithDynamicProperties();

    //    var values = jsonValues
    //        .Select(v => Newtonsoft.Json.JsonConvert.DeserializeObject<LocalizedString>(v.ToString()))
    //        .ToArray();

    //    var setResult = entity.DynamicPropertyAccessor.TrySetPropertyValue(propertyName, values);
    //    Assert.True(setResult);

    //    var getResult = entity.DynamicPropertyAccessor.TryGetPropertyValue(propertyName, out var resultValue);
    //    Assert.True(getResult);

    //    Assert.Equal(values, resultValue);

    //    // Check that DynamicProperties contains the property with correct value and type
    //    var dynamicProperty = entity.DynamicProperties
    //        .FirstOrDefault(p => p.Name == propertyName);

    //    Assert.NotNull(dynamicProperty);
    //    Assert.Equal(propertyType, dynamicProperty?.ValueType);

    //    // Additional validation for array values
    //    Assert.Equal(values.Length, dynamicProperty.Values.Count);
    //}

    [Fact]
    public void Set_Wrong_Value_ShortTextDynamicProperty()
    {
        var entity = new TestEntityWithDynamicProperties();

        dynamic properties = entity.DynamicPropertyAccessor;

        Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() =>
        {
            properties.ShortTextFieldSingleValue = 123;
        });
    }

    [Fact]
    public void Set_Wrong_Name()
    {
        var entity = new TestEntityWithDynamicProperties();

        dynamic properties = entity.DynamicPropertyAccessor;

        Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() =>
        {
            properties.WrongName = 123;
        });
    }

    [Fact]
    public void Get_Wrong_Name()
    {
        var entity = new TestEntityWithDynamicProperties();

        dynamic properties = entity.DynamicPropertyAccessor;

        Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() =>
        {
            var value = properties.WrongName;
        });
    }


    [Fact]
    public void Should_Allow_Get_When_DynamicProperties_IsNull()
    {
        // Arrange
        var entity = new TestEntityWithDynamicProperties
        {
            DynamicProperties = null // Simulate unloaded dynamic properties
        };

        // Should allow to read 
        dynamic properties = entity.DynamicPropertyAccessor;
        var value = properties.ShortTextFieldSingleValue;

        Assert.Null(value);
    }

    [Fact]
    public void Should_Throw_NotSupportedException_When_DynamicProperties_IsNull()
    {
        // Arrange
        var entity = new TestEntityWithDynamicProperties
        {
            DynamicProperties = null // Simulate unloaded dynamic properties
        };

        // Act & Assert
        // Using dynamic accessor should throw NotSupportedException
        var exception1 = Assert.Throws<NotSupportedException>(() =>
        {
            dynamic properties = entity.DynamicPropertyAccessor;
            properties.ShortTextFieldSingleValue = "Test Value";
        });

        Assert.Contains("Dynamic properties are not loaded", exception1.Message);


        // Using the TrySetPropertyValue method should also throw
        var exception2 = Assert.Throws<NotSupportedException>(() =>
        {
            entity.DynamicPropertyAccessor.TrySetPropertyValue("ShortTextFieldSingleValue", "Test Value");
        });

        Assert.Contains("Dynamic properties are not loaded", exception2.Message);
    }
}

