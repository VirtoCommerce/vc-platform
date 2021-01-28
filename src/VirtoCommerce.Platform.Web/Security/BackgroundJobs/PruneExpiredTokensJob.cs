using System.Threading.Tasks;
using Hangfire;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;

namespace VirtoCommerce.Platform.Web.Security.BackgroundJobs
{
    /// <summary>
    /// Periodic job for prune expired/invalid authorization tokens
    /// </summary>
    public class PruneExpiredTokensJob
    {
        private readonly OpenIddictTokenManager<OpenIddictToken> _openIddictTokenManager;
        private readonly OpenIddictAuthorizationManager<OpenIddictAuthorization> _openIddictAuthorizationManager;

        public PruneExpiredTokensJob(OpenIddictTokenManager<OpenIddictToken> openIddictTokenManager, OpenIddictAuthorizationManager<OpenIddictAuthorization> openIddictAuthorizationManager)
        {
            _openIddictTokenManager = openIddictTokenManager;
            _openIddictAuthorizationManager = openIddictAuthorizationManager;
        }

        [DisableConcurrentExecution(10)]
        // "DisableConcurrentExecutionAttribute" prevents to start simultaneous job payloads.
        // Should have short timeout, because this attribute implemented by following manner: newly started job falls into "processing" state immediately.
        // Then it tries to receive job lock during timeout. If the lock received, the job starts payload.
        // When the job is awaiting desired timeout for lock release, it stucks in "processing" anyway. (Therefore, you should not to set long timeouts (like 24*60*60), this will cause a lot of stucked jobs and performance degradation.)
        // Then, if timeout is over and the lock NOT acquired, the job falls into "scheduled" state (this is default fail-retry scenario).
        // Failed job goes to "Failed" state (by default) after retries exhausted.
        public async Task Process()
        {
            await _openIddictTokenManager.PruneAsync();
            await _openIddictAuthorizationManager.PruneAsync();            
        }
    }
}
