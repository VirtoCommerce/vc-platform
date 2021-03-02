using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserRoleAddedEvent :  DomainEvent
    {
        public UserRoleAddedEvent(ApplicationUser user, string role)
        {
            User = user;
            Role = role;
        }

        public ApplicationUser User { get; set; }
        public string Role { get; set; }
    }
}
