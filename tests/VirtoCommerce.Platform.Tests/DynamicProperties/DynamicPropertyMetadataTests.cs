using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.DynamicProperties;
using Xunit;

namespace VirtoCommerce.Platform.Tests.DynamicProperties;

public class TestEntity
{

}

[Trait("Category", "Unit")]
public class DynamicPropertyMetadataTests
{
    public DynamicPropertyMetadataTests()
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
    public async Task GetProperties_Generic_Calls_With_Entity_TypeName()
    {
        // Act
        var result = await DynamicPropertyMetadata.GetProperties<TestEntityWithDynamicProperties>();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetProperties_Generic_Calls_With_Entity_TypeName_2()
    {
        // Act
        var result = await DynamicPropertyMetadata.GetProperties(typeof(TestEntityWithDynamicProperties));

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task GetProperties_If_Not_Initialized()
    {
        var result = await DynamicPropertyMetadata.GetProperties<TestEntity>();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
