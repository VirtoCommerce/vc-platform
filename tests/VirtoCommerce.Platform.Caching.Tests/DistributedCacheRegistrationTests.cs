using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Redis;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    [Trait("Category", "Unit")]
    public class DistributedCacheRegistrationTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AddCaching_WithoutRedis_UsesLocalMemory(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddCaching(CreateConfiguration(connectionString));
            services.AddMvc();

            using var provider = services.BuildServiceProvider();
            var cache = provider.GetRequiredService<IDistributedCache>();
            cache.SetString("test", "value");

            Assert.Equal("value", cache.GetString("test"));
            Assert.IsType<MemoryDistributedCache>(cache);
            Assert.IsType<PlatformMemoryCache>(provider.GetRequiredService<IPlatformMemoryCache>());
            Assert.Single(provider.GetServices<IDistributedCache>());
        }

        [Fact]
        public void AddCaching_WithRedis_PreservesSelectedProviderWhenMvcIsAdded()
        {
            var services = new ServiceCollection();
            services.AddCaching(CreateConfiguration("localhost:6379"));
            services.AddMvc();

            using var provider = services.BuildServiceProvider();
            Assert.IsAssignableFrom<RedisCache>(provider.GetRequiredService<IDistributedCache>());
            Assert.Single(provider.GetServices<IDistributedCache>());
            Assert.Equal(typeof(RedisPlatformMemoryCache), services.Single(x => x.ServiceType == typeof(IPlatformMemoryCache)).ImplementationType);
        }

        [Theory]
        [InlineData(null, "VirtoCommerceChannel:cache:")]
        [InlineData("application-staging", "application-staging:cache:")]
        public void AddCaching_WithRedis_ConfiguresConnectionAndKeyPrefix(string channelName, string prefix)
        {
            var services = new ServiceCollection();
            services.AddCaching(CreateConfiguration("localhost:6379,defaultDatabase=3", channelName));

            using var provider = services.BuildServiceProvider();
            var options = provider.GetRequiredService<IOptions<RedisCacheOptions>>().Value;

            Assert.Equal("localhost:6379,defaultDatabase=3", options.Configuration);
            Assert.Equal(prefix, options.InstanceName);
            Assert.Null(options.ConnectionMultiplexerFactory);
        }

        private static IConfiguration CreateConfiguration(string connectionString, string channelName = null)
        {
            var settings = new Dictionary<string, string>
            {
                ["ConnectionStrings:RedisConnectionString"] = connectionString,
            };
            if (channelName != null)
            {
                settings["Caching:Redis:ChannelName"] = channelName;
            }

            return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }
    }
}
