using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// An installed admin UI: an <c>&lt;app&gt;</c> declared with <c>&lt;adminUI&gt;</c>, or the app configured as
/// <c>VirtoCommerce:PlatformUI:RootApp</c>.
/// </summary>
public class AdminUIApp
{
    public string Id { get; set; }

    public string ModuleId { get; set; }

    public string Title { get; set; }

    /// <summary>
    /// Where the UI is mounted besides the site root: <c>/apps/{appId}</c>.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Where to open the UI: <c>/</c> for the root admin UI, <c>/apps/{appId}/</c> for the others.
    /// </summary>
    public string Url { get; set; }

    public AdminUIHosting Hosting { get; set; }

    public int Priority { get; set; }

    public bool IsRoot { get; set; }
}
