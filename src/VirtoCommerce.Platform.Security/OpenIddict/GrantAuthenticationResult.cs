using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    /// <summary>
    /// The outcome of <see cref="GrantTypeHandlerBase.AuthenticateAsync"/>: the user the grant-specific
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
