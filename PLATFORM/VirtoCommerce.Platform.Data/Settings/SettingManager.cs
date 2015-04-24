using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings.Converters;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Settings
{
	public class SettingsManager : ISettingsManager
	{
		private readonly IModuleManifestProvider _manifestProvider;
		private readonly Func<IPlatformRepository> _repositoryFactory;
		private readonly CacheManager _cacheManager;

		public SettingsManager(IModuleManifestProvider manifestProvider, Func<IPlatformRepository> repositoryFactory, CacheManager cacheManager = null)
		{
			_manifestProvider = manifestProvider;
			_repositoryFactory = repositoryFactory;
			_cacheManager = cacheManager;
		}

		#region ISettingsManager Members

		public ModuleDescriptor[] GetModules()
		{
			var retVal = GetModuleManifestsWithSettings()
				.Select(x=>x.ToModel())
				.ToArray();

			return retVal;
		}

		public SettingDescriptor[] GetSettings(string moduleId)
		{
			var result = new List<SettingDescriptor>();

			var manifest = GetModuleManifestsWithSettings().FirstOrDefault(m => m.Id == moduleId);

			if (manifest != null && manifest.Settings != null)
			{
				var settingNames = manifest.Settings.SelectMany(g => g.Settings)
													.Select(s => s.Name)
													.Distinct().ToArray();

				if (settingNames.Any())
				{
					var settingEntities = LoadSettings(settingNames);

					foreach (var group in manifest.Settings)
					{
						if (group.Settings != null)
						{
							foreach(var setting in group.Settings)
							{
								var settingEntity = settingEntities.FirstOrDefault(x => x.Name == setting.Name);
								var settingDescriptor = setting.ToModel(settingEntity);
								settingDescriptor.GroupName = group.Name;
								result.Add(settingDescriptor);
							}
						}
					}
				}
			}

			return result.ToArray();
		}

		public void SaveSettings(SettingDescriptor[] settings)
		{
			if (settings != null)
			{
				var settingNames = settings
					.Select(s => s.Name)
					.Distinct()
					.ToArray();

				var existingSettings = LoadSettings(settingNames);

				using (var repository = _repositoryFactory())
				{
					var targetSettings = new ObservableCollection<SettingEntity>(existingSettings);
					var sourceSettings = new ObservableCollection<SettingEntity>(settings.Select(x => x.ToEntity()));
					var settingComparer = AnonymousComparer.Create((SettingEntity x) => x.Name);
					targetSettings.ObserveCollection(x => repository.Add(x), x => { repository.Attach(x); repository.Remove(x); });
					sourceSettings.Patch(targetSettings, settingComparer, (source, target) => source.Patch(target));

					repository.UnitOfWork.Commit();
				}

				UpdateCache(settingNames, existingSettings);
			}
		}

		public T[] GetArray<T>(string name, T[] defaultValue)
		{
			var result = defaultValue;

			var repositorySetting = LoadSettings(name).FirstOrDefault();
			if (repositorySetting != null)
			{
				result = repositorySetting.SettingValues
					.Select(v => (T)v.RawValue())
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
			var setting = name.ToModel<T>(value);

			SaveSettings(new[] { setting });
		}

		#endregion


		private IEnumerable<ModuleManifest> GetModuleManifestsWithSettings()
		{
			return _manifestProvider.GetModuleManifests().Values
				.Where(m => m.Settings != null && m.Settings.Any());
		}

		private IEnumerable<ModuleSetting> GetAllManifestSettings()
		{
			return GetModuleManifestsWithSettings()
				.SelectMany(m => m.Settings)
				.SelectMany(g => g.Settings);
		}

		private ModuleSetting LoadSettingFromManifest(string name)
		{
			return GetAllManifestSettings().FirstOrDefault(s => s.Name == name);
		}

	
		private List<SettingEntity> LoadSettings(params string[] settingNames)
		{
			var dic = settingNames.ToDictionary(x => x, x => _cacheManager.Get<SettingEntity>(CacheKey.Create("SettingManager", x)));
			var cachedSettings = dic.Values.Where(s => s != null).ToList();
			var settings = cachedSettings.OfType<SettingEntity>().ToList();

			// Load settings which are not in cache and put them to cache
			if (cachedSettings.Count != settingNames.Length)
			{
				var namesToLoad = dic.Where(pair => pair.Value == null).Select(pair => pair.Key).ToArray();
				var settingsNotInCache = LoadSettingsFromRepository(namesToLoad);
				UpdateCache(namesToLoad, settingsNotInCache);
				settings.AddRange(settingsNotInCache);
			}

			return settings;
		}

		private List<SettingEntity> LoadSettingsFromRepository(IEnumerable<string> settingNames)
		{
			using (var repository = _repositoryFactory())
			{
				return repository.Settings.Include(s => s.SettingValues)
					.Where(s => settingNames.Contains(s.Name))
					.ToList();
			}
		}

		private void UpdateCache(IEnumerable<string> names, IEnumerable<SettingEntity> settings)
		{
			var dictionary = names.ToDictionary(name => name, name => settings.FirstOrDefault(s => s.Name == name));
			foreach (var pair in dictionary)
			{
				var cacheKey = CacheKey.Create("SettingManager", pair.Key);
				_cacheManager.Put(cacheKey, pair.Value);
			}
		}
	}
}
