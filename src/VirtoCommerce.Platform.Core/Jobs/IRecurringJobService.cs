using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Engine-agnostic facade for registering recurring (cron) jobs.
/// <para>
/// The active implementation is provided by an installed background-job engine module. When no engine
/// module is installed, a no-op fallback is registered that logs a warning, so the platform still boots
/// without an engine (the recurring job simply is not scheduled until an engine is installed).
/// </para>
/// <para>
/// This is the platform-facing, Hangfire-free contract. It is distinct from the legacy
/// <c>VirtoCommerce.Platform.Hangfire.IRecurringJobService</c> (now shipped by the engine module and kept
/// for binary compatibility with existing modules).
/// </para>
/// </summary>
public interface IRecurringJobService
{
    /// <summary>
    /// Register a recurring job driven by platform settings: an enabler setting (on/off) and a cron setting.
    /// The job is re-registered automatically when either setting changes.
    /// </summary>
    void WatchJobSetting<T>(
        SettingDescriptor enablerSetting,
        SettingDescriptor cronSetting,
        Expression<Func<T, Task>> methodCall,
        string jobId = null,
        TimeZoneInfo timeZone = null,
        string queue = null);

    /// <summary>
    /// Add or update a recurring job with an explicit cron expression.
    /// </summary>
    void AddOrUpdate<T>(
        string recurringJobId,
        Expression<Func<T, Task>> methodCall,
        string cronExpression,
        TimeZoneInfo timeZone = null,
        string queue = null);

    /// <summary>
    /// Remove a recurring job if it exists.
    /// </summary>
    void RemoveIfExists(string recurringJobId);
}
