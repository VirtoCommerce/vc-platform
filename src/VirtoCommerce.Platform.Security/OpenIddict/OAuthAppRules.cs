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

    // Run script in the browser instead of reaching the client
    private static readonly HashSet<string> _scriptSchemes = new(StringComparer.OrdinalIgnoreCase) { "javascript", "vbscript", "data" };

    public virtual IReadOnlyCollection<string> ConsentTypeValues { get; } = [ConsentTypes.Explicit, ConsentTypes.Implicit, ConsentTypes.Systematic, ConsentTypes.External];

    public virtual void AddDefaultPermissions(OpenIddictApplicationDescriptor descriptor)
    {
        descriptor.Permissions.AddRange(GetDefaultGrantTypes(descriptor.ClientType));
        descriptor.Permissions.AddRange(GetDefaultScopes());
        NormalizePermissions(descriptor);
    }

    public virtual void Normalize(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored)
    {
        // OpenIddict compares the types ignoring case, but the platform's consent screen doesn't
        descriptor.ClientType = ToLowerOrNull(descriptor.ClientType) ?? ToLowerOrNull(stored?.ClientType) ?? GetClientType(descriptor);
        descriptor.ApplicationType = ToLowerOrNull(descriptor.ApplicationType);
        descriptor.ConsentType = ToLowerOrNull(descriptor.ConsentType);

        NormalizePermissions(descriptor);
    }

    /// <summary>
    /// Keeps the permissions the client asked for and sets the endpoint and response type permissions that its
    /// grant types need. A client that names no grant type gets the platform defaults.
    /// </summary>
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
    /// Expects a normalized application (<see cref="Normalize"/>). A redirect URI the application already had is not
    /// checked again, so existing applications still save. OpenIddict checks the rest when the application is saved,
    /// for example that a confidential client has a secret or a signing key.
    /// </summary>
    public virtual IList<string> Validate(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored)
    {
        return ValidateClient(descriptor, stored)
            .Concat(ValidateTypes(descriptor))
            .Concat(ValidateRedirectUris(descriptor, stored))
            .ToList();
    }

    /// <summary>
    /// A redirect URI must be absolute, without a fragment, not a script, and use HTTPS unless it points to the
    /// device the client runs on (a loopback address, RFC 8252).
    /// </summary>
    public virtual string GetRedirectUriError(string uri)
    {
        if (!Uri.TryCreate(uri, UriKind.Absolute, out var value) || value.IsFile)
        {
            return $"The redirect URI '{uri}' is not an absolute URI.";
        }

        if (_scriptSchemes.Contains(value.Scheme))
        {
            return $"The redirect URI '{uri}' can't use the '{value.Scheme}' scheme.";
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
    /// The client id, and the client type: public or confidential, unchanged for an existing application, and a
    /// public client without a secret.
    /// </summary>
    protected virtual IEnumerable<string> ValidateClient(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored)
    {
        if (string.IsNullOrWhiteSpace(descriptor.ClientId))
        {
            yield return "The client id is required.";
        }

        if (descriptor.ClientType != ClientTypes.Public && descriptor.ClientType != ClientTypes.Confidential)
        {
            yield return $"The client type must be '{ClientTypes.Public}' or '{ClientTypes.Confidential}'.";
        }
        else if (stored?.ClientType != null && !string.Equals(stored.ClientType, descriptor.ClientType, StringComparison.OrdinalIgnoreCase))
        {
            yield return "The client type of an existing application can't be changed. Create a new application instead.";
        }
        else if (descriptor.ClientType == ClientTypes.Public && !string.IsNullOrEmpty(descriptor.ClientSecret))
        {
            yield return "A public client can't have a client secret.";
        }
    }

    /// <summary>
    /// The consent and application types, when they are set.
    /// </summary>
    protected virtual IEnumerable<string> ValidateTypes(OpenIddictApplicationDescriptor descriptor)
    {
        if (descriptor.ConsentType != null && !ConsentTypeValues.Contains(descriptor.ConsentType))
        {
            yield return $"The consent type must be one of: {string.Join(", ", ConsentTypeValues)}.";
        }

        if (descriptor.ApplicationType != null && descriptor.ApplicationType != ApplicationTypes.Web && descriptor.ApplicationType != ApplicationTypes.Native)
        {
            yield return $"The application type must be '{ApplicationTypes.Web}' or '{ApplicationTypes.Native}'.";
        }
    }

    /// <summary>
    /// The redirect and post-logout redirect URIs the application doesn't have yet (<see cref="GetRedirectUriError"/>).
    /// </summary>
    protected virtual IEnumerable<string> ValidateRedirectUris(OpenIddictApplicationDescriptor descriptor, OpenIddictApplicationDescriptor stored)
    {
        return GetRedirectUriErrors(descriptor.RedirectUris, stored?.RedirectUris)
            .Concat(GetRedirectUriErrors(descriptor.PostLogoutRedirectUris, stored?.PostLogoutRedirectUris));
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

    /// <summary>
    /// The client type of a new application that names none, as OpenIddict infers it: confidential with a secret or
    /// a key set to authenticate with, public otherwise.
    /// </summary>
    protected virtual string GetClientType(OpenIddictApplicationDescriptor descriptor)
    {
        return !string.IsNullOrEmpty(descriptor.ClientSecret) || descriptor.JsonWebKeySet?.Keys.Count > 0
            ? ClientTypes.Confidential
            : ClientTypes.Public;
    }

    private IEnumerable<string> GetRedirectUriErrors(IEnumerable<Uri> uris, IEnumerable<Uri> storedUris)
    {
        var stored = storedUris?.ToHashSet() ?? [];

        return uris
            .Where(x => !stored.Contains(x))
            .Select(x => GetRedirectUriError(x.OriginalString))
            .Where(x => x != null);
    }

    private static string ToLowerOrNull(string value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToLowerInvariant();
    }

    private static bool IsGrantType(string permission)
    {
        return permission.StartsWith(Permissions.Prefixes.GrantType, StringComparison.Ordinal);
    }
}
