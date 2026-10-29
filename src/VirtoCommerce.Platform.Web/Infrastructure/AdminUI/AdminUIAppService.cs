using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Infrastructure.AdminUI;

/// <summary>
/// The admin UIs come from the installed modules' <c>&lt;app&gt;</c> elements with <c>&lt;adminUI&gt;</c>.
/// Installed modules don't change while the platform runs, so they are read once; the result is kept until
/// <c>RootApp</c> or <c>Enable</c> changes, because <see cref="AdminUIMiddleware"/> asks on every request.
/// </summary>
public class AdminUIAppService : IAdminUIAppService
{
    /// <summary>
    /// <c>VirtoCommerce:PlatformUI:RootApp</c> value for "no admin UI at the site root".
    /// </summary>
    public const string NoRootApp = "none";

    private readonly IOptionsMonitor<PlatformUIOptions> _options;
    private readonly ILogger<AdminUIAppService> _logger;
    private readonly Lazy<IReadOnlyList<(ManifestAppInfo App, string ModuleId)>> _apps;
    private volatile Snapshot _snapshot;

    public AdminUIAppService(IModuleService moduleService, IOptionsMonitor<PlatformUIOptions> options, ILogger<AdminUIAppService> logger)
    {
        _options = options;
        _logger = logger;
        _apps = new Lazy<IReadOnlyList<(ManifestAppInfo, string)>>(() => moduleService.GetInstalledModules()
            .Where(x => x.Errors.Count == 0)
            .SelectMany(module => module.Apps.Select(app => (app, module.Id)))
            .ToList());
    }

    /// <summary>
    /// The returned list is shared: read it, don't change it.
    /// </summary>
    public IList<AdminUIApp> GetAdminUIs() => GetSnapshot().AdminUIs;

    public AdminUIApp GetRootAdminUI() => GetSnapshot().Root;

    private Snapshot GetSnapshot()
    {
        var options = _options.CurrentValue;
        var rootAppId = options.RootApp?.Trim() ?? string.Empty;
        var snapshot = _snapshot;

        if (snapshot == null || snapshot.Enable != options.Enable || snapshot.RootAppId != rootAppId)
        {
            snapshot = CreateSnapshot(options.Enable, rootAppId);
            _snapshot = snapshot;
        }

        return snapshot;
    }

    private Snapshot CreateSnapshot(bool enable, string rootAppId)
    {
        var root = enable ? SelectRoot(rootAppId) : null;

        var adminUIs = _apps.Value
            .Where(x => x.App.AdminUI != null)
            .Select(x => CreateAdminUI(x.App, x.ModuleId, root?.Id))
            .ToList();

        // An app made the root by configuration without declaring <adminUI>
        if (root != null && !adminUIs.Any(x => x.Id.EqualsIgnoreCase(root.Id)))
        {
            adminUIs.Add(root);
        }

        return new Snapshot(enable, rootAppId, adminUIs.ToArray(), root);
    }

    private AdminUIApp SelectRoot(string rootAppId)
    {
        if (rootAppId.EqualsIgnoreCase(NoRootApp))
        {
            return null;
        }

        if (!string.IsNullOrEmpty(rootAppId))
        {
            var configured = _apps.Value.FirstOrDefault(x => x.App.Id.EqualsIgnoreCase(rootAppId));

            if (configured.App != null)
            {
                return CreateAdminUI(configured.App, configured.ModuleId, configured.App.Id);
            }

            _logger.LogWarning("VirtoCommerce:PlatformUI:RootApp names the app {RootApp}, which no installed module declares. The site root falls back to the admin UI with the highest priority.", rootAppId);
        }

        // The highest priority wins; on a tie, the module that comes first in dependency order
        var auto = _apps.Value
            .Where(x => x.App.AdminUI != null)
            .OrderByDescending(x => x.App.AdminUI.Priority)
            .FirstOrDefault();

        return auto.App == null ? null : CreateAdminUI(auto.App, auto.ModuleId, auto.App.Id);
    }

    private static AdminUIApp CreateAdminUI(ManifestAppInfo app, string moduleId, string rootAppId)
    {
        var path = $"/apps/{app.Id}";
        var isRoot = app.Id.EqualsIgnoreCase(rootAppId);

        return new AdminUIApp
        {
            Id = app.Id,
            ModuleId = moduleId,
            Title = app.Title,
            Path = path,
            Url = isRoot ? "/" : $"{path}/",
            Hosting = app.AdminUI?.Hosting ?? AdminUIHosting.App,
            Priority = app.AdminUI?.Priority ?? 0,
            IsRoot = isRoot,
        };
    }

    private sealed record Snapshot(bool Enable, string RootAppId, AdminUIApp[] AdminUIs, AdminUIApp Root);
}
