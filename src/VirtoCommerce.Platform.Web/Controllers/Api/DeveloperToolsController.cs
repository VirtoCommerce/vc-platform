using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core.DeveloperTools;
using Permissions = VirtoCommerce.Platform.Core.PlatformConstants.Security.Permissions;

namespace VirtoCommerce.Platform.Web.Controllers.Api;

[Authorize]
[Route("/api/platform/developer-tools")]
public class DeveloperToolsController(IDeveloperToolRegistrar developerToolRegistrar) : Controller
{
    [HttpGet]
    [Authorize(Permissions.DeveloperToolsAccess)]
    public ActionResult<DeveloperToolDescriptor[]> GetDeveloperTools()
    {
        return Ok(developerToolRegistrar.GetRegisteredTools(User));
    }
}
