using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security.TokenGrants
{
    /// <summary>
    /// The outcome of an <see cref="ITokenGrantHandler"/>: a signed-in principal or an error.
    /// Implements <see cref="IActionResult"/> so module code never needs to reference MVC's result types.
    /// </summary>
    public class TokenGrantResult : IActionResult
    {
        public bool Success => Error == null;

        public ClaimsPrincipal Principal { get; private init; }

        public AuthenticationProperties Properties { get; private init; }

        public string AuthenticationScheme { get; private init; }

        public TokenResponse Error { get; private init; }

        public static TokenGrantResult SignedIn(ClaimsPrincipal principal, AuthenticationProperties properties, string authenticationScheme)
        {
            return new TokenGrantResult
            {
                Principal = principal,
                Properties = properties,
                AuthenticationScheme = authenticationScheme,
            };
        }

        public static TokenGrantResult Failed(TokenResponse error)
        {
            return new TokenGrantResult
            {
                Error = error,
            };
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            IActionResult result = Success
                ? new SignInResult(AuthenticationScheme, Principal, Properties)
                : new BadRequestObjectResult(Error);

            return result.ExecuteResultAsync(context);
        }
    }
}
