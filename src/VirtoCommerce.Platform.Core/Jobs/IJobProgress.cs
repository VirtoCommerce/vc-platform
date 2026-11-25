#nullable enable
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Reports background-job progress to the admin UI (SignalR push). A no-op implementation is used when the job
/// was enqueued without progress.
/// </summary>
public interface IJobProgress
{
    Task Report(JobProgressInfo progress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reports how the job ended. Called by the job engine, possibly more than once; the default does nothing, and a
    /// reporter that wraps another one must forward it.
    /// </summary>
    Task Complete(JobOutcome outcome, string? error = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
