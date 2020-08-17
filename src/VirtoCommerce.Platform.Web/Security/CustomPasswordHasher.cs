using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Security
{
    public class CustomPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        private readonly IUserPasswordHasher _userPasswordHasher;

        public CustomPasswordHasher(IUserPasswordHasher userPasswordHasher)
        {
            _userPasswordHasher = userPasswordHasher;
        }

        public string HashPassword(ApplicationUser user, string password)
        {
            string result = _userPasswordHasher.HashPassword(user, password);

            // publish new hash event there

            return result;
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            return _userPasswordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }
    }
}
