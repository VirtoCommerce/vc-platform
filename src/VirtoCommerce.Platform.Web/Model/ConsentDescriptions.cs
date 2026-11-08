using System;
using System.Collections.Generic;
using System.Linq;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Web.Model;

/// <summary>
/// The words of the consent page for what an authorization request asks: the standard scopes and the resources.
/// Scopes of their own are described by the scope manager.
/// </summary>
public static class ConsentDescriptions
{
    /// <summary>
    /// The scopes described here; any other scope is described by its display name.
    /// </summary>
    public static IReadOnlyCollection<string> StandardScopes { get; } = [Scopes.OpenId, Scopes.Profile, Scopes.Email, Scopes.Roles, Scopes.OfflineAccess];

    public static IList<AuthorizePermission> GetPermissions(ICollection<string> scopes, ICollection<string> resources, string email)
    {
        var result = new List<AuthorizePermission>();

        if (resources.Count > 0)
        {
            result.Add(new AuthorizePermission
            {
                Icon = "work",
                Title = "Work with your permissions",
                Details = $"Everything your account is allowed to do at {string.Join(", ", resources.Select(GetResourceName))}",
            });
        }

        if (scopes.Contains(Scopes.OpenId) || scopes.Contains(Scopes.Profile))
        {
            result.Add(new AuthorizePermission
            {
                Icon = "user",
                Title = "Know who you are",
                Details = scopes.Contains(Scopes.Profile) ? "Your name and user name" : "Your user ID",
            });
        }

        if (scopes.Contains(Scopes.Email))
        {
            result.Add(new AuthorizePermission { Icon = "mail", Title = "See your email address", Details = email });
        }

        if (scopes.Contains(Scopes.Roles))
        {
            result.Add(new AuthorizePermission { Icon = "user", Title = "See your roles" });
        }

        if (scopes.Contains(Scopes.OfflineAccess))
        {
            result.Add(new AuthorizePermission { Icon = "clock", Title = "Stay signed in", Details = "Until you revoke access" });
        }

        return result;
    }

    /// <summary>
    /// A resource as a person reads it: the host and the path, such as shop.example.com/ucp/mcp.
    /// </summary>
    public static string GetResourceName(string resource)
    {
        return Uri.TryCreate(resource, UriKind.Absolute, out var uri)
            ? GetHost(uri) + uri.AbsolutePath.TrimEnd('/')
            : resource;
    }

    /// <summary>
    /// Where the user goes after they allow: the host, whether it is an application on this computer, and whether it
    /// is another site than <paramref name="currentHost"/>.
    /// </summary>
    public static (string Host, bool IsLocal, bool IsElsewhere) GetRedirect(string redirectUri, string currentHost)
    {
        if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri))
        {
            return (redirectUri, false, true);
        }

        var isLocal = uri.IsLoopback;
        var isElsewhere = !isLocal && !string.Equals(uri.Host, currentHost, StringComparison.OrdinalIgnoreCase);

        return (GetHost(uri), isLocal, isElsewhere);
    }

    private static string GetHost(Uri uri)
    {
        return uri.IsDefaultPort ? uri.Host : $"{uri.Host}:{uri.Port}";
    }
}
