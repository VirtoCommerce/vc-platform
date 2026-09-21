using System.Threading.Tasks;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security.TokenGrants
{
    /// <summary>
    /// Lets a module take over a custom OAuth grant type at "/connect/token". Register with
    /// <c>services.AddTransient&lt;ITokenGrantHandler, MyHandler&gt;()</c>; the platform picks the
    /// one whose <see cref="GrantType"/> matches the request.
    /// </summary>
    public interface ITokenGrantHandler
    {
        /// <summary>
        /// The "grant_type" value this handler takes over.
        /// </summary>
        string GrantType { get; }

        /// <summary>
        /// Handles the token request (<see cref="TokenRequestContext.Request"/>) and returns a signed-in
        /// principal or an error.
        /// </summary>
        Task<TokenGrantResult> HandleAsync(TokenRequestContext context);
    }
}
