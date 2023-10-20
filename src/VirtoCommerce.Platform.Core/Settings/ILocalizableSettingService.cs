using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Settings;

public interface ILocalizableSettingService
{
    IList<string> GetNames();
    Task<IList<KeyValue>> GetValuesAsync(string name, string languageCode);
    Task<DictionaryItemsAndLanguages> GetItemsAndLanguagesAsync(string name);
    Task SaveAsync(string name, IList<DictionaryItem> items);
    Task DeleteAsync(string name, IList<string> values);
}
