#nullable enable
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Engine-specific scheduler for message-based recurring jobs. The platform applies every recurring job declared
/// via <c>AddRecurringJob</c> to the active
/// implementation (resolving the effective cron from settings first).
/// <para>
/// The active implementation is provided by an installed background-job engine module (e.g. Hangfire uses its
/// native recurring scheduler; RabbitMQ uses an in-process cron scheduler). When no engine module is installed, a
/// no-op fallback is registered that logs a warning, so the platform still starts — recurring jobs simply are not
/// scheduled until an engine is installed.
/// </para>
/// </summary>
public interface IRecurringJobScheduler
{
    /// <summary>Add or update a recurring job with the given (already resolved) cron expression.</summary>
    Task AddOrUpdate(RecurringJobRegistration registration, string cronExpression, CancellationToken cancellationToken = default);

    /// <summary>Remove a recurring job if it exists (e.g. when its enabler setting is turned off).</summary>
    Task Remove(string recurringJobId, CancellationToken cancellationToken = default);
}
