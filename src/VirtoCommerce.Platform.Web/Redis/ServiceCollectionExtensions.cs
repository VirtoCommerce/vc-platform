using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Platform.DistributedLock;
using VirtoCommerce.Platform.DistributedLock.InProcess;
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

#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalDistributedLockService>();
#pragma warning restore VC0015
                services.AddSingleton<IDistributedLock, RedisDistributedLock>();
            }
            else
            {
#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalNoLockService>();
#pragma warning restore VC0015
                services.AddSingleton<IDistributedLock, InProcessDistributedLock>();
            }

            services.AddSingleton<IDistributedLockService, DistributedLockServiceAdapter>();

            return services;
        }
    }
}
