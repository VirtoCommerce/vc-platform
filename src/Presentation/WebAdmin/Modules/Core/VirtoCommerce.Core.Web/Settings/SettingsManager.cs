using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.Framework.Web.Settings;

namespace VirtoCommerce.CoreModule.Web.Settings
{
	public class SettingsManager : ISettingsManager
	{
		private readonly string _modulesPath;
		private readonly Func<IAppConfigRepository> _repositoryFactory;

		public SettingsManager(string modulesPath, Func<IAppConfigRepository> repositoryFactory)
		{
			_modulesPath = modulesPath;
			_repositoryFactory = repositoryFactory;
		}

		#region ISettingsManager Members

		public ModuleDescriptor[] GetModules()
		{
			var moduleDescriptors = GetModuleManifestsWithSettings()
				.Select(ConvertToModuleDescriptor)
				.ToArray();

			return moduleDescriptors;
		}

		public SettingDescriptor[] GetSettings(string moduleId)
		{
			var result = new List<SettingDescriptor>();

			var manifest = GetModuleManifestsWithSettings().FirstOrDefault(m => m.Id == moduleId);

			if (manifest != null && manifest.Settings != null)
			{
				var settingNames = manifest.Settings
					.SelectMany(g => g.Settings)
					.Select(s => s.Name)
					.Distinct()
					.ToArray();

				if (settingNames.Any())
				{
					var existingSettings = LoadSettings(settingNames);

					foreach (var group in manifest.Settings)
					{
						if (group.Settings != null)
						{
							result.AddRange(group.Settings.Select(s => ConvertToSettingsDescriptor(group.Name, s, existingSettings)));
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
					foreach (var settingDescriptor in settings)
					{
						var name = settingDescriptor.Name;
						var type = settingDescriptor.ValueType;
						var value = settingDescriptor.RawValue();

						SaveSettingValue(repository, name, type, value, existingSettings);
					}

					repository.UnitOfWork.Commit();
				}
			}
		}

		public T GetValue<T>(string name, T defaultValue)
		{
			var result = defaultValue;

			var repositoryValue = GetValueFromRepository(name);
			if (repositoryValue != null)
			{
				result = (T)repositoryValue.RawValue();
			}
			else
			{
				var manifestValue = GetSettingFromManifest(name);
				if (manifestValue != null)
				{
					result = (T)manifestValue.RawDefaultValue();
				}
			}

			return result;
		}

		public void SetValue<T>(string name, T value)
		{
			var descriptor = new SettingDescriptor
			{
				Name = name,
				Value = ((object)value) == null ? null : string.Format(CultureInfo.InvariantCulture, "{0}", value),
				ValueType = ConvertToModuleSettingType(typeof(T))
			};

			SaveSettings(new[] { descriptor });
		}

		#endregion


		private IEnumerable<ModuleManifest> GetModuleManifestsWithSettings()
		{
			// TODO: Add caching
			return ManifestModuleCatalog.GetModuleManifests(_modulesPath)
				.Values
				.Where(m => m.Settings != null && m.Settings.Any());
		}

		private IEnumerable<ModuleSetting> GetAllManifestSettings()
		{
			return GetModuleManifestsWithSettings()
				.SelectMany(m => m.Settings)
				.SelectMany(g => g.Settings);
		}

		private ModuleSetting GetSettingFromManifest(string name)
		{
			return GetAllManifestSettings().FirstOrDefault(s => s.Name == name);
		}

		SettingValue GetValueFromRepository(string name)
		{
			using (var repository = _repositoryFactory())
			{
				return repository.Settings
					.Expand(s => s.SettingValues)
					.Where(s => s.Name == name)
					.SelectMany(s => s.SettingValues)
					.FirstOrDefault();
			}
		}

		private static void SaveSettingValue(IRepository repository, string name, string valueType, object value, ICollection<Setting> existingSettings)
		{
			// Create new setting or attach existing
			var setting = existingSettings.FirstOrDefault(s => s.Name == name);
			if (setting == null)
			{
				setting = new Setting { Name = name };
				repository.Add(setting);
				existingSettings.Add(setting);
			}
			else
			{
				repository.Attach(setting);
			}

			setting.SettingValueType = ConvertToSettingValueType(valueType);

			// Create new value or use existing
			var settingValue = setting.SettingValues.FirstOrDefault();
			if (settingValue == null)
			{
				settingValue = new SettingValue();
				setting.SettingValues.Add(settingValue);
			}

			settingValue.ValueType = setting.SettingValueType;

			switch (valueType)
			{
				case ModuleSetting.TypeBoolean:
					settingValue.BooleanValue = (bool)value;
					break;
				case ModuleSetting.TypeInteger:
					settingValue.IntegerValue = (int)value;
					break;
				case ModuleSetting.TypeDecimal:
					settingValue.DecimalValue = (decimal)value;
					break;
				case ModuleSetting.TypeString:
				case ModuleSetting.TypeSecureString:
					settingValue.ShortTextValue = (string)value;
					break;
			}
		}

		private static string ConvertToSettingValueType(string valueType)
		{
			switch (valueType)
			{
				case ModuleSetting.TypeBoolean:
					return SettingValue.TypeBoolean;
				case ModuleSetting.TypeInteger:
					return SettingValue.TypeInteger;
				case ModuleSetting.TypeDecimal:
					return SettingValue.TypeDecimal;
				default:
					return SettingValue.TypeShortText;
			}
		}

		private static string ConvertToModuleSettingType(Type valueType)
		{
			if (valueType == typeof(bool))
				return ModuleSetting.TypeBoolean;

			if (valueType == typeof(int))
				return ModuleSetting.TypeInteger;

			if (valueType == typeof(decimal))
				return ModuleSetting.TypeDecimal;

			return ModuleSetting.TypeString;
		}

		private static ModuleDescriptor ConvertToModuleDescriptor(ModuleManifest manifest)
		{
			return new ModuleDescriptor
			{
				Id = manifest.Id,
				Title = manifest.Title,
			};
		}

		private List<Setting> LoadSettings(IEnumerable<string> settingNames)
		{
			using (var repository = _repositoryFactory())
			{
				return repository.Settings
					.Expand(s => s.SettingValues)
					.Where(s => settingNames.Contains(s.Name))
					.ToList();
			}
		}

		private static SettingDescriptor ConvertToSettingsDescriptor(string groupName, ModuleSetting setting, IEnumerable<Setting> existingSettings)
		{
			var result = new SettingDescriptor
			{
				GroupName = groupName,
				Name = setting.Name,
				Value = setting.DefaultValue,
				ValueType = setting.ValueType,
				AllowedValues = setting.AllowedValues,
				DefaultValue = setting.DefaultValue,
				Title = setting.Title,
				Description = setting.Description,
			};

			var existingValue = existingSettings
				.Where(s => s.Name == setting.Name)
				.SelectMany(s => s.SettingValues)
				.FirstOrDefault();

			if (existingValue != null)
			{
				result.Value = existingValue.ToString(CultureInfo.InvariantCulture);
			}

			return result;
		}
	}
}
