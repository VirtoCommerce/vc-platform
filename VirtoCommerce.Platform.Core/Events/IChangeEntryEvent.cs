using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Events
{
    public interface IChangeEntryEvent<T> : IEvent
    {
        IEnumerable<ChangedEntry<T>> ChangedEntries { get; }
    }
}
