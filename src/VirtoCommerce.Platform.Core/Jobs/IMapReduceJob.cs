#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Developer-facing facade for fan-out/aggregate jobs: splits work into items that each run as an independent map
/// task in parallel across all workers, then runs the reduce handler once when every item has finished.
/// Engine-agnostic — works on whichever job engine is active. Ships in Platform.Core, so a consumer module can define
/// and enqueue map/reduce jobs without a compile-time dependency on the Background Jobs module.
/// </summary>
public interface IMapReduceJob
{
    /// <summary>
    /// Enqueue a map/reduce batch for a SPECIFIC handler pair: each item runs through
    /// <typeparamref name="TMap"/> in parallel, then <typeparamref name="TReduce"/> runs once with all results plus the
    /// supplied <paramref name="state"/>. Returns the batch id. Naming the handlers makes the enqueue self-documenting
    /// and lets several map/reduce handler pairs share one item/result/state set. The item and state types are derived
    /// from the handlers' interfaces (<see cref="IMapJobHandler{TItem, TResult}"/> /
    /// <see cref="IReduceJobHandler{TState, TResult}"/>) and validated against <paramref name="items"/>/<paramref name="state"/>
    /// at enqueue time; the handlers must also agree on the result type, otherwise this throws.
    /// </summary>
    /// <typeparam name="TMap">The map handler — an <see cref="IMapJobHandler{TItem, TResult}"/>.</typeparam>
    /// <typeparam name="TReduce">The reduce handler — an <see cref="IReduceJobHandler{TState, TResult}"/>.</typeparam>
    /// <param name="items">
    /// The work split into independent units — the engine runs <b>one map task per item</b>, in parallel across all
    /// workers. Partition coarsely (e.g. a page of ids, not a single document) so the task count and each result stay
    /// small; this is the fan-out width. Each item's runtime type must match the map handler's item contract.
    /// </param>
    /// <param name="state">
    /// Shared, read-only context passed to the <b>reduce</b> step once (NOT to the map handler). Carry batch-level
    /// info the reducer needs — what was processed, a start timestamp, an index alias to swap, a correlation id, etc.
    /// </param>
    /// <param name="options">Queue, failure policy, and progress settings. See <see cref="MapReduceOptions"/>.</param>
    /// <param name="cancellationToken">Cancels enqueuing the batch (not the running jobs).</param>
    Task<string> Enqueue<TMap, TReduce>(
        IEnumerable<object> items,
        object state,
        MapReduceOptions? options = null,
        CancellationToken cancellationToken = default)
        where TMap : class
        where TReduce : class;
}
