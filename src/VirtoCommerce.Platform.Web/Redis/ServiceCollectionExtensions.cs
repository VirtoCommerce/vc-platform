using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Redis;
using VirtoCommerce.Platform.Core.SignalR;

namespace VirtoCommerce.Platform.Web.Redis
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString(RedisConfiguration.ConnectionStringName);

            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                services.AddSingleton<IConnectionMultiplexer>(redis);
                services.AddSingleton(redis.GetSubscriber());
            }

            return services;
        }

        public static void AddRedisBackplane(this ISignalRServerBuilder builder, IServiceCollection services, IConfiguration configuration)
        {
            var options = new RedisBackplaneSignalROptions();

            var section = configuration.GetSection($"{SignalRConfiguration.SectionName}:{SignalRConfiguration.RedisBackplane}");
            section.Bind(options);

            services.AddOptions<RedisBackplaneSignalROptions>().Bind(section).ValidateDataAnnotations();

            var redisConnectionString = configuration.GetConnectionString(RedisConfiguration.ConnectionStringName);
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                builder.AddStackExchangeRedis(redisConnectionString, o => o.Configuration.ChannelPrefix = options.ChannelName ?? "VirtoCommerceChannel");
            }
        }
    }
}
