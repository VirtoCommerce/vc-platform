using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Aggregates the results of a map/reduce batch. Runs exactly once, after every map item has reached a terminal
/// state. Receives every item's outcome (including failures when the failure policy is
/// <see cref="FailurePolicy.ContinueOnError"/>), so it can finalize, swap an index alias, notify, or enqueue a
/// targeted re-run of the failed items.
/// </summary>
public interface IReduceJobHandler<in TState, TResult>
    where TState : class
    where TResult : class
{
    Task Reduce(TState state, IReadOnlyCollection<MapResult<TResult>> results, IJobExecutionContext context, CancellationToken cancellationToken = default);
}
