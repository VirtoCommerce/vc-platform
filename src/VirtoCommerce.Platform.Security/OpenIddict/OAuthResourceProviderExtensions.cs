using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public static class OAuthResourceProviderExtensions
{
    /// <summary>
    /// A provider that fails is skipped, so that it doesn't take the resources of the others away.
    /// </summary>
    public static async Task<IList<OAuthResource>> GetAllResourcesAsync(this IEnumerable<IOAuthResourceProvider> providers,
        OAuthResourceContext context, ILogger logger, CancellationToken cancellationToken)
    {
        var result = new List<OAuthResource>();

        foreach (var provider in providers)
        {
            try
            {
                var resources = await provider.GetResourcesAsync(context, cancellationToken) ?? [];
                result.AddRange(resources.Where(x => !string.IsNullOrEmpty(x?.Uri)));
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "The OAuth resource provider {Provider} failed; its resources are not listed.", provider.GetType().FullName);
            }
        }

        return result;
    }
}
