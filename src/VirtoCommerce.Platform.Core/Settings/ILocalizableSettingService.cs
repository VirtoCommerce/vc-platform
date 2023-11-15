using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings;

public interface ILocalizableSettingService
{
    Task<LocalizableSettingsAndLanguages> GetSettingsAndLanguagesAsync();
    Task<string> TranslateAsync(string key, string settingName, string languageCode);
    Task<IList<KeyValue>> GetValuesAsync(string settingName, string languageCode);
    Task SaveAsync(string settingName, IList<DictionaryItem> items);
    Task DeleteAsync(string settingName, IList<string> values);
}
