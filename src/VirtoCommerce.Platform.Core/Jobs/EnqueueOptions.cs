#nullable enable
namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>Per-enqueue options for message-based background jobs.</summary>
public sealed record EnqueueOptions
{
    /// <summary>Target queue. Falls back to the configured default queue when null.</summary>
    public string? Queue { get; init; }

    /// <summary>
    /// Friendly title shown on the job's progress notification in the admin UI. Falls back to
    /// <c>"Background job: {PayloadType}"</c> when null.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>Maximum automatic retry attempts on failure. Falls back to the platform default when null.</summary>
    public int? MaxRetryAttempts { get; init; }

    /// <summary>Unique key — re-enqueuing with the same key collapses to a single job.</summary>
    public string? UniqueKey { get; init; }

    /// <summary>When true, the job reports progress to the admin UI over SignalR.</summary>
    public bool ReportProgress { get; init; }

    /// <summary>
    /// Existing push-notification id to report progress against. When null and <see cref="ReportProgress"/> is
    /// true, the engine creates one and returns it via the notification stream.
    /// </summary>
    public string? ProgressNotificationId { get; init; }
}
