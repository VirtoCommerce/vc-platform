namespace VirtoCommerce.Platform.Core.Settings;

/// <summary>
/// Provides settings overrides from configuration (appsettings/environment variables/KeyVault).
/// Supports both Global and Tenant (ObjectType/ObjectId) lookups.
/// </summary>
public interface ISettingsOverrideProvider
{
    bool TryGetCurrentValue(SettingDescriptor descriptor, string objectType, string objectId, out object value);
    bool TryGetDefaultValue(SettingDescriptor descriptor, string objectType, string objectId, out object value);
}
