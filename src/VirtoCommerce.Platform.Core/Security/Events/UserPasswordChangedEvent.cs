using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserPasswordChangedEvent : DomainEvent
    {
        public UserPasswordChangedEvent(string userId, string hashedPassword)
        {
            UserId = userId;
            HashedPassword = hashedPassword;
        }

        public string UserId { get; set; }

        /// <summary>
        /// Password hash for external hash storage. This provided as workaround until password hash storage would implemented
        /// </summary>         
        public string HashedPassword { get; set; }
    }
}
