using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Localizations;

/// <summary>
/// The regional settings a user can pick in their profile (<c>VirtoCommerce.Platform.UI.RegionalFormat</c> and
/// <c>VirtoCommerce.Platform.UI.TimeZone</c>). The platform owns the lists, so every admin UI offers the same choices;
/// each UI applies the chosen value itself.
/// </summary>
public interface IRegionalSettingsService
{
    /// <summary>
    /// Regional format codes in lower case (<c>en</c>, <c>en-us</c>, <c>de-de</c>), sorted
    /// </summary>
    Task<IList<string>> GetRegionalFormatsAsync();

    /// <summary>
    /// IANA time zone ids (<c>Europe/Berlin</c>, <c>Etc/UTC</c>), sorted
    /// </summary>
    Task<IList<string>> GetTimeZonesAsync();
}
