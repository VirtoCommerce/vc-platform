using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Localizations;

public class LocalizedItemChangingEvent : GenericChangedEntryEvent<LocalizedItem>
{
    public LocalizedItemChangingEvent(IEnumerable<GenericChangedEntry<LocalizedItem>> changedEntries)
        : base(changedEntries)
    {
    }
}
