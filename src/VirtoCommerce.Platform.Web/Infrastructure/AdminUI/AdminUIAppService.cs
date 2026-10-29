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
/// Installed modules don't change while the platform runs, so they are read once; the root app follows the options.
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
    private string _reportedUnknownRootApp;

    public AdminUIAppService(IModuleService moduleService, IOptionsMonitor<PlatformUIOptions> options, ILogger<AdminUIAppService> logger)
    {
        _options = options;
        _logger = logger;
        _apps = new Lazy<IReadOnlyList<(ManifestAppInfo, string)>>(() => moduleService.GetInstalledModules()
            .Where(x => x.Errors.Count == 0)
            .SelectMany(module => module.Apps.Select(app => (app, module.Id)))
            .ToList());
    }

    public IReadOnlyList<AdminUIApp> GetAdminUIs()
    {
        var root = GetRootAdminUI();

        var result = _apps.Value
            .Where(x => x.App.AdminUI != null)
            .Select(x => CreateAdminUI(x.App, x.ModuleId, root?.Id))
            .ToList();

        // An app made the root by configuration without declaring <adminUI>
        if (root != null && !result.Any(x => x.Id.EqualsIgnoreCase(root.Id)))
        {
            result.Add(root);
        }

        return result;
    }

    public AdminUIApp GetRootAdminUI()
    {
        var rootAppId = _options.CurrentValue.RootApp?.Trim();

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

            ReportUnknownRootApp(rootAppId);
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

    private void ReportUnknownRootApp(string rootAppId)
    {
        if (_reportedUnknownRootApp == rootAppId)
        {
            return;
        }

        _reportedUnknownRootApp = rootAppId;
        _logger.LogWarning("VirtoCommerce:PlatformUI:RootApp names the app {RootApp}, which no installed module declares. The site root falls back to the admin UI with the highest priority.", rootAppId);
    }
}
