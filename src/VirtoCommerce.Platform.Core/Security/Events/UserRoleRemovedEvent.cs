using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserRoleRemovedEvent :  DomainEvent
    {
        public UserRoleRemovedEvent(ApplicationUser user, string role)
        {
            User = user;
            Role = role;
        }

        public ApplicationUser User { get; set; }
        public string Role { get; set; }
    }
}
