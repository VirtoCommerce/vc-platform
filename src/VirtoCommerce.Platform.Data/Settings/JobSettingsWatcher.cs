using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class JobSettingsWatcher : IEventHandler<ObjectSettingChangedEvent>
    {
        private Dictionary<SettingDescriptor, Func<SettingDescriptor, ObjectSettingEntry>> _settingDescriptors = new Dictionary<SettingDescriptor, Func<SettingDescriptor, ObjectSettingEntry>>();

        public Task Handle(ObjectSettingChangedEvent message)
        {
            //foreach (var changedEntry in message.ChangedEntries)
            //{
            //    if (settingDescriptors.TryGetValue(changedEntry.NewEntry.Name, out  ))
            //}
            
            return Task.CompletedTask;
        }

        public void WatchJobSetting(SettingDescriptor settingDescriptor)
        {
            if (_settingDescriptors.TryGetValue(settingDescriptor, out var settings))
            {
                //settings.
            }
        }
    }
}
