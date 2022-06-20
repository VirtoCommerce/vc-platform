using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    /// - Load setting metainformation from module manifest and database
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

        public SettingsManager(Func<IPlatformRepository> repositoryFactory, IPlatformMemoryCache memoryCache, IEventPublisher eventPublisher)
        {
            _repositoryFactory = repositoryFactory;
            _memoryCache = memoryCache;
            _eventPublisher = eventPublisher;
        }

        #region ISettingsRegistrar Members

        public void RegisterSettingsForType(IEnumerable<SettingDescriptor> settings, string typeName)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            var existTypeSettings = _registeredTypeSettingsByNameDict[typeName];
            if (existTypeSettings != null)
            {
                settings = existTypeSettings.Concat(settings).Distinct().ToList();
            }
            _registeredTypeSettingsByNameDict[typeName] = settings;
        }

        public IEnumerable<SettingDescriptor> GetSettingsForType(string typeName)
        {
            return _registeredTypeSettingsByNameDict[typeName] ?? Enumerable.Empty<SettingDescriptor>();
        }

        public IEnumerable<SettingDescriptor> AllRegisteredSettings => _registeredSettingsByNameDict.Values;

        public void RegisterSettings(IEnumerable<SettingDescriptor> settings, string moduleId = null)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
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
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            return (await GetObjectSettingsAsync(new[] { name }, objectType, objectId)).FirstOrDefault();
        }

        public virtual async Task<IEnumerable<ObjectSettingEntry>> GetObjectSettingsAsync(IEnumerable<string> names, string objectType = null, string objectId = null)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }
            var cacheKey = CacheKey.With(GetType(), "GetSettingByNamesAsync", string.Join(";", names), objectType, objectId);
            var result = await _memoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                var resultObjectSettings = new List<ObjectSettingEntry>();
                var dbStoredSettings = new List<SettingEntity>();

                //Try to load setting value from DB
                using (var repository = _repositoryFactory())
                {
                    repository.DisableChangesTracking();
                    //try to load setting from db
                    dbStoredSettings.AddRange(await repository.GetObjectSettingsByNamesAsync(names.ToArray(), objectType, objectId));
                }

                foreach (var name in names)
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
                    var dbSetting = dbStoredSettings.FirstOrDefault(x => x.Name.EqualsInvariant(name));
                    if (dbSetting != null)
                    {
                        objectSetting = dbSetting.ToModel(objectSetting);
                    }
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
            if (objectSettings == null)
            {
                throw new ArgumentNullException(nameof(objectSettings));
            }
            using (var repository = _repositoryFactory())
            {
                foreach (var objectSetting in objectSettings)
                {
                    var dbSetting = repository.Settings.FirstOrDefault(x => x.Name == objectSetting.Name && x.ObjectType == objectSetting.ObjectType && x.ObjectId == objectSetting.ObjectId);
                    if (dbSetting != null)
                    {
                        repository.Remove(dbSetting);
                    }
                }
                await repository.UnitOfWork.CommitAsync();
                ClearCache(objectSettings);
            }
        }

        public virtual async Task SaveObjectSettingsAsync(IEnumerable<ObjectSettingEntry> objectSettings)
        {
            if (objectSettings == null)
            {
                throw new ArgumentNullException(nameof(objectSettings));
            }

            var changedEntries = new List<GenericChangedEntry<ObjectSettingEntry>>();

            using (var repository = _repositoryFactory())
            {
                var settingNames = objectSettings.Select(x => x.Name).Distinct().ToArray();
                var alreadyExistDbSettings = (await repository.Settings
                    .Include(s => s.SettingValues)
                    .Where(x => settingNames.Contains(x.Name))
                    .ToListAsync());

                var validator = new ObjectSettingEntryValidator();
                foreach (var setting in objectSettings.Where(x => x.ItHasValues))
                {
                    if (!validator.Validate(setting).IsValid)
                    {
                        throw new PlatformException($"Setting with name {setting.Name} is invalid");
                    }

                    var settingDescriptor = _registeredSettingsByNameDict[setting.Name];
                    if (settingDescriptor == null)
                    {
                        throw new PlatformException($"Setting with name {setting.Name} is not registered");
                    }

                    // We need to convert resulting DB entities to model. Use ValueObject.Equals to find already saved setting entity from passed setting
                    var originalEntity = alreadyExistDbSettings.Where(x => x.Name.EqualsInvariant(setting.Name))
                                                               .FirstOrDefault(x => x.ToModel(new ObjectSettingEntry(settingDescriptor)).Equals(setting));

                    var modifiedEntity = AbstractTypeFactory<SettingEntity>.TryCreateInstance().FromModel(setting);

                    if (originalEntity != null)
                    {
                        changedEntries.Add(new GenericChangedEntry<ObjectSettingEntry>(setting, originalEntity.ToModel(AbstractTypeFactory<ObjectSettingEntry>.TryCreateInstance()), EntryState.Modified));
                        modifiedEntity.Patch(originalEntity);
                    }
                    else
                    {
                        repository.Add(modifiedEntity);
                        changedEntries.Add(new GenericChangedEntry<ObjectSettingEntry>(setting, EntryState.Added));
                    }
                }

                await repository.UnitOfWork.CommitAsync();
            }

            ClearCache(objectSettings);

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
    }
}
