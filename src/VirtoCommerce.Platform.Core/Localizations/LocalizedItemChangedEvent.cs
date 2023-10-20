using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Localizations;

public class LocalizedItemChangedEvent : GenericChangedEntryEvent<LocalizedItem>
{
    public LocalizedItemChangedEvent(IEnumerable<GenericChangedEntry<LocalizedItem>> changedEntries)
        : base(changedEntries)
    {
    }
}
