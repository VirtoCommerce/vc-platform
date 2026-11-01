using System;
using System.ComponentModel;
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

    [XmlIgnore]
    public AdminUIHosting Hosting { get; set; }

    /// <summary>
    /// The <c>hosting</c> attribute as written in the manifest. Read as text so that a value this platform doesn't know
    /// (a misspelling, or a hosting added by a later platform) falls back to <see cref="AdminUIHosting.App"/> instead
    /// of making the whole manifest unreadable.
    /// </summary>
    [XmlAttribute("hosting")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public string HostingValue
    {
        get => Hosting.ToString();
        set => Hosting = Enum.TryParse<AdminUIHosting>(value, ignoreCase: true, out var hosting) && Enum.IsDefined(hosting)
            ? hosting
            : AdminUIHosting.App;
    }
}
