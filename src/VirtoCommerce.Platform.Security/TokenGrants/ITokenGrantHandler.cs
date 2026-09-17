using System.Threading.Tasks;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Security.OpenIddict;

namespace VirtoCommerce.Platform.Security.TokenGrants
{
    /// <summary>
    /// Lets a module take over a custom OAuth grant type at "/connect/token". Register with a keyed
    /// DI registration whose key is <see cref="GrantType"/> (e.g.
    /// <c>services.AddKeyedTransient&lt;ITokenGrantHandler, MyHandler&gt;("my_grant_type")</c>) - the
    /// platform resolves the one matching handler without constructing any others.
    /// </summary>
    public interface ITokenGrantHandler
    {
        /// <summary>
        /// The "grant_type" value this handler takes over. Must match the key it's registered under.
        /// </summary>
        string GrantType { get; }

        /// <summary>
        /// Handles the token request and returns a signed-in principal or an error.
        /// </summary>
        Task<TokenGrantResult> HandleAsync(OpenIddictRequest request, TokenRequestContext context);
    }
}
