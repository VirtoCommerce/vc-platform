using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserRequestPasswordResetEvent : DomainEvent
    {
        public UserRequestPasswordResetEvent(ApplicationUser user, string callbackUrl)
        {
            User = user;
            CallbackUrl = callbackUrl;
        }

        public ApplicationUser User { get; set; }

        public string CallbackUrl { get; set; }
    }

}
