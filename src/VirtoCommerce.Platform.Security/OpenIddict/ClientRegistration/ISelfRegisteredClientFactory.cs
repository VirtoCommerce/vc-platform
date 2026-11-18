using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public interface ISelfRegisteredClientFactory
{
    /// <summary>
    /// The redirect URIs are already validated.
    /// </summary>
    Task<OpenIddictApplicationDescriptor> CreateDescriptorAsync(string registrationType, string clientId, string displayName,
        IEnumerable<string> redirectUris, CancellationToken cancellationToken = default);
}
