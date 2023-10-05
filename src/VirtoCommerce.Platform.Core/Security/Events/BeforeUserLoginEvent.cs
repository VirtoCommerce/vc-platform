using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class BeforeUserLoginEvent : DomainEvent
    {
        public BeforeUserLoginEvent(ApplicationUser user)
        {
            User = user;
        }

        public BeforeUserLoginEvent(ApplicationUser user, object externalLoginInfo)
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
