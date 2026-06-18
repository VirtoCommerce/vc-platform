using System;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Core;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Security.Model.OpenIddict;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    /// <summary>
    /// Periodic job for prune expired/invalid authorization tokens. Runs as an engine-agnostic message-based
    /// recurring job via <see cref="Execute"/>; the parameterless <see cref="Process"/> is retained for direct use.
    /// </summary>
    public class PruneExpiredTokensJob : IBackgroundJobHandler<PruneExpiredTokensJobPayload>
    {
        private readonly OpenIddictTokenManager<VirtoOpenIddictEntityFrameworkCoreToken> _openIddictTokenManager;
        private readonly OpenIddictAuthorizationManager<VirtoOpenIddictEntityFrameworkCoreAuthorization> _openIddictAuthorizationManager;

        public PruneExpiredTokensJob(OpenIddictTokenManager<VirtoOpenIddictEntityFrameworkCoreToken> openIddictTokenManager, OpenIddictAuthorizationManager<VirtoOpenIddictEntityFrameworkCoreAuthorization> openIddictAuthorizationManager)
        {
            _openIddictTokenManager = openIddictTokenManager;
            _openIddictAuthorizationManager = openIddictAuthorizationManager;
        }

        // NOTE: concurrency protection (previously Hangfire's [DisableConcurrentExecution(10)]) is now an
        // engine-level concern owned by the background-job engine module. The attribute was removed to keep the
        // platform free of a direct Hangfire dependency; overlap handling will be reintroduced via the
        // recurring-job facade's overlap policy.
        public async Task Process()
        {
            await _openIddictTokenManager.PruneAsync(DateTimeOffset.UtcNow);
            await _openIddictAuthorizationManager.PruneAsync(DateTimeOffset.UtcNow);
        }

        public Task Execute(PruneExpiredTokensJobPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default)
            => Process();
    }
}
