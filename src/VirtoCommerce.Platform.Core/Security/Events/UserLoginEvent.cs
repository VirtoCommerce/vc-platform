using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserLoginEvent : DomainEvent
    {
        public UserLoginEvent(ApplicationUser user)
        {
            User = user;
        }

        public ApplicationUser User { get; set; }
    }
}
