#nullable enable
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// A background-job handler. Implement this for a payload type and register it with
/// <c>services.AddBackgroundJob&lt;TPayload, THandler&gt;()</c>. Resolved from DI by the active engine; open for
/// override (last DI registration wins).
/// </summary>
/// <typeparam name="TPayload">The serializable job payload (create via <c>AbstractTypeFactory</c> to stay overridable).</typeparam>
public interface IBackgroundJobHandler<in TPayload> where TPayload : class
{
    Task Execute(TPayload payload, IJobExecutionContext context, CancellationToken cancellationToken = default);
}
