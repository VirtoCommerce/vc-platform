using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InMemory;
using VirtoCommerce.Platform.DistributedLock.NoLock;
using VirtoCommerce.Platform.DistributedLock.Redis;

namespace VirtoCommerce.Platform.Web.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");

            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                services.AddSingleton<IConnectionMultiplexer>(redis);
                services.AddSingleton(redis.GetSubscriber());
                services.AddDataProtection()
                        .SetApplicationName("VirtoCommerce.Platform")
                        .PersistKeysToStackExchangeRedis(redis, "VirtoCommerce-Keys");

                var redLockFactory = RedLockFactory.Create(new[] { new RedLockMultiplexer(redis) });
                services.AddSingleton<IDistributedLockFactory>(redLockFactory);

                services.AddSingleton<IInternalDistributedLockService, InternalDistributedLockService>();
                services.AddSingleton<IDistributedLockService, DistributedLockService>();
            }
            else
            {
                services.AddSingleton<IInternalDistributedLockService, InternalNoLockService>();
                // Single-instance fallback: serialize per resource key within this process.
                // For multi-replica deployments, configure Redis so DistributedLockService is used instead.
                services.AddSingleton<IDistributedLockService, InMemoryLockService>();
            }

            return services;
        }
    }
}
