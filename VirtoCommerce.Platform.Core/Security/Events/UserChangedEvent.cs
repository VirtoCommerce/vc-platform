using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserChangedEvent : DomainEvent
    {
        public UserChangedEvent(ChangedEntry<ApplicationUserExtended> changedEntry)
        {
            ChangedEntry = changedEntry;
        }

        public ChangedEntry<ApplicationUserExtended> ChangedEntry { get; set; }
    }
}
