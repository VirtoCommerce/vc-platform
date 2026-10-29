using System.Xml.Serialization;

namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Declares an <c>&lt;app&gt;</c> as an admin UI that can own the site root:
/// <code>&lt;adminUI priority="50" hosting="App" /&gt;</code>
/// Platforms that don't know the element ignore it.
/// </summary>
public class ManifestAppAdminUI
{
    /// <summary>
    /// When <c>VirtoCommerce:PlatformUI:RootApp</c> is not set, the installed admin UI with the highest priority owns the site root.
    /// </summary>
    [XmlAttribute("priority")]
    public int Priority { get; set; }

    [XmlAttribute("hosting")]
    public AdminUIHosting Hosting { get; set; }
}
