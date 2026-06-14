using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Core.Jobs;

/// <summary>
/// Fallback <see cref="IRecurringJobService"/> registered by the platform (via <c>TryAdd</c>) when no
/// background-job engine module is installed.
/// <para>
/// Recurring jobs are registered during application startup. Unlike <see cref="NoEngineBackgroundJobProcessor"/>,
/// this fallback must NOT throw (that would crash boot); instead it logs a warning and no-ops, so the platform
/// starts normally and simply schedules nothing until an engine module is installed. When an engine module IS
/// installed, its real registration wins and this fallback is never used.
/// </para>
/// </summary>
public sealed class NoEngineRecurringJobService : IRecurringJobService
{
    private readonly ILogger<NoEngineRecurringJobService> _logger;

    public NoEngineRecurringJobService(ILogger<NoEngineRecurringJobService> logger)
    {
        _logger = logger;
    }

    public void WatchJobSetting<T>(
        SettingDescriptor enablerSetting,
        SettingDescriptor cronSetting,
        Expression<Func<T, Task>> methodCall,
        string jobId = null,
        TimeZoneInfo timeZone = null,
        string queue = null)
    {
        Warn(jobId ?? typeof(T).Name);
    }

    public void AddOrUpdate<T>(
        string recurringJobId,
        Expression<Func<T, Task>> methodCall,
        string cronExpression,
        TimeZoneInfo timeZone = null,
        string queue = null)
    {
        Warn(recurringJobId);
    }

    public void RemoveIfExists(string recurringJobId)
    {
        // No engine, nothing to remove.
    }

    private void Warn(string jobId)
    {
        _logger.LogWarning(
            "Recurring job '{JobId}' was not scheduled: {Message}",
            jobId,
            BackgroundJobEngineNotInstalledException.DefaultMessage);
    }
}
