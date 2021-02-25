using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserVerificationEmailEvent : DomainEvent
    {
        public ApplicationUser ApplicationUser { get; set; }

        public UserVerificationEmailEvent(ApplicationUser applicationUser)
        {
            ApplicationUser = applicationUser;
        }
    }
}
