using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// The login paths of the configuration: Authorization:OAuthLoginPaths for the hosts it lists, and
/// Authorization:OAuthLoginPath for every other host.
/// </summary>
public class OAuthLoginPathProvider : IOAuthLoginPathProvider
{
    private readonly AuthorizationOptions _options;

    public OAuthLoginPathProvider(IOptions<AuthorizationOptions> options)
    {
        _options = options.Value;
    }

    public virtual Task<string> GetLoginPathAsync(string host)
    {
        var path = _options.OAuthLoginPath;

        // Matched ignoring case here, since a dictionary bound from the configuration may not keep its comparer
        var listed = host == null ? null : _options.OAuthLoginPaths?.Keys.FirstOrDefault(x => string.Equals(x, host, StringComparison.OrdinalIgnoreCase));
        if (listed != null)
        {
            path = _options.OAuthLoginPaths[listed];
        }

        return Task.FromResult(string.IsNullOrEmpty(path) ? null : path);
    }
}
