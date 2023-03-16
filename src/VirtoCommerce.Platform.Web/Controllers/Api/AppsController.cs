using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Web.Model.Modularity;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/apps")]
    public class AppsController : Controller
    {
        private readonly ILocalModuleCatalog _localModuleCatalog;

        public AppsController(ILocalModuleCatalog localModuleCatalog)
        {
            _localModuleCatalog = localModuleCatalog;
        }

        [HttpGet]
        public ActionResult<AppDescriptor[]> GetApps()
        {
            var appList = _localModuleCatalog.Modules.OfType<ManifestModuleInfo>()
                .SelectMany(x => x.Apps)
                .OrderBy(x => x.Title)
                .Select(x => new AppDescriptor(x))
                .ToList();

            appList.Insert(0, new AppDescriptor
            {
                Id = "platform",
                Title = "Commerce Manager",
                Description = "Virto Commerce Platform",
                RelativeUrl = "/",
                IconUrl = "/images/platform_app.svg"
            });

            return Ok(appList.ToArray());

        }

    }
}
