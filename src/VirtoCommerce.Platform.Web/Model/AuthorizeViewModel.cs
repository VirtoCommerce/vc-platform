using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VirtoCommerce.Platform.Web.Model;

/// <summary>
/// The consent page: who asks, for which account, what the application will be able to do, and where the user goes
/// after they allow it, so a user can tell an application that is not what it claims.
/// </summary>
public class AuthorizeViewModel
{
    [Display(Name = "Application")]
    public string ApplicationName { get; set; }

    [Display(Name = "Scope")]
    public string Scope { get; set; }

    public string ClientId { get; set; }

    /// <summary>
    /// Null for an application an administrator registered.
    /// </summary>
    public string RegistrationType { get; set; }

    /// <summary>
    /// The host of the client's metadata document: the site that vouches for the application.
    /// </summary>
    public string ClientHost { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    /// <summary>
    /// The resources the token is for, such as an MCP endpoint.
    /// </summary>
    public IList<string> Resources { get; set; } = [];

    /// <summary>
    /// What the application will be able to do, in the user's words.
    /// </summary>
    public IList<AuthorizePermission> Permissions { get; set; } = [];

    public string RedirectUri { get; set; }

    /// <summary>
    /// The host, and the port when it is not the default one, that the user returns to.
    /// </summary>
    public string RedirectHost { get; set; }

    /// <summary>
    /// The user returns to an application on their own computer: a loopback address.
    /// </summary>
    public bool RedirectIsLocal { get; set; }

    /// <summary>
    /// The user returns to another site than this one, so the page asks them to check it.
    /// </summary>
    public bool RedirectIsElsewhere { get; set; }

    /// <summary>
    /// Any app on the user's computer could give the client id of a self-registered application that returns to a
    /// loopback address.
    /// </summary>
    public bool WarnAboutLocalApp => !string.IsNullOrEmpty(RegistrationType) && RedirectIsLocal;

    /// <summary>
    /// The previous submit was refused: the page was open too long, or the browser lost its cookie.
    /// </summary>
    public bool Expired { get; set; }
}

public class AuthorizePermission
{
    /// <summary>
    /// The icon of the permission on the consent page: work, user, mail, clock or key.
    /// </summary>
    public string Icon { get; set; }

    public string Title { get; set; }

    public string Details { get; set; }
}
