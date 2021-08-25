using System.Linq;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Core.Extensions
{
    public static class IdentityResultExtensions
    {
        public static SecurityResult ToSecurityResult(this IdentityResult identityResult)
        {
            return new SecurityResult()
            {
                Succeeded = identityResult.Succeeded,
                Errors = identityResult.Errors.Select(x => x.Description)
            };
        }

        public static SecurityResult CreateUserForbiddenEditResult()
        {
            var result = new SecurityResult();

#if DEBUG
            result.Errors = new[] { "It is forbidden to edit this user." };
#endif

            return result;
        }

        public static SecurityResult CreateUserNotFoundResult()
        {
            var result = new SecurityResult();

#if DEBUG
            result.Errors = new[] { "User not found." };
#endif

            return result;
        }
    }
}
