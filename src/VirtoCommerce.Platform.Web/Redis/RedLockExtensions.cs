using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Web.Redis
{
    public static class RedLockExtensions
    {
        public static IConnectionMultiplexer GetConnectionMultiplexer(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.ApplicationServices.GetRequiredService<IConnectionMultiplexer>();

        public static RedLockFactory GetFactory(this IConnectionMultiplexer connectionMultiplexer)
            => RedLockFactory.Create(new RedLockMultiplexer[]
            {
                new RedLockMultiplexer(connectionMultiplexer)
            });

        public static IRedLock CreateLock(this RedLockFactory factory, string resource, MigrationDistributedLockOptions options)
            => factory.CreateLock(resource,
                // Successfully acquired lock expiration time
                expiryTime: options.Expiry,
                // Total time to wait until the lock is available
                waitTime: options.Wait,
                // The span to acquire the lock in retries
                retryTime: options.Retry);
    }
}
