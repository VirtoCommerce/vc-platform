using System;
using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core;

/// <summary>
/// The server-side values an admin UI needs before the user signs in: what to show in the header,
/// whether module installation is allowed, how to connect to push notifications, and so on.
/// The platform ships no admin UI of its own; a UI module reads these values through
/// <see cref="IAdminUIContextProvider"/> or the <c>api/platform/common/ui/context</c> endpoint.
/// </summary>
public class AdminUIContext
{
    public string PlatformVersion { get; set; }

    /// <summary>
    /// The raw <c>VirtoCommerce:DemoCredentials</c> value, as configured.
    /// </summary>
    public string DemoCredentials { get; set; }

    /// <summary>
    /// The next demo reset time (UTC), when <c>VirtoCommerce:DemoResetTime</c> is configured.
    /// </summary>
    public DateTime? DemoResetTime { get; set; }

    /// <summary>
    /// The activated license without its raw data, or null when there is none.
    /// </summary>
    public object License { get; set; }

    public bool SendDiagnosticData { get; set; } = true;

    /// <summary>
    /// Whether modules can be installed, updated and uninstalled from the UI.
    /// Follows <c>VirtoCommerce:RefreshProbingFolderOnStart</c>.
    /// </summary>
    public bool AllowInstallModules { get; set; } = true;

    public bool ForceWebSockets { get; set; }

    public EnvironmentBannerOptions EnvironmentBanner { get; set; } = new();

    /// <summary>
    /// The installed admin UIs and where to open each, e.g. for the Vue admin to embed the AngularJS screens.
    /// </summary>
    public IList<AdminUIApp> AdminUIs { get; set; } = [];
}
