using Microsoft.AspNetCore.Http;

namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// What <see cref="IOAuthResourceProvider"/> is asked with.
/// </summary>
public class OAuthResourceContext
{
    /// <summary>
    /// The back office's request, which a provider may use to resolve the platform's public address. Null when the
    /// resources are listed outside a request.
    /// </summary>
    public HttpContext HttpContext { get; set; }
}
