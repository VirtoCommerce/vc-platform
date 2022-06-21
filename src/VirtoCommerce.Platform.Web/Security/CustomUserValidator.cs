using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Web.Security
{
    public class CustomUserValidator : IUserValidator<ApplicationUser>
    {
        private const string ValidationErrorMessage =
            "User name can be either a valid email or must contain only alphanumeric characters and symbols '_' and '.'. " +
            "Symbols cannot be at the beginig or the end of the name or be next to each other. " +
            "User name length must be from 3 to 128.";

        private readonly Regex _userNameRegex =
            new (@"^(?=.{3,128}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
        
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            if (user.UserName.IsValidEmail())
            {
                return Task.FromResult(IdentityResult.Success);
            }

            if (!_userNameRegex.IsMatch(user.UserName))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError
                {
                    Code = "Incorrect username",
                    Description = ValidationErrorMessage
                }));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
