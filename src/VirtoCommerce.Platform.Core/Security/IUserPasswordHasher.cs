using Microsoft.AspNetCore.Identity;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Basic interface for platform password hashers
    /// </summary>
    public interface IUserPasswordHasher : IPasswordHasher<ApplicationUser>
    {
    }
}
