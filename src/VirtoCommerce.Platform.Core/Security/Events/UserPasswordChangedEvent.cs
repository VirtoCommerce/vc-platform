using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    /// <summary>
    /// This event is published when a user's password is changed for any reason, including when the user changes or resets the password.
    /// </summary>
    public class UserPasswordChangedEvent : DomainEvent
    {
        public UserPasswordChangedEvent(string userId, string customPasswordHash)
        {
            UserId = userId;
            CustomPasswordHash = customPasswordHash;
        }

        public UserPasswordChangedEvent(ApplicationUser applicationUser)
        {
            UserId = applicationUser.Id;
            CustomPasswordHash = applicationUser.PasswordHash;
        }

        public string UserId { get; set; }

        /// <summary>
        /// Password hash for external hash storage. This provided as workaround until password hash storage is implemented
        /// </summary>         
        public string CustomPasswordHash { get; set; }
    }
}
