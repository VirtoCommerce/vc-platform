using VirtoCommerce.Platform.Core.Events;

namespace VirtoCommerce.Platform.Core.Security.Events
{
    public class UserPasswordHashedEvent : DomainEvent
    {
        public UserPasswordHashedEvent(string userId, string hashedPassword)
        {
            UserId = userId;
            HashedPassword = hashedPassword;
        }

        public string UserId { get; set; }
        public string HashedPassword { get; set; }
    }
}
