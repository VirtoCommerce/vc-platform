using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.Model.Modularity;


namespace VirtoCommerce.Platform.Web.Controllers.Api;

[Route("api/platform/apps")]
public class AppsController : Controller
{
    private readonly ILocalModuleCatalog _localModuleCatalog;

    public AppsController(ILocalModuleCatalog localModuleCatalog)
    {
        _localModuleCatalog = localModuleCatalog ?? throw new ArgumentNullException(nameof(localModuleCatalog));
    }

    /// <summary>
    ///  Gets the list of available apps, filtered by user permissions.
    /// </summary>
    /// <returns>The list of available apps</returns>
    [HttpGet]
    public IEnumerable<AppDescriptor> GetApps()
    {
        var apps = _localModuleCatalog.Modules.OfType<ManifestModuleInfo>()
            .SelectMany(x => x.Apps)
            .Select(x => new AppDescriptor(x))
            .OrderBy(x => x.Title)
            .ToList();

        apps.Insert(0, // Add Commerce Manager by Default
            new AppDescriptor
            {
                Id = "platform",
                Title = "Commerce Manager",
                Description = "Virto Commerce Platform",
                RelativeUrl = "/",
                IconUrl = "/images/platform_app.svg"
            });

        return apps;

    }
}