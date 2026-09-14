using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.NativeSignIn
{
    /// <summary>
    /// Lets a module complete a sign-in for a user it has already verified itself (e.g. a one-time
    /// code), without pretending to be an external OAuth/OIDC provider. Registered via DI as
    /// <see cref="System.Collections.Generic.IEnumerable{T}"/>; dispatched by the platform's
    /// "native_sign_in" token grant based on the request's "provider" parameter.
    /// </summary>
    public interface INativeSignInProvider
    {
        /// <summary>
        /// Matches the "provider" parameter of a native_sign_in token request (e.g. "OTP").
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Verifies the request's own credentials (whatever they are for this provider) and returns
        /// the user they belong to, or null if the request could not be verified.
        /// </summary>
        Task<ApplicationUser> ValidateAsync(OpenIddictRequest request);
    }
}
