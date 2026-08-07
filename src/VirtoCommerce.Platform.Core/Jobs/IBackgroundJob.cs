#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Developer-facing facade to enqueue background work (engine-agnostic).
/// </summary>
public interface IBackgroundJob
{
    Task<string> Enqueue<THandler>(object payload, EnqueueOptions? options = null,
        CancellationToken cancellationToken = default)
        where THandler : class;

    Task<string> Enqueue(Type handlerType, object payload, EnqueueOptions? options = null,
        CancellationToken cancellationToken = default)
        => throw new NotSupportedException(
            $"This {nameof(IBackgroundJob)} implementation does not support the non-generic Enqueue(Type, ...) overload.");

    Task<bool> Cancel(string jobId, CancellationToken cancellationToken = default) => Task.FromResult(false);

    bool SupportsCancellation => false;
}
