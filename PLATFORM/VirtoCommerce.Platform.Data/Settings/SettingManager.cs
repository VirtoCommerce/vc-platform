using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using CacheManager.Core;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Common;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings.Converters;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly ICacheManager<object> _cacheManager;
        private readonly ModuleManifest[] _predefinedManifests;
        private readonly IDictionary<string, List<SettingEntry>> _runtimeModuleSettingsMap = new Dictionary<string, List<SettingEntry>>();

        [CLSCompliant(false)]
        public SettingsManager(IModuleManifestProvider manifestProvider, Func<IPlatformRepository> repositoryFactory, ICacheManager<object> cacheManager, ModuleManifest[] predefinedManifests)
        {
            _manifestProvider = manifestProvider;
            _repositoryFactory = repositoryFactory;
            _cacheManager = cacheManager;
            _predefinedManifests = predefinedManifests ?? new ModuleManifest[0];
        }

        #region ISettingsManager Members

        public ModuleDescriptor[] GetModules()
        {
            var retVal = GetModuleManifestsWithSettings()
                .Select(x => x.ToModel())
                .ToArray();

            return retVal;
        }

        public SettingEntry GetSettingByName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            SettingEntry retVal = null;
            var manifestSetting = LoadSettingFromManifest(name);
            var storedSetting = GetAllEntities().FirstOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));
            if (manifestSetting != null)
            {
                retVal = manifestSetting.ToModel(storedSetting, null);
            }
            return retVal;
        }

        public void LoadEntitySettingsValues(Entity entity)
        {
            if (entity == null)
               throw new ArgumentNullException("entity");

            if (entity.IsTransient())
                throw new ArgumentException("entity transistent");

            var storedSettings = new List<SettingEntry>();
            var entityType = entity.GetType().Name;
            using (var repository = _repositoryFactory())
            {
                var settings = repository.Settings
                    .Include(s => s.SettingValues)
                    .Where(x => x.ObjectId == entity.Id && x.ObjectType == entityType)
                    .OrderBy(x => x.Name)
                    .ToList();

                storedSettings.AddRange(settings.Select(x => x.ToModel()));
            }

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
                        var storedSetting = storedSettings.FirstOrDefault(x => String.Equals(x.Name, setting.Name, StringComparison.InvariantCultureIgnoreCase));
                        //First try to used stored object setting values
                        if (storedSetting != null)
                        {
                            setting.Value = storedSetting.Value;
                            setting.ArrayValues = storedSetting.ArrayValues;
                        }
                        else if(setting.Value == null && setting.ArrayValues == null)
                        {
                            //try to use global setting value
                            var globalSetting = GetSettingByName(setting.Name);
                            if (setting.IsArray)
                            {
                                setting.ArrayValues = globalSetting.ArrayValues ?? new[] { globalSetting.DefaultValue };
                            }
                            else
                            {
                                setting.Value = globalSetting.Value ?? globalSetting.DefaultValue;
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
            if (entity == null)
                throw new ArgumentNullException("entity transistent");

            var objectType = entity.GetType().Name;
            using (var repository = _repositoryFactory())
            {
                var settings = repository.Settings.Include(s => s.SettingValues)
                                                  .Where(x => x.ObjectId == entity.Id && x.ObjectType == objectType).ToList();
                foreach (var setting in settings)
                {
                    repository.Remove(setting);
                }
                repository.UnitOfWork.Commit();
            }

        }

        public SettingEntry[] GetModuleSettings(string moduleId)
        {
            var result = new List<SettingEntry>();

            var manifest = GetModuleManifestsWithSettings().FirstOrDefault(m => m.Id == moduleId);

            if (manifest != null && manifest.Settings != null && manifest.Settings.Any())
            {
                var settingEntities = GetAllEntities();
                //Load settings from requested module manifest with values from database
                foreach (var group in manifest.Settings)
                {
                    if (group.Settings != null)
                    {
                        foreach (var setting in group.Settings)
                        {
                            var dbSetting = settingEntities.FirstOrDefault(x => x.Name == setting.Name && x.ObjectId == null);

                            var settingEntry = setting.ToModel(dbSetting, group.Name);
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
        public void  RegisterModuleSettings(string moduleId, params SettingEntry[] settings)
        {
            //check module exist
            if(!GetModules().Any(x=>x.Id == moduleId))
            {
                throw new ArgumentException(moduleId + " not exist");
            }
            List<SettingEntry> moduleSettings;
            if(!_runtimeModuleSettingsMap.TryGetValue(moduleId, out moduleSettings))
            {
                moduleSettings = new List<SettingEntry>();
                _runtimeModuleSettingsMap[moduleId] = moduleSettings;
            }
            moduleSettings.AddRange(settings);
        }

        public void SaveSettings(SettingEntry[] settings)
        {
            if (settings != null && settings.Any())
            {
                var settingKeys = settings.Select(x => String.Join("-", x.Name, x.ObjectType, x.ObjectId)).Distinct().ToArray();

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
                    var source = new { Settings = new ObservableCollection<SettingEntity>(settings.Select(x => x.ToEntity())) };

                    changeTracker.Attach(target);
                    var settingComparer = AnonymousComparer.Create((SettingEntity x) => String.Join("-", x.Name, x.ObjectType, x.ObjectId));
                    source.Settings.Patch(target.Settings, settingComparer, (sourceSetting, targetSetting) => sourceSetting.Patch(targetSetting));

                    repository.UnitOfWork.Commit();
                }

                ClearCache();
            }
        }

        public T[] GetArray<T>(string name, T[] defaultValue)
        {
            var result = defaultValue;

            var repositorySetting = GetAllEntities()
                .FirstOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));

            if (repositorySetting != null)
            {
                result = repositorySetting.SettingValues
                    .Select(v => v.RawValue())
                    .Where(rv => rv != null)
                    .Select(rv => (T)rv)
                    .ToArray();
            }
            else
            {
                var manifestSetting = LoadSettingFromManifest(name);

                if (manifestSetting != null)
                {
                    if (manifestSetting.ArrayValues != null)
                    {
                        result = manifestSetting.ArrayValues
                            .Select(v => (T)manifestSetting.RawValue(v))
                            .ToArray();
                    }
                    else if (manifestSetting.DefaultValue != null)
                    {
                        result = new[] { (T)manifestSetting.RawDefaultValue() };
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
            var setting = name.ToModel(value);
            SaveSettings(new[] { setting });
        }

        #endregion


        private IEnumerable<ModuleManifest> GetModuleManifestsWithSettings()
        {
            return _manifestProvider.GetModuleManifests().Values
                .Union(_predefinedManifests)
                .Where(m => m.Settings != null && m.Settings.Any());
        }

        private ModuleSetting LoadSettingFromManifest(string name)
        {
            return GetAllManifestSettings().FirstOrDefault(s => s.Name == name);
        }

        private IEnumerable<ModuleSetting> GetAllManifestSettings()
        {
            return GetModuleManifestsWithSettings()
                .SelectMany(m => m.Settings)
                .SelectMany(g => g.Settings);
        }

        private List<SettingEntity> GetAllEntities()
        {
            var result = _cacheManager.Get("AllSettings", "PlatformRegion", LoadAllEntities);
            return result;
        }

        private List<SettingEntity> LoadAllEntities()
        {
            using (var repository = _repositoryFactory())
            {
                return repository.Settings
                    .Where(x => x.ObjectType == null && x.ObjectId == null)
                    .Include(s => s.SettingValues)
                    .ToList();
            }
        }

        private void ClearCache()
        {
            _cacheManager.Remove("AllSettings", "PlatformRegion");
        }
    }
}
