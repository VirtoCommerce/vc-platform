#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Developer-facing facade to enqueue background work (engine-agnostic). The active implementation is provided by
/// an installed background-job engine module (e.g. <c>VirtoCommerce.BackgroundJobs</c>). When no engine is
/// installed, a fallback is registered that throws <see cref="BackgroundJobEngineNotInstalledException"/> with
/// actionable installation instructions.
/// <para>
/// Inject this to enqueue work; implement <see cref="IBackgroundJobHandler{TPayload}"/> to handle a payload.
/// </para>
/// </summary>
public interface IBackgroundJob
{
    /// <summary>
    /// Enqueue a message-based job: <typeparamref name="THandler"/> processes the supplied <paramref name="payload"/>
    /// on the active engine (Hangfire, RabbitMQ, …), with or without progress (see
    /// <see cref="EnqueueOptions.ReportProgress"/>). Naming the handler makes the enqueue self-documenting and lets
    /// several handlers share one payload type. The payload is validated against the handler at enqueue time:
    /// <typeparamref name="THandler"/> must implement <see cref="IBackgroundJobHandler{TPayload}"/> for the payload's
    /// runtime type, otherwise this throws.
    /// </summary>
    /// <typeparam name="THandler">The handler that will run the payload.</typeparam>
    /// <param name="payload">The serializable payload instance; its runtime type selects the handler's payload contract.</param>
    /// <param name="options">Per-enqueue options (queue, title, progress, retries, unique key).</param>
    /// <param name="cancellationToken">Cancels the enqueue operation.</param>
    /// <returns>The engine-specific job id.</returns>
    Task<string> Enqueue<THandler>(object payload, EnqueueOptions? options = null,
        CancellationToken cancellationToken = default)
        where THandler : class;

    /// <summary>
    /// Non-generic overload of <see cref="Enqueue{THandler}"/> for callers that only have the handler
    /// <see cref="Type"/> at runtime — e.g. a REST "run by name" endpoint that resolved a registered
    /// <see cref="BackgroundJobDescriptor"/>. <paramref name="handlerType"/> must be a concrete class implementing
    /// <see cref="IBackgroundJobHandler{TPayload}"/> for the payload's runtime type, otherwise this throws.
    /// </summary>
    /// <param name="handlerType">The concrete handler type that will run the payload.</param>
    /// <param name="payload">The serializable payload instance; its runtime type selects the handler's payload contract.</param>
    /// <param name="options">Per-enqueue options (queue, title, progress, retries, unique key).</param>
    /// <param name="cancellationToken">Cancels the enqueue operation.</param>
    /// <returns>The engine-specific job id.</returns>
    /// <remarks>
    /// A default (throwing) implementation is provided so this member can be added without breaking existing external
    /// implementers of <see cref="IBackgroundJob"/>; the platform's facade overrides it with the real behavior.
    /// </remarks>
    Task<string> Enqueue(Type handlerType, object payload, EnqueueOptions? options = null,
        CancellationToken cancellationToken = default)
        => throw new NotSupportedException(
            $"This {nameof(IBackgroundJob)} implementation does not support the non-generic Enqueue(Type, ...) overload.");
}
