using System;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core;

namespace VirtoCommerce.Platform.Web.Extensions;

public static class OpenIddictExtensions
{
    public static bool IsImpersonateGrantType(this OpenIddictRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return string.Equals(request.GrantType, PlatformConstants.Security.GrantTypes.Impersonate, StringComparison.Ordinal);
    }
}
