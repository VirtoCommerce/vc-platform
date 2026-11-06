using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Lists the resources a module serves, such as its MCP endpoints, so that administrators can allow OAuth applications
/// to request tokens for them in the back office. Register implementations in the service collection; the platform
/// asks every registered provider, and a provider that fails is skipped.
/// </summary>
public interface IOAuthResourceProvider
{
    /// <summary>
    /// The resources, with the public URIs that clients use.
    /// </summary>
    Task<IList<OAuthResource>> GetResourcesAsync(OAuthResourceContext context, CancellationToken cancellationToken);
}
