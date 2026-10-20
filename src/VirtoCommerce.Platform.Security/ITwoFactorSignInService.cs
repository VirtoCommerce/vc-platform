using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security;

public interface ITwoFactorSignInService
{
    Task<bool> IsTwoFactorRequiredAsync(ApplicationUser user);

    bool IsTwoFactorAvailable { get; }

    Task<ActionResult> ChallengeAsync(TokenRequestContext context);
}
