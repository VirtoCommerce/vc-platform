using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using VirtoCommerce.Platform.Modules;

namespace VirtoCommerce.Platform.Web.Infrastructure.HealthCheck
{
    public sealed class ModulesHealthChecker : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var errorsDictionary = ModuleRegistry.GetFailedModules()
                .ToDictionary(
                    x => x.Id,
                    x => new ModulesHealthReportRecord
                    {
                        Title = x.Title,
                        Version = x.Version.ToString(),
                        Errors = x.Errors.ToArray()
                    } as object);

            if (errorsDictionary.Count != 0)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, "Some modules have errors", exception: null, errorsDictionary));
            }

            return Task.FromResult(HealthCheckResult.Healthy("All modules are loaded"));
        }
    }
}
