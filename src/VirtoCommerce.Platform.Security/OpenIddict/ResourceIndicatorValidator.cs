using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Security.OpenIddict;

public static class ResourceIndicatorValidator
{
    public static bool IsSameOriginWebResource(string resource, Uri platformOrigin)
    {
        ArgumentNullException.ThrowIfNull(platformOrigin);

        return TryCreateWebResource(resource, out var resourceUri) &&
            string.Equals(resourceUri.Scheme, platformOrigin.Scheme, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(resourceUri.IdnHost, platformOrigin.IdnHost, StringComparison.OrdinalIgnoreCase) &&
            resourceUri.Port == platformOrigin.Port;
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
            string.Equals(leftUri.Scheme, rightUri.Scheme, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(leftUri.IdnHost, rightUri.IdnHost, StringComparison.OrdinalIgnoreCase) &&
            leftUri.Port == rightUri.Port &&
            string.Equals(leftUri.PathAndQuery, rightUri.PathAndQuery, StringComparison.Ordinal);
    }

    private static bool TryCreateWebResource(string resource, out Uri resourceUri)
    {
        var valid = Uri.TryCreate(resource, UriKind.Absolute, out resourceUri) &&
            (resourceUri.Scheme == Uri.UriSchemeHttps || resourceUri.Scheme == Uri.UriSchemeHttp) &&
            string.IsNullOrEmpty(resourceUri.UserInfo) &&
            string.IsNullOrEmpty(resourceUri.Fragment);

        if (!valid)
        {
            resourceUri = null;
        }

        return valid;
    }
}
