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
    /// Reports that the job has ended, so the admin UI stops showing it as running. The job engine calls it when the
    /// job ends, after its final attempt or on a requested cancel; a handler does not call it. It may be called again
    /// for the same job if the engine runs the job again, so an implementation must tolerate a repeated call.
    /// </summary>
    /// <remarks>
    /// The default does nothing, which keeps the behavior of a reporter or an engine that does not close a
    /// notification. A reporter that wraps another one, and is itself the reporter the engine completes, must override
    /// it and forward the call, or the outcome is lost.
    /// </remarks>
    /// <param name="outcome">How the job ended.</param>
    /// <param name="error">The error of a failed job; <c>null</c> otherwise.</param>
    /// <param name="cancellationToken">Cancels sending the report, not the job.</param>
    Task Complete(JobOutcome outcome, string? error = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
