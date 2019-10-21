using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;

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
        private readonly IModuleCatalog _moduleCatalog;
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly ICacheManager<object> _cacheManager;
        private readonly ManifestModuleInfo[] _predefinedModules;
        private readonly IDictionary<string, List<SettingEntry>> _runtimeModuleSettingsMap = new Dictionary<string, List<SettingEntry>>();

        [CLSCompliant(false)]
        public SettingsManager(IModuleCatalog moduleCatalog, Func<IPlatformRepository> repositoryFactory, ICacheManager<object> cacheManager, ManifestModuleInfo[] predefinedModules)
        {
            _moduleCatalog = moduleCatalog;
            _repositoryFactory = repositoryFactory;
            _cacheManager = cacheManager;
            _predefinedModules = predefinedModules ?? new ManifestModuleInfo[0];
        }

        #region ISettingsManager Members

        public ManifestModuleInfo[] GetModules()
        {
            var retVal = GetModulesWithSettings().ToArray();
            return retVal;
        }

        public SettingEntry GetSettingByName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            var result = _cacheManager.Get($"GetSettingByName-{name}", SettingConstants.CacheRegion, () =>
             {
                 SettingEntry setting = null;
                 //Get setting definition from module manifest first
                 var moduleSetting = GetModuleSettingByName(name);
                 if (moduleSetting != null)
                 {
                     setting = moduleSetting.ToSettingEntry();
                 }
                 using (var repository = _repositoryFactory())
                 {
                     //try to load setting from db
                     var settingEntity = repository.GetSettingByName(name);
                     if (settingEntity != null)
                     {
                         setting = settingEntity.ToModel(setting ?? AbstractTypeFactory<SettingEntry>.TryCreateInstance());
                     }
                 }
                 return setting;
             });
            return result;
        }

        public void LoadEntitySettingsValues(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (entity.IsTransient())
                throw new ArgumentException("entity transistent");

            var entityType = entity.GetType().Name;
            var storedSettings = _cacheManager.Get($"GetObjectSettings-{entityType}-{entity.Id}", SettingConstants.CacheRegion, () =>
           {
               using (var repository = _repositoryFactory())
               {
                   return repository.GetAllObjectSettings(entityType, entity.Id)
                                           .Select(x => x.ToModel(AbstractTypeFactory<SettingEntry>.TryCreateInstance())).ToList();
               }
           });
            //Deep load settings values for all object contains settings
            var haveSettingsObjects = entity.GetFlatObjectsListWithInterface<IHaveSettings>();
            foreach (var haveSettingsObject in haveSettingsObjects)
            {
                // Replace settings values with stored in database
                if (haveSettingsObject.Settings != null)
                {
                    //Need clone settings entry because it may be shared for multiple instances
                    haveSettingsObject.Settings = haveSettingsObject.Settings.Select(x => (SettingEntry)x.Clone()).ToList();

                    foreach (var setting in haveSettingsObject.Settings)
                    {
                        var storedSetting = storedSettings.FirstOrDefault(x => x.Name.EqualsInvariant(setting.Name));
                        //First try to used stored object setting values
                        if (storedSetting != null)
                        {
                            setting.Value = storedSetting.Value;
                            setting.ArrayValues = storedSetting.ArrayValues;
                        }
                        if (setting.Value == null && setting.ArrayValues == null)
                        {
                            //try to use global setting value
                            var globalSetting = GetSettingByName(setting.Name);
                            var defaultValue = (globalSetting ?? setting).DefaultValue;

                            if (setting.IsArray)
                            {
                                setting.ArrayValues = globalSetting?.ArrayValues ?? new[] { defaultValue };
                            }
                            else
                            {
                                setting.Value = globalSetting?.Value ?? defaultValue;
                            }
                        }
                    }
                }
            }
        }

        public void SaveEntitySettingsValues(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (entity.IsTransient())
                throw new ArgumentException("entity transistent");

            var objectType = entity.GetType().Name;

            var haveSettingsObjects = entity.GetFlatObjectsListWithInterface<IHaveSettings>();

            foreach (var haveSettingsObject in haveSettingsObjects)
            {
                var settings = new List<SettingEntry>();

                if (haveSettingsObject.Settings != null)
                {
                    //Save settings
                    foreach (var setting in haveSettingsObject.Settings)
                    {
                        setting.ObjectId = entity.Id;
                        setting.ObjectType = objectType;
                        settings.Add(setting);
                    }
                }
                SaveSettings(settings.ToArray());
            }
        }

        public void RemoveEntitySettings(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (entity.IsTransient())
                throw new ArgumentException("entity transistent", "entity");

            var objectType = entity.GetType().Name;
            using (var repository = _repositoryFactory())
            {
                var settings = repository.GetAllObjectSettings(objectType, entity.Id).ToArray();
                foreach (var setting in settings)
                {
                    repository.Remove(setting);
                }
                repository.UnitOfWork.Commit();
                ClearCache(settings.Select(x => x.ToModel(AbstractTypeFactory<SettingEntry>.TryCreateInstance())).ToArray());
            }
        }

        public SettingEntry[] GetModuleSettings(string moduleId)
        {
            var result = new List<SettingEntry>();

            var moduleManifest = GetModulesWithSettings().FirstOrDefault(m => m.Id == moduleId);

            if (moduleManifest != null && moduleManifest.Settings != null && moduleManifest.Settings.Any())
            {
                //Load settings from requested module manifest with values from database
                foreach (var group in moduleManifest.Settings)
                {
                    if (group.Settings != null)
                    {
                        foreach (var setting in group.Settings)
                        {
                            var settingEntry = GetSettingByName(setting.Name);
                            settingEntry.GroupName = group.Name;
                            settingEntry.ModuleId = moduleId;
                            result.Add(settingEntry);
                        }
                    }
                }
                //Try add runtime defined settings for requested module
                if (!string.IsNullOrEmpty(moduleId))
                {
                    List<SettingEntry> runtimeSettings;
                    if (_runtimeModuleSettingsMap.TryGetValue(moduleId, out runtimeSettings))
                    {
                        result.AddRange(runtimeSettings);
                    }
                }
            }
            return result.OrderBy(x => x.Name).ToArray();
        }

        /// <summary>
        /// Register module settings runtime
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="settings"></param>
        public void RegisterModuleSettings(string moduleId, params SettingEntry[] settings)
        {
            //check module exist
            if (!GetModules().Any(x => x.Id == moduleId))
            {
                throw new ArgumentException(moduleId + " not exist");
            }
            List<SettingEntry> moduleSettings;
            if (!_runtimeModuleSettingsMap.TryGetValue(moduleId, out moduleSettings))
            {
                moduleSettings = new List<SettingEntry>();
                _runtimeModuleSettingsMap[moduleId] = moduleSettings;
            }
            foreach (var setting in settings)
            {
                var clonedSetting = setting.Clone() as SettingEntry;
                clonedSetting.IsRuntime = true;
                moduleSettings.Add(clonedSetting);
            }
        }

        public void SaveSettings(SettingEntry[] settings)
        {
            if (settings != null && settings.Any())
            {
                var settingKeys = settings.Select(x => x.GenerateKey()).Distinct().ToArray();

                using (var repository = _repositoryFactory())
                using (var changeTracker = new ObservableChangeTracker())
                {
                    var alreadyExistSettings = repository.Settings
                        .Include(s => s.SettingValues)
                        .Where(x => settingKeys.Contains(x.Name + "-" + x.ObjectType + "-" + x.ObjectId))
                        .ToList();

                    changeTracker.AddAction = x => repository.Add(x);
                    //Need for real remove object from nested collection (because EF default remove references only)
                    changeTracker.RemoveAction = x => repository.Remove(x);

                    var target = new { Settings = new ObservableCollection<SettingEntity>(alreadyExistSettings) };
                    var source = new { Settings = new ObservableCollection<SettingEntity>(settings.Select(x => AbstractTypeFactory<SettingEntity>.TryCreateInstance().FromModel(x))) };

                    changeTracker.Attach(target);
                    var settingComparer = AnonymousComparer.Create((SettingEntity x) => x.GenerateKey());
                    source.Settings.Patch(target.Settings, settingComparer, (sourceSetting, targetSetting) => sourceSetting.Patch(targetSetting));

                    repository.UnitOfWork.Commit();
                }

                ClearCache(settings);
                SetRuntimeSettingValues(settings);
            }
        }

        public T[] GetArray<T>(string name, T[] defaultValue)
        {
            var result = defaultValue;

            // Check environment variables and appSettings first
            var stringValues = ConfigurationHelper.SplitNullableAppSettingsStringValue(name);

            if (stringValues != null)
            {
                result = stringValues.Select(ConvertFromString<T>).ToArray();
            }
            else
            {
                var setting = GetSettingByName(name);

                if (setting != null)
                {
                    if (!setting.RawArrayValues.IsNullOrEmpty())
                    {
                        result = setting.RawArrayValues.Cast<T>().ToArray();
                    }
                    else if (setting.RawValue != null)
                    {
                        result = new[] { (T)setting.RawValue };
                    }
                    else if (setting.RawDefaultValue != null)
                    {
                        result = new[] { (T)setting.RawDefaultValue };
                    }
                }
            }

            return result;
        }

        public T GetValue<T>(string name, T defaultValue)
        {
            var result = defaultValue;

            var values = GetArray(name, new[] { defaultValue });

            if (values.Any())
            {
                result = values.First();
            }

            return result;
        }

        public void SetValue<T>(string name, T value)
        {
            var type = typeof(T);
            var setting = AbstractTypeFactory<SettingEntry>.TryCreateInstance();
            setting.Name = name;

            if (type.IsArray)
            {
                setting.IsArray = true;
                setting.ValueType = type.GetElementType().ToSettingValueType();
                if (value != null)
                {
                    setting.ArrayValues = ((IEnumerable)value).OfType<object>()
                                        .Select(v => v == null ? null : string.Format(CultureInfo.InvariantCulture, "{0}", v))
                                        .ToArray();
                }
            }
            else
            {
                setting.ValueType = type.ToSettingValueType();
                setting.Value = value == null ? null : string.Format(CultureInfo.InvariantCulture, "{0}", value);
            }
            SaveSettings(new[] { setting });
        }

        #endregion

        private void SetRuntimeSettingValues(SettingEntry[] settings)
        {
            var runtimeSettingsByKey = _runtimeModuleSettingsMap.Values.SelectMany(x => x).ToDictionary(y => y.GenerateKey());

            foreach (var setting in settings)
            {
                if (runtimeSettingsByKey.TryGetValue(setting.GenerateKey(), out var runtimeSetting))
                {
                    runtimeSetting.Value = setting.Value;
                }
            }
        }

        private static T ConvertFromString<T>(string stringValue)
        {
            T result;

            var type = typeof(T);
            var isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable && string.IsNullOrWhiteSpace(stringValue))
            {
                result = (T)(object)null;
            }
            else
            {
                if (isNullable)
                {
                    type = Nullable.GetUnderlyingType(type);
                }

                result = (T)Convert.ChangeType(stringValue, type, CultureInfo.InvariantCulture);
            }

            return result;
        }

        private void ClearCache(SettingEntry[] settings)
        {
            //Clear setting from cache
            foreach (var setting in settings)
            {
                _cacheManager.Remove($"GetSettingByName-{setting.Name}", SettingConstants.CacheRegion);
            }
            //Clear
            foreach (var key in settings.Select(x => $"GetObjectSettings-{x.ObjectType}-{x.ObjectId}").Distinct())
            {
                _cacheManager.Remove(key, SettingConstants.CacheRegion);
            }
        }

        private IEnumerable<ManifestModuleInfo> GetModulesWithSettings()
        {
            return _moduleCatalog.Modules.OfType<ManifestModuleInfo>().Union(_predefinedModules)
                .Where(m => !m.Settings.IsNullOrEmpty());
        }

        private ModuleSetting GetModuleSettingByName(string name)
        {
            return GetAllModulesSettings().FirstOrDefault(s => s.Name == name);
        }

        private IEnumerable<ModuleSetting> GetAllModulesSettings()
        {
            return GetModulesWithSettings().SelectMany(m => m.Settings)
                .Where(g => g.Settings != null).SelectMany(g => g.Settings);
        }
    }
}
