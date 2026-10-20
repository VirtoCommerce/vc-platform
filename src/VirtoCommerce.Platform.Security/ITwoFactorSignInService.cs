using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security;

public interface ITwoFactorSignInService
{
    bool IsTwoFactorAvailable { get; }

    Task<bool> IsTwoFactorRequiredAsync(ApplicationUser user);

    Task<ActionResult> ChallengeAsync(TokenRequestContext context);
}
