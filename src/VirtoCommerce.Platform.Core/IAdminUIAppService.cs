using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// The installed admin UIs and the one that owns the site root (<c>VirtoCommerce:PlatformUI:RootApp</c>).
/// </summary>
public interface IAdminUIAppService
{
    IReadOnlyList<AdminUIApp> GetAdminUIs();

    /// <summary>
    /// The admin UI served at <c>/</c>, or null when the platform answers <c>/</c> with its own page.
    /// </summary>
    AdminUIApp GetRootAdminUI();
}
