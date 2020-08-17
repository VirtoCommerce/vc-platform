using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Security.Events;

namespace VirtoCommerce.Platform.Web.Security
{
    public class CustomPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        private readonly IUserPasswordHasher _userPasswordHasher;
        private readonly IEventPublisher _eventPublisher;

        public CustomPasswordHasher(IUserPasswordHasher userPasswordHasher, IEventPublisher eventPublisher)
        {
            _userPasswordHasher = userPasswordHasher;
            _eventPublisher = eventPublisher;
        }

        public string HashPassword(ApplicationUser user, string password)
        {
            string result = _userPasswordHasher.HashPassword(user, password);

            _eventPublisher.Publish(new UserPasswordHashedEvent(user.Id, result));

            return result;
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            return _userPasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
