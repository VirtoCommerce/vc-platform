namespace VirtoCommerce.Platform.Core.Settings
{
    public interface ISettingsManager
    {
        ModuleDescriptor[] GetModules();
        SettingDescriptor[] GetSettings(string moduleId);
        void SaveSettings(SettingDescriptor[] settings);

        T GetValue<T>(string name, T defaultValue);
        T[] GetArray<T>(string name, T[] defaultValue);
        void SetValue<T>(string name, T value);
    }
}
