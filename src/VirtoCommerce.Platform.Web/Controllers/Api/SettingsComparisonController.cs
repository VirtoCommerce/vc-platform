using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api;

[Route("api/platform/settings-comparison")]
[Authorize]
public class SettingsComparisonController : Controller
{
    [HttpGet]
    [Authorize(PlatformConstants.Security.Permissions.SettingComparison)]
    public Task<ActionResult<LocalizableSettingsAndLanguages>> GetEnvironments()
    {
        throw new System.NotImplementedException();
    }

    [HttpGet]
    [Authorize(PlatformConstants.Security.Permissions.SettingComparison)]
    public Task<ActionResult<LocalizableSettingsAndLanguages>> CompareSettings([FromQuery] IEnumerable<string> environments)
    {
        throw new System.NotImplementedException();
    }
}
