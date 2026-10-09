using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security
{
    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        private readonly IEnumerable<ITwoFactorPolicy> _twoFactorPolicies;

        public CustomSignInManager(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<ApplicationUser> confirmation,
            IEnumerable<ITwoFactorPolicy> twoFactorPolicies)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _twoFactorPolicies = twoFactorPolicies;
        }

        public override async Task<bool> IsTwoFactorEnabledAsync(ApplicationUser user)
        {
            if (await base.IsTwoFactorEnabledAsync(user))
            {
                return true;
            }

            foreach (var policy in _twoFactorPolicies)
            {
                if (await policy.IsTwoFactorRequiredAsync(user))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
