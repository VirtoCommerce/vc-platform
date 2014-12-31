namespace VirtoCommerce.Framework.Web.Settings
{
	public interface ISettingsManager
	{
		ModuleDescriptor[] GetModules();
		SettingsGroup[] GetSettings(string moduleId);
		void SaveSettings(SettingDescriptor[] settings);

		T GetValue<T>(string name, T defaultValue);
		void SetValue<T>(string name, T value);
	}
}
