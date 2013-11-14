using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
	public class SettingsManager
	{
		static SettingsManager()
		{
			_settings = GetSettings();
		}

		private static ApplicationSettingsBase GetSettings()
		{
			Assembly asm = Assembly.GetEntryAssembly();
			if (asm != null)
			{
				Type settingsType = (from t in asm.GetTypes()
									 where t.IsSubclassOf(typeof(ApplicationSettingsBase))
									 select t).FirstOrDefault();
				if (settingsType != null)
				{
					PropertyInfo pi = settingsType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
					if (pi != null)
					{
						var appSettings = pi.GetValue(null, null) as ApplicationSettingsBase;
						return appSettings;
					}
					else
					{
						return Activator.CreateInstance(settingsType) as ApplicationSettingsBase;
					}
				}
			}
			return null;
		}

		private static ApplicationSettingsBase _settings;

		public static ApplicationSettingsBase Settings
		{
			get { return _settings; }
			set { _settings = value; }
		}
	}
}
