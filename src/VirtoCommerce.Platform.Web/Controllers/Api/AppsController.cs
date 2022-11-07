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

        public AppsController(ILocalModuleCatalog externalModuleCatalog)
        {
            _localModuleCatalog = externalModuleCatalog;
        }

         [HttpGet]
        public ActionResult<AppDescriptor[]> GetApps()
        {
            var retVal = _localModuleCatalog.Modules.OfType<ManifestModuleInfo>()
                .SelectMany(x => x.Apps)
                .Where(x => string.IsNullOrEmpty(x.Permission) || User.HasGlobalPermission(x.Permission))
                .OrderBy(x => x.Title)
                .Select(x => new AppDescriptor(x))
                .ToArray();

            return Ok(retVal);

        }

    }
}
