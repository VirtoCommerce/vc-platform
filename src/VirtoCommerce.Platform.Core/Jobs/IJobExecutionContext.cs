#nullable enable
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>Ambient information handed to a running <see cref="IBackgroundJobHandler{TPayload}"/>.</summary>
public interface IJobExecutionContext
{
    /// <summary>The engine-specific job id.</summary>
    string JobId { get; }

    /// <summary>Progress reporter. A no-op when the job wasn't enqueued with progress.</summary>
    IJobProgress Progress { get; }

    /// <summary>Correlation / trace / tenant headers carried with the job.</summary>
    IReadOnlyDictionary<string, string> Headers { get; }
}
