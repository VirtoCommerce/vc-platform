using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OpenIddict.Abstractions;

namespace VirtoCommerce.Platform.Security.OpenIddict.ClientRegistration;

public static class SelfRegisteredClientExtensions
{
    /// <summary>
    /// Null for an application registered by an administrator.
    /// </summary>
    public static async ValueTask<string> GetRegistrationTypeAsync(this IOpenIddictApplicationManager manager, object application, CancellationToken cancellationToken = default)
    {
        if (application == null)
        {
            return null;
        }

        var properties = await manager.GetPropertiesAsync(application, cancellationToken);

        return properties.TryGetValue(SelfRegisteredClientConstants.RegistrationProperty, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;
    }

    public static async ValueTask<bool> IsSelfRegisteredAsync(this IOpenIddictApplicationManager manager, object application, CancellationToken cancellationToken = default)
    {
        return await manager.GetRegistrationTypeAsync(application, cancellationToken) != null;
    }

    public static async ValueTask EnsureCreatedAsync(this IOpenIddictApplicationManager manager, object application, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(application);

        var id = await manager.GetIdAsync(application, cancellationToken);
        if (await manager.FindByIdAsync(id, cancellationToken) == null)
        {
            await manager.CreateAsync(application, cancellationToken);
        }
    }

    public static async ValueTask<IList<string>> GetConsentedResourcesAsync(this IOpenIddictAuthorizationManager manager, object authorization, CancellationToken cancellationToken = default)
    {
        var properties = await manager.GetPropertiesAsync(authorization, cancellationToken);

        if (!properties.TryGetValue(SelfRegisteredClientConstants.ResourcesProperty, out var value) || value.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return value.EnumerateArray()
            .Where(x => x.ValueKind == JsonValueKind.String)
            .Select(x => x.GetString())
            .ToList();
    }

    public static bool CoversResources(ICollection<string> consented, IEnumerable<string> requested)
    {
        return requested.All(x => consented.Contains(x, StringComparer.Ordinal));
    }
}
