using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security;

public class VirtoSignInManager : SignInManager<ApplicationUser>
{
    private readonly ITwoFactorSignInService _twoFactorSignInService;

    public VirtoSignInManager(
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUser>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation,
        ITwoFactorSignInService twoFactorSignInService)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _twoFactorSignInService = twoFactorSignInService;
    }

    public override Task<bool> IsTwoFactorEnabledAsync(ApplicationUser user)
    {
        return _twoFactorSignInService.IsTwoFactorRequiredAsync(user);
    }
}
