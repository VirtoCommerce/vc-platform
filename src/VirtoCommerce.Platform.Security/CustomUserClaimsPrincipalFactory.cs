using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security
{
    /// <summary>
    /// Custom UserCalimsPrincipalFactory responds to add claims with system roles based on user properties that can be used for authorization checks
    /// </summary>
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, Role>
    {
        public CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var result = await base.GenerateClaimsAsync(user);
            var userType = EnumUtility.SafeParse(user.UserType, UserType.Customer);
            //need to transform isAdministrator flag and user types into special system roles claims
            if (user.IsAdministrator)
            {
                result.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, PlatformConstants.Security.SystemRoles.Administrator));
            }
            else if (userType == UserType.Customer)
            {
                result.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, PlatformConstants.Security.SystemRoles.Customer));
            }
            else if (userType == UserType.Manager)
            {
                result.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, PlatformConstants.Security.SystemRoles.Manager));
            }
            return result;
        }
    }
}
