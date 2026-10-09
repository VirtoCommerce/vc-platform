using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// Starts the second authentication factor for the password grant of the token endpoint.
/// The cookie sign-in uses the ASP.NET Core Identity two-factor flow instead.
/// </summary>
public interface ITwoFactorSignInHandler
{
    /// <summary>
    /// Called instead of issuing tokens when the password and the token request validators have succeeded
    /// and <see cref="Microsoft.AspNetCore.Identity.SignInManager{TUser}.IsTwoFactorEnabledAsync"/> requires a second factor.
    /// Starts the second factor, for example sends a code, and returns the response for the client.
    /// May set <see cref="TokenRequestContext.FailureReason"/> for the sign-in log.
    /// </summary>
    Task<ActionResult> ChallengeAsync(TokenRequestContext context);
}
