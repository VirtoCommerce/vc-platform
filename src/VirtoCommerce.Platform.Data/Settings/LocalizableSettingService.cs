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

    public virtual async Task<LocalizableSettingsAndLanguages> GetSettingsAndLanguagesAsync()
    {
        var settings = _settingsManager.AllRegisteredSettings
            .Where(IsShortTextDictionary)
            .OrderBy(x => x.Name);

        var tasks = settings.Select(async x =>
            new LocalizableSetting
            {
                Name = x.Name,
                IsLocalizable = x.IsLocalizable,
                Items = await GetItems(x.Name),
            });

        return new LocalizableSettingsAndLanguages
        {
            Settings = (await Task.WhenAll(tasks)).Where(x => x.Items != null).ToList(),
            Languages = await GetLanguages(),
        };
    }

    public virtual async Task<string> TranslateAsync(string key, string settingName, string languageCode)
    {
        if (string.IsNullOrEmpty(settingName) || string.IsNullOrEmpty(languageCode))
        {
            return key;
        }

        var values = await GetValuesAsync(settingName, languageCode);

        return values
            ?.Where(x => x.Key.EqualsInvariant(key))
            .Select(x => x.Value)
            .FirstOrDefault()?.EmptyToNull() ?? key;
    }

    public virtual async Task<IList<KeyValue>> GetValuesAsync(string settingName, string languageCode)
    {
        var items = await GetItems(settingName);

        if (items is null)
        {
            return Array.Empty<KeyValue>();
        }

        var languages = await GetLanguages();

        if (languages.Contains(languageCode, _ignoreCase))
        {
            return items
                .Select(item => new KeyValue
                {
                    Key = item.Alias,
                    Value = item.LocalizedValues.FirstOrDefault(value => value.LanguageCode.EqualsInvariant(languageCode))?.Value.EmptyToNull() ?? item.Alias,
                })
                .ToList();
        }

        // If language code is a two-letter code
        var languagePrefix = languageCode + "-";
        const StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;

        if (languages.Any(x => x.StartsWith(languagePrefix, ignoreCase)))
        {
            return items
                .Select(item => new KeyValue
                {
                    Key = item.Alias,
                    Value = item.LocalizedValues.FirstOrDefault(value => value.LanguageCode.StartsWith(languagePrefix, ignoreCase))?.Value.EmptyToNull() ?? item.Alias,
                })
                .ToList();
        }

        return items
            .Select(item => new KeyValue
            {
                Key = item.Alias,
                Value = item.Alias,
            })
            .ToList();
    }

    public virtual async Task SaveAsync(string settingName, IList<DictionaryItem> items)
    {
        // Save setting values
        var setting = await GetLocalizableSetting(settingName);

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

        var localizedItemsByAlias = (await GetLocalizedItems(settingName))
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
                    if (localizedItem.Value != value)
                    {
                        localizedItem.Value = value;
                        itemsToSave.Add(localizedItem);
                    }
                }
                else
                {
                    localizedItem = AbstractTypeFactory<LocalizedItem>.TryCreateInstance();
                    localizedItem.Name = settingName;
                    localizedItem.Alias = alias;
                    localizedItem.LanguageCode = language;
                    localizedItem.Value = value;

                    itemsToSave.Add(localizedItem);
                }
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

    public virtual async Task DeleteAsync(string settingName, IList<string> values)
    {
        // Delete setting values
        var setting = await GetLocalizableSetting(settingName);

        if (setting is null)
        {
            return;
        }

        setting.AllowedValues = setting.AllowedValues
            .Where(x => !values.Contains(x as string, _ignoreCase))
            .ToArray();

        await SaveSetting(setting);

        // Delete localization
        var localizedItems = await GetLocalizedItems(settingName, values);

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

    private async Task<IList<string>> GetLanguages()
    {
        return await GetDictionaryValues(PlatformConstants.Settings.General.Languages.Name);
    }

    private async Task<IList<DictionaryItem>> GetItems(string settingName)
    {
        // Load setting values
        var values = await GetDictionaryValues(settingName);

        if (values is null)
        {
            return null;
        }

        // Load localization
        var localizedValues = (await GetLocalizedItems(settingName))
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

        return values
            .Select(x => new DictionaryItem { Alias = x, LocalizedValues = localizedValues.GetValueSafe(x) ?? Array.Empty<LocalizedValue>(), })
            .ToList();
    }

    private async Task<IList<string>> GetDictionaryValues(string settingName)
    {
        var setting = await GetDictionarySetting(settingName);

        return setting?.AllowedValues?.OfType<string>().OrderBy(x => x).ToList() ?? new List<string>();
    }

    private async Task<ObjectSettingEntry> GetLocalizableSetting(string settingName)
    {
        var setting = await GetDictionarySetting(settingName);

        return IsLocalizable(setting) ? setting : null;
    }

    private async Task<ObjectSettingEntry> GetDictionarySetting(string settingName)
    {
        var setting = await _settingsManager.GetObjectSettingAsync(settingName);

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

    private Task<IList<LocalizedItem>> GetLocalizedItems(string name, IList<string> aliases = null)
    {
        var criteria = AbstractTypeFactory<LocalizedItemSearchCriteria>.TryCreateInstance();
        criteria.Names = new[] { name };
        criteria.Aliases = aliases;

        return _localizedItemSearchService.SearchAllNoCloneAsync(criteria);
    }
}
