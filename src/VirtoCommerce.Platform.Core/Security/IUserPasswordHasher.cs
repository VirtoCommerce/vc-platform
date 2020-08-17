using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Basic interface for platform password hashers
    /// </summary>
    public interface IUserPasswordHasher
    {
        public string HashPassword(ApplicationUser user, string password);
        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword);
    }
}
