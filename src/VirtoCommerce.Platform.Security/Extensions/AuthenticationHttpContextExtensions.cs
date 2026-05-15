using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation.AspNetCore;

namespace VirtoCommerce.Platform.Security.Extensions;

public static class AuthenticationHttpContextExtensions
{
    /// <summary>
    /// Authenticate the current request using the token authentication scheme (OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme).
    /// For manual request authentication.
    /// </summary>
    public static Task<AuthenticateResult> AuthenticateTokenAsync(this HttpContext context) =>
        context.AuthenticateAsync(scheme: OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
}
