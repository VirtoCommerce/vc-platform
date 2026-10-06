using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        /// <summary>
        /// Key of the <see cref="IDistributedLock"/> used for platform startup synchronization; its Redis lease is
        /// <c>DistributedLock:StartupExpiry</c> instead of <c>DistributedLock:Expiry</c>.
        /// </summary>
        public const string StartupLockServiceKey = "VirtoCommerce.Platform.Startup";

        private const string SingleAttemptLockFactoryKey = "VirtoCommerce.Platform.DistributedLock.SingleAttempt";

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

                // RedLock logs failed lock extensions only through its logger factory; without one they are discarded.
                services.AddSingleton<IDistributedLockFactory>(provider =>
                    RedLockFactory.Create(new[] { new RedLockMultiplexer(redis) }, provider.GetRequiredService<ILoggerFactory>()));

                // IDistributedLock makes one RedLock attempt at a time and handles waiting, backoff and cancellation itself.
                services.AddKeyedSingleton<IDistributedLockFactory>(SingleAttemptLockFactoryKey, (provider, _) =>
                    RedLockFactory.Create(new[] { new RedLockMultiplexer(redis) }, new RedLockRetryConfiguration(retryCount: 1), provider.GetRequiredService<ILoggerFactory>()));

#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalDistributedLockService>();
#pragma warning restore VC0015
                services.AddSingleton<IDistributedLock>(provider => CreateRedisLock(provider, options => options));
                services.AddKeyedSingleton<IDistributedLock>(StartupLockServiceKey, (provider, _) =>
                    CreateRedisLock(provider, options => WithExpiry(options, options.StartupExpiry)));
            }
            else
            {
#pragma warning disable VC0015 // Platform startup lock is internal to Platform
                services.AddSingleton<IInternalDistributedLockService, InternalNoLockService>();
#pragma warning restore VC0015
                services.AddSingleton<IDistributedLock, InProcessDistributedLock>();
                // In-process locks have no lease, so startup shares the default instance.
                services.AddKeyedSingleton<IDistributedLock>(StartupLockServiceKey, (provider, _) => provider.GetRequiredService<IDistributedLock>());
            }

            services.AddSingleton<IDistributedLockService, DistributedLockServiceAdapter>();

            return services;
        }

        private static RedisDistributedLock CreateRedisLock(IServiceProvider provider, Func<DistributedLockOptions, DistributedLockOptions> configure)
        {
            var options = configure(provider.GetRequiredService<IOptions<DistributedLockOptions>>().Value);

            return new RedisDistributedLock(
                provider.GetRequiredKeyedService<IDistributedLockFactory>(SingleAttemptLockFactoryKey),
                Options.Create(options),
                provider.GetRequiredService<ILogger<RedisDistributedLock>>());
        }

        // Copies every option; DistributedLockRegistrationTests compares all public properties, so a new option cannot be dropped here.
        private static DistributedLockOptions WithExpiry(DistributedLockOptions options, TimeSpan expiry)
        {
            return new DistributedLockOptions
            {
                WaitTime = options.WaitTime,
                DefaultTimeout = options.DefaultTimeout,
                Expiry = expiry,
                RetryInterval = options.RetryInterval,
                MaxRetryInterval = options.MaxRetryInterval,
                StartupExpiry = options.StartupExpiry,
                KeyPrefix = options.KeyPrefix,
            };
        }
    }
}
