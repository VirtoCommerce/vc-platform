using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public static class ResourceIndicatorValidator
{
    public static bool IsSameOriginWebResource(string resource, Uri platformOrigin)
    {
        ArgumentNullException.ThrowIfNull(platformOrigin);

        return TryCreateWebResource(resource, out var resourceUri) && HaveSameOrigin(resourceUri, platformOrigin);
    }

    public static bool IsSubset(IEnumerable<string> requestedResources, IEnumerable<string> grantedResources)
    {
        var granted = grantedResources?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray() ?? [];

        return requestedResources == null || requestedResources
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .All(requested => granted.Any(value => AreEquivalent(requested, value)));
    }

    public static bool AreEquivalent(string left, string right)
    {
        return TryCreateWebResource(left, out var leftUri) &&
            TryCreateWebResource(right, out var rightUri) &&
            HaveSameOrigin(leftUri, rightUri) &&
            string.Equals(leftUri.PathAndQuery, rightUri.PathAndQuery, StringComparison.Ordinal);
    }

    private static bool TryCreateWebResource(string resource, out Uri resourceUri)
    {
        if (!Uri.TryCreate(resource, UriKind.Absolute, out resourceUri))
        {
            resourceUri = null;
            return false;
        }

        var isWebResource = resourceUri.Scheme == Uri.UriSchemeHttps || resourceUri.Scheme == Uri.UriSchemeHttp;

        return isWebResource &&
            string.IsNullOrEmpty(resourceUri.UserInfo) &&
            string.IsNullOrEmpty(resourceUri.Fragment);
    }

    private static bool HaveSameOrigin(Uri left, Uri right)
    {
        return string.Equals(left.Scheme, right.Scheme, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(left.IdnHost, right.IdnHost, StringComparison.OrdinalIgnoreCase) &&
            left.Port == right.Port;
    }
}
