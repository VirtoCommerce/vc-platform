using System;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// Marks the action that renders an admin UI page at <c>/</c>, for an admin UI hosted as
/// <see cref="Modularity.AdminUIHosting.Endpoint"/>. Several admin UIs can map <c>/</c>; the platform routes a request
/// to the one that owns the site root, or to the one whose <c>/apps/{appId}/</c> path the request came through.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AdminUIPageAttribute(string appId) : Attribute
{
    /// <summary>
    /// The <c>&lt;app&gt;</c> id of the admin UI. Null marks the platform's own page for "no admin UI".
    /// </summary>
    public string AppId { get; } = appId;
}
