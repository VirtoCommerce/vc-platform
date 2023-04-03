using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Security.Authorization;
using VirtoCommerce.Platform.Web.Model.Modularity;


namespace VirtoCommerce.Platform.Web.Controllers.Api
{
    [Route("api/platform/apps")]
    public class AppsController : Controller
    {
        private readonly ILocalModuleCatalog _localModuleCatalog;
        private readonly IAuthorizationService _authorizationService;

        public AppsController(ILocalModuleCatalog localModuleCatalog, IAuthorizationService authorizationService)
        {
            _localModuleCatalog = localModuleCatalog ?? throw new ArgumentNullException(nameof(localModuleCatalog));
            _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        /// <summary>
        ///  Gets the list of available apps, filtered by user permissions.
        /// </summary>
        /// <returns>The list of available apps</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppDescriptor>>> GetApps()
        {

            var authorizedApps = new List<AppDescriptor>
            {
                // Add Commerce Manager by Default
                new AppDescriptor
                {
                    Id = "platform",
                    Title = "Commerce Manager",
                    Description = "Virto Commerce Platform",
                    RelativeUrl = "/",
                    IconUrl = "/images/platform_app.svg"
                }
            };

            var applicationList = _localModuleCatalog.Modules.OfType<ManifestModuleInfo>()
                .SelectMany(x => x.Apps)
                .OrderBy(x => x.Title);

            foreach (var moduleAppInfo in applicationList)
            {
                if (await AuthorizeAppAsync(moduleAppInfo.Permission))
                {
                    authorizedApps.Add(new AppDescriptor(moduleAppInfo));
                }
            }

            return authorizedApps;

        }

        private async Task<bool> AuthorizeAppAsync(string permission)
        {
            if (string.IsNullOrEmpty(permission))
            {
                return true;
            }

            var result = await _authorizationService.AuthorizeAsync(User, null,
                new PermissionAuthorizationRequirement(permission));

            return result.Succeeded;
        }
    }
}
