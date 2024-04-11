using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    /// <summary>
    /// This event is published when a user has changed their password.
    /// </summary>
    public class UserChangedPasswordEvent : DomainEvent
    {
        public UserChangedPasswordEvent(string userId, string customPasswordHash)
        {
            UserId = userId;
            CustomPasswordHash = customPasswordHash;
        }

        public string UserId { get; set; }

        /// <summary>
        /// Password hash for external hash storage. This provided as workaround until password hash storage is implemented
        /// </summary>         
        public string CustomPasswordHash { get; set; }
    }
}
