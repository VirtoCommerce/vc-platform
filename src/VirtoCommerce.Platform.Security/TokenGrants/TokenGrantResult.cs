using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security.TokenGrants
{
    /// <summary>
    /// The outcome of an <see cref="IGrantTypeHandler"/>: a signed-in principal or an error. Carries only
    /// data, no ASP.NET Core MVC types, so handlers stay free of a web-layer dependency.
    /// </summary>
    public class TokenGrantResult
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
    }
}
