using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Exceptions;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Core.Settings.Events;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Validators;

namespace VirtoCommerce.Platform.Data.Settings
{
    /// <summary>
    /// Provide next functionality to working with settings
    /// - Load settings meta information from module manifest and database
    /// - Deep load all settings for entity
    /// - Mass update all entity settings
    /// </summary>
    public class SettingsManager : ISettingsManager
    {
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly IPlatformMemoryCache _memoryCache;
        private readonly IDictionary<string, SettingDescriptor> _registeredSettingsByNameDict = new Dictionary<string, SettingDescriptor>(StringComparer.OrdinalIgnoreCase).WithDefaultValue(null);
        private readonly IDictionary<string, IEnumerable<SettingDescriptor>> _registeredTypeSettingsByNameDict = new Dictionary<string, IEnumerable<SettingDescriptor>>(StringComparer.OrdinalIgnoreCase).WithDefaultValue(null);
        private readonly IEventPublisher _eventPublisher;
        private readonly Dictionary<string, ObjectSettingEntry> _fixedSettingsDict;
        private readonly ISettingsOverrideProvider _overrideProvider;

        public SettingsManager(Func<IPlatformRepository> repositoryFactory,
            IPlatformMemoryCache memoryCache,
            IEventPublisher eventPublisher,
            IOptions<FixedSettings> fixedSettings,
            ISettingsOverrideProvider overrideProvider)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
            _eventPublisher = eventPublisher;
            _overrideProvider = overrideProvider;

            _fixedSettingsDict = fixedSettings.Value.Settings?.ToDictionary(x => x.Name, x => x, StringComparer.OrdinalIgnoreCase)
                                 ?? new Dictionary<string, ObjectSettingEntry>(StringComparer.OrdinalIgnoreCase);
        }

        #region ISettingsRegistrar Members

        public void RegisterSettingsForType(IEnumerable<SettingDescriptor> settings, string typeName)
        {
            ArgumentNullException.ThrowIfNull(settings);

            var existTypeSettings = _registeredTypeSettingsByNameDict[typeName];
            if (existTypeSettings != null)
            {
                settings = existTypeSettings.Concat(settings).Distinct().ToList();
            }
            _registeredTypeSettingsByNameDict[typeName] = settings;
        }

        public IEnumerable<SettingDescriptor> GetSettingsForType(string typeName)
        {
            return _registeredTypeSettingsByNameDict[typeName] ?? [];
        }

        public IEnumerable<SettingDescriptor> AllRegisteredSettings => _registeredSettingsByNameDict.Values;

        public void RegisterSettings(IEnumerable<SettingDescriptor> settings, string moduleId = null)
        {
            ArgumentNullException.ThrowIfNull(settings);

            foreach (var setting in settings)
            {
                setting.ModuleId = moduleId;
                _registeredSettingsByNameDict[setting.Name] = setting;
            }
        }

        #endregion ISettingsRegistrar Members

        #region ISettingsManager Members

        public virtual async Task<ObjectSettingEntry> GetObjectSettingAsync(string name, string objectType = null, string objectId = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            return (await GetObjectSettingsAsync([name], objectType, objectId)).FirstOrDefault();
        }

        public virtual async Task<IEnumerable<ObjectSettingEntry>> GetObjectSettingsAsync(IEnumerable<string> names, string objectType = null, string objectId = null)
        {
            ArgumentNullException.ThrowIfNull(names);

            var settingNames = names as string[] ?? names.ToArray();
            var cacheKey = CacheKey.With(GetType(), "GetSettingByNamesAsync", string.Join(";", settingNames), objectType, objectId);
            var result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheEntry =>
            {
                var resultObjectSettings = new List<ObjectSettingEntry>();
                var dbStoredSettings = new List<SettingEntity>();

                //Try to load setting value from DB
                using (var repository = _repositoryFactory())
                {
                    repository.DisableChangesTracking();
                    //try to load setting from db
                    dbStoredSettings.AddRange(await repository.GetObjectSettingsByNamesAsync(settingNames, objectType, objectId));
                }

                foreach (var name in settingNames)
                {
                    var objectSetting = _fixedSettingsDict.ContainsKey(name)
                        ? GetFixedSetting(name)
                        : GetSettingWithOverrides(name, dbStoredSettings, objectType, objectId);

                    resultObjectSettings.Add(objectSetting);

                    //Add cache  expiration token for setting
                    cacheEntry.AddExpirationToken(SettingsCacheRegion.CreateChangeToken(objectSetting));
                }

                return resultObjectSettings;
            });

            return result;
        }

        public virtual async Task RemoveObjectSettingsAsync(IEnumerable<ObjectSettingEntry> objectSettings)
        {
            ArgumentNullException.ThrowIfNull(objectSettings);

            var settingEntries = objectSettings as ObjectSettingEntry[] ?? objectSettings.ToArray();
            using (var repository = _repositoryFactory())
            {
                foreach (var objectSetting in settingEntries)
                {
                    var dbSetting = repository.Settings.FirstOrDefault(x =>
                        x.Name == objectSetting.Name && x.ObjectType == objectSetting.ObjectType &&
                        x.ObjectId == objectSetting.ObjectId);
                    if (dbSetting != null)
                    {
                        repository.Remove(dbSetting);
                    }
                }

                await repository.UnitOfWork.CommitAsync();
            }

            ClearCache(settingEntries);
        }

        public virtual async Task SaveObjectSettingsAsync(IEnumerable<ObjectSettingEntry> objectSettings)
        {
            ArgumentNullException.ThrowIfNull(objectSettings);

            var changedEntries = new List<GenericChangedEntry<ObjectSettingEntry>>();

            // Ignore unregistered settings, fixed settings, and settings without values
            var settings = objectSettings
                .Where(x => _registeredSettingsByNameDict.ContainsKey(x.Name) &&
                            !_fixedSettingsDict.ContainsKey(x.Name) &&
                            x.ItHasValues)
                .ToArray();

            // If a setting is forced by configuration, ignore incoming value and restore to config
            // by removing any persisted DB override for that scope.
            // This makes "Save" idempotent and admin-friendly (no errors, value simply stays as configured).
            var forcedSettings = Array.Empty<ObjectSettingEntry>();
            if (_overrideProvider != null && settings.Length != 0)
            {
                forcedSettings = settings
                    .Where(s =>
                    {
                        var descriptor = _registeredSettingsByNameDict[s.Name];
                        return descriptor != null && _overrideProvider.TryGetCurrentValue(descriptor, s.ObjectType, s.ObjectId, out _);
                    })
                    .ToArray();

                if (forcedSettings.Length != 0)
                {
                    settings = settings.Except(forcedSettings).ToArray();
                }
            }

            using (var repository = _repositoryFactory())
            {
                // First: restore forced settings by deleting any DB overrides for that scope
                foreach (var forced in forcedSettings)
                {
                    var dbSetting = await repository.Settings
                        .Include(x => x.SettingValues)
                        .FirstOrDefaultAsync(x => x.Name == forced.Name && x.ObjectType == forced.ObjectType && x.ObjectId == forced.ObjectId);

                    if (dbSetting != null)
                    {
                        var descriptor = _registeredSettingsByNameDict[forced.Name];
                        var oldEntry = dbSetting.ToModel(new ObjectSettingEntry(descriptor));
                        repository.Remove(dbSetting);
                        changedEntries.Add(new GenericChangedEntry<ObjectSettingEntry>(oldEntry, EntryState.Deleted));
                    }
                }

                if (forcedSettings.Length != 0)
                {
                    // Ensure cache invalidation for forced scope values as well
                    ClearCache(forcedSettings);
                }

                var settingNames = settings.Select(x => x.Name).Distinct().ToArray();

                var alreadyExistDbSettings = await repository.Settings
                    .Include(s => s.SettingValues)
                    .Where(x => settingNames.Contains(x.Name))
                    .AsSplitQuery()
                    .ToListAsync();

                var validator = new ObjectSettingEntryValidator();

                foreach (var setting in settings)
                {
                    var settingDescriptor = _registeredSettingsByNameDict[setting.Name];

                    if (!(await validator.ValidateAsync(setting)).IsValid)
                    {
                        throw new PlatformException($"Setting with name {setting.Name} is invalid");
                    }

                    // We need to convert resulting DB entities to model. Use ValueObject.Equals to find already saved setting entity from passed setting
                    var originalEntity = alreadyExistDbSettings.FirstOrDefault(x =>
                        x.Name.EqualsIgnoreCase(setting.Name) &&
                        x.ToModel(new ObjectSettingEntry(settingDescriptor)).Equals(setting));

                    var modifiedEntity = AbstractTypeFactory<SettingEntity>.TryCreateInstance().FromModel(setting);

                    if (originalEntity != null)
                    {
                        var oldEntry = originalEntity.ToModel(new ObjectSettingEntry(settingDescriptor));

                        modifiedEntity.Patch(originalEntity);

                        var newEntry = originalEntity.ToModel(new ObjectSettingEntry(settingDescriptor));
                        changedEntries.Add(new GenericChangedEntry<ObjectSettingEntry>(newEntry, oldEntry, EntryState.Modified));
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                        changedEntries.Add(new GenericChangedEntry<ObjectSettingEntry>(setting, EntryState.Added));
                    }
                }

                await repository.UnitOfWork.CommitAsync();
            }

            ClearCache(settings);

            await _eventPublisher.Publish(new ObjectSettingChangedEvent(changedEntries));
        }

        #endregion ISettingsManager Members

        protected virtual void ClearCache(IEnumerable<ObjectSettingEntry> objectSettings)
        {
            //Clear setting from cache
            foreach (var setting in objectSettings)
            {
                SettingsCacheRegion.ExpireSetting(setting);
            }
        }

        protected virtual ObjectSettingEntry GetRegularSetting(string name, List<SettingEntity> dbStoredSettings, string objectType, string objectId)
        {
            var settingDescriptor = _registeredSettingsByNameDict[name];
            if (settingDescriptor == null)
            {
                throw new PlatformException($"Setting with name {name} is not registered");
            }

            var objectSetting = new ObjectSettingEntry(settingDescriptor)
            {
                ObjectType = objectType,
                ObjectId = objectId
            };
            var dbSetting = dbStoredSettings.FirstOrDefault(x => x.Name.EqualsIgnoreCase(name));
            if (dbSetting != null)
            {
                objectSetting = dbSetting.ToModel(objectSetting);
            }

            return objectSetting;
        }

        protected virtual ObjectSettingEntry GetSettingWithOverrides(string name, List<SettingEntity> dbStoredSettings, string objectType, string objectId)
        {
            var settingDescriptor = _registeredSettingsByNameDict[name];
            if (settingDescriptor == null)
            {
                throw new PlatformException($"Setting with name {name} is not registered");
            }

            ObjectSettingEntry objectSetting;

            // Forced override from config wins (read-only)
            if (_overrideProvider != null &&
                _overrideProvider.TryGetCurrentValue(settingDescriptor, objectType, objectId, out var forced))
            {
                objectSetting = CreateOverriddenSetting(settingDescriptor, objectType, objectId, forced, isReadOnly: true);
            }
            else
            {
                // Otherwise load from DB (if present)
                objectSetting = GetRegularSetting(name, dbStoredSettings, objectType, objectId);
            }

            // Apply default value override 
            if (_overrideProvider != null &&
                _overrideProvider.TryGetDefaultValue(settingDescriptor, objectType, objectId, out var @default))
            {
                objectSetting.DefaultValue = @default;
            }

            return objectSetting;
        }

        private static ObjectSettingEntry CreateOverriddenSetting(SettingDescriptor descriptor, string objectType, string objectId, object rawValue, bool isReadOnly)
        {
            var entry = new ObjectSettingEntry(descriptor)
            {
                ObjectType = objectType,
                ObjectId = objectId,
                IsReadOnly = isReadOnly
            };

            if (entry.IsDictionary)
            {
                entry.AllowedValues = rawValue as object[] ?? Array.Empty<object>();
            }
            else
            {
                entry.Value = rawValue;
            }

            return entry;
        }

        protected virtual ObjectSettingEntry GetFixedSetting(string name)
        {
            var entry = _fixedSettingsDict[name];
            entry.IsReadOnly = true;

            entry.Value = ConvertValueType(entry.Value, entry.ValueType);
            entry.DefaultValue = ConvertValueType(entry.DefaultValue, entry.ValueType);

            return entry;
        }

        private static object ConvertValueType(object value, SettingValueType valueType)
        {
            value = valueType switch
            {
                SettingValueType.Boolean => Convert.ToBoolean(value),
                SettingValueType.DateTime => Convert.ToDateTime(value),
                SettingValueType.Decimal => Convert.ToDecimal(value, CultureInfo.InvariantCulture),
                SettingValueType.Integer or SettingValueType.PositiveInteger => Convert.ToInt32(value, CultureInfo.InvariantCulture),
                _ => Convert.ToString(value)
            };

            return value;
        }
    }
}
