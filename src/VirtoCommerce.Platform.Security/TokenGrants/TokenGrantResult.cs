using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security.TokenGrants
{
    /// <summary>
    /// The outcome of an <see cref="ITokenGrantHandler"/>: a signed-in principal or an error. The
    /// caller (<c>AuthorizationController</c>) is responsible for turning this into an HTTP response.
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
