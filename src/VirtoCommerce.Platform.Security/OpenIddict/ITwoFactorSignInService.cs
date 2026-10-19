using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public interface ITwoFactorSignInService
{
    bool IsAvailable { get; }

    Task<ActionResult> ChallengeAsync(TokenRequestContext context);
}
