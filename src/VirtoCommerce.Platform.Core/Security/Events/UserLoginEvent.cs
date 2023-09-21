using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserLoginEvent : DomainEvent
    {
        public UserLoginEvent(ApplicationUser user)
        {
            User = user;
        }

        public UserLoginEvent(ApplicationUser user, object externalLoginInfo)
        {
            User = user;
            ExternalLoginInfo = externalLoginInfo;
        }

        /// <summary>
        /// Gets the user for the current login
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// Gets the external login information for the current login
        /// </summary>
        public object ExternalLoginInfo { get; set; }
    }
}
