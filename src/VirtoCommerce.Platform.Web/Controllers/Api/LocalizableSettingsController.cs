using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Web.Controllers.Api;

[Route("api/platform/localizable-settings")]
[Authorize]
public class LocalizableSettingsController : Controller
{
    private readonly ILocalizableSettingService _localizableSettingService;

    public LocalizableSettingsController(ILocalizableSettingService localizableSettingService)
    {
        _localizableSettingService = localizableSettingService;
    }

    [HttpGet("names")]
    [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
    public ActionResult<IList<string>> GetLocalizableSettingNames()
    {
        var result = _localizableSettingService.GetNames();
        return Ok(result);
    }

    [HttpGet("{name}/dictionary-items/{language}/values")]
    [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
    public async Task<ActionResult<IList<KeyValue>>> GetDictionaryValues([FromRoute] string name, [FromRoute] string language)
    {
        var result = await _localizableSettingService.GetValuesAsync(name, language);
        return Ok(result);
    }

    [HttpGet("{name}/dictionary-items")]
    [Authorize(PlatformConstants.Security.Permissions.SettingQuery)]
    public async Task<ActionResult<DictionaryItemsAndLanguages>> GetDictionaryItemsAndLanguagesAsync([FromRoute] string name)
    {
        var result = await _localizableSettingService.GetItemsAndLanguagesAsync(name);
        return Ok(result);
    }

    [HttpPost("{name}/dictionary-items")]
    [Authorize(PlatformConstants.Security.Permissions.SettingUpdate)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> SaveDictionaryItems([FromRoute] string name, [FromBody] IList<DictionaryItem> items)
    {
        await _localizableSettingService.SaveAsync(name, items);
        return NoContent();
    }

    [HttpDelete("{name}/dictionary-items")]
    [Authorize(PlatformConstants.Security.Permissions.SettingUpdate)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteDictionaryItems([FromRoute] string name, [FromQuery] IList<string> values)
    {
        await _localizableSettingService.DeleteAsync(name, values);
        return Ok();
    }
}
