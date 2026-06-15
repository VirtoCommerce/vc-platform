#nullable enable
using System;
using System.Linq.Expressions;
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
    /// <summary>Enqueue a fire-and-forget job expressed as a method call. Requires the Hangfire provider.</summary>
    /// <returns>The engine-specific job id.</returns>
    string Enqueue(Expression<Action> methodCall);

    /// <summary>Enqueue a fire-and-forget asynchronous job expressed as a method call. Requires the Hangfire provider.</summary>
    /// <returns>The engine-specific job id.</returns>
    string Enqueue(Expression<Func<Task>> methodCall);

    /// <summary>
    /// Enqueue a message-based job: a serializable <paramref name="payload"/> handled by a registered
    /// <see cref="IBackgroundJobHandler{TPayload}"/>. Works on any active engine (Hangfire, RabbitMQ, …), with or
    /// without progress (see <see cref="EnqueueOptions.ReportProgress"/>).
    /// </summary>
    /// <returns>The engine-specific job id.</returns>
    Task<string> Enqueue<TPayload>(TPayload payload, EnqueueOptions? options = null,
        CancellationToken cancellationToken = default)
        where TPayload : class;
}
