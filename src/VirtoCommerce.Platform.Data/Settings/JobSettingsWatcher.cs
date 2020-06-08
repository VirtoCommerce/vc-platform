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
        private readonly Dictionary<string, List<Expression<Func<Task>>>> _handlers = new Dictionary<string, List<Expression<Func<Task>>>>();

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
                if (_handlers.TryGetValue(changedEntry.NewEntry.Name, out var settingSubscriptions))
                {
                    foreach (var handler in settingSubscriptions)
                    {
                        var func = handler.Compile();
                        await func();
                    }
                }
            }
        }

        public void WatchJobSetting(SettingDescriptor enablerSetting, SettingDescriptor cronSetting, string jobId, Expression<Func<Task>> methodCall)
        {
            Expression<Func<Task>> handler = () => RunJob(enablerSetting, cronSetting, jobId, methodCall);
            RegisterHandler(enablerSetting.Name, handler);
            RegisterHandler(cronSetting.Name, handler);
        }


        private void RegisterHandler(string settingName, Expression<Func<Task>> handler)
        {
            if (_handlers.TryGetValue(settingName, out var settingSubscriptions))
            {
                settingSubscriptions.Add(handler);
            }
            else
            {
                _handlers.Add(settingName, new List<Expression<Func<Task>>> { handler });
            }
        }

        private async Task RunJob(SettingDescriptor enablerSetting, SettingDescriptor cronSetting, string jobId, Expression<Func<Task>> methodCall)
        {
            var processJobEnable = await _settingsManager.GetValueAsync(enablerSetting.Name, (bool)enablerSetting.DefaultValue);
            if (processJobEnable)
            {
                RecurringJob.RemoveIfExists(jobId);
                var cronExpression = await _settingsManager.GetValueAsync(cronSetting.Name, cronSetting.DefaultValue.ToString());
                RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);
            }
            else
            {
                RecurringJob.RemoveIfExists(jobId);
            }
        }
    }
}
