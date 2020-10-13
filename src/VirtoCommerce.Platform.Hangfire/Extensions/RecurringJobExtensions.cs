using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;

namespace VirtoCommerce.Platform.Hangfire.Extensions
{
    public static class RecurringJobExtensions
    {
        //key is the observed setting name, value is the object of setting job
        private static ConcurrentDictionary<string, SettingCronJob> _observedSettingsDict = new ConcurrentDictionary<string, SettingCronJob>();

        /// <summary>
        /// use SettingCronJobBuilder for preparing SettingCronJob
        /// </summary>
        /// <param name="recurringJobManager"></param>
        /// <param name="settingsManager"></param>
        /// <param name="settingCronJob"></param>
        /// <returns></returns>
        public static void WatchJobSetting(this IRecurringJobManager recurringJobManager,
            ISettingsManager settingsManager,
            SettingCronJob settingCronJob)
        {
            recurringJobManager.WatchJobSettingAsync(settingsManager, settingCronJob).GetAwaiter().GetResult();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recurringJobManager"></param>
        /// <param name="settingsManager"></param>
        /// <param name="enablerSetting"></param>
        /// <param name="cronSetting"></param>
        /// <param name="methodCall"></param>
        /// <param name="jobId"></param>
        /// <param name="queue"></param>
        public static void WatchJobSetting<T>(this IRecurringJobManager recurringJobManager,
            ISettingsManager settingsManager,
            SettingDescriptor enablerSetting,
            SettingDescriptor cronSetting,
            Expression<Func<T, Task>> methodCall,
            string jobId,
            TimeZoneInfo timeZoneInfo,
            string queue)
        {
            if (recurringJobManager == null)
            {
                throw new ArgumentNullException(nameof(recurringJobManager));
            }
            if (settingsManager == null)
            {
                throw new ArgumentNullException(nameof(settingsManager));
            }

            var settingCronJob = new SettingCronJobBuilder(new SettingCronJob())
                .SetEnablerSetting(enablerSetting)
                .SetCronSetting(cronSetting)
                .SetJobId(jobId)
                .SetQueueName(queue)
                .SetTimeZoneInfo(timeZoneInfo)
                .ToJob(methodCall)
                .Build();

            recurringJobManager.WatchJobSettingAsync(settingsManager, settingCronJob).GetAwaiter().GetResult();
        }

        /// <summary>
        /// use SettingCronJobBuilder for preparing SettingCronJob
        /// </summary>
        /// <param name="recurringJobManager"></param>
        /// <param name="settingsManager"></param>
        /// <param name="settingCronJob"></param>
        /// <returns></returns>
        public static Task WatchJobSettingAsync(this IRecurringJobManager recurringJobManager,
            ISettingsManager settingsManager,
            SettingCronJob settingCronJob)
        {
            if (recurringJobManager == null)
            {
                throw new ArgumentNullException(nameof(recurringJobManager));
            }
            if (settingsManager == null)
            {
                throw new ArgumentNullException(nameof(settingsManager));
            }

            _observedSettingsDict.AddOrUpdate(settingCronJob.EnableSetting.Name, settingCronJob, (settingName, сronJob) => settingCronJob);
            _observedSettingsDict.AddOrUpdate(settingCronJob.CronSetting.Name, settingCronJob, (settingName, сronJob) => settingCronJob);

            return recurringJobManager.RunOrRemoveJobAsync(settingsManager, settingCronJob);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="recurringJobManager"></param>
        /// <param name="settingsManager"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Task HandleSettingChangeAsync(this IRecurringJobManager recurringJobManager, ISettingsManager settingsManager, ObjectSettingChangedEvent message)
        {
            if (recurringJobManager == null)
            {
                throw new ArgumentNullException(nameof(recurringJobManager));
            }
            if (settingsManager == null)
            {
                throw new ArgumentNullException(nameof(settingsManager));
            }
            return recurringJobManager.HandleSettingChangeAsyncIntnl(settingsManager, message);
        }

        private static async Task HandleSettingChangeAsyncIntnl(this IRecurringJobManager recurringJobManager, ISettingsManager settingsManager, ObjectSettingChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries.Where(x => x.EntryState == EntryState.Modified
                                              || x.EntryState == EntryState.Added))
            {
                if (_observedSettingsDict.TryGetValue(changedEntry.NewEntry.Name, out var settingCronJob))
                {
                    await recurringJobManager.RunOrRemoveJobAsync(settingsManager, settingCronJob);
                }
            }
        }

        private static async Task RunOrRemoveJobAsync(this IRecurringJobManager recurringJobManager, ISettingsManager settingsManager, SettingCronJob settingCronJob)
        {
            var processJobEnable = await settingsManager.GetValueAsync(settingCronJob.EnableSetting.Name, (bool)settingCronJob.EnableSetting.DefaultValue);
            if (processJobEnable)
            {
                var cronExpression = await settingsManager.GetValueAsync(settingCronJob.CronSetting.Name, settingCronJob.CronSetting.DefaultValue.ToString());
                recurringJobManager.AddOrUpdate(
                    settingCronJob.RecurringJobId,
                    settingCronJob.Job,
                    cronExpression,
                    settingCronJob.TimeZone,
                    settingCronJob.Queue);
            }
            else
            {
                recurringJobManager.RemoveIfExists(settingCronJob.RecurringJobId);
            }
        }
    }
}
