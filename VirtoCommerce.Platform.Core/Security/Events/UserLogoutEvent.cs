using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserLogoutEvent : DomainEvent
    {
        public UserLogoutEvent(ApplicationUserExtended user)
        {
            User = user;
        }

        public ApplicationUserExtended User { get; set; }
    }
}
