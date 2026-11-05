using System;
using System.Collections.Generic;
using System.Linq;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// What the platform checks and derives when an OAuth application is saved: the permissions that follow from the
/// grant types, the defaults of a request that names no grant type, and the rules of client types and redirect URIs.
/// </summary>
public static class OAuthAppRules
{
    public static IReadOnlyCollection<string> ConsentTypeValues { get; } = [ConsentTypes.Explicit, ConsentTypes.Implicit, ConsentTypes.Systematic, ConsentTypes.External];

    // Granted to a client that names no grant type, as the API always did
    private static readonly string[] _defaultGrantTypes = [Permissions.GrantTypes.AuthorizationCode, Permissions.GrantTypes.ClientCredentials, Permissions.GrantTypes.RefreshToken];
    private static readonly string[] _defaultScopes = [Permissions.Scopes.Email, Permissions.Scopes.Profile];

    // Follow from the grant types; any other endpoint or response type permission is kept as it is
    private static readonly string[] _derivedPermissions = [Permissions.Endpoints.Authorization, Permissions.Endpoints.EndSession, Permissions.Endpoints.Token, Permissions.ResponseTypes.Code];

    /// <summary>
    /// The permissions of a new application with the platform defaults.
    /// </summary>
    public static void AddDefaultPermissions(OpenIddictApplicationDescriptor descriptor)
    {
        descriptor.Permissions.AddRange(GetDefaultGrantTypes(descriptor.ClientType));
        descriptor.Permissions.AddRange(_defaultScopes);
        NormalizePermissions(descriptor);
    }

    /// <summary>
    /// Keeps the permissions the client asked for and sets the endpoint and response type permissions that its
    /// grant types need. A client that names no grant type gets the platform defaults.
    /// </summary>
    public static void NormalizePermissions(OpenIddictApplicationDescriptor descriptor)
    {
        var permissions = descriptor.Permissions;
        permissions.RemoveWhere(string.IsNullOrWhiteSpace);

        if (!permissions.Any(IsGrantType))
        {
            permissions.AddRange(GetDefaultGrantTypes(descriptor.ClientType));
            permissions.AddRange(_defaultScopes);
        }

        permissions.ExceptWith(_derivedPermissions);

        if (permissions.Contains(Permissions.GrantTypes.AuthorizationCode) || permissions.Contains(Permissions.GrantTypes.Implicit))
        {
            permissions.Add(Permissions.Endpoints.Authorization);
            permissions.Add(Permissions.Endpoints.EndSession);
        }

        if (permissions.Contains(Permissions.GrantTypes.AuthorizationCode))
        {
            permissions.Add(Permissions.ResponseTypes.Code);
        }

        if (permissions.Any(x => IsGrantType(x) && x != Permissions.GrantTypes.Implicit))
        {
            permissions.Add(Permissions.Endpoints.Token);
        }
    }

    /// <summary>
    /// The reasons the application can't be saved. <paramref name="stored"/> is the saved state of an existing
    /// application, or null for a new one. A redirect URI the application already had is not checked again.
    /// </summary>
    public static IList<string> Validate(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(descriptor.ClientId))
        {
            errors.Add("The client id is required.");
        }

        if (descriptor.ClientType != ClientTypes.Public && descriptor.ClientType != ClientTypes.Confidential)
        {
            errors.Add($"The client type must be '{ClientTypes.Public}' or '{ClientTypes.Confidential}'.");
        }
        else if (stored != null && stored.ClientType != null && !string.Equals(stored.ClientType, descriptor.ClientType, StringComparison.OrdinalIgnoreCase))
        {
            errors.Add("The client type of an existing application can't be changed. Create a new application instead.");
        }
        else if (descriptor.ClientType == ClientTypes.Public && !string.IsNullOrEmpty(descriptor.ClientSecret))
        {
            errors.Add("A public client can't have a client secret.");
        }
        else if (stored == null && descriptor.ClientType == ClientTypes.Confidential && string.IsNullOrEmpty(descriptor.ClientSecret))
        {
            errors.Add("A confidential client needs a client secret.");
        }

        if (!string.IsNullOrEmpty(descriptor.ConsentType) && !ConsentTypeValues.Contains(descriptor.ConsentType))
        {
            errors.Add($"The consent type must be one of: {string.Join(", ", ConsentTypeValues)}.");
        }

        if (!string.IsNullOrEmpty(descriptor.ApplicationType) && descriptor.ApplicationType != ApplicationTypes.Web && descriptor.ApplicationType != ApplicationTypes.Native)
        {
            errors.Add($"The application type must be '{ApplicationTypes.Web}' or '{ApplicationTypes.Native}'.");
        }

        errors.AddRange(GetRedirectUriErrors(descriptor.RedirectUris, stored?.RedirectUris));
        errors.AddRange(GetRedirectUriErrors(descriptor.PostLogoutRedirectUris, stored?.PostLogoutRedirectUris));

        return errors;
    }

    /// <summary>
    /// Why a redirect URI can't be used, or null: it must be absolute, without a fragment, and use HTTPS unless it
    /// points to the device the client runs on (a loopback address, RFC 8252).
    /// </summary>
    public static string GetRedirectUriError(string uri)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out var value) || value.IsFile)
        {
            return $"The redirect URI '{uri}' is not an absolute URI.";
        }

        if (!string.IsNullOrEmpty(value.Fragment))
        {
            return $"The redirect URI '{uri}' can't have a fragment.";
        }

        if (value.Scheme == Uri.UriSchemeHttp && !value.IsLoopback)
        {
            return $"The redirect URI '{uri}' must use HTTPS: HTTP is allowed only for loopback addresses such as localhost and 127.0.0.1.";
        }

        return null;
    }

    private static IEnumerable<string> GetRedirectUriErrors(IEnumerable<Uri> uris, IEnumerable<Uri> storedUris)
    {
        var stored = storedUris?.ToHashSet() ?? [];

        return uris
            .Where(x => !stored.Contains(x))
            .Select(x => GetRedirectUriError(x.OriginalString))
            .Where(x => x != null);
    }

    private static IEnumerable<string> GetDefaultGrantTypes(string clientType)
    {
        // A public client holds no secret, so it can't use the client credentials grant
        return clientType == ClientTypes.Public
            ? _defaultGrantTypes.Where(x => x != Permissions.GrantTypes.ClientCredentials)
            : _defaultGrantTypes;
    }

    private static bool IsGrantType(string permission)
    {
        return permission.StartsWith(Permissions.Prefixes.GrantType, StringComparison.Ordinal);
    }
}
