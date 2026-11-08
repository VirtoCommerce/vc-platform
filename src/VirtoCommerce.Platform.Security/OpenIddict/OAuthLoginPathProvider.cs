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
        var listed = host == null ? null : _options.OAuthLoginPaths?.FirstOrDefault(x => string.Equals(x?.Host, host, StringComparison.OrdinalIgnoreCase));
        var path = listed != null ? listed.Path : _options.OAuthLoginPath;

        return Task.FromResult(string.IsNullOrEmpty(path) ? null : path);
    }
}
