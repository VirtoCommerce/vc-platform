#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// A recurring-job registration captured by
/// <c>AddRecurringJob</c> and registered as a
/// singleton. The background-job engine's scheduler discovers all registrations via
/// <c>IEnumerable&lt;RecurringJobRegistration&gt;</c>, computes occurrences from the cron, and on each occurrence
/// invokes <see cref="Trigger"/> (which enqueues the payload through <see cref="IBackgroundJob"/>).
/// </summary>
public sealed record RecurringJobRegistration
{
    /// <summary>Stable, unique recurring-job id (de-duplication/lock key).</summary>
    public required string Id { get; init; }

    /// <summary>Fixed cron expression, or null when the schedule is setting-driven (see <see cref="CronSetting"/>).</summary>
    public string? CronExpression { get; init; }

    /// <summary>Enabler setting for a setting-driven schedule, or null for a fixed cron.</summary>
    public SettingDescriptor? EnablerSetting { get; init; }

    /// <summary>Cron setting for a setting-driven schedule, or null for a fixed cron.</summary>
    public SettingDescriptor? CronSetting { get; init; }

    /// <summary>Time zone the cron is evaluated in.</summary>
    public TimeZoneInfo TimeZone { get; init; } = TimeZoneInfo.Utc;

    /// <summary>Enqueues a fresh payload instance through <see cref="IBackgroundJob"/>; invoked once per due occurrence.</summary>
    public required Func<IBackgroundJob, CancellationToken, Task> Trigger { get; init; }
}
