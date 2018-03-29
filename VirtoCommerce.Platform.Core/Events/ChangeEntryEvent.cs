using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Events
{
    public class ChangeEntryEvent<T> : DomainEvent, IChangeEntryEvent<T>
    {
        public ChangeEntryEvent(IEnumerable<ChangedEntry<T>> changedEntries)
        {
            ChangedEntries = changedEntries;
        }

        public IEnumerable<ChangedEntry<T>> ChangedEntries { get; private set; }
    }
}
