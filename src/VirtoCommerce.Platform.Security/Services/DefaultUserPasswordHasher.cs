using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.Services
{
    public class DefaultUserPasswordHasher : PasswordHasher<ApplicationUser>, IUserPasswordHasher
    {
    }
}
