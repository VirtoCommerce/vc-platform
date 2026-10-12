using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public interface ITwoFactorSignInHandler
{
    Task<ActionResult> ChallengeAsync(TokenRequestContext context);
}
