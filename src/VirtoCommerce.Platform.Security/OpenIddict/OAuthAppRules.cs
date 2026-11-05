using System;
using System.Collections.Generic;
using System.Linq;
using OpenIddict.Abstractions;
using VirtoCommerce.Platform.Core.Common;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security.OpenIddict;

/// <summary>
/// The platform's rules of OAuth applications: the permissions that follow from the grant types, the defaults of a
/// request that names no grant type, and the rules of client types and redirect URIs.
/// </summary>
public class OAuthAppRules : IOAuthAppRules
{
    // Follow from the grant types; any other endpoint or response type permission is kept as it is
    private static readonly string[] _derivedPermissions = [Permissions.Endpoints.Authorization, Permissions.Endpoints.EndSession, Permissions.Endpoints.Token, Permissions.ResponseTypes.Code];

    public virtual IReadOnlyCollection<string> ConsentTypeValues { get; } = [ConsentTypes.Explicit, ConsentTypes.Implicit, ConsentTypes.Systematic, ConsentTypes.External];

    public virtual void AddDefaultPermissions(OpenIddictApplicationDescriptor descriptor)
    {
        descriptor.Permissions.AddRange(GetDefaultGrantTypes(descriptor.ClientType));
        descriptor.Permissions.AddRange(GetDefaultScopes());
        NormalizePermissions(descriptor);
    }

    public virtual void NormalizePermissions(OpenIddictApplicationDescriptor descriptor)
    {
        var permissions = descriptor.Permissions;
        permissions.RemoveWhere(string.IsNullOrWhiteSpace);

        if (!permissions.Any(IsGrantType))
        {
            permissions.AddRange(GetDefaultGrantTypes(descriptor.ClientType));
            permissions.AddRange(GetDefaultScopes());
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
    /// A redirect URI the application already had is not checked again, so existing applications still save.
    /// </summary>
    public virtual IList<string> Validate(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored)
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
    /// A redirect URI must be absolute, without a fragment, and use HTTPS unless it points to the device the client
    /// runs on (a loopback address, RFC 8252).
    /// </summary>
    public virtual string GetRedirectUriError(string uri)
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

    /// <summary>
    /// Granted to a client that names no grant type, as the API always did. A public client holds no secret, so it
    /// can't use the client credentials grant.
    /// </summary>
    protected virtual IEnumerable<string> GetDefaultGrantTypes(string clientType)
    {
        yield return Permissions.GrantTypes.AuthorizationCode;

        if (clientType != ClientTypes.Public)
        {
            yield return Permissions.GrantTypes.ClientCredentials;
        }

        yield return Permissions.GrantTypes.RefreshToken;
    }

    protected virtual IEnumerable<string> GetDefaultScopes()
    {
        return [Permissions.Scopes.Email, Permissions.Scopes.Profile];
    }

    private IEnumerable<string> GetRedirectUriErrors(IEnumerable<Uri> uris, IEnumerable<Uri> storedUris)
    {
        var stored = storedUris?.ToHashSet() ?? [];

        return uris
            .Where(x => !stored.Contains(x))
            .Select(x => GetRedirectUriError(x.OriginalString))
            .Where(x => x != null);
    }

    private static bool IsGrantType(string permission)
    {
        return permission.StartsWith(Permissions.Prefixes.GrantType, StringComparison.Ordinal);
    }
}
