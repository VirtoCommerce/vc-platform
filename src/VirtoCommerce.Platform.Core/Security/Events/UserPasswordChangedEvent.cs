using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserPasswordChangedEvent : DomainEvent
    {
        public UserPasswordChangedEvent(string userId, string сustomPasswordHash)
        {
            UserId = userId;
            CustomPasswordHash = сustomPasswordHash;
        }

        public UserPasswordChangedEvent(ApplicationUser applicationUser)
        {
            UserId = applicationUser.Id;
            CustomPasswordHash = applicationUser.PasswordHash;
        }

        public string UserId { get; set; }

        /// <summary>
        /// Password hash for external hash storage. This provided as workaround until password hash storage would implemented
        /// </summary>         
        public string CustomPasswordHash { get; set; }
    }
}
