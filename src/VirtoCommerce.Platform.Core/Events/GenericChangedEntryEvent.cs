using System.Collections.Generic;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.Events
{
    public class GenericChangedEntryEvent<T> : DomainEvent
    {
        [JsonConstructor]
        public GenericChangedEntryEvent(IEnumerable<GenericChangedEntry<T>> changedEntries)
        {
            ChangedEntries = changedEntries;
        }

        public IEnumerable<GenericChangedEntry<T>> ChangedEntries { get; private set; }
    }
}
