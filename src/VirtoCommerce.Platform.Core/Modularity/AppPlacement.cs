namespace VirtoCommerce.Platform.Core.Modularity;

/// <summary>
/// Where a host app surfaces in the platform admin's navigation.
/// </summary>
public enum AppPlacement
{
    /// <summary>
    /// External-app card in the Apps dropdown. This is the default for
    /// modules that don't declare a placement and don't ship the legacy
    /// <c>&lt;supportEmbeddedMode&gt;true&lt;/supportEmbeddedMode&gt;</c>.
    /// </summary>
    AppMenu = 0,

    /// <summary>
    /// Inline main-menu item; opens inside the platform shell. Equivalent
    /// to the legacy <c>&lt;supportEmbeddedMode&gt;true&lt;/supportEmbeddedMode&gt;</c>.
    /// </summary>
    MainMenu,

    /// <summary>
    /// Not user-facing; reserved for system / programmatic apps such as
    /// the platform shell itself. Hidden apps still appear in
    /// <c>GET /api/platform/apps</c> for diagnostic access but are not
    /// rendered in any menu.
    /// </summary>
    Hidden,
}
