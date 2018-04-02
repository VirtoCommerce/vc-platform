using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Events
{
    public class ChangedEntryEvent<T> : DomainEvent, IChangedEntryEvent<T>
    {
        public ChangedEntryEvent(IEnumerable<ChangedEntry<T>> changedEntries)
        {
            ChangedEntries = changedEntries;
        }

        public IEnumerable<ChangedEntry<T>> ChangedEntries { get; private set; }
    }
}
