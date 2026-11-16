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

    public static async ValueTask<TApplication> EnsureCreatedAsync<TApplication>(this IOpenIddictApplicationManager manager, TApplication application,
        CancellationToken cancellationToken = default)
        where TApplication : class
    {
        var id = await manager.GetIdAsync(application, cancellationToken);
        if (await manager.FindByIdAsync(id, cancellationToken) != null)
        {
            return application;
        }

        var clientId = await manager.GetClientIdAsync(application, cancellationToken);

        try
        {
            await manager.CreateAsync(application, cancellationToken);
            return application;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            if (await manager.FindByClientIdAsync(clientId, cancellationToken) is TApplication stored)
            {
                return stored;
            }

            throw;
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

    public static async ValueTask<bool> HasResourcePermissionsAsync(this IOpenIddictApplicationManager manager, object application,
        IEnumerable<string> resources, CancellationToken cancellationToken = default)
    {
        return await resources.ToAsyncEnumerable().AllAsync(async (resource, token) =>
            await manager.HasPermissionAsync(application, OpenIddictConstants.Permissions.Prefixes.Resource + resource, token), cancellationToken);
    }

    public static bool CoversResources(ICollection<string> consented, IEnumerable<string> requested)
    {
        return requested.All(x => consented.Contains(x, StringComparer.Ordinal));
    }
}
