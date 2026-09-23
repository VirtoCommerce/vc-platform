using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security.TokenGrants
{
    /// <summary>
    /// The outcome of <see cref="TokenGrantHandlerBase.AuthenticateAsync"/>: the user the grant-specific
    /// credential resolved to, or the error to report for it.
    /// </summary>
    public class GrantAuthenticationResult
    {
        public bool Success => Error == null;

        public ApplicationUser User { get; private init; }

        public TokenResponse Error { get; private init; }

        public static GrantAuthenticationResult Authenticated(ApplicationUser user)
        {
            return new GrantAuthenticationResult
            {
                User = user,
            };
        }

        public static GrantAuthenticationResult Failed(TokenResponse error)
        {
            return new GrantAuthenticationResult
            {
                Error = error,
            };
        }
    }
}
