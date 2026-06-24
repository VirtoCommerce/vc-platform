#nullable enable
using System;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Fluent configuration for a recurring (cron) job registered via
/// <c>AddRecurringJob</c>.
/// Supply either a fixed cron (<see cref="WithCron"/>) or a setting-driven schedule (<see cref="FromSettings"/>).
/// </summary>
public interface IRecurringJobScheduleBuilder
{
    /// <summary>Stable, unique recurring-job id (used as the de-duplication/lock key). Required.</summary>
    IRecurringJobScheduleBuilder WithId(string recurringJobId);

    /// <summary>Fixed cron expression (5- or 6-field). Mutually exclusive with <see cref="FromSettings"/>.</summary>
    IRecurringJobScheduleBuilder WithCron(string cronExpression);

    /// <summary>
    /// Setting-driven schedule: an enabler setting (on/off) and a cron setting. The schedule re-evaluates
    /// automatically when either setting changes. Mutually exclusive with <see cref="WithCron"/>.
    /// </summary>
    IRecurringJobScheduleBuilder FromSettings(SettingDescriptor enablerSetting, SettingDescriptor cronSetting);

    /// <summary>Target queue for the enqueued payload (falls back to the configured default queue when unset).</summary>
    IRecurringJobScheduleBuilder WithQueue(string queue);

    /// <summary>Time zone the cron expression is evaluated in (defaults to UTC).</summary>
    IRecurringJobScheduleBuilder WithTimeZone(TimeZoneInfo timeZone);
}
