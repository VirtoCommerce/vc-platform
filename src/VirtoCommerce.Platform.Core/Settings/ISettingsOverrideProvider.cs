namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Provides settings overrides from configuration (appsettings/environment variables/KeyVault).
    /// Supports both Global and Tenant (ObjectType/ObjectId) lookups.
    /// </summary>
    public interface ISettingsOverrideProvider
    {
        bool TryGetForced(SettingDescriptor descriptor, string objectType, string objectId, out object value);
        bool TryGetDefault(SettingDescriptor descriptor, string objectType, string objectId, out object value);
    }
}


