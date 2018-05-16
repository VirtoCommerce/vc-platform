using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserLoginEvent : DomainEvent
    {
        public UserLoginEvent(ApplicationUserExtended user)
        {
            User = user;
        }

        public ApplicationUserExtended User { get; set; }
    }
}
