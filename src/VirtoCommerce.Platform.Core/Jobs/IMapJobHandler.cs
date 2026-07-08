using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Processes one item of a map/reduce batch and returns its partial result. Runs in parallel with every other item
/// across all workers (on whatever engine is active). Register with
/// <c>services.AddMapReduceJob&lt;TItem, TResult, TState, TMap, TReduce&gt;()</c>.
/// <para>Keep <typeparamref name="TResult"/> small — it is persisted per item until the reduce step runs.</para>
/// </summary>
public interface IMapJobHandler<in TItem, TResult>
    where TItem : class
    where TResult : class
{
    Task<TResult> Map(TItem item, IJobExecutionContext context, CancellationToken cancellationToken = default);
}
