using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Lists the resources a module serves, such as its MCP endpoints, so that administrators can allow OAuth applications
    /// to request tokens for them in the back office. Register implementations in the service collection; the platform
    /// asks every registered provider.
    /// </summary>
    public interface IOAuthResourceProvider
    {
        /// <summary>
        /// The resources, with the public URIs that clients use. The request is the back office's request, which a
        /// provider may use to resolve the platform's public address.
        /// </summary>
        Task<IList<OAuthResource>> GetResourcesAsync(HttpContext httpContext);
    }
}
