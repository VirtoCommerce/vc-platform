using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Engine-agnostic facade for enqueuing in-process background jobs.
/// <para>
/// The active implementation is provided by an installed background-job engine module
/// (e.g. <c>VirtoCommerce.BackgroundJobs</c> with the Hangfire engine). When no engine module is
/// installed, <see cref="NoEngineBackgroundJobProcessor"/> is registered as a fallback and throws
/// <see cref="BackgroundJobEngineNotInstalledException"/> with actionable installation instructions.
/// </para>
/// </summary>
public interface IBackgroundJobProcessor
{
    /// <summary>
    /// Enqueue a fire-and-forget job expressed as a method call. The job runs in-process on a worker.
    /// </summary>
    /// <returns>The engine-specific job id.</returns>
    string Enqueue(Expression<Action> methodCall);

    /// <summary>
    /// Enqueue a fire-and-forget asynchronous job expressed as a method call.
    /// </summary>
    /// <returns>The engine-specific job id.</returns>
    string Enqueue(Expression<Func<Task>> methodCall);
}
