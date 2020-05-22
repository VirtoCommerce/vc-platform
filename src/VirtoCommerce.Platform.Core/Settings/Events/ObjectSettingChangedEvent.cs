using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Settings.Events
{
    public class ObjectSettingChangedEvent : GenericChangedEntryEvent<ObjectSettingEntry>
    {
        public ObjectSettingChangedEvent(IEnumerable<GenericChangedEntry<ObjectSettingEntry>> changedEntries) : base(changedEntries)
        {
        }
    }
}
