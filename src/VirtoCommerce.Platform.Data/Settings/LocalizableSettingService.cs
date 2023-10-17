using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Localizations;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.Platform.Data.Settings;

public class LocalizableSettingService : ILocalizableSettingService
{
    private static readonly StringComparer _ignoreCase = StringComparer.OrdinalIgnoreCase;

    private readonly ISettingsManager _settingsManager;
    private readonly ILocalizedItemSearchService _localizedItemSearchService;
    private readonly ILocalizedItemService _localizedItemService;

    public LocalizableSettingService(
        ISettingsManager settingsManager,
        ILocalizedItemSearchService localizedItemSearchService,
        ILocalizedItemService localizedItemService)
    {
        _settingsManager = settingsManager;
        _localizedItemSearchService = localizedItemSearchService;
        _localizedItemService = localizedItemService;
    }

    public virtual IList<string> GetNames()
    {
        return _settingsManager.AllRegisteredSettings
            .Where(IsLocalizable)
            .Select(x => x.Name)
            .OrderBy(x => x)
            .ToList();
    }

    public virtual async Task<IList<KeyValue>> GetValuesAsync(string name, string languageCode)
    {
        var result = await GetItemsAndLanguagesAsync(name);

        return result.Items
            .Select(x => new KeyValue
            {
                Key = x.Alias,
                Value = GetValue(x.LocalizedValues, languageCode).EmptyToNull() ?? x.Alias,
            })
            .ToList();
    }

    public virtual async Task<DictionaryItemsAndLanguages> GetItemsAndLanguagesAsync(string name)
    {
        // Load setting values
        var values = await GetDictionaryValues(name);

        if (values is null)
        {
            return null;
        }

        // Load localization
        var localizedValues = (await GetLocalizedItems(name))
            .GroupBy(x => x.Alias)
            .ToDictionary(
                g => g.Key,
                g => g
                    .Select(x => new LocalizedValue
                    {
                        LanguageCode = x.LanguageCode,
                        Value = x.Value
                    })
                    .ToArray());

        return new DictionaryItemsAndLanguages
        {
            Items = values
                .Select(x => new DictionaryItem
                {
                    Alias = x,
                    LocalizedValues = localizedValues.GetValueSafe(x) ?? Array.Empty<LocalizedValue>(),
                })
                .ToList(),
            Languages = await GetDictionaryValues(PlatformConstants.Settings.General.Languages.Name),
        };
    }

    public virtual async Task SaveAsync(string name, IList<DictionaryItem> items)
    {
        // Save setting values
        var setting = await GetLocalizableSetting(name);

        if (setting is null)
        {
            return;
        }

        setting.AllowedValues = items
            .Where(x => !string.IsNullOrWhiteSpace(x.Alias))
            .Select(x => x.Alias as object)
            .ToArray();

        await SaveSetting(setting);

        // Save localization
        var itemsToSave = new List<LocalizedItem>();
        var itemIdsToDelete = new List<string>();

        var itemsByAlias = items
            .GroupBy(x => x.Alias, _ignoreCase)
            .ToDictionary(
                g => g.Key,
                g => g
                    .SelectMany(x => x.LocalizedValues)
                    .GroupBy(g2 => g2.LanguageCode)
                    .ToDictionary(
                        g2 => g2.Key,
                        g2 => g2.First().Value,
                        _ignoreCase),
                _ignoreCase);

        var localizedItemsByAlias = (await GetLocalizedItems(name))
            .GroupBy(x => x.Alias, _ignoreCase)
            .ToDictionary(
                g => g.Key,
                g => g.ToDictionary(x => x.LanguageCode),
                _ignoreCase);

        // Delete
        foreach (var (alias, localizedItemsByLanguage) in localizedItemsByAlias)
        {
            if (itemsByAlias.TryGetValue(alias, out var valuesByLanguage))
            {
                foreach (var (language, localizedItem) in localizedItemsByLanguage)
                {
                    if (!valuesByLanguage.ContainsKey(language))
                    {
                        itemIdsToDelete.Add(localizedItem.Id);
                    }
                }
            }
            else
            {
                itemIdsToDelete.AddRange(localizedItemsByLanguage.Values.Select(x => x.Id));
            }
        }

        // Add or update
        foreach (var (alias, valuesByLanguage) in itemsByAlias)
        {
            var localizedItemsByLanguage = localizedItemsByAlias.GetValueSafe(alias);

            foreach (var (language, value) in valuesByLanguage)
            {
                var localizedItem = localizedItemsByLanguage?.GetValueSafe(language);

                if (localizedItem != null)
                {
                    localizedItem.Value = value;
                }
                else
                {
                    localizedItem = AbstractTypeFactory<LocalizedItem>.TryCreateInstance();
                    localizedItem.Name = name;
                    localizedItem.Alias = alias;
                    localizedItem.LanguageCode = language;
                    localizedItem.Value = value;
                }

                itemsToSave.Add(localizedItem);
            }
        }

        if (itemIdsToDelete.Any())
        {
            await _localizedItemService.DeleteAsync(itemIdsToDelete);
        }

        if (itemsToSave.Any())
        {
            await _localizedItemService.SaveChangesAsync(itemsToSave);
        }
    }

    public virtual async Task DeleteAsync(string name, IList<string> values)
    {
        // Delete setting values
        var setting = await GetLocalizableSetting(name);

        if (setting is null)
        {
            return;
        }

        setting.AllowedValues = setting.AllowedValues
            .Where(x => !values.Contains(x as string, _ignoreCase))
            .ToArray();

        await SaveSetting(setting);

        // Delete localization
        var localizedItems = await GetLocalizedItems(name, values);

        if (localizedItems.Any())
        {
            var ids = localizedItems.Select(x => x.Id).ToArray();
            await _localizedItemService.DeleteAsync(ids);
        }
    }


    private async Task SaveSetting(ObjectSettingEntry setting)
    {
        // Workaround for saving an empty list of allowed values
        if (setting.AllowedValues.IsNullOrEmpty() && setting.Value == null)
        {
            setting.Value = string.Empty;
        }

        await _settingsManager.SaveObjectSettingsAsync(new[] { setting });
    }

    private async Task<IList<string>> GetDictionaryValues(string name)
    {
        var setting = await GetDictionarySetting(name);

        return setting?.AllowedValues?.OfType<string>().ToList() ?? new List<string>();
    }

    private async Task<ObjectSettingEntry> GetLocalizableSetting(string name)
    {
        var setting = await GetDictionarySetting(name);

        return IsLocalizable(setting) ? setting : null;
    }

    private async Task<ObjectSettingEntry> GetDictionarySetting(string name)
    {
        var setting = await _settingsManager.GetObjectSettingAsync(name);

        return IsShortTextDictionary(setting) ? setting : null;
    }

    private static bool IsLocalizable(SettingDescriptor setting)
    {
        return setting.IsLocalizable && IsShortTextDictionary(setting);
    }

    private static bool IsShortTextDictionary(SettingDescriptor setting)
    {
        return
            setting != null &&
            setting.IsDictionary &&
            setting.ValueType == SettingValueType.ShortText;
    }

    private static string GetValue(IList<LocalizedValue> values, string languageCode)
    {
        var value = values.FirstOrDefault(x => x.LanguageCode.EqualsInvariant(languageCode));

        if (value != null)
        {
            return value.Value;
        }

        // If language code is a two-letters code
        languageCode += "-";
        value = values.FirstOrDefault(x => x.LanguageCode.StartsWith(languageCode, StringComparison.OrdinalIgnoreCase));

        return value?.Value;
    }

    private Task<IList<LocalizedItem>> GetLocalizedItems(string name, IList<string> aliases = null)
    {
        var criteria = AbstractTypeFactory<LocalizedItemSearchCriteria>.TryCreateInstance();
        criteria.Names = new[] { name };
        criteria.Aliases = aliases;

        return _localizedItemSearchService.SearchAllNoCloneAsync(criteria);
    }
}
