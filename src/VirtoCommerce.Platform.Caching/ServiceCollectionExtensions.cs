using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Redis;

namespace VirtoCommerce.Platform.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");

            services.AddOptions<CachingOptions>().Bind(configuration.GetSection("Caching")).ValidateDataAnnotations();


            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddOptions<RedisCachingOptions>().Bind(configuration.GetSection("Caching:Redis")).ValidateDataAnnotations();

                services.AddSingleton<IPlatformMemoryCache, RedisPlatformMemoryCache>();
            }
            else
            {
                //Use MemoryCache decorator to use global platform cache settings
                services.AddSingleton<IPlatformMemoryCache, PlatformMemoryCache>();
            }

            services.AddScoped<IRequestScopedCache, RequestScopedCache>();

            //Lets consumers whose lifetime is not bounded by one request reach the request's cache without
            //taking IHttpContextAccessor themselves. Singleton: it captures only the accessor, which reads
            //the ambient context from an AsyncLocal. AddHttpContextAccessor is TryAdd-based, so calling it
            //here makes the registration self-sufficient without overriding a host that already added it.
            services.AddHttpContextAccessor();
            services.AddSingleton<IRequestScopedCacheAccessor, HttpRequestScopedCacheAccessor>();

            return services;
        }
    }
}
