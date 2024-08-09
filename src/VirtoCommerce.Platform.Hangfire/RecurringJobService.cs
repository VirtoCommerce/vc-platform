// This service is functionally identical to the obsolete RecurringJobExtensions
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;
using VirtoCommerce.Platform.Hangfire.Extensions;

namespace VirtoCommerce.Platform.Hangfire;

public class RecurringJobService : IRecurringJobService, IEventHandler<ObjectSettingChangedEvent>
{
    // Key is the observed setting name, value is the object of setting job
    private static readonly ConcurrentDictionary<string, SettingCronJob> _observedSettingsDict = new();

    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ISettingsManager _settingsManager;

    public RecurringJobService(IRecurringJobManager recurringJobManager, ISettingsManager settingsManager)
    {
        _recurringJobManager = recurringJobManager;
        _settingsManager = settingsManager;
    }

    public void WatchJobSetting<T>(
        SettingDescriptor enablerSetting,
        SettingDescriptor cronSetting,
        Expression<Func<T, Task>> methodCall,
        string jobId,
        TimeZoneInfo timeZoneInfo,
        string queue)
    {
        var settingCronJob = new SettingCronJobBuilder(new SettingCronJob())
            .SetEnablerSetting(enablerSetting)
            .SetCronSetting(cronSetting)
            .SetJobId(jobId)
            .SetQueueName(queue)
            .SetTimeZoneInfo(timeZoneInfo)
            .ToJob(methodCall)
            .Build();

        WatchJobSetting(settingCronJob);
    }

    /// <summary>
    /// Use SettingCronJobBuilder for creating SettingCronJob
    /// </summary>
    public void WatchJobSetting(SettingCronJob settingCronJob)
    {
        WatchJobSettingAsync(settingCronJob).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Use SettingCronJobBuilder for creating SettingCronJob
    /// </summary>
    public Task WatchJobSettingAsync(SettingCronJob settingCronJob)
    {
        _observedSettingsDict.AddOrUpdate(settingCronJob.EnableSetting.Name, settingCronJob, (_, _) => settingCronJob);
        _observedSettingsDict.AddOrUpdate(settingCronJob.CronSetting.Name, settingCronJob, (_, _) => settingCronJob);

        return RunOrRemoveJobAsync(settingCronJob);
    }

    public async Task Handle(ObjectSettingChangedEvent message)
    {
        foreach (var settingName in message.ChangedEntries
                     .Where(x => x.EntryState is EntryState.Modified or EntryState.Added)
                     .Select(x => x.NewEntry.Name))
        {
            if (_observedSettingsDict.TryGetValue(settingName, out var settingCronJob))
            {
                await RunOrRemoveJobAsync(settingCronJob);
            }
        }

        // Temporary solution for backward compatibility
#pragma warning disable VC0008 // Type or member is obsolete
        await _recurringJobManager.HandleSettingChangeAsync(_settingsManager, message);
#pragma warning restore VC0008 // Type or member is obsolete
    }

    private async Task RunOrRemoveJobAsync(SettingCronJob settingCronJob)
    {
        var processJobEnableSettingValue = await _settingsManager.GetValueAsync<object>(settingCronJob.EnableSetting);
        var processJobEnable = settingCronJob.EnabledEvaluator(processJobEnableSettingValue);

        if (processJobEnable)
        {
            var cronExpression = await _settingsManager.GetValueAsync<string>(settingCronJob.CronSetting);

            var options = new RecurringJobOptions
            {
                TimeZone = settingCronJob.TimeZone,
#pragma warning disable CS0618 // Type or member is obsolete
                // Remove when Hangfire.MySqlStorage will be updated to support JobStorageFeatures.JobQueueProperty
                QueueName = settingCronJob.Queue,
#pragma warning restore CS0618 // Type or member is obsolete
            };

            _recurringJobManager.AddOrUpdate(
                settingCronJob.RecurringJobId,
                settingCronJob.Job,
                cronExpression,
                options);
        }
        else
        {
            _recurringJobManager.RemoveIfExists(settingCronJob.RecurringJobId);
        }
    }
}
