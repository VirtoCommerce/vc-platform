using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Jobs;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class JobSettingsWatcher : IEventHandler<ObjectSettingChangedEvent>
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IRecurringJobManager _recurringJobManager;

        public JobSettingsWatcher(ISettingsManager settingsManager, IRecurringJobManager recurringJobManager)
        {
            _settingsManager = settingsManager;
            _recurringJobManager = recurringJobManager;
        }
        

        public async Task Handle(ObjectSettingChangedEvent message)
        {
            await _recurringJobManager.HandleSettingChange(_settingsManager, message);
        }

        public void WatchJobSetting(SettingCronJob settingCronJob)
        {
            WatchJobSettingAsync(settingCronJob).GetAwaiter().GetResult();
        }

        public Task WatchJobSettingAsync(SettingCronJob settingCronJob)
        {
            return _recurringJobManager.WatchJobSettingAsync(_settingsManager, settingCronJob);
        }

        public void WatchJobSetting<T>(SettingDescriptor enablerSetting,
            SettingDescriptor cronSetting,
            Expression<Func<T, Task>> methodCall,
            string jobId,
            TimeZoneInfo timeZoneInfo,
            string queue)
        {
            _recurringJobManager.WatchJobSetting(_settingsManager, enablerSetting, cronSetting, methodCall, jobId, timeZoneInfo, queue);
        }
    }
}
