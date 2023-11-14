using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Settings;

public interface ILocalizableSettingService
{
    Task<LocalizableSettingsAndLanguages> GetSettingsAndLanguagesAsync();
    Task<IList<KeyValue>> GetValuesAsync(string name, string languageCode);
    Task SaveAsync(string name, IList<DictionaryItem> items);
    Task DeleteAsync(string name, IList<string> values);
}
