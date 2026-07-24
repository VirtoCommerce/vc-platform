#nullable enable
namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>How a map/reduce batch treats map items that fail.</summary>
public enum FailurePolicy
{
    /// <summary>If any item fails, the reduce handler is NOT run; the batch is marked faulted (default).</summary>
    FailFast,

    /// <summary>Failed items are recorded and reduce runs with the full set of results (successes and failures).</summary>
    ContinueOnError,
}

/// <summary>
/// Optional per-batch settings for <see cref="IMapReduceJob.Enqueue{TMap, TReduce}"/>. All are optional;
/// the defaults (default queue, <see cref="FailurePolicy.FailFast"/>, no progress) suit a fire-and-forget batch.
/// </summary>
public sealed class MapReduceOptions
{
    /// <summary>
    /// Queue that BOTH the map tasks and the reduce task run on. Set it to route a large batch to a dedicated worker
    /// pool so it doesn't starve short interactive jobs. Falls back to the configured default queue when null.
    /// </summary>
    public string? Queue { get; set; }

    /// <summary>
    /// How map-item failures are handled. <see cref="FailurePolicy.FailFast"/> (default) marks the batch faulted and
    /// skips reduce if any item fails; <see cref="FailurePolicy.ContinueOnError"/> records failures and runs reduce
    /// with the full result set (so the reducer can surface the failed items for a targeted re-run).
    /// </summary>
    public FailurePolicy FailurePolicy { get; set; } = FailurePolicy.FailFast;

    /// <summary>
    /// When true, the batch reports aggregate progress (completed/total items) to the admin UI on one shared
    /// notification. Set it for long batches an operator is watching; leave off for fire-and-forget work.
    /// </summary>
    public bool ReportProgress { get; set; }
}
