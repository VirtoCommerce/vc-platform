using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Web.Infrastructure.HealthCheck
{
    public sealed class CacheHealthChecker : IHealthCheck
    {
        private readonly IPlatformMemoryCache _memoryCache;

        public CacheHealthChecker(IPlatformMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (!(_memoryCache as PlatformMemoryCache)?.CacheEnabled ?? false)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Cache is off"));
            }
            
            return Task.FromResult(HealthCheckResult.Healthy("Cache is active"));
        }
    }
}
