using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Infrastructure.HealthCheck
{
    public sealed class ModulesHealthChecker : IHealthCheck
    {
        private readonly ILocalModuleCatalog _localModuleCatalog;

        public ModulesHealthChecker(ILocalModuleCatalog localModuleCatalog)
        {
            _localModuleCatalog = localModuleCatalog;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var errorsDictionary = _localModuleCatalog.Modules
                .OfType<ManifestModuleInfo>()
                .Where(x => x.Errors.Any())
                .ToDictionary(
                    x => x.Id,
                    x => new ModulesHealthReportRecord
                    {
                        Title = x.Title,
                        Version = x.Version.ToString(),
                        Errors = x.Errors.ToArray()
                    } as object);

            if (errorsDictionary.Any())
            {
                return Task.FromResult(HealthCheckResult.Degraded("Some modules have errors", exception: null, errorsDictionary));
            }

            return Task.FromResult(HealthCheckResult.Healthy("All modules are loaded"));
        }
    }
}
