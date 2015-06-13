using VirtoCommerce.Platform.Core.Common;
namespace VirtoCommerce.Platform.Core.Settings
{
    public interface ISettingsManager
    {
        ModuleDescriptor[] GetModules();
		SettingEntry[] GetObjectSettings(string objectType, string objectId);
		void RemoveObjectSettings(string objectType, string objectId);
		SettingEntry GetSettingByName(string name);
		SettingEntry[] GetModuleSettings(string moduleId);
		void SaveSettings(SettingEntry[] settings);

        T GetValue<T>(string name, T defaultValue);
        T[] GetArray<T>(string name, T[] defaultValue);
        void SetValue<T>(string name, T value);
    }
}
