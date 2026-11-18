using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Security;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

/// <summary>
/// Nothing of the application comes from the client but its id, name and redirect URIs.
/// </summary>
public class SelfRegisteredClientFactory : ISelfRegisteredClientFactory
{
    private readonly IEnumerable<IOAuthResourceProvider> _resourceProviders;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SelfRegisteredClientFactory> _logger;

    public SelfRegisteredClientFactory(
        IEnumerable<IOAuthResourceProvider> resourceProviders,
        IHttpContextAccessor httpContextAccessor,
        ILogger<SelfRegisteredClientFactory> logger)
    {
        _resourceProviders = resourceProviders;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public virtual async Task<OpenIddictApplicationDescriptor> CreateDescriptorAsync(string registrationType, string clientId, string displayName,
        IEnumerable<string> redirectUris, CancellationToken cancellationToken = default)
    {
        var uris = redirectUris.Select(x => new Uri(x, UriKind.Absolute)).ToList();

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = displayName,
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Explicit,
            // A native application may use any port of a loopback redirect URI that has none (RFC 8252)
            ApplicationType = uris.Any(x => x.IsLoopback) ? ApplicationTypes.Native : ApplicationTypes.Web,
        };

        descriptor.RedirectUris.UnionWith(uris);
        descriptor.Permissions.UnionWith(
        [
            Permissions.Endpoints.Authorization,
            Permissions.Endpoints.Token,
            Permissions.GrantTypes.AuthorizationCode,
            Permissions.GrantTypes.RefreshToken,
            Permissions.ResponseTypes.Code,
            Permissions.Scopes.Profile,
        ]);
        descriptor.Permissions.UnionWith((await GetResourcesAsync(cancellationToken)).Select(x => Permissions.Prefixes.Resource + x));
        descriptor.Requirements.Add(Requirements.Features.ProofKeyForCodeExchange);
        descriptor.Properties[SelfRegisteredClientConstants.RegistrationProperty] = JsonSerializer.SerializeToElement(registrationType);

        return descriptor;
    }

    protected virtual async Task<IList<string>> GetResourcesAsync(CancellationToken cancellationToken)
    {
        var context = new OAuthResourceContext { HttpContext = _httpContextAccessor.HttpContext };
        var resources = await _resourceProviders.GetAllResourcesAsync(context, _logger, cancellationToken);

        return resources
            .Where(x => x.AllowSelfRegisteredClients)
            .Select(x => x.Uri)
            .Distinct(StringComparer.Ordinal)
            .ToList();
    }
}
