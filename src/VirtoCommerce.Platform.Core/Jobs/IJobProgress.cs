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
}
