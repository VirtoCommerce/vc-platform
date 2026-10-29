using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Modules;
using VirtoCommerce.Platform.Web.Model.Modularity;


namespace VirtoCommerce.Platform.Web.Controllers.Api;

[Route("api/platform/apps")]
[ApiController]
public class AppsController(IModuleService moduleService, IAdminUIAppService adminUIAppService) : ControllerBase
{
    /// <summary>
    ///  Gets the list of available apps, filtered by user permissions.
    /// </summary>
    /// <returns>The list of available apps</returns>
    [HttpGet]
    public IEnumerable<AppDescriptor> GetApps()
    {
        // An admin UI opens at the site root when it owns it, so its link follows VirtoCommerce:PlatformUI:RootApp
        var adminUIs = adminUIAppService.GetAdminUIs()
            .GroupBy(x => x.Id, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);

        return moduleService.GetInstalledModules()
            .SelectMany(x => x.Apps)
            .Select(x =>
            {
                var app = new AppDescriptor(x);

                if (adminUIs.TryGetValue(x.Id, out var adminUI))
                {
                    app.RelativeUrl = adminUI.IsRoot ? adminUI.Url : adminUI.Path;
                }

                return app;
            })
            // "platform", the AngularJS admin (Commerce Manager), keeps its place at the top
            .OrderBy(x => x.Id != AppManifestService.PlatformAppId)
            .ThenBy(x => x.Title)
            .ToList();
    }
}
