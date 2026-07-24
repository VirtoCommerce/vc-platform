using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Jobs;

namespace VirtoCommerce.Platform.Web.Jobs
{
    /// <summary>
    /// Minimal <see cref="IJobExecutionContext"/> for running a job handler inline (e.g. the bootstrap
    /// auto-install path) without a background-job engine. Progress is a no-op; module-management handlers report
    /// progress through their own <c>ModulePushNotification</c>.
    /// </summary>
    internal sealed class InlineJobExecutionContext : IJobExecutionContext
    {
        public string JobId => string.Empty;

        public IJobProgress Progress { get; } = new NoOpJobProgress();

        public IReadOnlyDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        private sealed class NoOpJobProgress : IJobProgress
        {
            public Task Report(JobProgressInfo progress, CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
    }
}
