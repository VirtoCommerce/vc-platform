using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class JobSettingsWatcher : IEventHandler<ObjectSettingChangedEvent>
    {
        private readonly Dictionary<string, List<Expression<Func<Task>>>> _mapHandlers = new Dictionary<string, List<Expression<Func<Task>>>>();

        private readonly ISettingsManager _settingsManager;

        public JobSettingsWatcher(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        public async Task Handle(ObjectSettingChangedEvent message)
        {
            foreach (var changedEntry in message.ChangedEntries.Where(x => x.EntryState == EntryState.Modified
                                              || x.EntryState == EntryState.Added))
            {
                if (_mapHandlers.TryGetValue(changedEntry.NewEntry.Name, out var handlers))
                {
                    foreach (var handler in handlers)
                    {
                        var func = handler.Compile();
                        await func();
                    }
                }
            }
        }

        public void WatchJobSetting<T>(SettingDescriptor enablerSetting, SettingDescriptor cronSetting, Expression<Func<T, Task>> methodCall)
        {
            Expression<Func<Task>> handler = () => RunJob(enablerSetting, cronSetting, methodCall);
            RegisterHandler(enablerSetting.Name, handler);
            RegisterHandler(cronSetting.Name, handler);
        }


        private void RegisterHandler(string settingName, Expression<Func<Task>> handler)
        {
            if (_mapHandlers.TryGetValue(settingName, out var settingSubscriptions))
            {
                settingSubscriptions.Add(handler);
            }
            else
            {
                _mapHandlers.Add(settingName, new List<Expression<Func<Task>>> { handler });
            }
        }

        private async Task RunJob<T>(SettingDescriptor enablerSetting, SettingDescriptor cronSetting, Expression<Func<T, Task>> methodCall)
        {
            var processJobEnable = await _settingsManager.GetValueAsync(enablerSetting.Name, (bool)enablerSetting.DefaultValue);
            if (processJobEnable)
            {
                RecurringJob.RemoveIfExists(nameof(T));
                var cronExpression = await _settingsManager.GetValueAsync(cronSetting.Name, cronSetting.DefaultValue.ToString());
                RecurringJob.AddOrUpdate(methodCall, cronExpression);
            }
            else
            {
                RecurringJob.RemoveIfExists(nameof(T));
            }
        }
    }
}
