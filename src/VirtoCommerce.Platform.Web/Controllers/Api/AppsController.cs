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
    [Authorize(PlatformConstants.Security.Permissions.ModuleManage)]
    public class AppsController : Controller
    {
        private readonly IExternalModuleCatalog _externalModuleCatalog;

        public AppsController(IExternalModuleCatalog externalModuleCatalog)
        {
            _externalModuleCatalog = externalModuleCatalog;
        }

        private void EnsureModulesCatalogInitialized()
        {
            _externalModuleCatalog.Initialize();
        }

        [HttpGet]
        public ActionResult<AppDescriptor[]> GetApps()
        {
            EnsureModulesCatalogInitialized();

            var retVal = _externalModuleCatalog.Modules.OfType<ManifestModuleInfo>()
                .SelectMany(x => x.Apps)
                .Where(x => string.IsNullOrEmpty(x.Permission) || User.HasGlobalPermission(x.Permission))
                .OrderBy(x => x.Title)
                .Select(x => new AppDescriptor(x))
                .ToArray();

            return Ok(retVal);

        }

    }
}
