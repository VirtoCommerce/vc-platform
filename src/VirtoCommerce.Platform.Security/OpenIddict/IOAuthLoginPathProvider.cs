using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// Chooses the sign-in page of an OAuth authorization request by the host it arrives on. Replace the registration,
/// or derive from <see cref="OAuthLoginPathProvider"/>, to take the hosts from data, such as the stores' URLs,
/// instead of the configuration.
/// </summary>
public interface IOAuthLoginPathProvider
{
    /// <summary>
    /// The local path of the front-end's sign-in page on <paramref name="host"/>, which establishes the browser
    /// session through /connect/session, or null for the built-in Platform login page.
    /// </summary>
    Task<string> GetLoginPathAsync(string host);
}
